/*using SingularFS;
using System;
using System.Collections.Generic;

namespace NetBase.FileProvider
{
	public class SingularFSFileLoader : IFileLoader
	{
		private readonly FS fs;
		public SingularFSFileLoader(string path)
		{
			fs = FSMod.Import(path);
		}
		public string Load(string path)
		{
#if DEBUG
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("Reading: " + path);
			Console.ResetColor();
#endif
			return fs.ReadAllText(path);
		}
		public string[] GetFiles()
		{
			List<string> files = new List<string>();
			fs.files.ForEach(f => { files.Add(f.FileName); });
			return files.ToArray();
		}

	}
}*/
