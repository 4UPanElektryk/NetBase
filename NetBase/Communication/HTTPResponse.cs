using System;
using System.Collections.Generic;

namespace NetBase.Communication
{
	public class HTTPResponse
	{
		public StatusCode Status;
		public ContentType contentType;
		public HTTPCookies Cookies;
		public Dictionary<string,string> Headers;
		public string Body;
		public HTTPResponse(StatusCode status, HTTPCookies cookies, Dictionary<string, string> headers, string body)
		{
			Status = status;
			Cookies = cookies;
			Headers = headers;
			Body = body;
		}

		public string ToString()
		{
			string ReasonPhrase = Enum.GetName(typeof(StatusCode), (int)Status).Replace("_", " ");
			string contType = Enum.GetName(typeof(ContentType), (int)contentType).Replace("_", "/");
			string Response =
				$"HTTP/1.1 {(int)Status} {ReasonPhrase}\r\n" +
				$"Cache-Control: no-cache\r\n" +
				$"Content-Type: {contType}\r\n";
			return "";
		}
	}
}
