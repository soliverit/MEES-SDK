using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// The .inp window thermal bridge definition.
	/// </summary>
	public class SbemWindowTmBridge : SbemObject
	{
		/// <summary>
		/// C# doesn't have late static binding so we need to add this to all SbemObject.
		/// </summary>
		public const string OBJECT_NAME  = "WINDOW-TM-BRIDGE";
		/// <summary>
		/// C# doesn't have late static binding so we need to add this to all SbemObject.
		/// </summary>
		public override string ObjectName() { return OBJECT_NAME; }
		public SbemWindowTmBridge(string name, List<string> propertiesString) : base(name, propertiesString)
		{

		}
	}
}
