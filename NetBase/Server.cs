using SimpleLogs4Net;
using SimpleTCP;
using System;
using System.Text;
using NetBase.Configs;
using NetBase.Communication;

namespace NetBase
{
	public class Server
	{
		static SimpleTcpServer server;
		public delegate HTTPResponse Router(HTTPRequest request);
		public static Router router;
		public static void Start(ServerConfig config)
		{
			server = new SimpleTcpServer
			{
				StringEncoder = Encoding.UTF8,
				Delimiter = (byte)'\n'
			};
			server.DataReceived += Server_DataReceived;
			try
			{
				server.Start(config.address, config.port);
			}
			catch
			{
				Log.Write("Startup Error", EType.Error);
			}
			if (server.IsStarted)
			{
				Log.AddEvent(new Event("Server Started", EType.Informtion));
			}
			else
			{
				Log.AddEvent(new Event("Server Failed to Start", EType.Informtion));
			}

		}
		private static void Server_DataReceived(object sender, Message e)
		{
			HTTPRequest r = HTTPRequest.Parse(e.MessageString);
			HTTPResponse response;
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
			}
			e.ReplyLine(response.ToString());
		}
	}
}
