using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NetBase.Communication;
using NetBase.Templating;
using NetBase.Templating.Components;
using NetBase.FileProvider;
using NetBase.StaticRouting;
using System.ComponentModel;

namespace NetBase.Demo
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Server.router = Funcrouter;
			Server.Start(IPAddress.Loopback,8080);
			LocalFileLoader lo = new LocalFileLoader("Docs\\");
			Router.Add(lo, "index.html", "",(r) => { return r.Cookies.Get("Logged") != null;});
			Router.Add(lo, "login.html", "login");
			Console.ReadLine();
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
				return new HTTPResponse(StatusCode.Method_Not_Allowed, new HTTPCookies());
			}
		}
		public static HTTPResponse HandleGET(HTTPRequest request) 
		{
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
            Console.WriteLine(request.body);
            if (false)
			{
				 
			}
			else 
			{
				return new HTTPResponse(StatusCode.Not_Found);
			}
		}
	}
}
