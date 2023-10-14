using System.Collections.Generic;
using SingularFS;

namespace NetBase.FileProvider
{
	public class SingularFSFileLoader : IFileLoader
	{
		private readonly FS fs;
		public SingularFSFileLoader(string path) 
		{ 
			fs = FSMod.Import(path);
		}

		public string[] GetFiles()
		{
			List<string> files = new List<string>();
			fs.files.ForEach(f => { files.Add(f.FileName); });
			return files.ToArray(); 
		}

		public string Load(string path)
		{
			return fs.ReadAllText(path);
		}
	}
}
