using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem.Retrofitting.Measures
{
	public class NCMHeatPumpR5Example : RetrofitBase
	{
		public NCMHeatPumpR5Example(SbemModel model) : base(model) { }
		/// <summary>
		/// Apply the retrofit 
		/// </summary>
		public override void Apply()
		{
			// Check all constructions
			for (int constructionID = 0; constructionID < Model.Constructions.Length; constructionID++)
			{
				SbemHvacSystem hvac = Model.HvacSystems[constructionID];
				// We're only interested in gas-fired heating and direct electric heating systems
				if (!hvac.IsLTHWBoiler && !hvac.IsDirectOrStorageElectricHeater)
					continue;
				// Track the changes
				AddModifiedObject(hvac);
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
					for (int hvacID = 0; hvacID < ModifiedHvacSystems.Length; hvacID++)
						_cost   += ModifiedHvacSystems[hvacID].Area * 50;
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
					for (int hvacID = 0; hvacID < ModifiedHvacSystems.Length; hvacID++)
						_area   += ModifiedHvacSystems[hvacID].Area;
				return _area;
			}
			protected set { _area = value; }
		}
	}
}
