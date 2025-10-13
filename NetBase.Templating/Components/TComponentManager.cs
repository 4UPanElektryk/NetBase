using NetBase.FileProvider;
using System.Collections.Generic;

namespace NetBase.Templating.Components
{
	public class TComponentManager
	{
		private static List<TComponent> Components = new List<TComponent>();
		public TComponentManager(IFileLoader loader)
		{
			foreach (string item in loader.GetFiles())
			{
				if (item.EndsWith(".comp"))
				{
					Components.Add(new ImportableComponent(item, loader));
				}
			}
		}
		public static TComponent GetComponet(string name)
		{
			foreach (TComponent component in Components)
			{
				if (component.AssetName == name)
				{
					return component;
				}
			}
			return new TComponent("Dummy") { component = $"Missing File \"{name}\" \n" };
		}
	}
}
