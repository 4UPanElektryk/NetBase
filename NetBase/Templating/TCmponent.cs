using System.IO;

namespace NetBase.Templating
{
	public class TComponent
	{
		private string component;
		public TComponent(string path)
		{
			component = File.ReadAllText(path);
		}
		public void 
	}
}
