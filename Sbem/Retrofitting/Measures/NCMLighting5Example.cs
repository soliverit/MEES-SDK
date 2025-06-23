using MeesSDK.DataManagement;
using MeesSDK.RdSAP.Reference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem.Retrofitting.Measures
{
	public class NCMLighting5Example : RetrofitBase<SbemZone>
	{
		public NCMLighting5Example(SbemModel sbemModel) : base(sbemModel) { }
		/// <summary>
		/// Apply the retrofit, track the modified objects.
		/// </summary>
		public override void Apply()
		{
			/*
			 * There's nothing complicated. Iterate over the zones and
			 * update the ones that have T8s.
			 * 
			 * Important: make sure to AddModifiedObject(modifiedZone) for costs
			 * and schedules later
			 */
			// Check all Zones for T8s
			for (int zoneID = 0; zoneID < Model.Zones.Length; zoneID++)
			{
				SbemZone zone = Model.Zones[zoneID];
				// Skip zones with lighting not defined by template
				if (zone.GetStringProperty("LIGHT-CASE").Value != "UNKNOWN")
					continue;
				// Replace T8 lamps
				if (zone.GetStringProperty("LIGHT-TYPE").Value.StartsWith("T8"))
				{
					// Tell SBEM we're using efficacy then set the efficacy
					zone.SetNumericProperty("LAMP-BALLAST-EFF", 60);
					zone.SetStringProperty("LIGHT-TYPE", "LED");
					AddModifiedObject(zone);
				}
			}
		}
	}
}
