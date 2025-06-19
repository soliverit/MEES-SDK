using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.RdSAP.Reference
{
	public class DoorConstructionReference : ReferenceDataBase<DoorConstructionRecord>
	{
		public static DoorConstructionReference ParseFile(string path)
		{
			var instance = new DoorConstructionReference();

			if (!File.Exists(path))
				throw new FileNotFoundException($"Could not find door construction data at {path}");

			string[] lines = File.ReadAllLines(path);
			for (int lineID = 1; lineID < lines.Length; lineID++)
			{
				string[] row = lines[lineID].Split(",");
				var record = new DoorConstructionRecord(
					band: row[0],
					factor: float.Parse(row[1])
				);

				instance.Records.Add(record);
				if (!string.IsNullOrWhiteSpace(record.Band))
					instance.BandsDictionary[record.Band] = record;
			}

			return instance;
		}
	}
}

