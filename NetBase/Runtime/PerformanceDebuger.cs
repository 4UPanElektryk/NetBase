using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBase.Runtime
{
	public class PerformanceDebuger
	{
		private Dictionary<string, long> _storedData;
		private Stopwatch _stopwatch;
		private string _currentKey;
		public PerformanceDebuger()
		{
			_storedData = new Dictionary<string, long>();
		}
		public void Start(string key)
		{
			if (key != null) { throw new Exception("Cannot start a new performance debug session while another is running"); }
			_currentKey = key;
			_stopwatch = Stopwatch.StartNew();
		}
		public void Stop()
		{
			if (_currentKey == null) { throw new Exception("Cannot stop a performance debug session while none is running"); }
			_stopwatch.Stop();
			_storedData.Add(_currentKey, _stopwatch.ElapsedMilliseconds);
			_currentKey = null;
			_stopwatch = null;
		}
		public string ToHeader()
		{
			StringBuilder sb = new StringBuilder();
			foreach (var item in _storedData)
			{
				sb.Append(item.Key);
				sb.Append(";dur=");
				sb.Append(item.Value);
				sb.Append(",");
			}
			return sb.ToString().TrimEnd(',');
		}
	}
}
