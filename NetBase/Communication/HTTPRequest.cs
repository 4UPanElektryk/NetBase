using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NetBase.Communication
{
	public class HTTPRequest
	{
		public string Url;
		public Dictionary<string, string> URLParamenters;
		public string HTTPVersion;
		public HTTPMethod Method;
		public Dictionary<string,string> Headers;
		public HTTPCookies Cookies;
		public string body;
		public HTTPRequest()
		{
			URLParamenters = new Dictionary<string, string>();
			Headers = new Dictionary<string, string>();
			Cookies = new HTTPCookies();
		}
		public static HTTPRequest Parse(string data)
		{
			string[] lines = data.Split("\r\n".ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
			string path = lines[0].Split(' ')[1];
			HTTPRequest request = new HTTPRequest
			{
				Method = (HTTPMethod)Enum.Parse(typeof(HTTPMethod), lines[0].Split(' ')[0]),
				HTTPVersion = lines[0].Split(' ')[2]
			};
			if (path.Contains("?"))
			{
				request.Url = path.Split('?')[0].TrimStart('/');
				if (path.Split('?')[1].Contains("&"))
				{
					foreach (var item in path.Split('?')[1].Split('&'))
					{
						request.URLParamenters.Add(
							item.Split('=')[0], 
							item.Split('=')[1]
						);
					}
				}
				else
				{
					string item = path.Split('?')[1];
					request.URLParamenters.Add(
						item.Split('=')[0],
						item.Split('=')[1]
					);
				}
			}
			else
			{
				request.Url = path.TrimStart('/');
			}
			for (int i = 1; i < lines.Count() - 1; i++)
			{
				if (lines[i].Split(':')[0] == "Cookie")
				{
					request.Cookies.ImportCookies(lines[i].Split(':')[1].TrimStart(' '));
				}
				else
				{
					request.Headers.Add(
						lines[i].Split(':')[0],
						lines[i].Split(':')[1].TrimStart(' ')
					);
				}
			}
			request.body = lines.Last();
			return request;
		}
	}
}
