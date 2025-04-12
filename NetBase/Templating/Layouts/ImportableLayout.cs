using NetBase.FileProvider;
namespace NetBase.Templating.Layouts
{
	public class ImportableLayout : Layout
	{
		public ImportableLayout(string name, IFileLoader loader) : base(name)
		{
			Analize(loader.Load(name));
		}
	}
}
