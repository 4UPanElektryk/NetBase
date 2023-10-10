using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetBase.Communication
{
	public class HTTPRequest
	{
		public string HTTPVersion;
		public HTTPAction HTTPAction;
		public Dictionary<string,string> Cookies;
		public List<string> OtherHeaders;
	}
}
