using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public class SbemWindowTmBridge : SbemObject
	{
		public const string OBJECT_NAME  = "WINDOW-TM-BRIDGE"; 
		public override string ObjectName() { return OBJECT_NAME; }
		public SbemWindowTmBridge(string name, List<string> propertiesString) : base(name, propertiesString)
		{

		}
	}
}
