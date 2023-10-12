using System;
using System.Collections.Generic;

namespace NetBase.Templating
{
	public class DataProvider
	{
		public List<Dictionary<string,string>> Data;
		public DataProvider()
		{
			Data = new List<Dictionary<string, string>>();
		}
		public void ForEach(Action<Dictionary<string,string>> Components)
		{
			foreach (Dictionary<string, string> item in Data)
			{
				Components(item);
			}
		}
	}
}
