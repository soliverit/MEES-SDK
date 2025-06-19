using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public class SbemHvacSystemData : SbemObject
	{
		public SbemHvacSystemData(string name, List<string> currentProperties) : base(name, currentProperties) { }
		public const string OBJECT_NAME = "HVAC-SYSTEM-DATA";
		public override string ObjectName() { return OBJECT_NAME; }

	}
}
