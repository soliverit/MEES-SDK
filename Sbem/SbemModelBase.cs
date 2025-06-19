using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public class SbemModelBase
	{
		public SbemModelBase() { }
		public List<SbemError> Errors = new List<SbemError>();
		public SbemGeneral General { get; set; }
		public void AddError(SbemError.ErrorCode code, string message)
		{
		Errors.Add(new SbemError(code, message));
		}

		public bool HasErrors() => Errors.Count > 0;

		public List<SbemError> GetErrors() => new(Errors);
	}
}
