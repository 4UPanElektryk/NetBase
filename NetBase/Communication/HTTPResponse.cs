using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetBase.Communication
{
	public class HTTPResponse
	{
		public string ReturnCode;
		public string ReturnMessage;
		public HTTPCookie[] ToSend;
	}
}
