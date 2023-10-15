using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBase.Communication
{
	public enum StatusCode
	{
		// 2xx Succes
		OK =							200,
		Created =						201,
		// 3xx Redirect
		Moved_Permanently =				301,
		Found =							302,
		Temporary_Redirect =			307,
		// 4xx Client Error
		Bad_Request =					400,
		Unauthorized =					401,
		Payment_Required =				402,
		Forbidden =						403,
		Not_Found =						404,
		Method_Not_Allowed =			405,
		Gone =							410,
		// 5xx Server Error
		Internal_Server_Error =			500,
		Not_Implemented =				501,
		Service_Unavailable =			502,
	}
}
