using System;
using NetBase.Communication;
using System.Net;

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
					StatusCode.Not_Found,
					new HTTPCookies()
				);
			}
			if (request.Cookies.Get("test") != null)
			{
				return new HTTPResponse(
					StatusCode.OK,
					new HTTPCookies(),
					$"<html><head><title>Test</title></head><body><h1>Default Response</h1>Cookie was set</body></html>",
					ContentType.text_html
				);
			}
			HTTPCookies c = new HTTPCookies();
			c.Set("test", "d");
			HTTPResponse response = new HTTPResponse(
				StatusCode.OK,
				c,
				$"<html><head><title>Test</title></head><body><h1>Default Response</h1>{request.Url}</body></html>",
				ContentType.text_html
			);
			return response;
		}
	}
}
