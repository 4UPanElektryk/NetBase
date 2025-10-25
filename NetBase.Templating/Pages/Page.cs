using NetBase.StaticRouting;
using NetBase.Templating.Components;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NetBase.Templating.Pages
{
	public class Page : TemplateAsset
	{
		public string Component { get; set; }
		public string AssetName { get; set; }
		public string LayoutName { get { return PageData.ContainsKey("Layout") ? PageData["Layout"] : "Default.lt"; } }
		public Dictionary<string, string> PageData;
		public Page(string name)
		{
			PageData = new Dictionary<string, string>();
			AssetName = name;
		}
		// Component would be with {$name.comp$}
		public string Use(Dictionary<string, DataProvider> elements = null, Dictionary<string, string> provider = null)
		{
			Regex components = new Regex(@"\{\$(\w+.\w+)\$\}", RegexOptions.Compiled);
			Regex data = new Regex(@"\$(\w+)\$", RegexOptions.Compiled);
			string ret = "";
			if (elements != null)
			{
				ret += components.Replace(Component, match => Test(match, elements));
			}
			else
			{
				ret = Component;
			}
			if (provider != null)
			{
				ret = data.Replace(ret, match => provider.ContainsKey(match.Groups[1].Value) ? provider[match.Groups[1].Value] : $"<!-- ?missing \"{match.Groups[1].Value}\" -->");
			}
			return ret;
		}
		private string Test(Match match, Dictionary<string, DataProvider> elements)
		{
			string nmatch = match.Groups[1].Value;
			if (TComponentManager.GetComponet(nmatch) != null)
			{
				return TComponentManager.GetComponet(nmatch).Use(elements.ContainsKey(nmatch) ? elements[nmatch] : null);
			}
			return $"<!-- ?missing \"{nmatch}\" -->";
		}
		protected void Analize(string component)
		{
			string[] comp = component.Split('\n');
			Dictionary<string, string> data = new Dictionary<string, string>();
			string newcomponent = "";
			foreach (var item in comp)
			{
				if (item.StartsWith("@"))
				{
					string key, val;
					key = item.Substring(1).Split(':')[0];
					val = item.Substring(key.Length + 2).Trim();
					data.Add(key, val);
				}
				else
				{
					newcomponent += item + "\n";
				}
			}
			Layouts.LayoutManager.Add(this);
			this.PageData = data;
			this.Component = newcomponent;
			if (PageData.ContainsKey("AutoRout"))
			{
				Router.PagesRoutingTable.Add(PageData["AutoRout"], this.AssetName);
			}
		}
	}
}
