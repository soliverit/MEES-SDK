using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MeesSDK.RdSAP.Reference
{
	public class GlazingTypeReference : ReferenceDataBase<GlazingTypeRecord>
	{
		public static GlazingTypeReference ParseFile(string path)
		{
			var instance = new GlazingTypeReference();

			if (!File.Exists(path))
				throw new FileNotFoundException($"Could not find glazing type data at {path}");

			string[] lines = File.ReadAllLines(path);
			for (int lineID = 1; lineID < lines.Length; lineID++)
			{
				string[] row = lines[lineID].Split(",");
				var label = row[0];
				var when = row[1];
				var uValue = float.Parse(row[2]);
				var gValue = float.Parse(row[3]);

				var record = new GlazingTypeRecord(label, when, uValue, gValue);
				instance.Records.Add(record);

				if (!string.IsNullOrWhiteSpace(record.Label))
					instance.LabelsDictionary[record.Label] = record;
			}
			return instance;
		}
	}
}

