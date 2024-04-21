using System;
using NetBase.Communication;
using nt = NetBase.Templating;
using NetBase.FileProvider;
using NetBase.StaticRouting;
using System.IO;
using System.Collections.Generic;

namespace NetBase.Demo
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Server.router = Funcrouter;
			Server.Start(System.Net.IPAddress.Loopback,8080);
			IFileLoader lo = /*new SingularFSFileLoader("docs.fs_");*/ new LocalFileLoader("docs" + Path.DirectorySeparatorChar);
			new nt.Components.TComponentManager(lo);
			new nt.Pages.PageManager(lo);
			new nt.Layouts.LayoutManager(lo);
			Router.InitFromINI(lo);
			Console.ReadLine();
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
			/*if (request.Url == "login")
			{
				return new HttpResponse(StatusCode.OK,null,nt.Pages.PageManager.GetPagePlain("login.page"),ContentType.text_html);
			}
			if (request.Url == "test")
			{
				return new HttpResponse(StatusCode.OK, null, nt.Pages.PageManager.GetPagePlain("strona.page"), ContentType.text_html);
			}*/
			return new HttpResponse(StatusCode.Not_Found);
			/*if (request.Url == "" && request.Cookies.Get("Logged") == "true")
			{
				Dictionary<string, string> dp = new Dictionary<string, string>() { { "name", "somebody" } };
                HttpResponse response = new HttpResponse(
					StatusCode.OK,
					new HttpCookies(),
					//DocumentManager.GetComponet("index.t.html").Use(null,dp),
					"to fix",
					ContentType.text_html
				);
				return response;
			}
			else if (request.Url == "logout") 
			{
				HttpCookies cookies = new HttpCookies();
				HttpCookies cookies = new HttpCookies();
				cookies.Set("Logged", "false");
				HttpResponse response = new HttpResponse(StatusCode.Moved_Permanently, cookies);
				HttpResponse response = new HttpResponse(StatusCode.Moved_Permanently, cookies);
				response.Headers.Add("Location", "/");
				return response;
			}
			else
			{
				return new HttpResponse(StatusCode.Not_Found);
			}*/
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
