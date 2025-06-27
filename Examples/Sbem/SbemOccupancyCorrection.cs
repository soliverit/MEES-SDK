using MeesSDK.Sbem;
using MeesSDK.Sbem.ConsumerCalendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Examples.Sbem
{
	public class SbemOccupancyCorrection : SbemExampleBase
	{
		public SbemOccupancyCorrection(SbemProject project, SbemService sbem) : base(project, sbem) { }
		public override void RunTheExample()
		{
			RunSbemExample();
		}
		protected void RunSbemExample()
		{
			// Clone the input building
			SbemModel baseModel = Project.AsBuiltSbemModel.Clone();
			// Calculate the zone-level Fuel Type and DHW consumption calendars
			SbemHandler.SilentMode  = true;
			SbemProject.CalculateZonalEnergyDemand(baseModel, SbemHandler);
			for(int zoneID = 0; zoneID < baseModel.Zones.Length; zoneID++)
			{
				SbemZone zone = baseModel.Zones[zoneID];
				Console.WriteLine("//////////////////////////");
				Console.WriteLine($"Zone: {zone.Name}, Area: {zone.Area}");
				Console.WriteLine("//////////////////////////");
				ConsumerConsumptionCalendar consumerCalendar	= zone.EndUseConsumerCalendar.Clone();

				HeatingDemandCalendar heatingDemandCalendar		= zone.HeatingEnergyDemandCalendar.Clone();
				//zone.EndUseConsumerCalendar.Print();
				//zone.FuelUseConsumerCalendar.Print();
				//zone.HeatingEnergyDemandCalendar.Print();
				//zone.InternalHeatProductionCalendar.Print();
			}
		}

		public override string GetDescription()
		{
			return @"Accounting for known discrepancies between design and operational occupancy.

";
		}
	}
}
