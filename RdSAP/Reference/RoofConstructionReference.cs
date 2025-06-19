using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MeesSDK.RdSAP.Reference
{
	public class RoofConstructionReference : ReferenceDataBase<RoofConstructionRecord>
	{
		public static RoofConstructionReference ParseFile(string path)
		{
			var instance = new RoofConstructionReference();

			if (!File.Exists(path))
				throw new FileNotFoundException($"Could not find roof construction data at {path}");

			string[] lines = File.ReadAllLines(path);
			for (int lineID = 1; lineID < lines.Length; lineID++)
			{
				string[] row = lines[lineID].Split(",");
				var record = new RoofConstructionRecord(
					band: row[0],
					pitched: float.Parse(row[1]),
					flat: float.Parse(row[2]),
					room: float.Parse(row[3])
				);

				instance.Records.Add(record);
				instance.BandsDictionary[record.Band] = record;
			}

			return instance;
		}
	}
}
