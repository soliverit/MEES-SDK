using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.DataManagement
{
	using Microsoft.ML;
	using Microsoft.ML.Data;
	using System.Globalization;
	using System.IO;
	using System.Linq;

	public static class DataViewDumper
	{
		public static void DumpToCsv(IDataView dataView, string filePath, MLContext mlContext, string[] features)
		{
			using var streamWriter = new StreamWriter(filePath);
			var preview = dataView.Preview(maxRows: int.MaxValue);

			// Header
			streamWriter.WriteLine(string.Join(",", preview.Schema.Where(col => features.Contains(col.Name))));

			// Rows
			foreach (var row in preview.RowView)
			{
				var values = row.Values.Select(v => v.Value?.ToString()?.Replace(",", " ") ?? "NULL");
				streamWriter.WriteLine(string.Join(",", values));
			}

			Console.WriteLine($"Data dumped to: {Path.GetFullPath(filePath)}");
		}
	}

}
