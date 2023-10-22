using NetBase.Communication;
using NetBase.FileProvider;
using System;
using System.Collections.Generic;
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
		public static Rout Missing = new Rout();
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
			foreach (var rout in RoutingTable)
			{
				if (rout.ServerPath == r.Url)
				{
					if (rout.OverrideCase == null)
					{
						return true;
					}
					else if (!rout.OverrideCase(r))
					{
						return true;
					}
				}
			}
			return false;
		}
		private static Rout GetRout(HTTPRequest r) 
		{ 
			foreach (var rout in RoutingTable) 
			{
				if (rout.ServerPath == r.Url)
				{
					if (rout.OverrideCase == null)
					{
						return rout;
					}
					else if (!rout.OverrideCase(r))
					{
						return rout;
					}
				}
			}
			return Missing;
		} 
		public static HTTPResponse Respond(HTTPRequest request) 
		{
			Rout r = GetRout(request);
			ContentType type = ContentType.text_plain;
			string ext = r.LocalPath.Split('.').Last();
			if (lookupTable.ContainsKey(ext)) {type = lookupTable[ext];}

			return new HTTPResponse(StatusCode.OK,new HTTPCookies(), r.loader.Load(r.LocalPath), type);
		}
	}
}
