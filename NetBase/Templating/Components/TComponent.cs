using System.Text.RegularExpressions;

namespace NetBase.Templating.Components
{
	public class TComponent
	{
		public string component;
		public readonly string AssetName;
		public TComponent(string name)
		{
			AssetName = name;
		}
		// Component would be with $name$
		public string Use(DataProvider provider)
		{
			Regex re = new Regex(@"\$(\w+)\$", RegexOptions.Compiled);
			string ret = "";
			provider.ForEach(r => { 
				ret += re.Replace(component, match => r[match.Groups[1].Value]); 
			});
			return ret;
		}
	}
}
