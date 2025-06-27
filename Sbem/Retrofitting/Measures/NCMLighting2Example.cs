using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem.Retrofitting.Measures
{
	public class NCMLighting2Example : RetrofitBase
	{
		public NCMLighting2Example(SbemModel sbemModel) : base(sbemModel) { }
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
				if (zone.GetStringProperty("LIGHT-TYPE").Value == "Tungsten or Halogen2")
				{
					// Tell SBEM we're using efficacy then set the efficacy
					zone.SetNumericProperty("LAMP-BALLAST-EFF", 60);
					zone.SetStringProperty("LIGHT-CASE", "CHOSEN");
					AddModifiedObject(zone);
				}
			}
		}
		/// <summary>
		/// Get the cost to implement the retrofit. In the example, 50 * Area
		/// </summary>
		public override float Cost
		{
			get
			{
				// Only calculate it once
				if (_cost == 0)
					for (int zoneID = 0; zoneID < ModifiedZones.Length; zoneID++)
						_cost   += ModifiedZones[zoneID].Area * 20;
				return _cost;
			}
			protected set { _cost = value; }
		}
		/// <summary>
		/// Get the area of affected objects. In the example, walls via the SbemConstruction has many SbemWall
		/// </summary>
		public override float Area
		{
			get
			{
				// Only calculate it once
				if (_area == 0)
					for (int zoneID = 0; zoneID < ModifiedZones.Length; zoneID++)
						_area   += ModifiedConstructions[zoneID].Area;
				return _area;
			}
			protected set { _area = value; }
		}
	}
}
