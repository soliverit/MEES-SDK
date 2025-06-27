using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem.Retrofitting.Measures
{
	internal class NCMExtnerWallInsulation8Example : RetrofitBase
	{
		/// <summary>
		/// The maximum U-Value allowed before wall insulation is required. Se. 
		/// </summary>
		public const float U_VALUE_THRESHOLD			= 0.7f;
		/// <summary>
		/// The replacement U-Value. Table 4.2 Part L2. Limiting U-Values
		/// </summary>
		public const float L2_MAX_RETROFIT_U_VALUE		= 0.3f;
		public NCMExtnerWallInsulation8Example(SbemModel model) : base(model){}
		/// <summary>
		/// Apply the retrofit 
		/// </summary>
		public override void Apply()
		{
			// Check all constructions
			for (int constructionID = 0; constructionID < Model.Constructions.Length; constructionID++)
			{
				SbemConstruction construction = Model.Constructions[constructionID];
				// Skip constructions that aren't for external walls or whose U-Value is under the threshhold
				if (!construction.IsExternalWall || construction.GetNumericProperty("U-VALUE").Value > U_VALUE_THRESHOLD)
					continue;
				// Apply the retorift
				construction.SetNumericProperty("U-VALUE", L2_MAX_RETROFIT_U_VALUE);
				// Track the changes
				ModifiedConstructions.Add(construction);
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
					for (int constructionID = 0; constructionID < ModifiedConstructions.Length; constructionID++)
						_cost   += ModifiedConstructions[constructionID].Area * 50;
				return _cost;
			}
			protected set { _cost = value; }
		}
		/// <summary>
		/// Get the area of affected objects. In the example, walls via the SbemConstruction has many SbemWall
		/// </summary>
		public override float Area
		{
			get	{
				// Only calculate it once
				if (_area == 0)
					for(int constructionID  = 0; constructionID < ModifiedConstructions.Length; constructionID++) 
						_area   += ModifiedConstructions[constructionID].Area;
				return _area;
			}
			protected set { _area = value; }
		}
	}
}
