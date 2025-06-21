using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// The base class for SbemNumericProperty and SbemStringProperty.
	/// </summary>
	public abstract class SbemPropertyBase
	{
		/// <summary>
		/// The property's name. E.g HEAT-SOURCE is a property of HVAC-SYSTEM (SbemHvacSystem)
		/// </summary>
		public string Name { get; protected set; }
		/// <summary>
		/// All properties have string names. Pass it to the base constructor.
		/// </summary>
		/// <param name="name"></param>
		public SbemPropertyBase(string name) { Name = name; }
	}
}
