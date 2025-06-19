using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.RdSAP.Reference
{
	public class LivingAreaFactorReference : ReferenceDataBase<LivingAreaFactorRecord>
	{
		public Dictionary<int, LivingAreaFactorRecord> IndexDictionary { get; } = new();

		public static LivingAreaFactorReference ParseFile(string path)
		{
			var instance = new LivingAreaFactorReference();

			if (!File.Exists(path))
				throw new FileNotFoundException($"Could not find index factor data at {path}");

			string[] lines = File.ReadAllLines(path);
			for (int lineID = 1; lineID < lines.Length; lineID++)
			{
				string[] row = lines[lineID].Split(",");
				var record = new LivingAreaFactorRecord(
					index: int.Parse(row[0]),
					factor: float.Parse(row[1])
				);

				instance.Records.Add(record);
				instance.IndexDictionary[record.Index] = record;
			}

			return instance;
		}
	}
}
