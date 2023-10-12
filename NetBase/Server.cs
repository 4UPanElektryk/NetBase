using SimpleLogs4Net;
using SimpleTCP;
using System;
using System.Text;
using NetBase.Configs;

namespace NetBase
{
	public class Server
	{
		static SimpleTcpServer server;
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
			/*string msg = e.MessageString;
			string path = e.MessageString.Split(' ')[1];
			string replyline = "";
			if (e.MessageString.StartsWith("GET"))
			{
				path = path.TrimStart('/');
				if (path == "")
				{
					path = "index.html";
				}
				foreach (string item in RequestMenager.Run(path, ExtractCookies(msg)))
				{
					replyline += item + "\r\n";
				}
			}
			if (e.MessageString.StartsWith("POST"))
			{
				string[] lines = msg.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
				string[] data = lines[lines.Length - 1].Replace("%40", "@").Split('&');
				foreach (string item in PostsMenager.GetPost(path, data, ExtractCookies(msg)))
				{
					replyline += item + "\r\n";
				}
			}
			e.ReplyLine(replyline);*/
		}
		public static string ExtractCookies(string text)
		{
			string cookie = "";
			string[] request = text.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
			foreach (string item in request)
			{
				if (item.StartsWith("Cookie: "))
				{
					cookie = item.Split(':')[1];
					cookie = cookie.TrimStart(' ');
					return cookie;
				}
			}
			return cookie;
		}
	}
}
