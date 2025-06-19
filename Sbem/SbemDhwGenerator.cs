


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public class SbemDhwGenerator : SbemObject
	{
		public const string OBJECT_NAME  = "DHW-GENERATOR"; 
		public override string ObjectName() { return OBJECT_NAME; }
		public SbemDhwGenerator(string currentName, List<string> currentProperties) : base(currentName, currentProperties)
		{

		}
	}
}
