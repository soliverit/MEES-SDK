using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MeesSDK.DataManagement
{
	public class CsvHandler
	{
		/*=====================
		 *	Constructors
		 =====================*/
		public CsvHandler(List<string> headers) { SetHeaders(headers); }
		public CsvHandler() { }
		/*=====================
		 *  Static Members
		 =====================*/

		/*=====================
		 *  Static Methods
		 ====================*/
		/*
		 * Parse CSV file
		*/
		public static CsvHandler ParseCSV(string path)
		{
			CsvHandler data = new CsvHandler();
			string tName = data.GetType().Name;
			// If the path is null for some reason
			if (path == null)
			{
				data.Errors.Add($"{data.GetType().Name}::ParseCSV received null file path.");
				return data;
			}

			// If the path doesn't exist
			if (!File.Exists(path))
			{
				data.Errors.Add($"{data.GetType().Name}::ParseCSV couldn't find file '{path}'.");
				return data;
			}
			bool configError = false;
			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				BadDataFound = args =>
				{
					data.Errors.Add($"{tName}::ParseCSV. The target csv file is corrupt '{path}'");
					configError = true;
				}
			};

			List<List<string>> rows = new List<List<string>>();
			try
			{
				using var reader = new StreamReader(path);
				using var csv = new CsvReader(reader, config);
				// Deal with when the header row isn't found
				csv.Read();
				csv.ReadHeader();
				if (csv.HeaderRecord == null)
				{
					data.Errors.Add($"{tName}::ParseCSV. Couldn't find header row in '{path}");
					return data;
				}
				data.SetHeaders(csv.HeaderRecord.ToList());
				List<string> f = csv.HeaderRecord.ToList();
				
				// For each row in the CSV file
				while (csv.Read())
				{
					// Check file format corrupt type errors.
					if (configError)
					{
						data.Errors.Add($"{tName}::ParseCSV. Path points to corrupt CSV file '{path}");
						return data;
					}
					List<string> row = new List<string>();
					// For each cell in the row
					for (int headerID = 0; headerID < csv.Parser.Count; headerID++)
					{
						// Get value as string
						row.Add(csv.Parser[headerID]);
					}
					// Check if the row has the same number of cells as there are headers
					if (row.Count < data.Headers.Count)
					{
						data.AddError($"{tName}::ParseCSV. Row{csv.Parser.Row} has less cells than there are headers");
					}
					else if (row.Count > data.Headers.Count)
					{
						data.AddError($"{tName}::ParseCSV. Row{csv.Parser.Row} has more cells than there are headers");
					}
					data.Rows.Add(row);
				}
			}
			catch (HeaderValidationException ex)
			{
				data.AddError($"{tName}::ParseCSV. Header row issue found: {ex.Message}");
			}
			catch (ReaderException ex)
			{
				data.AddError($"{tName}::ParseCSV. Unknown CsvHelper Reader exception: {ex.Message}");
			}
			catch (Exception ex)
			{
				data.AddError($"{tName}::ParseCSV. Unknown exception while parsing: {ex.Message}");
			}
			return data;
		}
		/*=====================
		 *  Instance Members
		 =====================*/
		public int Length { get { return Rows.Count; } }
		public List<string> Errors { get; protected set; } = new List<string>();
		public List<string> Headers { get; protected set; } = new List<string>();
		public List<List<string>> Rows { get; protected set; } = new List<List<string>>();
		protected Dictionary<string, float[]> NumericColumns = new Dictionary<string, float[]>();
		public Dictionary<string, int> HeaderDictionary { get; protected set; } = new Dictionary<string, int>();
		/*=====================
		 *  Instance Methods
		 =====================*/
		// Set headers: We're a generic <T> so we can't declare in constructor. Use AddHeader() for public
		protected void SetHeaders(List<string> headers)
		{
			// Populat headers
			Headers = headers;
			// Populate the header name lookup dictionary 
			for (int headerID = 0; headerID < headers.Count; headerID++)
			{
				HeaderDictionary.Add(headers[headerID], headerID);
			}
		}
		public bool HasErrors() { return Errors.Count > 0; }
		public void AddError(string error) { Errors.Add(error); }
		public bool ColumnExists(string name) { return Headers.Contains(name); }
		public bool ColumnIsNumeric(string name)
		{
			// Can't do it if the column doesn't exist
			if (!ColumnExists(name)) return false;
			// If exists and there's already a numeric array for it
			if (NumericColumns.ContainsKey(name)) return true;
			// Where the parsed floats go
			float[] values = new float[Rows.Count];
			// Which column index is the target column's
			int headerIndex = HeaderDictionary[name];
			// Where TryParse dumps parsed floats
			float theNumber = 0;
			// Do every row in the CSV
			for (int i = 0; i < Rows.Count; i++)
			{
				// If parsing to float fails, there's no point continuing
				if (!float.TryParse(Rows[i][headerIndex], out theNumber)) return false;
				// Otherwise, track the value
				values[i]	= theNumber;
			}
			// Track the column so we don't have to parse it again.
			NumericColumns.Add(name, values);
			// Self-explanatory
			return true;
		}
		/// <summary>
		/// Get a columns values as floats.
		/// </summary>
		/// <param name="columnName"></param>
		/// <returns></returns>
		public float[] GetNumericColumnValues(string columnName)
		{
			if (!ColumnIsNumeric(columnName))
				return Array.Empty<float>();
			return NumericColumns[columnName];
		}
		
		/// <summary>
		/// Find corrupt cells in the string data 
		/// </summary>
		/// <param name="columnName"></param>
		/// <returns></returns>
		public List<int> FindCorruptNumberRowIndicesInColumn(string columnName)
		{
			// Where the corrupt cell's row ID goes
			List<int> corruptIDs = new List<int>();
			// Which column index is the target column's
			int headerIndex = HeaderDictionary[columnName];
			// Where TryParse puts successful parses
			float theNumber = 0;
			// Check every row
			for (int i = 0; i < Rows.Count; i++)
				// Check if the value's a float. Includes integers
				if (!float.TryParse(Rows[i][headerIndex], out theNumber))
					corruptIDs.Add(i);
			// Self-explanatory
			return corruptIDs;
		}
		/// <summary>
		/// Print all errors
		/// </summary>
		public void PrintErrors()
		{
			for (int i = 0; i < Errors.Count; i++)
				Console.WriteLine($"{i}:\t{Errors[i]}");
		}
	}
}
