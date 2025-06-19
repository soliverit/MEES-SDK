using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.RdSAP.Reference
{
	namespace MOOSandbox.RdSAP.Reference
	{
		public class HeatingControlReference : ReferenceDataBase<HeatingControlRecord>
		{
			public Dictionary<string, HeatingControlRecord> MeasureDictionary { get; } = new();

			public static HeatingControlReference ParseFile(string path)
			{
				var instance = new HeatingControlReference();

				if (!File.Exists(path))
					throw new FileNotFoundException($"Could not find heating control data at {path}");

				StreamReader reader = new StreamReader(path);
				List<string[]> rows;
				CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture);
				csv.Read();
				while(csv.Read())
				{
					string[] row = csv.Context.Parser.Record;
					var record = new HeatingControlRecord(
						measure: row[0],
						auto: int.Parse(row[1]),
						appThermo: int.Parse(row[2]),
						commCtrl: int.Parse(row[3]),
						trv: int.Parse(row[4]),
						prog: int.Parse(row[5]),
						roomThermo: int.Parse(row[6]),
						flatRate: int.Parse(row[7]),
						comment: row.Length > 8 ? row[8] : ""
					);

					instance.Records.Add(record);

					if (!string.IsNullOrWhiteSpace(record.Measure))
						instance.MeasureDictionary[record.Measure] = record;
				}

				return instance;
			}
		}
	}

}
