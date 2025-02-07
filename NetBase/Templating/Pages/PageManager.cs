using NetBase.FileProvider;
using NetBase.Templating.Components;
using System.Collections.Generic;

namespace NetBase.Templating.Pages
{
	public class PageManager
	{
		private static List<Page> Pages = new List<Page>();
		public PageManager(IFileLoader loader) 
		{
			foreach (string item in loader.GetFiles())
			{
				if (item.EndsWith(".page"))
				{
					Pages.Add(new ImportablePage(item, loader));
				}
			}
		}
		public static Page GetPage(string name)
		{
			foreach (Page page in Pages)
			{
				if (page.AssetName == name)
				{
					return page;
				}
			}
			return new Page("Dummy") { Component = $"Missing File \"{name}\" \n" };
		}
		public static string GetPagePlain(string name){
			foreach (Page page in Pages)
			{
				if (page.AssetName == name)
				{
					return Layouts.LayoutManager.UseLayout(page);
				}
			}
			return $"Missing File \"{name}\" \n";
		}
	}
}
