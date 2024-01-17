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
			Regex sections = new Regex(@"\[([\w\.]+)\]([\n|a-zA-Z0-9\/\=\t\.\s]*)", RegexOptions.Multiline);
			Regex rest = new Regex(@"(\w+)\s*=\s*\n?((?:\t?.+\n?)+)", RegexOptions.Multiline);
			MatchCollection sectionsCollection = sections.Matches(data);
			if (sectionsCollection.Count == 0)
			{
				MatchCollection restCollection = rest.Matches(data);
				foreach (Match item in restCollection)
				{
					d.Add($"{item.Groups[1].Value}", item.Groups[2].Value);
				}
			}
			foreach (Match item in sectionsCollection)
			{
				string local = item.Groups[1].Value;
				MatchCollection restCollection = rest.Matches(item.Groups[2].Value);
				foreach (Match item2 in restCollection)
				{
					d.Add($"{local}.{item2.Groups[1].Value}", item2.Groups[2].Value);
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
				prefix = data["prefix"];
			}

            foreach (var item in data["defaultRoutes"].Split('\n'))
			{
				string pitem = item.Trim("\t\r".ToCharArray());
				Add(loader, prefix + pitem, pitem);
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
			Console.WriteLine($"Checking Rout ({r.Url})");
#endif
			if (r.Method != HTTPMethod.GET) {
#if DEBUG
				Console.WriteLine($"Routing not met for using diffrent method ({r.Method})"); 
#endif
				return false; 
			}
            foreach (var rout in RoutingTable)
			{
				if (rout.ServerPath != r.Url) { continue; }
				if (rout.OverrideCase == null)
				{
#if DEBUG
					Console.WriteLine($"Static Rout found ({r.Url})");
#endif
					return true;
				}
				else if (!rout.OverrideCase(r))
				{
#if DEBUG
					Console.WriteLine($"Override case not met Rout ({r.Url})");
#endif
					return true;
				}
			}
#if DEBUG
			Console.WriteLine($"Not Static Rout ({r.Url})");
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
		public static HTTPResponse Respond(HTTPRequest request) 
		{
			Rout r = GetRout(request);
            ContentType type = ContentType.text_plain;
			string ext = r.LocalPath.Split('.').Last();
            if (lookupTable.ContainsKey(ext)) {type = lookupTable[ext];}
			try
			{
				return new HTTPResponse(StatusCode.OK, null, r.loader.Load(r.LocalPath), type);
			}
			catch (Exception ex)
			{
				Log.Incident(ex);
				if(ex is FileNotFoundException)
				{
					return new HTTPResponse(StatusCode.Not_Found, null, 
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
