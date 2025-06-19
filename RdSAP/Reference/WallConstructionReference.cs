using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MeesSDK.RdSAP.Reference
{
	public class WallConstructionReference : ReferenceDataBase<WallConstructionRecord>
	{
		public static WallConstructionReference ParseFile(string path)
		{
			var instance = new WallConstructionReference();

			if (!File.Exists(path))
				throw new FileNotFoundException($"Could not find wall construction data at {path}");

			StreamReader reader = new StreamReader(path);
			List<string[]> rows;
			CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture);
			csv.Read();
			while (csv.Read())
			{
				string[] row = csv.Context.Parser.Record;
				string wallType = row[0];
				string insulation = row[1];
				Dictionary<string, float> bandsDict = new Dictionary<string, float>()
				{
					["A"] = float.Parse(row[2]),
					["B"] = float.Parse(row[3]),
					["C"] = float.Parse(row[4]),
					["D"] = float.Parse(row[5]),
					["E"] = float.Parse(row[6]),
					["F"] = float.Parse(row[7]),
					["G"] = float.Parse(row[8]),
					["G"] = float.Parse(row[8]),
					["H"] = float.Parse(row[8]),
					["I"] = float.Parse(row[8]),
					["J"] = float.Parse(row[8]),
					["K"] = float.Parse(row[8]),
					["L"] = float.Parse(row[8]),
				};
				var record = new WallConstructionRecord(wallType, insulation, bandsDict);
				instance.Records.Add(record);
			}

			return instance;
		}
	}
}
