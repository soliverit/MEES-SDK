using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single; 

namespace MeesSDK.DataManagement
{
	public class MathNetRetrofitsTable
	{
		/*
		 *	Constructors
		 */
		public MathNetRetrofitsTable(CsvHandler csv, string[] retrofitAliases)
		{
			CsvHandler			= csv;
			ActiveBuildingMask	= Enumerable.Range(0, csv.Length).ToArray();
			BuildTables(retrofitAliases);
		}

		/*
		 *  Static Members
		 */
		public static readonly string DIFF_SUFFIX	= "-Diff";
		public static readonly string COST_SUFFIX	= "-Cost";
		/*
		 *  Static Methods
		 */
		/*
		 *  Instance Members
		 */
		public int Length { get => Retrofits.Length; }
		protected CsvHandler CsvHandler { get; set; }
		public bool BuiltTables;
		public int[] ActiveBuildingMask;
		public int[][] Retrofits									=  Array.Empty<int[]>();
		protected Dictionary<string, int> RetrofitAliasedIndices	= new Dictionary<string, int>();
		protected Dictionary<int, string> RetrofitIndexAliases		= new Dictionary<int, string>();
		public Matrix<float> Costs									= Matrix<float>.Build.Dense(0, 0);
		public Matrix<float> Differences							= Matrix<float>.Build.Dense(0, 0);
		public Dictionary<string, Vector<float>> DataTables			= new Dictionary<string, Vector<float>>();
		/*
		 *  Instance Methods
		 */
		public void BuildTables(string[] retrofitAliases)
		{
			// Make sure we don't build the tables again
			if (BuiltTables)
				return;
			float[][] costs = new float[retrofitAliases.Length][];
			float[][] diffs = new float[retrofitAliases.Length][];
			// Prepare all retroifts
			for (int aliasID = 0; aliasID < retrofitAliases.Length; aliasID++)
			{
				RetrofitAliasedIndices[retrofitAliases[aliasID]]	= aliasID;
				RetrofitIndexAliases[aliasID]						= retrofitAliases[aliasID];
				string costColumnName								= retrofitAliases[aliasID] + COST_SUFFIX;
				string diffColumnName								= retrofitAliases[aliasID] + DIFF_SUFFIX;
				costs[aliasID]										= CsvHandler.GetNumericColumnValues(costColumnName);
				diffs[aliasID]										= CsvHandler.GetNumericColumnValues(diffColumnName);
			}
			// Transpose the table
			int rows = CsvHandler.Length;
			int cols = retrofitAliases.Length;
			float[,] transposedCosts = new float[rows, cols];
			float[,] transposedDiffs = new float[rows, cols];
			
			for (int columnID = 0; columnID < cols; columnID++)
			{
				for (int rowID = 0; rowID < rows; rowID++)
				{
					transposedCosts[rowID, columnID] = costs[columnID][rowID];
					transposedDiffs[rowID, columnID] = diffs[columnID][rowID];
				}
			}
			int[][] retrofits	= new int[rows][];
			for(int rowID = 0; rowID < rows; rowID++)
				retrofits[rowID]	= Enumerable.Range(0, cols).ToArray();
			// Create arrays
			Costs		= Matrix.Build.DenseOfArray(transposedCosts);
			Differences	= Matrix.Build.DenseOfArray(transposedDiffs);
			Retrofits   = retrofits;
			// Make sure we don't build the tables again
			BuiltTables = true;
		}
		/*
			Create dictionary
		 */
		public bool CreateDictionary(string columnName)
		{
			// Return false if the key doesn't exist in the Csv data
			if(!CsvHandler.ColumnExists(columnName)) 
				return false;
			// Return false if the dictionary already exists
			if(DataTables.ContainsKey(columnName)) 
				return false; 
			if(!CsvHandler.ColumnIsNumeric(columnName)) 
				return false;
			// Add the new 
			DataTables[columnName] = Vector.Build.DenseOfArray(CsvHandler.GetNumericColumnValues(columnName).ToArray());
			return true;
		}
		//---- Math stuff ---//
		public float SumCosts(int[] rows, int[] columns)
		{
			float sum = 0;
			for(int i  = 0; i < rows.Length; i++)
			{
				sum += Costs[rows[i],columns[i]];
			}
			return sum;
		}
	}
}
