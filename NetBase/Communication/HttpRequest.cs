using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
namespace NetBase.Communication
{
	public class HttpRequest
	{
		public string Url;
		public Dictionary<string, string> URLParamenters;
		public string HTTPVersion;
		public HttpMethod Method;
		public Dictionary<string,string> Headers;
		public Dictionary<string,string> PostData;
		public HttpCookies Cookies;
		public string body;
		public HttpRequest()
		{
			URLParamenters = new Dictionary<string, string>();
			Headers = new Dictionary<string, string>();
			PostData = new Dictionary<string, string>();
			Cookies = new HttpCookies();
		}
		public static HttpRequest Parse(HttpListenerRequest data)
		{
			/*string[] lines = data.Split("\r\n".ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
			string path = lines[0].Split(' ')[1];
			HttpRequest request = new HttpRequest
			{
				Method = (HttpMethod)Enum.Parse(typeof(HttpMethod), lines[0].Split(' ')[0]),
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
							Uri.UnescapeDataString(item.Split('=')[1])
						);
					}
				}
				else
				{
					string item = path.Split('?')[1];
					request.URLParamenters.Add(
						item.Split('=')[0],
						Uri.UnescapeDataString(item.Split('=')[1])
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
			if (request.Headers.ContainsKey("Content-Length"))
			{
				request.body = data.Substring(data.Length - int.Parse(request.Headers["Content-Length"]), int.Parse(request.Headers["Content-Length"]));
			}
			if (request.Method == HttpMethod.POST)
			{
				foreach (var item in request.body.Split('&'))
				{
					request.PostData.Add(
						item.Split('=')[0],
						Uri.UnescapeDataString(item.Substring(item.Split('=')[0].Length + 1).Replace('+',' '))
					);
				}
			}*/
			HttpRequest request = new HttpRequest();
			request.Url = data.Url.AbsolutePath.TrimStart('/');
			request.Method = (HttpMethod)Enum.Parse(typeof(HttpMethod), data.HttpMethod);
			request.HTTPVersion = data.ProtocolVersion.ToString();
			request.Headers = data.Headers.AllKeys.ToDictionary(
				x => x,
				x => data.Headers[x]
			);
			request.URLParamenters = new Dictionary<string, string>();
			if (data.Url.Query != "")
			{
				if (data.Url.Query.Contains("&"))
				{
					foreach (var item in data.Url.Query.Split('&'))
					{
						request.URLParamenters.Add(
							item.Split('=')[0],
							Uri.UnescapeDataString(item.Split('=')[1])
						);
					}
				}
				else
				{
					string item = data.Url.Query;
					request.URLParamenters.Add(
						item.Split('=')[0],
						Uri.UnescapeDataString(item.Split('=')[1])
					);
				}
			}
			request.Cookies = new HttpCookies();
			request.Cookies.ImportCookies(data.Cookies);
			if (request.Method == HttpMethod.POST)
			{
				request.PostData = GetRequestPostData(data)
					.Split('&')
					.ToDictionary(
						x => x.Split('=')[0],
						x => Uri.UnescapeDataString(x.Substring(x.Split('=')[0].Length + 1).Replace('+', ' '))
					);
			}
			else
			{
				request.PostData = new Dictionary<string, string>();
			}
			request.body = GetRequestPostData(data);
			return request;
		}
		private static string GetRequestPostData(HttpListenerRequest request)
		{
			if (!request.HasEntityBody)
			{
				return null;
			}
			using (System.IO.Stream body = request.InputStream) // here we have data
			{
				using (var reader = new System.IO.StreamReader(body, request.ContentEncoding))
				{
					return reader.ReadToEnd();
				}
			}
		}
	}
}
