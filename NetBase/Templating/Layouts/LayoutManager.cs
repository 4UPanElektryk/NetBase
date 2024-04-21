using System;
using NetBase.FileProvider;
using System.Collections.Generic;
using NetBase.Templating.Pages;
using System.Linq;
namespace NetBase.Templating.Layouts
{
	public class LayoutManager
	{
		private static List<Layout> Layouts = new List<Layout>();
		private static Dictionary<string, List<Page>> LayoutsPages = new Dictionary<string, List<Page>>();
		public static void Add(Page p)
		{
			List<Page> old = new List<Page>();
			if (LayoutsPages.ContainsKey(p.LayoutName))
			{
				old = LayoutsPages[p.LayoutName];
				LayoutsPages.Remove(p.LayoutName);
			}
			old.Add(p);
			LayoutsPages.Add(p.LayoutName, old);
		}
		public LayoutManager(IFileLoader loader)
		{

			foreach (string item in loader.GetFiles())
			{
				if (item.EndsWith(".lt"))
				{
					Layouts.Add(new ImportableLayout(item, loader));
				}
			}
		}
		public static string UseLayout(Page p,Dictionary<string,DataProvider> elements = null, Dictionary<string,string> provider = null)
		{
			Layout lt = Layouts.Find((obj) => obj.AssetName == p.LayoutName);
			DataProvider pr = new DataProvider();
            foreach (var item in LayoutsPages[p.LayoutName])
			{
				if (item.PageData.ContainsKey("Visible") && bool.Parse(item.PageData["Visible"]))
				{
                    Console.WriteLine(item.AssetName);
                    Dictionary <string, string> temp = item.PageData.ToDictionary(entry => entry.Key,entry => entry.Value);
					temp.Add("Active", item.AssetName == p.AssetName ? lt.LayoutData["ActiveElementValue"] : "");
					pr.Add(temp);
				}
			}
			Dictionary<string, string> dict = p.PageData.ToDictionary(entry => entry.Key, entry => entry.Value);
			dict.Add("page", p.Use(elements, provider));
			return lt.Use(
				new Dictionary<string, DataProvider> { { lt.LayoutData["LinkElement"], pr } },
				dict
			);
		}
	}
}
