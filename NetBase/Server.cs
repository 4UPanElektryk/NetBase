using NetBase.Communication;
using NetBase.RuntimeLogger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading;

namespace NetBase
{
	public class Server
	{
		private HttpListener listener;
		private bool _isRunning = false;
		private Thread thread = null;
		public bool IsRunning { get { return _isRunning; } }
		public delegate HttpResponse DataReceived(HttpRequest request);
		//public event EventLogEntry OnDataReceived;
		public DataReceived HandeRequest;
		public void Start(string prefix)
		{
			new Log("Logs\\");
			listener = new HttpListener();
			listener.Prefixes.Add(prefix);
			_isRunning = true;

			try
			{
				listener.Start();
				thread = new Thread(ListenerThread);
				thread.Start();
			}
			catch (HttpListenerException htle)
			{
				Log.Write($"Internall Startup Error! {htle.Message}! (If possible please create a github issue)");
				throw htle;
			}
			catch (Exception ex)
			{
				Log.Write($"Startup Error {ex.Message}");
				throw ex;
			}
			finally
			{
				if (listener.IsListening)
				{
					Log.Write($"Server Started on {prefix}");
				}
				else
				{
					Log.Write($"Server Failed to Start on {prefix}");
					_isRunning = false;
				}
			}
		}

		public void Start(IPAddress address, int port)
		{
			Start($"http://{address}:{port}/");
		}
		public void Stop()
		{
			if (_isRunning)
			{
				_isRunning = false;
				listener.Stop();
				Log.Write("Server Stopped");
			}
			else
			{
				Log.Write("Server is not running");
			}
		}
		private async void ListenerThread()
		{
			while (_isRunning)
			{
				HttpListenerContext ctx = await listener.GetContextAsync();

				HttpListenerRequest req = ctx.Request;
				HttpListenerResponse res = ctx.Response;
				//string requestString = new StreamReader(req.InputStream, req.ContentEncoding).ReadToEnd();
				OnRequest(req, res);
			}
		}
		private void OnRequest(HttpListenerRequest Request, HttpListenerResponse hlresponse)
		{
			Dictionary<string, long> timings = new Dictionary<string, long>();
			Stopwatch sw = Stopwatch.StartNew();
			HttpRequest r = HttpRequest.Parse(Request);
			sw.Stop(); timings.Add("RequestParse", sw.ElapsedMilliseconds);
			HttpResponse response;
			if (StaticRouting.Router.IsStatic(r))
			{
				sw.Restart();
				response = StaticRouting.Router.Respond(r);
				sw.Stop(); timings.Add("StaticRouting", sw.ElapsedMilliseconds);
			}
			else
			{
				sw.Restart();
				try
				{
					response = HandeRequest.Invoke(r);
				}
				catch (Exception ex)
				{
					response = new HttpResponse(
						StatusCode.Internal_Server_Error,
						$"<html><head>" +
						$"<title>500 Internal Server Error</title>" +
						$"</head><body>" +
						$"<h1><center>500 Internal Server Error</center></h1>" +
						$"<h2>{ex.Message}</h2>" +
						$"<p>Server Encountered an exception while trying to complete the reques</p>" +
						$"<p>{ex.StackTrace}</p>" +
						$"<hr> <center><a href=\"https://github.com/4UPanElektryk/NetBase\">NetBase</a></center>" +
						$"</body></html>",
						new HttpCookies(),
						Encoding.UTF8,
						ContentType.text_html
					);
					//TODO reimplement logs
					Log.Incident(ex, "");
#if DEBUG
					throw ex;
#endif
				}
				if (response.Content == null && (int)response.Status >= 400)
				{
					string ReasonPhrase = Enum.GetName(typeof(StatusCode), (int)response.Status).Replace("_", " ");
					response.ContentEncoding = Encoding.UTF8;
					response.Body =
						$"<html><head>" +
						$"<title>{(int)response.Status} {ReasonPhrase}</title>" +
						$"</head><body>" +
						$"<center><h1>{(int)response.Status} {ReasonPhrase}</h1></center>" +
						$"<hr><center><a href=\"https://github.com/4UPanElektryk/NetBase\">NetBase</a></center>" +
						$"</body></html>";
					response.contentType = "text/html";
				}
				sw.Stop(); timings.Add("DynamicRouting", sw.ElapsedMilliseconds);
			}
			if (response.Content == null && response.Status == StatusCode.Not_Found)
			{
				string ReasonPhrase = Enum.GetName(typeof(StatusCode), (int)response.Status).Replace("_", " ");
				response.Body =
					$"<html><head>" +
					$"<title>{(int)response.Status} {ReasonPhrase}</title>" +
					$"</head><body>" +
					$"<center><h1>{(int)response.Status} {ReasonPhrase}</h1><p>The requested url was not located on this server \"{r.Url}\"</p></center>" +
					$"<hr><center><a href=\"https://github.com/4UPanElektryk/NetBase\">NetBase</a></center>" +
					$"</body></html>";
				response.contentType = "text/html";
			}
			string servertiming = "";
			foreach (var item in timings)
			{
				servertiming += $"{item.Key};dur={item.Value},";
			}
			servertiming.TrimEnd(',');
			response.Headers.Add("Server-Timing", servertiming);
			hlresponse.StatusCode = (int)response.Status;
			hlresponse.StatusDescription = Enum.GetName(typeof(StatusCode), (int)response.Status).Replace("_", " ");
			hlresponse.ContentType = response.contentType;
			foreach (var item in response.Headers)
			{
				hlresponse.Headers.Add(item.Key, item.Value);
			}
			hlresponse.Cookies.Add(response.Cookies.ExportCookies());
			hlresponse.ProtocolVersion = new Version(1, 1);
			hlresponse.ContentLength64 = response.Content != null ? response.Content.Length : 0;
			if (response.Content != null)
			{
				hlresponse.OutputStream.Write(response.Content, 0, response.Content.Length);
			}
			hlresponse.Close();
		}
	}
}
