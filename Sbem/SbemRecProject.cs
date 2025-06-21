


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// The _epc.inp summary of the building performance.Poor, Fair, Good ratings for 
	/// consumers. Each has ratings for: notional Energy use and CO2 emissions, those
	/// adjusted for seasonal and system losses
	/// </summary>
	public class SbemRecProject : SbemObject
	{
		/// <summary>
		/// C# doesn't have late static binding, so we need to add this line to all SbemObject.
		/// </summary>
		public const string OBJECT_NAME = "REC-PROJECT";
		/// <summary>
		/// C# doesn't have late static binding, so we need to add this method to all SbemObject
		/// </summary>
		/// <returns></returns>
		public override string ObjectName() { return OBJECT_NAME; }
		public SbemRecProject(string currentName, List<string> currentProperties) : base(currentName, currentProperties) { }
	}
}
