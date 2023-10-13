using System;
using System.Collections.Generic;
using NetBase.Templating.Components;
using NetBase.Templating;
using NetBase.FileProvider;

namespace NetBase
{
	internal class Program
	{
		static void Main(string[] args)
		{
			LocalFileLoader loader = new LocalFileLoader(AppDomain.CurrentDomain.BaseDirectory + "Tests\\");
			TComponentManager manager = new TComponentManager(loader);
			DataProvider provider = new DataProvider();
			provider.Data.Add(
				new Dictionary<string, string> 
				{ 
					{ "filled", "thing" } 
				}
			);
			provider.Data.Add(
				new Dictionary<string, string>
				{
					{ "filled", "or not" }
				}
			);
			provider.Data.Add(
				new Dictionary<string, string>
				{
					{ "filled", "funny mic" }
				}
			);
			TComponent component = TComponentManager.GetComponet("test.comp");
			Console.WriteLine(component.Use(provider));
			TComponent Qomponent = TComponentManager.GetComponet("ftest.comp");
			Console.WriteLine(Qomponent.Use(provider));
			Console.ReadLine();
		}
	}
}
