using System.IO;
using System.Text.RegularExpressions;
using NetBase.FileProvider;

namespace NetBase.Templating
{
	public class TComponent
	{
		private readonly string component;
		public readonly string AssetName;
		public TComponent(string path, IFileLoader loader)
		{
			AssetName = path;
			component = loader.Load(path);
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
