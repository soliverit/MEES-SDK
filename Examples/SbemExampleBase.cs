using MeesSDK.Sbem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Examples
{
	/// <summary>
	/// The abstract SBEM example. SBEM examples always require an SbemProject
	/// and SbemService. 
	/// </summary>
	public abstract  class SbemExampleBase : IMeesSDKExample
	{
		/// <summary>
		/// The SBEM service
		/// </summary>
		public SbemService SbemHandler { get; }
		/// <summary>
		/// The base project of the example.
		/// </summary>
		public SbemProject Project { get; }
		/// <summary>
		/// IMeesSDKExample: Get a string describing the example.
		/// </summary>
		/// <returns></returns>
		public abstract string GetDescription();
		/// <summary>
		/// IMeesSDKExample: Run the example.
		/// </summary>
		public abstract void RunTheExample();

		public SbemExampleBase(SbemProject project, SbemService service)
		{
			Project     = project;
			SbemHandler = service;
		}
	}
}
