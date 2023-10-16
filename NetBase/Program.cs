using System;
using System.Collections.Generic;
using NetBase.Templating.Components;
using NetBase.Templating;
using NetBase.FileProvider;
using NetBase.Communication;
using System.Net;

namespace NetBase
{
	internal class Program
	{
		static void Main(string[] args)
		{
			#region Templates
			IFileLoader loader =
				new LocalFileLoader(AppDomain.CurrentDomain.BaseDirectory + "Tests\\"); 
				//new SingularFSFileLoader("data.fs_");
			TComponentManager manager = new TComponentManager(loader);
			DataProvider provider = new DataProvider();
			provider.Data.AddRange( 
				new List<Dictionary<string,string>> {
				new Dictionary<string, string>
				{
					{ "filled", "thing" },
					{ "element", "thing" }
				},
				new Dictionary<string, string>
				{
					{ "filled", "or not" }
				},
				new Dictionary<string, string>
				{
					{ "filled", "funny mic" }
				}
			});
			TComponent component = TComponentManager.GetComponet("test.comp");
			Console.WriteLine(component.Use(provider));
			TComponent Qomponent = TComponentManager.GetComponet("ftest.comp");
			Console.WriteLine(Qomponent.Use(provider));
			#endregion
			#region Comms
			Server.Start(new Configs.ServerConfig() { address = IPAddress.Loopback, port = 8080});
			Server.router = funcrouter;
            #endregion
			Console.ReadLine();
        }
		public static HTTPResponse funcrouter(HTTPRequest request)
		{
			HTTPResponse response = new HTTPResponse(
				StatusCode.OK,
				new HTTPCookies(),
				"<html><head><title>Test</title></head><body><h1>Default Response</h1></body></html>",
				ContentType.text_html
			);
			return response;
		}
	}
}
