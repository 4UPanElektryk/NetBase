using System;
using NetBase.Communication;
using NetBase.Templating;
using NetBase.Templating.Components;
using NetBase.FileProvider;
using NetBase.StaticRouting;

namespace NetBase.Demo
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Server.router = Funcrouter;
			Server.Start(System.Net.IPAddress.Loopback,80);
			LocalFileLoader lo = new LocalFileLoader("Docs\\");
			Router.Missing = new Rout()
			{
				loader = lo,
				ServerPath = "404",
				LocalPath = "index.html"
			};
			Router.Add(lo, "index.html", "",(r) => { return r.Cookies.Get("Logged") != null;});
			Router.Add(lo, "login.html", "login");
			Console.ReadKey(true);
		}
		public static HTTPResponse Funcrouter(HTTPRequest request)
		{
			if (request.Method == HTTPMethod.GET)
			{
				return HandleGET(request);
			}
			else if (request.Method == HTTPMethod.POST)
			{
				return HandlePOST(request);
			}
			else
			{
				return new HTTPResponse(StatusCode.Method_Not_Allowed);
			}
		}
		public static HTTPResponse HandleGET(HTTPRequest request) 
		{
			Console.WriteLine($"GET{request.Url}");
			if (request.Cookies.Get("Logged") == "true")
			{
				HTTPResponse response = new HTTPResponse(
					StatusCode.OK,
					new HTTPCookies(),
					$"<html><head><title>Test</title></head><body><h1>You're Logged</h1></body></html>",
					ContentType.text_html
				);
				return response;
			}
			else
			{
				return new HTTPResponse(StatusCode.Not_Found);
			}
		}
		public static HTTPResponse HandlePOST(HTTPRequest request)
		{
			if (request.body == "email=joe%40example.com&password=1234")
			{
				HTTPCookies cookies = new HTTPCookies();
				cookies.Set("Logged", "true");
				HTTPResponse response = new HTTPResponse(StatusCode.Moved_Permanently, cookies);
				response.Headers.Add("Location","/index.html");
				return response;
			}
			else 
			{
				return new HTTPResponse(StatusCode.Not_Found);
			}
		}
	}
}
