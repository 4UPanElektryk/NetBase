using NetBase.FileProvider;
using NetBase.Templating.Components;
using System.Collections.Generic;

namespace NetBase.Templating.Templates
{
	public class TemplateManager
	{
		private static List<Template> Templates = new List<Template>();
		public TemplateManager(IFileLoader loader) 
		{
			foreach (string item in loader.GetFiles())
			{
				if (item.EndsWith(".t.html"))
				{
					Templates.Add(new ImportableTemplate(item, loader));
				}
			}
		}
		public static Template GetComponet(string name)
		{
			foreach (Template component in Templates)
			{
				if (component.AssetName == name)
				{
					return component;
				}
			}
			return new Template("Dummy") { component = $"Missing File \"{name}\" \n" };
		}
	}
}
