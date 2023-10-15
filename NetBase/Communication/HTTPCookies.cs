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
			string[] sd = data.Split('&');
			foreach (string s in sd) 
			{
				string[] d = s.Split('=');
				Cookies.Add(d[0], d[1]);
			}
		}
		public string[] ExportCookies()
		{
			List<string> output = new List<string>();
			foreach (var item in Cookies)
			{
				output.Add($"{item.Key}={item.Value}");
			}
			return output.ToArray();
		}
	}
}
