using NetBase.FileProvider;
using NetBase.Templating.Components;
using System.Collections.Generic;

namespace NetBase.Templating.Templates
{
	public class DocumentManager
	{
		private static List<Document> Templates = new List<Document>();
		public DocumentManager(IFileLoader loader) 
		{
			foreach (string item in loader.GetFiles())
			{
				if (item.EndsWith(".t.html"))
				{
					Templates.Add(new ImportableDocument(item, loader));
				}
			}
		}
		public static Document GetComponet(string name)
		{
			foreach (Document component in Templates)
			{
				if (component.AssetName == name)
				{
					return component;
				}
			}
			return new Document("Dummy") { component = $"Missing File \"{name}\" \n" };
		}
	}
}
