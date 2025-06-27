
namespace MeesSDK.Sbem.Retrofitting.Measures
{
	public class NCMRenewables4Example : RetrofitBase
	{
		/// <summary>
		/// The percentage of roof area to be covered
		/// </summary>
		public const float PERCENT_COVERAGE = 0.4f;
		public NCMRenewables4Example(SbemModel model) : base(model) { }
		public override float Area
		{
			get { return _area; }
			protected set { _area = value; }
		}
		public override float Cost
		{
			get { return _cost; }
			protected set { _cost = value; }
		}
		public override void Apply()
		{
			// Don't replace an existing system
			if (Model.HasPvs)
				return;
			// Check the three orientations
			string[] orientations = [SbemConstruction.SOUTH_EAST, SbemConstruction.SOUTH, SbemConstruction.SOUTH_WEST];
			for (int orientationID = 0; orientationID < orientations.Length; orientationID++)
			{
				// Get total surface area
				float area = 0;
				for (int constructionID = 0; constructionID < Model.Constructions.Length; constructionID++)
				{
					SbemConstruction construction = Model.Constructions[constructionID];
					// Only do roofs
					if (!construction.IsRoof)
						continue;
					// Only do South, south-west, and west facing roofs
					if (construction.PropertyEquals("ORIENTATION", orientations[orientationID]))
						area    += construction.SurfaceArea;
				}
				// Don't add solar panels to nothing
				if (area ==  0)
					continue;
				// Create a new PVS for the current SES-ORIENTATION
				SbemPvs pvs = new SbemPvs("EPC-R5 PVS", new List<string>());
				pvs.SetNumericProperty("AREA", area * PERCENT_COVERAGE);
				pvs.SetNumericProperty("MULTIPLIER", 1);
				pvs.SetNumericProperty("SES-INCLINATION", 30);
				pvs.SetStringProperty("SES-ORIENTATION", orientations[orientationID]);
				pvs.SetStringProperty("WATT-PEAK-TYPE", "Mono crystalline silicon");
				pvs.SetStringProperty("VENT-STRATEGY", "Unventilated modules");
				pvs.SetStringProperty("OVERSHADE-TYPE", "None or very little (<20%)");
				// Add it to the model
				Model.Pvs.Add(pvs);
				// Add it to modified objects
				AddModifiedObject(pvs);
			}
		}
	}
}
