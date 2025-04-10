using System;
using System.Collections.Generic;
using System.Text;

namespace NetBase.Communication
{
	public class HttpResponse
	{
		public StatusCode Status;
		public string contentType;
		public HttpCookies Cookies;
		public Dictionary<string, string> Headers;
		public Encoding ContentEncoding = null;
		public byte[] Content;
		public string Body { set { Content = ContentEncoding.GetBytes(value); } }
		public HttpResponse(StatusCode status, string body, HttpCookies cookies = null, Encoding encoding = null, ContentType contenttype = ContentType.text_plain)
		{
			Status = status;
			if (cookies == null)
			{
				cookies = new HttpCookies();
			}
			Cookies = cookies;
			Headers = new Dictionary<string, string>();
			contentType = Enum.GetName(typeof(ContentType), (int)contenttype).Replace("_", "/"); ;
			if (encoding == null && body != null)
			{
				encoding = Encoding.UTF8;
			}
			Content = encoding.GetBytes(body);
			ContentEncoding = encoding;
		}
		public HttpResponse(StatusCode status, byte[] content, HttpCookies cookies = null, string contenttype = "", Encoding encoding = null)
		{ 
			Status = status;
			if (cookies == null)
			{
				cookies = new HttpCookies();
			}
			Cookies = cookies;
			Headers = new Dictionary<string, string>();
			contentType = contenttype;
		}
		public HttpResponse(StatusCode status, HttpCookies cookies = null)
		{
			Status = status;
			if (cookies == null)
			{
				cookies = new HttpCookies();
			}
			Cookies = cookies;
			Headers = new Dictionary<string, string>();
			Content = null;
			ContentEncoding = null;
		}
	}
}
