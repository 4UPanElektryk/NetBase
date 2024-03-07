using NetBase.FileProvider;
using System.Collections.Generic;

namespace NetBase.Templating.Templates
{
	public class ImportablePage : Page
	{
		public ImportablePage(string name, IFileLoader loader) : base(name)
		{
			PageManager.Analize(loader.Load(name));
		}
    }
}
