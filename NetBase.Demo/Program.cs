using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NetBase.Communication;
using NetBase.Templating;
using NetBase.Templating.Components;
using NetBase.FileProvider;
using ContentType = NetBase.Communication.ContentType;

namespace NetBase.Demo
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Server.router = Funcrouter;
			Server.Start(IPAddress.Loopback,8080);
			Console.ReadLine();
		}
		public static HTTPResponse Funcrouter(HTTPRequest request)
		{
			if (request.Url.EndsWith(".ico"))
			{
				return new HTTPResponse(
					StatusCode.Service_Unavailable,
					new HTTPCookies()
				);
			}
			ImportableComponent component = new ImportableComponent("test.comp", new LocalFileLoader("test\\"));
			DataProvider provider = new DataProvider();
			foreach (var item in request.URLParamenters)
			{
				provider.Data.Add(new Dictionary<string, string> { { "pram", item.Key }, { "val", item.Value } });
			}

			HTTPResponse response = new HTTPResponse(
				StatusCode.OK,
				new HTTPCookies(),
				$"<html><head><title>Test</title></head><body><h1>Default Response</h1><ul>{component.Use(provider)}</ul></body></html>",
				ContentType.text_html
			);
			return response;
		}
	}
}
