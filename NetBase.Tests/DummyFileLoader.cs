using NetBase.FileProvider;
using System.Collections.Generic;
using System.Linq;

namespace NetBase.Tests
{
	internal class DummyFileLoader : IFileLoader
	{
		Dictionary<string,string> Files;
		public DummyFileLoader(Dictionary<string,string> files) { Files = files; }
		public string[] GetFiles()
		{
			return Files.Keys.ToArray();
		}

		public string Load(string path)
		{
			return Files[path];
		}
	}
}
