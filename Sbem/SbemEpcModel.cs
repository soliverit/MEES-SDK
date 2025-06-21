using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static MeesSDK.Sbem.SbemError;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// The _epc.inp model. Same file format as .inp SbemModel with different objects
	/// and couple of relationships.
	/// </summary>
	public class SbemEpcModel : SbemModelBase
	{
		public SbemBuildingData Actual { get; protected set; }
		public SbemBuildingData Notional { get; protected set; }
		public SbemBuildingData Reference { get; protected set; }
		public SbemRecProject RecProject { get; protected set; }
		public SbemEpc Epc { get; protected set; }
		public List<SbemRecommendation> Recommendations { get; } = new List<SbemRecommendation>();
		public static SbemEpcModel ParseInpFile(string path)
		{
			if (!File.Exists(path))
			{
				var model = new SbemEpcModel();
				model.AddError((int)SbemError.ErrorCode.CONTENT_FILE_NOT_EXISTS, $"Inp file '{path}' doesn't exist.");
				return model;
			}
			string content = File.ReadAllText(path);
			return ParseEpcInpContent(content);
		}
		public static SbemEpcModel ParseEpcInpContent(string content)
		{
			SbemEpcModel model = new SbemEpcModel();
			using StringReader reader = new StringReader(content);

			string line;
			string currentName = null;
			string currentType = null;
			SbemBuildingData currentSbemBuildingData = new SbemBuildingData("shoe", new List<string>());
			List<string> currentProperties = new List<string>();

			bool inObject = false;
			int lineNumber = 0;
			while ((line = reader.ReadLine()) != null)
			{
				lineNumber++;
				line = line.Trim();
				if (string.IsNullOrWhiteSpace(line) || line.StartsWith("$"))
					continue;
				else if (!inObject)
				{
					var header = SbemObject.GetHeaderLine(line);
					if (header.isHeader)
					{
						inObject = true;
						currentName = header.name;
						currentType = header.type;
						currentProperties.Clear();
					}
					else
					{
						model.AddError(ErrorCode.CONTENT_CORRUPT, $"Line {lineNumber} was expected to be a header line. '{line}'");
					}
				}
				else
				{
					if (SbemObject.IsCloseLine(line))
					{
						inObject = false;
						var objectType = currentType;
						switch (objectType)
						{
							case SbemGeneral.OBJECT_NAME:
								model.General = new SbemGeneral(currentName, currentProperties);
								break;
							case SbemEpc.OBJECT_NAME:
								model.Epc = new SbemEpc(currentName, currentProperties);
								break;
							case SbemRecProject.OBJECT_NAME:
								model.RecProject = new SbemRecProject(currentName, currentProperties);
								break;
							case SbemRecommendation.OBJECT_NAME:
								model.Recommendations.Add(new SbemRecommendation(currentName, currentProperties));
								break;
							case SbemBuildingData.OBJECT_NAME:
								currentSbemBuildingData = new SbemBuildingData(currentName, currentProperties);
								switch (currentSbemBuildingData.GetStringProperty("ANALYSIS").Value)
								{
									case "ACTUAL":
										model.Actual = currentSbemBuildingData;
										break;
									case "NOTIONAL":
										model.Notional = currentSbemBuildingData;
										break;
									case "REFERENCE":
										model.Reference = currentSbemBuildingData;
										break;
								}
								model.RecProject = new SbemRecProject(currentName, currentProperties);
								break;
							case SbemHvacSystemData.OBJECT_NAME:
								currentSbemBuildingData.HvacSystems.Add(new SbemHvacSystemData(currentName, currentProperties));
								break;
						}
					}
					else
					{
						currentProperties.Add(line);
					}
				}
			}
			return model;
		}

		public override string ToString()
		{
			StringBuilder content = new StringBuilder();
			content.AppendLine(General.ToString());
			content.AppendLine(Epc.ToString());

			foreach (SbemRecommendation obj in Recommendations)
				content.AppendLine(obj.ToString());
			content.AppendLine(Actual.ToString());
			content.AppendLine(Notional.ToString());
			content.AppendLine(Reference.ToString());
			return content.ToString();

		}

	}
}