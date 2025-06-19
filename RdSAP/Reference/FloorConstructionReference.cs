using MeesSDK.RdSAP.Reference.MOOSandbox.RdSAP.Reference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MeesSDK.RdSAP.Reference
{
	public class FloorConstructionReference : ReferenceDataBase<FloorConstructionRecord>
	{
		public static FloorConstructionReference ParseFile(string path)
		{
			var instance = new FloorConstructionReference();

			if (!File.Exists(path))
				throw new FileNotFoundException($"Could not find floor construction data at {path}");

			string[] lines = File.ReadAllLines(path);
			for (int lineID = 1; lineID < lines.Length; lineID++)
			{
				string[] row = lines[lineID].Split(",");
				var record = new FloorConstructionRecord(
					band: row[0],
					unknown: float.Parse(row[1]),
					u50: float.Parse(row[2]),
					u100: float.Parse(row[3]),
					u150: float.Parse(row[4])
				);

				instance.Records.Add(record);
				instance.BandsDictionary[record.Band] = record;
			}
			return instance;
		}
	}
}
