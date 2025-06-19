using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.ML
{
	/// <summary>
	/// Estimator Input Data
	/// 
	/// Most 
	/// </summary>
	public class EstimatorInputData
	{
		public string[] Headers { get; }
		public List<float[]> Rows { get; protected set; } = new List<float[]>();
		public int Length { get => Rows.Count; }
		public EstimatorInputData(string[] headers)
		{
			Headers	= headers;
		}
		public void AddRow(float[] row)
		{
			Rows.Add(row);
		}
		public void RemoveRow(int rowID) 
		{ 
			Rows.Remove(Rows[rowID]); 
		}
		
		public EstimatorInputData CloneRecordsFrom(int startIndex, int count)
		{
			EstimatorInputData data = new EstimatorInputData(Headers);
			for (int rowID = 0; rowID < count; rowID++)
			{
				data.Rows.Add(Rows[rowID]);
			}
			return data;
		}
		public EstimatorInputData GetRecordsBetween(int startIndex, int count)
		{
			EstimatorInputData data = new EstimatorInputData(Headers);
			for (int rowID = 0; rowID < count; rowID++)
			{
				data.Rows.Add(Rows[rowID]);
			}
			return data;
		}
		public EstimatorInputData GetLastRecords(int count)
		{
			EstimatorInputData data = new EstimatorInputData(Headers);
			count = count < Length ? count : Length;
			for (int rowID = Length - count; rowID < count;rowID++)
				data.AddRow(Rows[rowID]);
			return data;
		}
		public EstimatorInputData GetFirstRecords(int count)
		{
			EstimatorInputData data = new EstimatorInputData(Headers);
			count = count < Length ? count : Length;
			for (int rowID = 0; rowID < count; rowID++)
				data.AddRow(Rows[rowID]);
			return data;
		}
		public void FilterByFunction(Func<float[], bool> test)
		{
			List<float[]> existingRows = Rows;
			int length = Length;
			Rows = new List<float[]>();
			for (int rowID = 0; rowID < length; rowID++)
				if (!test(Rows[rowID]))
					AddRow(existingRows[rowID]);
		}
		public bool ToCSV(string path)
		{
			try
			{
				using (var writer = new StreamWriter(path))
				{
					writer.WriteLine(string.Join(",", Headers));
					for (int rowID = 0; rowID < Length; rowID++)
					{
						var row = new string[Rows[rowID].Length];
						for (int columnID = 0; columnID < Headers.Length; columnID++)
						{
							row[columnID] = Rows[rowID][columnID].ToString();
						}
						writer.WriteLine(string.Join(",", row));
					}

				}
			}
			catch(Exception exception)
			{
				Console.WriteLine(exception.Message);
				return false;
			}
			return true;
		}
	}
}
