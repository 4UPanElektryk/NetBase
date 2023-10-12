using NetBase.FileProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBase.Templating
{
	public class TComponentManager
	{
		private List<TComponent> Components = new List<TComponent>();
		public TComponentManager(IFileLoader loader) 
		{
			foreach (string item in loader.GetFiles())
			{
				if (item.EndsWith(".comp"))
				{
					Components.Add(new TComponent(item,loader));
				}
			}
		}
		public TComponent GetComponet(string name)
		{
			foreach(TComponent component in Components) 
			{
				if (component.AssetName == name)
				{
					return component;
				}
			}
			return new TComponent("Dummy");
		}
	}
}
