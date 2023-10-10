using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBase.Communication
{
	public class HTTPCookies
	{
		public Dictionary<string, string> Cookies;
		public HTTPCookies() 
		{ 
			Cookies = new Dictionary<string, string>();
		}
		public void ImportCookies(string data)
		{

		}
		public string ExportCoookies()
		{
			
		}
	}
}
