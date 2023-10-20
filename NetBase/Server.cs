using SimpleTCP;
using System;
using System.Text;
using NetBase.Communication;
using NetBase.RuntimeLogger;
using System.Net;

namespace NetBase
{
	public class Server
	{
		static SimpleTcpServer server;
		public delegate HTTPResponse DataReceived(HTTPRequest request);
		public static DataReceived router;
		public static void Start(IPAddress address, int port)
		{
			new Log("Logs\\");
			server = new SimpleTcpServer
			{
				StringEncoder = Encoding.UTF8,
				Delimiter = (byte)'\n'
			};
			server.DataReceived += Server_DataReceived;
			try
			{
				server.Start(address, port);
			}
			catch
			{
				Log.Write("Startup Error");
			}
			if (server.IsStarted)
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
			HTTPRequest r = HTTPRequest.Parse(e.MessageString);
			HTTPResponse response;
			if (StaticRouting.Router.IsStatic(r.Url)) 
			{
				response = StaticRouting.Router.Respond(r);
				e.ReplyLine(response.ToString());
				return;
			}
			try
			{
				response = router.Invoke(r);
			}
			catch (Exception ex)
			{
				response = new HTTPResponse(
					StatusCode.Internal_Server_Error,
					new HTTPCookies(),
					$"<html><head>" +
					$"<title>500 Internal Server Error</title>" +
					$"</head><body>" +
					$"<h1>500 Internal Server Error</h1>" +
					$"<h2>{ex.Message}</h2>" +
					$"<p>Server Encountered an exception while trying to complete the reques</p>" +
					$"<p>{ex.StackTrace}</p>" +
					$"<hr> <a href=\"https://github.com/4UPanElektryk/NetBase\">NetBase</a>" +
					$"</body></html>",
					ContentType.text_html
				);
				Log.Incident(ex);
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
			e.ReplyLine(response.ToString());
		}
	}
}
