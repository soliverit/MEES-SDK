using MeesSDK.Examples;
using MeesSDK.Sbem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Examples
{
	public class ConsumptionDisaggregtionForZones : IMeesSDKExample
	{
		public void DoTheExample(SbemProject project, SbemService sbem)
		{
			/*
			 * It's just two lines. What's happening under the hood?
			 * 
			 * For every SbemZone in the input SbemModel, create a Single-HVAC, Single-Zone SbemModel
			 * and get the results using the SbemService. After all SbemRequest have been processed by
			 * the SbemService, attach Single-HVAC, Single-Zone SbemModel amd attach its Project End 
			 * Use and Fuel consmption calendars to the associated Zone in the input SbemModel
			 */
			// Disaggregate Hot Water to HVAC-level
			SbemProject.DisaggregateHVACHotWater(project.AsBuiltSbemModel, sbem);
			// Disaggregate End Use and Fuel consumption to Zone-level
			SbemProject.CalculateZonalEnergyDemand(project.AsBuiltSbemModel, sbem);
		}
		public string Name { get; set; } = "Disaggregating End Use and Fuel Type consumption to Zone-level";
		public string GetDescription()
		{
			return @""" End Use and Fuel Type consumption disaagregation

SBEM doesn't calculate the HVAC-level Hot Water consumption because for its purpose, it doesn't 
mean anything. Similarly, zones that contibute to Hot Water demand often don't consume themselves. This is why toilet-only
SbemModel don't have Hot Water demand.

All the same, it tells us a something about the relationship between occupacny assumptions, demand, and 
Generator efficiency.

In this example. We will: 
1. Disaggregate Hot Water conumption to HVAC-lvel
2. Disaggregate all End Use and Fuel consumption to Zone-levele

In a later example, we'll reference this data to attempt occupancy correction.

""";
		}
	}
}
