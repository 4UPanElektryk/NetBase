using System;
using System.IO;

namespace NetBase.RuntimeLogger
{
	public class Log
	{
		private static string Dir;
		public Log(string dir)
		{
			Dir = AppDomain.CurrentDomain.BaseDirectory + dir;
		}
		public static void Write(string input)
		{
			if (!Directory.Exists(Dir))
			{
				Directory.CreateDirectory(Dir);
			}
			if (!File.Exists(Dir + "Latest.Log"))
			{
				File.WriteAllText(Dir + "Latest.Log","");
			}
			string Time = DateTime.UtcNow.ToString("dd.MM.yyyy - HH:mm:ss:FFF");
			string ToWrite = $"{Time}|{input}\n";
			Console.WriteLine(ToWrite);
			string read = File.ReadAllText(Dir + "Latest.Log");
			File.WriteAllText(Dir + "Latest.Log",
				read +
				ToWrite
			);
		}
		public static void Incident(Exception ex)
		{
			string Time = DateTime.UtcNow.ToString("dd_MM_yyyy-HH_mm_ss_FFF") + ".log";
			Console.WriteLine($"{Time}|Error:\n{ex}");
			File.WriteAllText($"{Dir}{Time}.log",
				ex.ToString()
			);
		}
	}
}
