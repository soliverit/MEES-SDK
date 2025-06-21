

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// The .inp solar energy system (solar hot water) definition.
	/// </summary>
	public class SbemSes : SbemObject
	{
		/// <summary>
		/// C# doesn't have late static binding so we need to add this to all SbemObject.
		/// </summary>
		public const string OBJECT_NAME  = "SES";
		/// <summary>
		/// C# doesn't have late static binding so we need to add this to all SbemObject.
		/// </summary>
		public override string ObjectName() { return OBJECT_NAME; }
		/// <summary>
		/// The default constructor
		/// </summary>
		/// <param name="currentName"></param>
		/// <param name="currentProperties"></param>
		public SbemSes(string currentName, List<string> currentProperties) : base(currentName, currentProperties) {}
	}
}
