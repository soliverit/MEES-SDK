


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public class SbemCompliance : SbemObject
	{
		public const string OBJECT_NAME  = "COMPLIANCE"; 
		public override string ObjectName() { return OBJECT_NAME; }
		public SbemCompliance(string currentName, List<string> currentProperties) : base(currentName, currentProperties)
		{

		}
	}
}
