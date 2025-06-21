


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// The .inp shower defintion.
	/// <code>Relationships:
	/// - Has one SbemDhwGenerator
	/// </code>
	/// </summary>
	public class SbemShower : SbemObject
	{
		/// <summary>
		/// C# doesn't have late static binding so we need to add this to all SbemObject.
		/// </summary>
		public const string OBJECT_NAME  = "SHOWER";
		/// <summary>
		/// C# doesn't have late static binding so we need to add this to all SbemObject.
		/// </summary>
		public override string ObjectName() { return OBJECT_NAME; }
		public SbemShower(string currentName, List<string> currentProperties) : base(currentName, currentProperties)
		{

		}
	}
}
