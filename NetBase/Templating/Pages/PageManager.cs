using NetBase.FileProvider;
using NetBase.Templating.Components;
using System.Collections.Generic;

namespace NetBase.Templating.Templates
{
	public class PageManager
	{
		private static List<Page> Templates = new List<Page>();
		public PageManager(IFileLoader loader) 
		{
			foreach (string item in loader.GetFiles())
			{
				if (item.EndsWith(".page"))
				{
					Templates.Add(new ImportablePage(item, loader));
				}
			}
		}
		public static Page GetPage(string name)
		{
			foreach (Page page in Templates)
			{
				if (page.AssetName == name)
				{
					return page;
				}
			}
			return new Page("Dummy") { component = $"Missing File \"{name}\" \n" };
		}
        public static Page Analize(string name, string compon)
        {
            string[] comp = compon.Split('\n');
            Dictionary<string, string> data = new Dictionary<string, string>();
            string component = "";
            foreach (var item in comp)
            {
                if (item.StartsWith("@"))
                {
                    string key, val;
                    key = item.Substring(1).Split(':')[0];
                    val = item.Substring(key.Length + 2).TrimStart(' ');
                }
                else
                {
                    component += item + "\n";
                }
            }
            Page p = new Page(name);
            p.component = component;
            p.LayoutName = data.ContainsKey("layout") ? data["layout"] : "default.lt";
        }
    }
}
