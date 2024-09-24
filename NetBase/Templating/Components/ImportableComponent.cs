using NetBase.FileProvider;

namespace NetBase.Templating.Components
{
	public class ImportableComponent : TComponent
	{
		public ImportableComponent(string name, IFileLoader loader) : base(name)
		{
			this.component  = loader.Load(name);
		}
	}
}
