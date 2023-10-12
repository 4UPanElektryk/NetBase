using System;
using System.Collections.Generic;
using NetBase.Templating;
using NetBase.FileProvider;

namespace NetBase
{
	internal class Program
	{
		static void Main(string[] args)
		{
			LocalFileLoader loader = new LocalFileLoader(AppDomain.CurrentDomain.BaseDirectory + "Tests\\");
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
			TComponent component = new TComponent("test.comp", loader);
			Console.WriteLine(component.Use(provider));
			Console.ReadLine();
		}
	}
}
