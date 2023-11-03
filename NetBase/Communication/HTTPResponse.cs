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
		public HTTPResponse(StatusCode status, HTTPCookies cookies = null, string body = "", ContentType contenttype = ContentType.text_plain)
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
			string ReasonPhrase = Enum.GetName(typeof(StatusCode), (int)Status).Replace("_", " ");
			string contType = Enum.GetName(typeof(ContentType), (int)contentType).Replace("_", "/");
            Console.WriteLine(contType);
            if (Body != "")
			{
				if (contType.Contains("text"))
				{
					Headers.Add("Content-Type", contType + "; charset=utf-8");
				}
				else
				{
					Headers.Add("Content-Type", contType + "");
				}
				Headers.Add("Content-Length", Encoding.UTF8.GetByteCount(Body).ToString());
			}
			Headers.Add("Cache-Control", "no-cache");
			string Response = $"HTTP/1.1 {(int)Status} {ReasonPhrase}\r\n";
			foreach (var item in Headers){Response += $"{item.Key}: {item.Value}\r\n";}
			if (Cookies.ExportCookies().Length != 0)
			{
				foreach (var item in Cookies.ExportCookies()){Response += $"Set-Cookie: {item}\r\n";}
			}

			if (Body != ""){Response += $"\r\n{Body}";}
			return Response;
		}
	}
}
