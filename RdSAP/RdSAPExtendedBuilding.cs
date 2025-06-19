using MeesSDK.RdSAP.Reference;
using MeesSDK.RdSAP.Reference.MOOSandbox.RdSAP.Reference;
using MeesSDK.RsSAP;

using System.Text.RegularExpressions;

namespace MeesSDK.RdSAP
{
	public class RdSAPExtendedBuilding : RdSAPBuilding
	{
		public RdSAPExtendedBuilding(Dictionary<string, string> data, RdSAPReferenceDataSet reference) : base(data)
		{
			
		}

		public string ConstructionAgeBand { get; }
		public float FloorUValue { get; }
		public float ThermalBridgingFactor { get; }
		public float WindowArea { get; }
		public float WindowTofloorRatio { get; }
		public float GlassGValue { get; }
		public float GlassUValue { get; }
		public float RoofUValue { get; }
		public float WallUValue {  get; }
		public float WallThickness { get; }
		
		
		public RdSAPReferenceDataSet ReferenceData { get; }
		public ConstructionAgeRecord ConstructionAgeData { get; }
		public DoorConstructionRecord DoorConstructionData { get; }
		public FloorConstructionRecord FloorConstructionData { get; }
		public GlazingTypeRecord GlazingTypeData { get; }
		public HeatingControlRecord HeatingControlData { get; }
		public LivingAreaFactorRecord LivingAreaFactorData { get; }
		public RoofConstructionRecord RoofConstructionData { get; }
		public ThermalBridgingRecord ThermalBridgingData { get; }
		public WallConstructionRecord WallConstructionData { get; }
		public WallThicknessRecord WallThicknessData { get; }
		public WindowSizeParametersRecord WindowSizeParametersData { get; }
	}
}	
