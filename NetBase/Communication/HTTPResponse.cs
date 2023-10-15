using System;
using System.Collections.Generic;
using System.Text;

namespace NetBase.Communication
{
	public class HTTPResponse
	{
		public StatusCode Status;
		public ContentType contentType;
		public HTTPCookies Cookies;
		public Dictionary<string,string> Headers;
		public string Body;
		public HTTPResponse(StatusCode status, HTTPCookies cookies, string body = "", ContentType contenttype = ContentType.text_plain)
		{
			Status = status;
			Cookies = cookies;
			Headers = new Dictionary<string, string>();
			contentType = contenttype;
			Body = body;
		}

		public string ToString()
		{
			string ReasonPhrase = Enum.GetName(typeof(StatusCode), (int)Status).Replace("_", " ");
			string contType = Enum.GetName(typeof(ContentType), (int)contentType).Replace("_", "/");
			if (Body != "")
			{
				Headers.Add("Content-Type", contType);
				Headers.Add("Content-Length", Encoding.UTF8.GetByteCount(Body).ToString());
			}
			string Response =
				$"HTTP/1.1 {(int)Status} {ReasonPhrase}\r\n" +
				$"Cache-Control: no-cache\r\n";
			if (Cookies.ExportCookies().Length != 0)
			{
				foreach (var item in Cookies.ExportCookies()){Response += $"Set-Cookie: {item}\r\n";}
			}
			foreach (var item in Headers){Response += $"{item.Key}: {item.Value}\r\n";}

			if (Body != ""){Response += $"\r\n{Body}";}
			return Response;
		}
	}
}
