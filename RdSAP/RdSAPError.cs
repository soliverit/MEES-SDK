using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.RdSAP
{
	public class RdSAPError
	{
		public string Code { get;  }
		public string Message { get; }
		public RdSAPError(string code, string message)
		{
			Code = code;
			Message = message;
		}
		
	}
}
