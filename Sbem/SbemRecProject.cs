


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public class SbemRecProject : SbemObject
	{
		public const string OBJECT_NAME  = "REC-PROJECT"; 
		public override string ObjectName() { return OBJECT_NAME; }
		public SbemRecProject(string currentName, List<string> currentProperties) : base(currentName, currentProperties)
		{

		}
	}
}
