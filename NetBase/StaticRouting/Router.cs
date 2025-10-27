using NetBase.Communication;
using NetBase.FileProvider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static NetBase.Server;

namespace NetBase.StaticRouting
{
	public class Router
	{
		internal static IReadOnlyDictionary<string, ContentType> lookupTable = new Dictionary<string, ContentType>
		{
			{ "txt", ContentType.text_plain },
			{ "html", ContentType.text_html },
			{ "htm", ContentType.text_html },
			{ "js", ContentType.text_javascript },
			{ "css", ContentType.text_css },
			{ "json", ContentType.application_json },
			{ "pdf", ContentType.application_pdf },

		};

		public List<RouterEntry> RoutingTable;
		public DataReceived HandeRequest;
		public Router()
		{
			RoutingTable = new List<RouterEntry>();
		}
		public HttpResponse OnRequest(HttpRequest request)
		{
#if DEBUG
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine($"Checking Rout ({request.Url})");
			Console.ResetColor();
#endif
			if (request.Method != HttpMethod.GET)
			{
#if DEBUG
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"Routing not met for using diffrent method ({request.Method})");
				Console.ResetColor();
#endif
				return HandeRequest(request);
			}
			foreach (var rout in RoutingTable)
			{
				if (rout.ServerPath != request.Url) { continue; }
				if (rout.OverrideCase == null)
				{
#if DEBUG
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine($"Static Rout found ({request.Url})");
					Console.ResetColor();
#endif
					return Respond(request, rout);
				}
				else if (!rout.OverrideCase(request))
				{
#if DEBUG
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine($"Override case not met Rout ({request.Url})");
					Console.ResetColor();
#endif
					return Respond(request, rout);
				}
			}
#if DEBUG
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine($"Not Static Rout ({request.Url})");
			Console.ResetColor();
#endif
			return HandeRequest(request);
		}

		public void InitFromINI(IFileLoader loader, string path = "Router.ini")
		{
			Dictionary<string, string> data = ParseData(loader.Load(path));
			string prefix = "";
			if (data.ContainsKey("prefix"))
			{
				prefix = data["prefix"].Trim("\t\r ".ToCharArray());
			}

			foreach (var item in data["defaultRoutes"].Split('\n'))
			{
				string pitem = item.Trim("\t\r ".ToCharArray());
				Add(loader, pitem, prefix + pitem);
			}
		}
		public void Add(IFileLoader loader, string LocalPath, string Url = null, Func<HttpRequest, bool> Overrdide = null)
		{
			if (loader == null)
				throw new ArgumentNullException(nameof(loader));
			if (Url == null)
				Url = LocalPath;
			RoutingTable.Add(new RouterEntry()
			{
				loader = loader,
				LocalPath = LocalPath,
				ServerPath = Url,
				OverrideCase = Overrdide,
			});
		}


		private static HttpResponse Respond(HttpRequest request, RouterEntry entry)
		{
			RouterEntry r = entry;
			ContentType type = ContentType.text_plain;
			string ext = r.LocalPath.Split('.').Last();
			if (lookupTable.ContainsKey(ext)) { type = lookupTable[ext]; }
			try
			{
				return new HttpResponse(StatusCode.OK, r.loader.Load(r.LocalPath), null, Encoding.UTF8, type);
			}
			catch (Exception ex)
			{
				if (ex is FileNotFoundException)
				{
					return new HttpResponse(
						StatusCode.Not_Found,
						$"<html><head><title>File was not found</title></head>" +
						$"<body><h1>File was not found by Router</h1>" +
						$"<p>Local File: \"{r.LocalPath}\"</p>" +
						$"<hr><center><a href=\"https://github.com/4UPanElektryk/NetBase\">NetBase</a></center>" +
						$"</body></html>",
						null,
						Encoding.UTF8,
						ContentType.text_html);
				}
				throw ex;
			}
		}
		private Dictionary<string, string> ParseData(string data)
		{
			Dictionary<string, string> d = new Dictionary<string, string>();
			//! Idon't know why It Works but it works
			string[] c = data.Split('\n');
			string lastkey = null;
			foreach (var linem in c)
			{
				string line = linem.Trim("\r".ToCharArray());
				if (line.StartsWith("#") || line == " ")
				{
					// comments go brrr
				}
				else if (line.StartsWith("\t") || line.StartsWith("    "))
				{
					if (lastkey != null)
					{
						string val = d[lastkey];
						d.Remove(lastkey);
						if (!(val == " " || val == ""))
						{
							d.Add(lastkey, val + "\n" + line.Trim("\t\r ".ToCharArray()));
						}
						else
						{
							d.Add(lastkey, line.Trim("\t\r ".ToCharArray()));
						}
					}
				}
				else if (line.Contains("="))
				{
					d.Add(
						line.Split('=')[0],
						line.Substring(
							line.Split('=')[0].Length + 1).Trim("\t\r".ToCharArray()
						)
					);
					lastkey = line.Split('=')[0];
				}
			}
			return d;
		}
	}
}
