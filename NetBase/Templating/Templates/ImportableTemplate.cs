using NetBase.FileProvider;

namespace NetBase.Templating.Templates
{
	public class ImportableTemplate : Template
	{
		public ImportableTemplate(string name, IFileLoader loader) : base(name)
		{
			this.component = loader.Load(name);
		}
	}
}
