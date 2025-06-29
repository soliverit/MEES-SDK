using MeesSDK.DataManagement;
using MeesSDK.RdSAP.Reference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem.Retrofitting.Measures
{
	public class NCMLighting6Example : RetrofitBase
	{
		public NCMLighting6Example(SbemModel sbemModel) : base(sbemModel) { }

		/// <summary>
		/// The associated EPC/Measure code. E.g EPC-L5 for T8 lamp replacement. 
		/// Codes taken from the SBEM technical manual where possible.
		/// </summary>
		public const string MEASURE_REFERENCE_CODE = "EPC-L6";
		/// <summary>
		/// The SbemZone LIGHT-TYPE keyword for high-pressure sodium
		/// </summary>
		public const string HPS_KEYWORD = "High Pressure Sodium";
		/// <summary>
		/// Apply the retrofit, track the modified objects.
		/// </summary>
		public override void Apply()
		{
			/*
			 * There's nothing complicated. Iterate over the zones and
			 * update the ones that have high-pressure sodium lamps
			 * 
			 * Important: make sure to AddModifiedObject(modifiedZone) for costs
			 */
			// Check all Zones for hpS lamps
			for (int zoneID = 0; zoneID < Model.Zones.Length; zoneID++)
			{
				SbemZone zone = Model.Zones[zoneID];
				// Skip zones with lighting not defined by template
				if (! zone.PropertyEquals("LIGHT-CASE", "UNKNOWN"))
					continue;
				// Replace high-pressure mercury and high-pressure sodium lamps
				if (zone.PropertyEquals("LIGHT-TYPE", HPS_KEYWORD))
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
