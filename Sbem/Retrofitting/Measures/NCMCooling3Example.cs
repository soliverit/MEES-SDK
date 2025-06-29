using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem.Retrofitting.Measures
{
	public class NCMCooling3Example : RetrofitBase
	{
		public NCMCooling3Example(SbemModel model) : base(model) { }
		/// <summary>
		/// Maximum leakage before EPC-C3 is triggered. See SBEM technical manual
		/// B2.3.2.2 Duct and AHU leakage
		/// </summary>
		public const float LEAKAGE_CUT_OFF			= 10;
		/// <summary>
		/// The associated EPC/Measure code. E.g EPC-L5 for T8 lamp replacement. 
		/// Codes taken from the SBEM technical manual where possible.
		/// </summary>
		public const string MEASURE_REFERENCE_CODE	= "EPC-C3";
		/// <summary>
		/// Assumed leakage percent after sealing. See SBEM technical manual 
		/// B2.3.2.2 Duct and AHU leakage
		/// </summary>
		public const float LEAKAGE_SEALED_PERCENT	= 5;
		/// <summary>
		/// The name of the SBEM property that defines duct work leakage percent
		/// </summary>
		public const string DUCT_LEAKAGE_PROPERTY	= "DUCT-LEAKAGE-PC";
		public override void Apply()
		{
			// Check every HVAC-SYSTEM
			for (int hvacID = 0; hvacID < Model.HvacSystems.Length; hvacID++)
			{
				SbemHvacSystem hvac	= Model.HvacSystems[hvacID];
				// Check for mechanical ventilation. It's in the colling category, but it's ducts
				if (hvac.HasMechanicalVentilation)
					// If the duct leakage is too high (See B2.3.2.2, SBEM technical manual)
					if (hvac.PropertyGreaterThan("DUCT-LEAKAGE-PC", LEAKAGE_CUT_OFF))
					{
						// Seal the ducts. (See B2.3.2.2, SBEM technical manual for new value)
						hvac.SetNumericProperty(DUCT_LEAKAGE_PROPERTY, LEAKAGE_SEALED_PERCENT);
						AddModifiedObject(hvac);
					}
			}

		}
		public override float Area { get
		{
				if (_area == 0)
					for (int hvacID = 0; hvacID < ModifiedHvacSystems.Length; hvacID++)
						_area+=  ModifiedHvacSystems[hvacID].Area;
			return _area;
		}
			protected set { _area = value; } }
		public override float Cost
		{
			get
			{
				if (_cost == 0)
					for (int hvacID = 0; hvacID < ModifiedHvacSystems.Length; hvacID++)
						_cost   += ModifiedHvacSystems[hvacID].Area  * 40;
				return _cost;
			}
			protected set {  _cost = value; }
		}
	}
}
