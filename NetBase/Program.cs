using System;
using NetBase.Communication;
using System.Net;
using NetBase.Templating.Components;
using NetBase.FileProvider;
using NetBase.Templating;

namespace NetBase
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Server.router = Funcrouter;
			Server.Start(
				new Configs.ServerConfig() 
				{ 
					address = IPAddress.Loopback, 
					port = 8080
				}
			);
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
			ImportableComponent component = new ImportableComponent("test.comp",new LocalFileLoader("test\\"));
			DataProvider provider = new DataProvider();
			foreach (var item in request.URLParamenters)
			{
				provider.Data.Add(new System.Collections.Generic.Dictionary<string, string> { { "pram", item.Key }, { "val", item.Value } });
			}

			HTTPResponse response = new HTTPResponse(
				StatusCode.OK,
				new HTTPCookies(),
				$"<html><head><title>Test</title></head><body><h1>Default Response</h1><ul>{component.Use(provider)}</ul></body></html>",
				ContentType.text_html
			);
			throw new Exception();
			return response;
		}
	}
}
