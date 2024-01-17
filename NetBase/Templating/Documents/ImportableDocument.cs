using NetBase.FileProvider;

namespace NetBase.Templating.Templates
{
	public class ImportableDocument : Document
	{
		public ImportableDocument(string name, IFileLoader loader) : base(name)
		{
			this.component = loader.Load(name);
		}
	}
}
