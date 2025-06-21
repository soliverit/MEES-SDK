using MeesSDK.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// The base class for .inp files - SbemModel and SbemEpcModel
	/// </summary>
	public class SbemModelBase : IValidatable
	{
		public SbemModelBase() { }
		/// <summary>
		/// List of errors associated with the building
		/// </summary>
		public List<SbemError> Errors = new List<SbemError>();
		public SbemGeneral General { get; set; }
		public void AddError(SbemError.ErrorCode code, string message)
		{
			Errors.Add(new SbemError(code, message));
		}
		/// <summary>
		/// Is there at least one SbemError in this model? From IValidatable
		/// </summary>
		public bool HasError { get => Errors.Any(); }
	}
}
