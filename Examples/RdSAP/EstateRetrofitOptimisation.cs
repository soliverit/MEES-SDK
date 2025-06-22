using MeesSDK.Chromosome;
using MeesSDK.DataManagement;
using MeesSDK.Optimisation;
using MeesSDK.RdSAP;


namespace MeesSDK.Examples.RdSAP
{
	public class EstateRetrofitOptimisation : IMeesSDKExample
	{
		public EstateRetrofitOptimisation(string inputDataPath) { InputDataPath = inputDataPath; }
		/// <summary>
		/// The path to the certificates.csv retrofits. This csv has three columns per retrofit label
		/// of all combinations of four heating, glazing, wall, and roof retrofit measures.
		/// </summary>
		public string InputDataPath { get; protected set; }
		/// <summary>
		/// Generating an optimal residential estate retrofit strategy using a genetic algorithm
		/// </summary>
		public void RunTheExample()
		{

			// Load data
			CsvHandler csvHandler = CsvHandler.ParseCSV(InputDataPath);
			csvHandler.PrintErrors();


			MathNetRetrofitsTable data		= new MathNetRetrofitsTable(csvHandler, RdSAPRetrofitOption.ALL_RETROFIT_OPTION_KEYS.ToArray());
			// Create a genetic algorithm
			RdSAPEstateOptimiser optimiser = new RdSAPEstateOptimiser(data);
			optimiser.Run();
		}
		public string GetDescription()
		{
			return @"

";
		}
	}
}
