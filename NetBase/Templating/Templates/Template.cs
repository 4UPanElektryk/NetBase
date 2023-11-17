using NetBase.Templating.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NetBase.Templating.Templates
{
	public class Template
	{
		public string component;
		public readonly string AssetName;
		public Template(string name)
		{
			AssetName = name;
		}
		// Component would be with {$name$}
		public string Use(ReadOnlyDictionary<string,DataProvider> elements = null, DataProvider provider = null)
		{
			Regex components = new Regex(@"\{\$(\w+)\$\}", RegexOptions.Compiled);
			Regex data = new Regex(@"\$(\w+)\$", RegexOptions.Compiled);
			string ret = "";
			if (elements != null)
			{
				foreach (var item in elements)
				{
					ret += components.Replace(component, match => TComponentManager.GetComponet(item.Key).Use(item.Value));
				}
			}
			provider?.ForEach(r => {
					ret += data.Replace(component, match => r.ContainsKey(match.Groups[1].Value) ? r[match.Groups[1].Value] : $"<!-- ?missing \"{match.Groups[1].Value}\" -->");
				});
			return ret;
		}
	}
}
