using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.ML
{
	public interface IValidatable
	{
		public bool HasError { get; }
	}
}
