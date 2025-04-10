using System.Collections.Generic;
using System.Net;
namespace NetBase.Communication
{
	public class HttpCookies
	{
		private List<HttpCookie> Cookies;
		//private Dictionary<string, string> Cookies;
		public HttpCookie this[string name] 
		{ 
			get { return Get(name); } 
			set { Set(name, value.Value); } 
		}
		public HttpCookies() 
		{ 
			Cookies = new List<HttpCookie>();
		}
		public HttpCookie Get(string key)
		{
			return Cookies.Find((x) => x.Key == key);
		}
		public string GetValue(string key)
		{
			HttpCookie c = Cookies.Find((x) => x.Key == key);
			if (c != null)
			{
				return c.Value;
			}
			return "";
		}
		public void Set(string key, string value, string path = "/")
		{
			HttpCookie c = Cookies.Find((x) => x.Key == key);
			if (c != null)
			{
				Cookies.Remove(c);
			}
			else
			{
				c = new HttpCookie(key, value, path);
			}
			c.Value = value;
			Cookies.Add(c);
		}
		public void ImportCookies(CookieCollection collection)
		{
			foreach (Cookie c in collection)
			{
				Cookies.Add(new HttpCookie(c.Name, c.Value, c.Path));
			}
		}
		public void ImportCookies(string data)
		{
			string[] sd = data.Contains("&") ? data.Split('&'): new string[] { data };
			foreach (string s in sd) 
			{
				string[] d = s.Split('=');

				Cookies.Add(new HttpCookie(d[0], d[1]));
			}
		}
		public CookieCollection ExportCookies()
		{
			CookieCollection cookies = new CookieCollection();
			foreach (var item in Cookies)
			{
				cookies.Add(new Cookie(item.Key, item.Value, item.Path));
			}
			return cookies;
		}
	}
}
