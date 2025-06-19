

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public class SbemWindGenerator : SbemObject
	{
		public const string OBJECT_NAME  = "WIND-GENERATOR"; 
		public override string ObjectName() { return OBJECT_NAME; }
		public SbemWindGenerator(string currentName, List<string> currentProperties) : base(currentName, currentProperties)
		{

		}
	}
}
