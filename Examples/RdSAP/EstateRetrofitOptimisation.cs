using MeesSDK.Chromosome;
using MeesSDK.DataManagement;
using MeesSDK.Optimisation;
using MeesSDK.RdSAP;
using MeesSDK.RdSAP.Optimisation;


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
			// Print any errors
			csvHandler.PrintErrors();
			// Build the data set.	
			MathNetRetrofitsTable data		= new MathNetRetrofitsTable(csvHandler, RdSAPRetrofitOption.ALL_RETROFIT_OPTION_KEYS.ToArray());
			// Create a genetic algorithm
			RdSAPEstateOptimiser optimiser	= new RdSAPEstateOptimiser(data, Enumerable.Range(0, data.Length).ToArray());
			// Set the number of agents in the initial population
			optimiser.InitialPopulationSize = 50;
			// Set the maximum number of agents in generation 2 onwards
			optimiser.MaximumPopulationSize = 100;
			// The probability that the secondary parent's gene(s) will be replaced by the second's
			optimiser.CrossoverProbability	= 0.75f;
			// The probability that genes will mutate after the crossover round.
			optimiser.MutationProbability	= 0.02f;
			// Maximum number of generations (Called Termination in GeneticSharp)
			optimiser.Generations           = 500;
			// Run it.
			optimiser.Run();
		}
		public string GetDescription()
		{
			return @"

";
		}
	}
}
