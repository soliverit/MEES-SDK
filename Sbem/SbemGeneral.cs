


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public class SbemGeneral : SbemObject
	{
		public const string OBJECT_NAME  = "GENERAL"; 
		public override string ObjectName() { return OBJECT_NAME; }
		public SbemGeneral(string currentName, List<string> currentProperties) : base(currentName, currentProperties)
		{

		}
	}
}
