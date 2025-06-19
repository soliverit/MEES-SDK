


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public class SbemImprovementMeasure : SbemObject
	{
		public const string OBJECT_NAME  = "IMPROVEMENT-MEASURE"; 
		public override string ObjectName() { return OBJECT_NAME; }
		public SbemImprovementMeasure(string currentName, List<string> currentProperties) : base(currentName, currentProperties)
		{

		}
	}
}
