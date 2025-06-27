using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem.Retrofitting.Measures
{
	internal class NCMGlazing8example : RetrofitBase
	{
		/// <summary>
		/// The default constructor
		/// </summary>
		/// <param name="model"></param>
		public NCMGlazing8example(SbemModel model) : base(model) { }
		/// <summary>
		/// The U-Value cut off for glazing being replaced
		/// </summary>
		public const float U_VALUE_THRESHOLD	= 2.0f;
		/// <summary>
		/// The replacement U-Value. Table 4.2 Part L2. Limiting U-Values
		/// </summary>
		public const float L2_U_VALUE			= 1.6f;
		public override void Apply()
		{
			// Check all constructions
			for (int glassID = 0; glassID < Model.Glasses.Length; glassID++)
			{
				SbemGlass glass	= Model.Glasses[glassID];
				// Skip glasses whose U-Value is under the threshold
				if (glass.GetNumericProperty("U-VALUE").Value > U_VALUE_THRESHOLD)
					continue;
				// Apply the retrofit
				glass.SetNumericProperty("U-VALUE", U_VALUE_THRESHOLD);
				// Track the changes
				AddModifiedObject(glass);
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
					for (int glassID = 0; glassID < ModifiedGlasses.Length; glassID++)
						_cost   += ModifiedGlasses[glassID].Area * 50;
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
					for (int glassID = 0; glassID < ModifiedGlasses.Length; glassID++)
						_area   += ModifiedGlasses[glassID].Area;
				return _area;
			}
			protected set { _area = value; }
		}
	}
}
