using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public struct SbemErrorFileRecord
	{
		public int LineNumber { get; set; }
		public string ErrorType { get; set; }
		public string Key { get; set; }
		public string Value { get; set; }
	}
}
