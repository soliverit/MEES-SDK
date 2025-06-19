using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MeesSDK.RdSAP.Reference
{
	public class ThermalBridgeReference : ReferenceDataBase<ThermalBridgingRecord>
	{
		public static ThermalBridgeReference ParseFile(string path)
		{
			var instance = new ThermalBridgeReference();

			if (!File.Exists(path))
				throw new FileNotFoundException($"Could not find thermal bridging data at {path}");

			string[] lines = File.ReadAllLines(path);
			for (int lineID = 1; lineID < lines.Length; lineID++)
			{
				string[] row = lines[lineID].Split(",");
				var record = new ThermalBridgingRecord(
					band: row[0],
					factor: float.Parse(row[1])
				);

				instance.Records.Add(record);
				instance.BandsDictionary[record.Band] = record;
			}

			return instance;
		}
	}
}
