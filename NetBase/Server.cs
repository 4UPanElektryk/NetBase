using NetBase.Communication;
using NetBase.Runtime;
using NetBase.StaticRouting;
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
		private HttpListener _listener;
		private bool _isRunning = false;
		private Thread thread = null;
		public bool IsRunning { get { return _isRunning; } }
		public delegate HttpResponse DataReceived(HttpRequest request);
		public Router router;
		public DataReceived HandeRequest;
		private Log log;
		public Server(string serverLogPath = null)
		{
			if (serverLogPath == null)
			{

			}
			log = new Log("Logs\\");
			_listener = new HttpListener();
		}
		public void Start(string prefix)
		{
			_listener.Prefixes.Add(prefix);
			_isRunning = true;
			try
			{
				_listener.Start();
				thread = new Thread(ListenerThread);
				thread.Start();
			}
			catch (HttpListenerException htle)
			{
				log.Write($"Internall Startup Error! {htle.Message}! (If possible please create a github issue)");
				throw htle;
			}
			catch (Exception ex)
			{
				log.Write($"Startup Error {ex.Message}");
				throw ex;
			}
			finally
			{
				if (_listener.IsListening)
				{
					log.Write($"Server Started on {prefix}");
				}
				else
				{
					log.Write($"Server Failed to Start on {prefix}");
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
				_listener.Stop();
				log.Write("Server Stopped");
			}
			else
			{
				log.Write("Server is not running");
			}
		}
		private async void ListenerThread()
		{
			while (_isRunning)
			{
				HttpListenerContext ctx = await _listener.GetContextAsync();

				HttpListenerRequest req = ctx.Request;
				HttpListenerResponse res = ctx.Response;
				//string requestString = new StreamReader(req.InputStream, req.ContentEncoding).ReadToEnd();
				OnRequest(req, res);
			}
		}
		private void OnRequest(HttpListenerRequest Request, HttpListenerResponse hlresponse)
		{
			#region Setup
			PerformanceDebuger timings = new PerformanceDebuger();
			timings.Start("RequestParse");
			HttpRequest r = HttpRequest.Parse(Request);
			timings.Stop();
			HttpResponse response;
			#endregion

			#region Routing
			if (StaticRouting.Router.IsStatic(r))
			{
				timings.Start("StaticRouting");
				response = StaticRouting.Router.Respond(r);
				timings.Stop();
			}
			else
			{
				timings.Start("DynamicRouting");
				try
				{
					response = HandeRequest.Invoke(r);
				}
				catch (Exception ex)
				{
					response = new HttpResponse(
						StatusCode.Internal_Server_Error,
						$"<html><head>" +
						$"<meta charset=\"UTF-8\">" +
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
					log.Incident(ex, "");
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
				timings.Stop();
			}
			#endregion

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
