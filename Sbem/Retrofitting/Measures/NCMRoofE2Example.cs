using Microsoft.ML.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem.Retrofitting.Measures
{
	public class NCMRoofE2Example : RetrofitBase
	{
		/// <summary>
		/// The associated EPC/Measure code. E.g EPC-L5 for T8 lamp replacement. 
		/// Codes taken from the SBEM technical manual where possible.
		/// </summary>
		public const string MEASURE_REFERENCE_CODE	= "EPC-E2";
		/// <summary>
		/// The maximum U-Value allowed before wall insulation is required. Se. 
		/// </summary>
		public const float U_VALUE_THRESHOLD		= 0.7f;
		/// <summary>
		/// The replacement U-Value. Table 4.2 Part L2. Limiting U-Values
		/// </summary>
		public const float L2_MAX_RETROFIT_U_VALUE	= 0.3f;
		public NCMRoofE2Example(SbemModel model) : base(model) { }
		public override void Apply()
		{
			// Check all constructions
			for (int hvacID = 0; hvacID < Model.Constructions.Length; hvacID++)
			{
				SbemConstruction construction = Model.Constructions[hvacID];
				// Skip constructions that aren't for external walls or whose U-Value is under the threshhold
				if (!construction.IsRoof || construction.GetNumericProperty("U-VALUE").Value > U_VALUE_THRESHOLD)
					continue;
				// Apply the retrofit
				construction.SetNumericProperty("U-VALUE", L2_MAX_RETROFIT_U_VALUE);
				// Track the changes
				AddModifiedObject(construction);
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
			get
			{
				// Only calculate it once
				if (_area == 0)
					for (int constructionID = 0; constructionID < ModifiedConstructions.Length; constructionID++)
						_area   += ModifiedConstructions[constructionID].Area;
				return _area;
			}
			protected set { _area = value; }
		}
	}
}
