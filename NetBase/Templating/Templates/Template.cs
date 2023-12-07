using NetBase.Templating.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
		public string Use(ReadOnlyDictionary<string,DataProvider> elements = null, Dictionary<string, string> provider = null)
		{
			Regex components = new Regex(@"\{\$(\w+).(\w+)\$\}", RegexOptions.Compiled);
			Regex data = new Regex(@"\$(\w+)\$", RegexOptions.Compiled);
			string ret = "";
			if (elements != null)
			{
                Console.WriteLine(components.Replace(component, match => test(match, elements)));
                ret += components.Replace(component, match => test(match, elements));
			}
			if (provider != null)
			{
				ret = data.Replace(ret, match => provider.ContainsKey(match.Groups[1].Value) ? provider[match.Groups[1].Value] : $"<!-- ?missing \"{match.Groups[1].Value}\" -->");
			}
			return ret;
		}
		private string test(Match match, ReadOnlyDictionary<string, DataProvider> elements)
		{
			string nmatch = match.Groups[1].Value + "." + match.Groups[2].Value;
			Console.WriteLine(nmatch);
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
