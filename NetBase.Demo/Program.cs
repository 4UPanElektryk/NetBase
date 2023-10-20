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
			Router.RoutingTable.Add(new Rout()
			{
				loader = lo,
				LocalPath = "index.html",
				ServerPath = ""
			});
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
			if (request.Cookies.Get("Logged") == "true")
			{
				HTTPResponse response = new HTTPResponse(
					StatusCode.OK,
					new HTTPCookies(),
					$"<html><head><title>Test</title></head><body><h1>Default Response</h1><ul>{component.Use(provider)}</ul></body></html>",
					ContentType.text_html
				);
				return response;
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
