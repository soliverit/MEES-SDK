using MeesSDK.RdSAP.Reference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.RdSAP.Reference
{
	public class ConstructionAgeReference : ReferenceDataBase<ConstructionAgeRecord>
	{
		public static ConstructionAgeReference ParseFile(string path)
		{
			var instance = new ConstructionAgeReference();

			if (!File.Exists(path))
				throw new FileNotFoundException($"Could not find construction age data at {path}");

			string[] lines = File.ReadAllLines(path);

			// Skip header with 1
			for (int lineID = 1; lineID < lines.Length; lineID++)
			{
				string[] row = lines[lineID].Split(",");
				ConstructionAgeRecord record = new ConstructionAgeRecord(
					label: row[0],
					band: row[1],
					index: int.Parse(row[2])
				);

				instance.Records.Add(record);

				if (!string.IsNullOrWhiteSpace(record.Label))
					instance.LabelsDictionary[record.Label] = record;

				if (!string.IsNullOrWhiteSpace(record.Band))
					instance.BandsDictionary[record.Band] = record;
			}

			return instance;
		}
	}

}
