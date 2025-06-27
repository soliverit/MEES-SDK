using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem.Exceptions
{
	public class MixedConstructionException : SbemExceptionBase
	{
		public MixedConstructionException(string message) : base(message) { }
	}
}
