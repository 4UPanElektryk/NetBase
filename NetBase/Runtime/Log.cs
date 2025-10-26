using System;
using System.IO;

namespace NetBase.Runtime
{
	public class Log
	{
		private string _dir;
		public string _prefix;
		private bool _writeToFile;
		public Log(string dir = null)
		{
			_writeToFile = dir != null;
			if (_writeToFile)
			{
				_dir = AppDomain.CurrentDomain.BaseDirectory + dir.Replace('\\', Path.DirectorySeparatorChar);
			}
		}
		public void Write(string input)
		{
			string Time = DateTime.UtcNow.ToString("dd.MM.yyyy - HH:mm:ss:FFF");
			string ToWrite = $"{Time}| {_prefix} |{input}\n";
			Console.WriteLine(ToWrite);

			if (!_writeToFile) { return; }

			if (!Directory.Exists(_dir))
			{
				Directory.CreateDirectory(_dir);
			}
			if (!File.Exists(_dir + "Latest.Log"))
			{
				File.WriteAllText(_dir + "Latest.Log", "");
			}
			File.AppendAllText(_dir + "Latest.Log", ToWrite);
		}
		public void Incident(Exception ex, string request = null)
		{
			string Time = DateTime.UtcNow.ToString("dd_MM_yyyy-HH_mm_ss_FFF");
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Error.WriteLine($"{Time}| {_prefix} |Error:\n{ex}");
			Console.ResetColor();
			if (_writeToFile)
			{
				File.WriteAllText($"{_dir}{Time}.log",
					$"{ex}\n\n{request}"
				);
			}
		}
	}
}
