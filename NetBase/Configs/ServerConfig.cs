using System.Collections.Generic;
using System.Net;

namespace NetBase.Configs
{
	public class ServerConfig
	{
		/// <summary>
		/// key is the path on the website
		/// value is the path on the server
		/// </summary>
		public Dictionary<string, string> Router = new Dictionary<string, string>();
		public IPAddress address;
		public int port;
	}
}
