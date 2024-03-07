using System.Collections.Generic;
using System.Text.RegularExpressions;
using NetBase.Templating.Components;

namespace NetBase.Templating.Templates
{
	public class Page
	{
		public string component;
		public readonly string AssetName;
        public string LayoutName { get { return PageData["Layout"]; } }
        public Dictionary<string, string> PageData;
		public Page(string name)
		{
			AssetName = name;
		}
		// Component would be with {$na.me$}
		public string Use(Dictionary<string,DataProvider> elements = null, Dictionary<string, string> provider = null)
		{
			Regex components = new Regex(@"\{\$(\w+.\w+)\$\}", RegexOptions.Compiled);
			Regex data = new Regex(@"\$(\w+)\$", RegexOptions.Compiled);
			string ret = "";
			if (elements != null)
			{
                ret += components.Replace(component, match => Test(match, elements));
			}
			else
			{
				ret = component;
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
				if (elements.ContainsKey(nmatch))
				{
					return TComponentManager.GetComponet(nmatch).Use(elements[nmatch]);
				}
				else
				{
					return TComponentManager.GetComponet(nmatch).Use(null);
				}
			}
			else
			{
				return $"<!-- ?missing \"{nmatch}\" -->";
			}
		}
	}
}
