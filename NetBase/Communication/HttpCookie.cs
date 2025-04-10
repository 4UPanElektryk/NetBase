using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBase.Communication
{
	public class HttpCookie
	{
		public string Key;
		public string Value;
		public string Path;
		public HttpCookie(string key, string value, string path = "/")
		{
			Key = key;
			Value = value;
			Path = path;
		}
	}
}
