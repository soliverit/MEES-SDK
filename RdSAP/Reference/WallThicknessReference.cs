using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.RdSAP.Reference
{
	public class WallThicknessReference : ReferenceDataBase<WallThicknessRecord>
	{
		public static WallThicknessReference ParseFile(string path)
		{
			var instance = new WallThicknessReference();

			if (!File.Exists(path))
				throw new FileNotFoundException($"Could not find wall thickness data at {path}");

			var lines = File.ReadAllLines(path);
			var header = lines[0].Split(',');
			var bandHeaders = header.Skip(1).ToList(); // A–L

			var rows = lines.Skip(1)
							.Select(line => line.Split(','))
							.ToList();

			foreach (var row in rows)
			{
				var key = row[0];
				var values = row.Skip(1).ToList();

				var record = new WallThicknessRecord(key, bandHeaders, values);
				instance.Records.Add(record);
				// optionally: index by key or first band
			}

			return instance;
		}
	}
}
