


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public class SbemShower : SbemObject
	{
		public const string OBJECT_NAME  = "SHOWER"; 
		public override string ObjectName() { return OBJECT_NAME; }
		public SbemShower(string currentName, List<string> currentProperties) : base(currentName, currentProperties)
		{

		}
	}
}
