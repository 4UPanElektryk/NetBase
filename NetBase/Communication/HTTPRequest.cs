using System.Collections.Generic;

namespace NetBase.Communication
{
	public struct HTTPRequest
	{
		public string Url;
		public Dictionary<string, string> URLParamenters;
		public string HTTPVersion;
		public HTTPMethod Method;
		public Dictionary<string,string> Headers;
		public HTTPCookies Cookies;
		public string body;
	}
}
