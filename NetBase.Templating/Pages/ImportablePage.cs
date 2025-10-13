using NetBase.FileProvider;

namespace NetBase.Templating.Pages
{
	public class ImportablePage : Page
	{
		public ImportablePage(string name, IFileLoader loader) : base(name)
		{
			Analize(loader.Load(name));
		}
	}
}
