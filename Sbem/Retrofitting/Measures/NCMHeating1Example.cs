using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem.Retrofitting.Measures
{
	/// <summary>
	/// Example boiler replacement Retrofit
	/// </summary>
	public class NCMHeating1Example : RetrofitBase<SbemHvacSystem>
	{
		public const float CUTOFF_SEFF = 0.85f;
		public NCMHeating1Example(SbemModel model) : base(model) { }
		public override void Apply()
		{
			// Select LTHW boilers
			SbemObjectSet<SbemHvacSystem> hvacs = Model.HvacSystems.Select(hvac => !hvac.TypeIs(SbemHvacSystem.NO_HEATING_OR_COOLING) && hvac.HeatSourceIs(SbemHvacSystem.LTHW_BOILER));
			for (int hvacID = 0; hvacID < hvacs.Length; hvacID++)
			{
				SbemHvacSystem hvac	= hvacs[hvacID];
				// Only replace inefficient boilers
				if (hvacs[hvacID].GetNumericProperty("HEAT-GEN-SEFF").Value < CUTOFF_SEFF)
				{
					// Update the SSEFF
					hvac.SetNumericProperty("HEAT-SSEFF", hvac.GetRelativeSSEFF(0.92f));
					// Tracked the updated HVAC
					AddModifiedObject(hvac);
				}
			}
		}
	}
}
