using NetBase.Communication;
using NetBase.FileProvider;
using System;

namespace NetBase.StaticRouting
{
	public struct RouterEntry
	{
		public string LocalPath;
		public string ServerPath;
		public IFileLoader loader;
		public Func<HttpRequest, bool> OverrideCase;
	}
}
