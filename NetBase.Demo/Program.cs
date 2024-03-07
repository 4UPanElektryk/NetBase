using System;
using NetBase.Communication;
using NetBase.Templating;
using NetBase.Templating.Templates;
using NetBase.Templating.Components;
using NetBase.FileProvider;
using NetBase.StaticRouting;
using System.Collections.Generic;
using System.Collections;

namespace NetBase.Demo
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Server.router = Funcrouter;
			Server.Start(System.Net.IPAddress.Loopback,80);
			IFileLoader lo = /*new SingularFSFileLoader("docs.fs_");*/ new LocalFileLoader("Docs\\");
			new DocumentManager(lo);
			Router.Add(lo, "index.html", "", (r) => { return r.Cookies.Get("Logged") != null; });
			Router.Add(lo, "index.html", "", (r) => { return r.Cookies.Get("Logged") == "true"; });
			Router.Add(lo, "login.html", "login");
			Console.ReadKey(true);
		}
		public static HttpResponse Funcrouter(HttpRequest request)
		{
			HttpResponse res = new HttpResponse(StatusCode.Method_Not_Allowed);
			string ReasonPhrase = Enum.GetName(typeof(HttpMethod), (int)request.Method);
			Console.WriteLine($"{ReasonPhrase} ({request.Url})");
			if (request.Method == HttpMethod.GET)
			{
				res = HandleGET(request);
			}
			else if (request.Method == HttpMethod.POST)
			{
				res = HandlePOST(request);
			}
            return res;
		}
		public static HttpResponse HandleGET(HttpRequest request) 
		{
			if (request.Url == "" && request.Cookies.Get("Logged") == "true")
			{
				Dictionary<string, string> dp = new Dictionary<string, string>() { { "name", "somebody" } };
				HttpResponse response = new HttpResponse(
					StatusCode.OK,
					new HttpCookies(),
					PageManager.GetPage("index.t.html").Use(null,dp),
					ContentType.text_html
				);
				return response;
			}
			else if (request.Url == "logout") 
			{
				HttpCookies cookies = new HttpCookies();
				cookies.Set("Logged", "false");
				HttpResponse response = new HttpResponse(StatusCode.Moved_Permanently, cookies);
				response.Headers.Add("Location", "/");
				return response;
			}
			else
			{
				return new HttpResponse(StatusCode.Not_Found);
			}
		}
		public static HttpResponse HandlePOST(HttpRequest request)
		{
            if (request.PostData["email"] == "joe@example.com" && request.PostData["password"] == "1234")
			{
                HttpCookies cookies = new HttpCookies();
				cookies.Set("Logged", "true");
				HttpResponse response = new HttpResponse(StatusCode.Moved_Permanently, cookies);
				response.Headers.Add("Location", "/");
				return response;
			}
			else 
			{
				return new HttpResponse(StatusCode.Not_Found);
			}
		}
	}
}
