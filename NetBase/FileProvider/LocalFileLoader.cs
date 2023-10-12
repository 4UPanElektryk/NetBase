using System.IO;

namespace NetBase.FileProvider
{
	public class LocalFileLoader : IFileLoader
	{
		readonly string directory;
		public LocalFileLoader(string directory) 
		{ 
			this.directory = directory;
		}
		public string Load(string path)
		{
			return File.ReadAllText(directory + path);
		}
		public string[] GetFiles()
		{
			return Directory.GetFiles(directory);
		}
	}
}
