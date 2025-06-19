using MeesSDK.RdSAP.Reference.MOOSandbox.RdSAP.Reference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.RdSAP.Reference
{
	public class RdSAPReferenceDataSet
	{
		public RdSAPReferenceDataSet(
			ConstructionAgeReference constructionAge,
			DoorConstructionReference dorrConstruction,
			FloorConstructionReference floorConstruction,
			GlazingTypeReference glazingType,
			HeatingControlReference heatingControl,
			LivingAreaFactorReference livingFactor,
			RoofConstructionReference roofConstruction,
			ThermalBridgeReference thermalBridge,
			WallConstructionReference wallConstruction,
			WallThicknessReference wallThickness,
			WindowSizeParameterReference winodwSizeParameter
			) 
		{
			ConstructionAge = constructionAge;
			DoorConstruction = dorrConstruction;
			FloorConstruction = floorConstruction;
			GlazingType = glazingType;
			HeatingControl = heatingControl;
			LivingAreaFactor = livingFactor;
			RoofConstruction = roofConstruction;
			ThermalBrdige = thermalBridge;
			WallConstruction = wallConstruction;
			WallThickness = wallThickness;
			WindowSizeParameter = winodwSizeParameter;
		}
		public ConstructionAgeReference ConstructionAge { get; }
		public DoorConstructionReference DoorConstruction { get; }	
		public FloorConstructionReference FloorConstruction { get; }
		public GlazingTypeReference GlazingType { get; }
		public HeatingControlReference HeatingControl { get; }
		public LivingAreaFactorReference LivingAreaFactor { get; }
		public RoofConstructionReference RoofConstruction { get; }
		public ThermalBridgeReference ThermalBrdige { get; }
		public WallConstructionReference WallConstruction { get; }
		public WallThicknessReference WallThickness { get; }
		public WindowSizeParameterReference WindowSizeParameter { get; }
		public static RdSAPReferenceDataSet Build(
				string constructionAgePath,
				string doorConstructionPath,
				string floorConstructionPath,
				string glazingTypepath,
				string heatingControlPath,
				string livingFactorPath,
				string roofConstructionPath,
				string thermablBrdigePath,
				string wallConstructionPath,
				string wallThicknessPath,
				string windowSizeParameterPath
			)
		{
			return new RdSAPReferenceDataSet(
					ConstructionAgeReference.ParseFile(constructionAgePath),
					DoorConstructionReference.ParseFile(doorConstructionPath),
					FloorConstructionReference.ParseFile(floorConstructionPath),
					GlazingTypeReference.ParseFile(glazingTypepath),
					HeatingControlReference.ParseFile(heatingControlPath),
					LivingAreaFactorReference.ParseFile(livingFactorPath),
					RoofConstructionReference.ParseFile(roofConstructionPath),
					ThermalBridgeReference.ParseFile(thermablBrdigePath),
					WallConstructionReference.ParseFile(wallConstructionPath),
					WallThicknessReference.ParseFile(wallThicknessPath),
					WindowSizeParameterReference.ParseFile(windowSizeParameterPath)
				);
		}
	}
}
