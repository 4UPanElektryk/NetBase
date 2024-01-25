using System;
using System.IO;

namespace NetBase.FileProvider
{
	public class LocalFileLoader : IFileLoader
	{
		readonly string directory;
		public LocalFileLoader(string directory) 
		{ 
			this.directory = directory.Replace('\\', Path.DirectorySeparatorChar);
		}
		public string Load(string path)
		{
#if DEBUG
			Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Reading: " + directory + path);
			Console.ResetColor();
#endif
			return File.ReadAllText(directory + path);
		}
		public string[] GetFiles()
		{
			string[] files = Directory.GetFiles(directory);
			for (int i = 0; i < files.Length; i++)
			{
				files[i] = files[i].Substring(directory.Length);
			}
			return files;
		}
	}
}
