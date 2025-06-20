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
			FloorConstructionReference floorConstruction,
			GlazingTypeReference glazingType,
			HeatingControlReference heatingControl,
			RoofConstructionReference roofConstruction,
			WallConstructionReference wallConstruction,
			WallThicknessReference wallThickness,
			WindowSizeParameterReference winodwSizeParameter
			) 
		{
			ConstructionAge		= constructionAge;
			FloorConstruction	= floorConstruction;
			GlazingType			= glazingType;
			HeatingControl		= heatingControl;
			RoofConstruction	= roofConstruction;
			WallConstruction	= wallConstruction;
			WallThickness		= wallThickness;
			WindowSizeParameter	= winodwSizeParameter;
		}
		public ConstructionAgeReference ConstructionAge { get; }
		public FloorConstructionReference FloorConstruction { get; }
		public GlazingTypeReference GlazingType { get; }
		public HeatingControlReference HeatingControl { get; }
		public RoofConstructionReference RoofConstruction { get; }
		public WallConstructionReference WallConstruction { get; }
		public WallThicknessReference WallThickness { get; }
		public WindowSizeParameterReference WindowSizeParameter { get; }
		public static RdSAPReferenceDataSet Build(
				string constructionAgePath,
				string floorConstructionPath,
				string glazingTypepath,
				string heatingControlPath,
				string roofConstructionPath,
				string wallConstructionPath,
				string wallThicknessPath,
				string windowSizeParameterPath
			)
		{
			return new RdSAPReferenceDataSet(
					ConstructionAgeReference.ParseFile(constructionAgePath),
					FloorConstructionReference.ParseFile(floorConstructionPath),
					GlazingTypeReference.ParseFile(glazingTypepath),
					HeatingControlReference.ParseFile(heatingControlPath),
					RoofConstructionReference.ParseFile(roofConstructionPath),
					WallConstructionReference.ParseFile(wallConstructionPath),
					WallThicknessReference.ParseFile(wallThicknessPath),
					WindowSizeParameterReference.ParseFile(windowSizeParameterPath)
				);
		}
	}
}
