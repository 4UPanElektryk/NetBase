﻿using System.Collections.Generic;
namespace NetBase.Communication
{
	public class HttpCookies
	{
		private Dictionary<string, string> Cookies;
		public string this[string name] 
		{ 
			get { return Get(name); } 
			set { Set(name, value); } 
		}
		public HttpCookies() 
		{ 
			Cookies = new Dictionary<string, string>();
		}
		public string Get(string key)
		{
			if (!Cookies.ContainsKey(key)) { return null; }
			return Cookies[key];
		}
		public void Set(string key, string value)
		{
			if (Cookies.ContainsKey(key))
			{
				Cookies.Remove(key);
			}
			Cookies.Add(key,value);
		}
		public void ImportCookies(string data)
		{
			string[] sd = data.Contains("&") ? data.Split('&'): new string[] { data };
			foreach (string s in sd) 
			{
				string[] d = s.Split('=');
				Cookies.Add(d[0], d[1]);
			}
		}
		public string[] ExportCookies()
		{
			string[] strings = new string[Cookies.Count];
			int i = 0;
			foreach (var item in Cookies)
			{
				strings[i++] = item.Key + "=" + item.Value;
			}
			return strings;
		}
	}
}
