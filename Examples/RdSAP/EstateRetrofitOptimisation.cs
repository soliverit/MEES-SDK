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
			// Build the data set.	
			MathNetRetrofitsTable data		= new MathNetRetrofitsTable(csvHandler, RdSAPRetrofitOption.ALL_RETROFIT_OPTION_KEYS.ToArray());
			// Create a genetic algorithm
			RdSAPEstateOptimiser optimiser	= new RdSAPEstateOptimiser(data);
			optimiser.InitialPopulationSize = 50;
			optimiser.MaximumPopulationSize = 100;
			optimiser.MutationProbability	= 0.02f;
			optimiser.Generations           = 5000;
			// Optimise.
			optimiser.Run();
		}
		public string GetDescription()
		{
			return @"

";
		}
	}
}
