using System;
using System.Collections.Generic;
using NetBase.Templating.Components;
using NetBase.Templating;
using NetBase.FileProvider;
using System.Collections;

namespace NetBase
{
	internal class Program
	{
		static void Main(string[] args)
		{
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
			Console.ReadLine();
		}
	}
}
