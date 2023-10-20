using NetBase.FileProvider;

namespace NetBase.StaticRouting
{
	public struct Rout
	{
		public string LocalPath;
		public string ServerPath;
		public IFileLoader loader;
	}
}
