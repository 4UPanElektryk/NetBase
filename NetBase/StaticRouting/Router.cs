using NetBase.Communication;
using NetBase.FileProvider;
using NetBase.RuntimeLogger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
		public static void Add(IFileLoader loader, string LocalPath, string Url, Func<HTTPRequest, bool> Overrdide = null)
		{
			if (loader == null)
				throw new ArgumentNullException(nameof(loader));
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
            Console.WriteLine($"Checking Rout ({r.Url})");
			if (r.Method != HTTPMethod.GET) { Console.WriteLine($"Routing not met for using diffrent method ({r.Method})"); return false; }
            foreach (var rout in RoutingTable)
			{
				if (rout.ServerPath == r.Url)
				{
					if (rout.OverrideCase == null)
					{
						Console.WriteLine($"Static Rout found ({r.Url})");
						return true;
					}
					else if (!rout.OverrideCase(r))
					{
						Console.WriteLine($"Override case not met Rout ({r.Url})");
						return true;
					}
				}
			}
			Console.WriteLine($"Not Static Rout ({r.Url})");
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
						$"<hr><center><a href=\\\"https://github.com/4UPanElektryk/NetBase\\\">NetBase</a></center>" +
						$"</body></html>");
				}
				throw ex;
			}
		}
	}
}
