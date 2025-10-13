using System;
using System.IO;

namespace NetBase.Runtime
{
	public class Log
	{
		private string _dir;
		private bool _writeToFile;
		public Log(string dir, bool writeToFile)
		{
			_dir = AppDomain.CurrentDomain.BaseDirectory + dir.Replace('\\', Path.DirectorySeparatorChar);
		}
		public void Write(string input)
		{
			if (!Directory.Exists(_dir))
			{
				Directory.CreateDirectory(_dir);
			}
			if (!File.Exists(_dir + "Latest.Log"))
			{
				File.WriteAllText(_dir + "Latest.Log", "");
			}

			string Time = DateTime.UtcNow.ToString("dd.MM.yyyy - HH:mm:ss:FFF");
			string ToWrite = $"{Time}|{input}\n";
			Console.WriteLine(ToWrite);

			File.AppendAllText(_dir + "Latest.Log", ToWrite);
		}
		public void Incident(Exception ex, string request = null)
		{
			string Time = DateTime.UtcNow.ToString("dd_MM_yyyy-HH_mm_ss_FFF");
			Console.WriteLine($"{Time}|Error:\n{ex}");
			File.WriteAllText($"{_dir}{Time}.log",
				$"{ex}\n\n{request}"
			);
		}
	}
}
