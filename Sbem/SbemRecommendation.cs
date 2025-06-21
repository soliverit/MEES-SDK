using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	///  The _epc.inp definition of recommendations from SBEM. The source of cost-effective measures
	///  on the EPC second page.
	/// </summary>
	//"REC-EPC-W1" = RECOMMENDATION
	//	CATEGORY             = HOT-WATER
	//	CODE = EPC-W1
	//	REC-SOURCE           = CALC-IN
	//	ENERGY-IMPACT        = HIGH
	//	CO2-IMPACT           = HIGH
	//	PAY-BACK-NUM         = 0.0191434
	//	CO2-SAVE-POUND       = GOOD
	//	ENERGY-IMPACT-SC     = CALC
	//	CO2-IMPACT-SC        = CALC
	//	PAY-BACK-SC          = CALC
	//	CO2-SAVE-POUND-SC    = CALC
	//	..
	public class SbemRecommendation : SbemObject
	{
		public SbemRecommendation(string name, List<string> currentProperties) : base(name, currentProperties) { }
		public const string OBJECT_NAME = "RECOMMENDATION";
		public override string ObjectName() { return OBJECT_NAME; }

	}
}
