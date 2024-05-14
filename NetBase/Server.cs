using SimpleTCP;
using System;
using System.Text;
using NetBase.Communication;
using NetBase.RuntimeLogger;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace NetBase
{
	public class Server
	{
		static SimpleTcpServer _server;
		public delegate HttpResponse DataReceived(HttpRequest request);
		public static DataReceived router;
		public static void Start(IPAddress address, int port)
		{
			new Log("Logs\\");
			_server = new SimpleTcpServer
			{
				StringEncoder = Encoding.UTF8,
				Delimiter = (byte)'\0',
			};
			_server.DataReceived += Server_DataReceived;
			try
			{
				_server.Start(address, port);
			}
			catch (Exception ex)
			{
				Log.Write($"Startup Error {ex.Message}");
			}
			if (_server.IsStarted)
			{
				Log.Write($"Server Started on http://{address}:{port}/");
			}
			else
			{
				Log.Write("Server Failed to Start");
			}

		}
		private static void Server_DataReceived(object sender, Message e)
		{
			Dictionary<string, long> timings = new Dictionary<string, long>();
			Stopwatch sw = Stopwatch.StartNew();
			HttpRequest r = HttpRequest.Parse(e.MessageString);
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
					response = router.Invoke(r);
				}
				catch (Exception ex)
				{
					response = new HttpResponse(
						StatusCode.Internal_Server_Error,
						new HttpCookies(),
						$"<html><head>" +
						$"<title>500 Internal Server Error</title>" +
						$"</head><body>" +
						$"<h1><center>500 Internal Server Error</center></h1>" +
						$"<h2>{ex.Message}</h2>" +
						$"<p>Server Encountered an exception while trying to complete the reques</p>" +
						$"<p>{ex.StackTrace}</p>" +
						$"<hr> <center><a href=\"https://github.com/4UPanElektryk/NetBase\">NetBase</a></center>" +
						$"</body></html>",
						ContentType.text_html
					);
					Log.Incident(ex, e.MessageString);
				}
				if (response.Body == "" && (int)response.Status >= 400)
				{
					string ReasonPhrase = Enum.GetName(typeof(StatusCode), (int)response.Status).Replace("_", " ");
					response.Body =
						$"<html><head>" +
						$"<title>{(int)response.Status} {ReasonPhrase}</title>" +
						$"</head><body>" +
						$"<center><h1>{(int)response.Status} {ReasonPhrase}</h1></center>" +
						$"<hr><center><a href=\"https://github.com/4UPanElektryk/NetBase\">NetBase</a></center>" +
						$"</body></html>";
					response.contentType = ContentType.text_html;
				}
				sw.Stop(); timings.Add("DynamicRouting", sw.ElapsedMilliseconds);
			}
			if (response.Body == "" && response.Status == StatusCode.Not_Found)
			{
				string ReasonPhrase = Enum.GetName(typeof(StatusCode), (int)response.Status).Replace("_", " ");
				response.Body =
					$"<html><head>" +
					$"<title>{(int)response.Status} {ReasonPhrase}</title>" +
					$"</head><body>" +
					$"<center><h1>{(int)response.Status} {ReasonPhrase}</h1><p>The requested url was not located on this server \"{r.Url}\"</p></center>" +
					$"<hr><center><a href=\"https://github.com/4UPanElektryk/NetBase\">NetBase</a></center>" +
					$"</body></html>";
				response.contentType = ContentType.text_html;
			}
			string servertiming = "";
			foreach (var item in timings)
			{
				servertiming += $"{item.Key};dur={item.Value},";
			}
			servertiming.TrimEnd(',');
			response.Headers.Add("Server-Timing", servertiming);
			e.ReplyLine(response.ToString());
		}
	}
}
