using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBase.Templating
{
	public class DataProvider
	{
		public TComponent Template = null;
		public List<Dictionary<string,string>> Data = new List<Dictionary<string, string>>();
		public string ForEach(Action<Dictionary<string,string>> Components)
		{
			return Template;
		}
	}
}
