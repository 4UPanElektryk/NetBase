using System;
using System.Collections.Generic;
using System.Text;

namespace NetBase.Communication
{
	public class HttpResponse
	{
		public StatusCode Status;
		public ContentType contentType;
		public HTTPCookies Cookies;
		public Dictionary<string,string> Headers;
		public string Body;
		public HttpResponse(StatusCode status, HTTPCookies cookies = null, string body = "", ContentType contenttype = ContentType.text_plain)
		{
			Status = status;
			if (cookies == null)
			{
				cookies = new HTTPCookies();
			}
			Cookies = cookies;
			Headers = new Dictionary<string, string>();
			contentType = contenttype;
			Body = body;
		}

		public override string ToString()
		{
			Dictionary<string,string> respheaders = Headers;
			string reasonPhrase = Enum.GetName(typeof(StatusCode), (int)Status).Replace("_", " ");
			string contType = Enum.GetName(typeof(ContentType), (int)contentType).Replace("_", "/");
            if (contType.Contains("text"))
			{
				respheaders.Add("Content-Type", $"{contType}; charset=UTF-8");
			}
			else
			{
				respheaders.Add("Content-Type", contType);
			}
			respheaders.Add("Cache-Control", "no-cache");
			respheaders.Add("Content-Length", Encoding.UTF8.GetByteCount(Body).ToString());
			string response = $"HTTP/1.1 {(int)Status} {reasonPhrase}\r\n";
			foreach (var item in respheaders) {response += $"{item.Key}: {item.Value}\r\n";}
			if (Cookies.ExportCookies().Length != 0)
			{
				foreach (var item in Cookies.ExportCookies()){response += $"Set-Cookie: {item}; Path=/\r\n";}
			}
			response += $"\r\n{Body}\r\n";
			return response;
		}
	}
}
