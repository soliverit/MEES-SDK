


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// The .inp object for custom improvement measures. The code manual was 10 years of of date
	/// when this was added, so we can only go on the sbem.csv IMPROVEMENT-MEASURE definition. 
	/// <code>
	/// KWH/M2-CONSUM-END-USE
	/// KWH/M2-CONSUM-FUEL
	/// KWH/M2-DISP-RENEW
	/// KG/M2-CO2
	/// OPERAT-ENERGY-SAVING
	/// OPERAT-COST-SAVING
	/// ASSET-ENERGY-SAVING
	/// ASSET-COST-SAVING
	/// </code>
	/// </summary>
	public class SbemImprovementMeasure : SbemObject
	{
		public const string OBJECT_NAME  = "IMPROVEMENT-MEASURE"; 
		public override string ObjectName() { return OBJECT_NAME; }
		public SbemImprovementMeasure(string currentName, List<string> currentProperties) : base(currentName, currentProperties)
		{

		}
	}
}
