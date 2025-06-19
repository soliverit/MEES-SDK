using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MeesSDK.RdSAP.Reference
{
	public class WindowSizeParameterReference : ReferenceDataBase<WindowSizeParametersRecord>
	{
		public Dictionary<string, WindowSizeParametersRecord> LabelsDictionary { get; } = new();

		public static WindowSizeParameterReference ParseFile(string path)
		{
			var instance = new WindowSizeParameterReference();

			if (!File.Exists(path))
				throw new FileNotFoundException($"Could not find window size parameters at {path}");

			StreamReader reader = new StreamReader(path);
			List<string[]> rows;
			CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture);
			csv.Read();
			while (csv.Read())
			{
				string[] row = csv.Context.Parser.Record;
				var record = new WindowSizeParametersRecord(
					label: row[0],
					house: float.Parse(row[1]),
					bungalow: float.Parse(row[2]),
					flat: float.Parse(row[3]),
					maisonette: float.Parse(row[4]),
					housePlus: float.Parse(row[5]),
					bungalowPlus: float.Parse(row[6]),
					flatPlus: float.Parse(row[7]),
					maisonettePlus: float.Parse(row[8])
				);

				instance.Records.Add(record);
				instance.LabelsDictionary[record.Label] = record;

			}

			return instance;
		}
	}
}
