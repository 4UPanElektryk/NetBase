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
			Regex re = new Regex(@"\{\$(\w+)\$\}", RegexOptions.Compiled);
			string ret = "";
			foreach (var item in elements)
			{
				ret += re.Replace(component, match => TComponentManager.GetComponet(item.Key).Use(item.Value));
			}
			Regex reg = new Regex(@"\$(\w+)\$", RegexOptions.Compiled);
			provider.ForEach(r => {
				ret += reg.Replace(component, match => r.ContainsKey(match.Groups[1].Value) ? r[match.Groups[1].Value] : $"<!-- ?missing \"{match.Groups[1].Value}\" -->");
			});
			return ret;
		}
	}
}
