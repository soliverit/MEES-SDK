using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// A class for helpful printing methods. E.g. PadArray or PrinTable (to console).
	/// - Pad arrays
	/// </summary>
	public class PHelper
	{
		/// <summary>
		/// An alias for Console.WriteLine.
		/// </summary>
		/// <param name="value"></param>
		public static void p(string value) { Console.WriteLine(value); }
		/// <summary>
		/// Pad a string from the right until it's the target length. 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="length"></param>
		/// <param name="padChar">The character the string is padded with</param>
		/// <returns></returns>
		public static string Pad(string value, int length, string padChar= " ")
		{
			// Prevent infinite loop
			if (padChar == "")
				padChar = " ";
			while (value.Length < length)
				value += padChar;
			return value;
		}
		/// <summary>
		/// Pad a 2D list by the max widths of values in the columns.
		/// </summary>
		/// <param name="values"></param>
		/// <returns></returns>
		public static List<List<string>> PadByColumns(List<List<string>> values)
		{
			for (int columnID = 0; columnID < values[0].Count; columnID++)
			{
				int currentLength = 0;
				for (int rowID = 0; rowID < values.Count; rowID++)
					if (values[rowID][columnID].Length > currentLength)
						currentLength = values[rowID][columnID].Length;
				currentLength += 1;
				for (int rowID = 0; rowID < values.Count; rowID++)
					values[rowID][columnID] = Pad(values[rowID][columnID], currentLength);
			}
			return values;
		}
		/// <summary>
		/// Print a formatted table to the console.
		/// </summary>
		/// <param name="cells"></param>
		public static void PrintTable(List<List<string>> cells)
		{
			cells = PadByColumns(cells);
			// Get line length
			int lineLength = 0;
			for (int cellID = 0; cellID < cells[0].Count; cellID++)
				lineLength += cells[0][cellID].Length;
			string bar = Pad("-", lineLength, "-");
			Console.WriteLine(bar);
			Console.WriteLine(string.Join("", cells[0]));
			Console.WriteLine(bar);
			for (int rowID = 1; rowID < cells.Count; rowID++)
				Console.WriteLine(string.Join("", cells[rowID]));
			Console.WriteLine(bar);
		}
	}
}
