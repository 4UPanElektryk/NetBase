using NetBase.Communication;
using NetBase.FileProvider;
using NetBase.RuntimeLogger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
		public static List<Rout> RoutingTable = new List<Rout>();
		private static Dictionary<string, string> ParseData(string data)
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
		public static void InitFromINI(IFileLoader loader, string path = "Router.ini")
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
				Add(loader,pitem, prefix + pitem);
			}
		}
		public static void Add(IFileLoader loader, string LocalPath, string Url = null, Func<HTTPRequest, bool> Overrdide = null)
		{
			if (loader == null)
				throw new ArgumentNullException(nameof(loader));
			if (Url == null)
				Url = LocalPath;
			RoutingTable.Add(new Rout()
			{
				loader = loader,
				LocalPath = LocalPath,
				ServerPath = Url,
				OverrideCase = Overrdide,
			});
		}
		public static bool IsStatic(HTTPRequest r)
		{
#if DEBUG
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine($"Checking Rout ({r.Url})");
			Console.ResetColor();
#endif
			if (r.Method != HTTPMethod.GET) {
#if DEBUG
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"Routing not met for using diffrent method ({r.Method})");
				Console.ResetColor();
#endif
				return false; 
			}
			foreach (var rout in RoutingTable)
			{
				if (rout.ServerPath != r.Url) { continue; }
				if (rout.OverrideCase == null)
				{
#if DEBUG
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine($"Static Rout found ({r.Url})");
					Console.ResetColor();
#endif
					return true;
				}
				else if (!rout.OverrideCase(r))
				{
#if DEBUG
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine($"Override case not met Rout ({r.Url})");
					Console.ResetColor();
#endif
					return true;
				}
			}
#if DEBUG
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine($"Not Static Rout ({r.Url})");
			Console.ResetColor();
#endif
			return false;
		}
		private static Rout GetRout(HTTPRequest r) 
		{ 
			foreach (var rout in RoutingTable) 
			{
				if (rout.ServerPath == r.Url)
				{
					return rout;
				}
			}
			throw new NotImplementedException("This should be imposible becouse this already checks if file is routed via this");
		} 
		public static HttpResponse Respond(HTTPRequest request) 
		{
			Rout r = GetRout(request);
			ContentType type = ContentType.text_plain;
			string ext = r.LocalPath.Split('.').Last();
			if (lookupTable.ContainsKey(ext)) {type = lookupTable[ext];}
			try
			{
				return new HttpResponse(StatusCode.OK, null, r.loader.Load(r.LocalPath), type);
			}
			catch (Exception ex)
			{
				Log.Incident(ex);
				if(ex is FileNotFoundException)
				{
					return new HttpResponse(StatusCode.Not_Found, null, 
						$"<html><head><title>File was not found</title></head>" +
						$"<body><h1>File was not found by Router</h1>" +
						$"<p>Local File: \"{r.LocalPath}\"</p>" +
						$"<hr><center><a href=\"https://github.com/4UPanElektryk/NetBase\">NetBase</a></center>" +
						$"</body></html>", ContentType.text_html);
				}
				throw ex;
			}
		}
	}
}
