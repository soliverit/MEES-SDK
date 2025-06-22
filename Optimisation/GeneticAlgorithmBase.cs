using GeneticSharp;
using MeesSDK.Chromosome;
using MeesSDK.DataManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LinqToDB.Common.Configuration;

namespace MeesSDK.Optimisation
{
	public abstract class GeneticAlgorithmBase
	{	
		/// <summary>
		/// The Genetic Algorithm.
		/// </summary>
		public GeneticAlgorithm Algorithm { get; protected set; }
		/// <summary>
		/// The function that determines the quality of a given solution. E.g. payback period.
		/// </summary>
		protected FuncFitness FitnessFunction { get; set; }
		/// <summary>
		/// Every population from initial to final is stored here. 
		/// <code>- CurrentGeneration
		/// - BestChromosome
		/// - InitialPopulation</code>
		/// </summary>
		public Population PopulationHistory { get; protected set; }
		/// <summary>
		/// The collection of 2D arrays that define the decision space.
		/// </summary>
		public MathNetRetrofitsTable Data { get; protected set; }
		/// <summary>
		/// Initial population size
		/// </summary>
		public int InitialPopulationSize { get; set; }		= 50;
		/// <summary>
		/// Maximum population size
		/// </summary>
		public int MaximumPopulationSize {  get; set; }		= 50;
		/// <summary>
		/// Maximum number of generation/rounds
		/// </summary>
		public int Generations { get; set; }				= 50;
		/// <summary>
		/// The probability of crossover
		/// </summary>
		public float CrossoverProbability { get; set; }		= 0.9f;

		/// <summary>
		/// The weight of each score. TODO: Add weighting.
		/// </summary>
		public float[] ObjectiveWeights { get; set; }		= new float[1] { 1.0f };
		/// <summary>
		/// Crossover Type: 
		/// <code>- OnePointCrossover (single-point split)
		/// - TwoPointCrossover(double split)
		/// - UniformCrossover(mix by gene with MixProbability
		///	- OrderedCrossover
		///	- CycleCrossover,...  </code>
		/// </summary>
		public ICrossover CrossoverType { get; set; }			= new UniformCrossover();
		public MutationBase MutationType { get; set; }			= new UniformMutation(true);
		public SelectionBase SelectionType { get; set; }		= new EliteSelection();
		public int[] RecordIDs { get; protected set; }
		public GeneticAlgorithmBase(MathNetRetrofitsTable data)
		{
			Data		= data;
			RecordIDs	= Enumerable.Range(0, data.Length).ToArray();
		}
		/// <summary>
		/// The fitness / score function. This determines how good a chromosome is.
		/// </summary>
		/// <param name="chromosome"></param>
		/// <returns></returns>
		public abstract float[] Score(IChromosome chromosome);
		public virtual IChromosome CreateChromosome()
		{
			return new MixedIntegerChromosome(GetLowerBounds(), GetUpperBounds());
		}
		/// <summary>
		/// Define the minimum value for each gene
		/// </summary>
		/// <returns></returns>
		public virtual int[] GetLowerBounds()
		{
			return new int[Data.Length]; 
		}
		/// <summary>
		/// Get the maximum value for 
		/// </summary>
		/// <returns></returns>
		public abstract int[] GetUpperBounds();
		public void Run()
		{
			int[] ids			= Enumerable.Range(0, Data.Length).ToArray();
			PopulationHistory   = new Population(InitialPopulationSize, MaximumPopulationSize, CreateChromosome());
			FuncFitness fitness	= new FuncFitness(c =>
			{
				MixedIntegerChromosome f = c as MixedIntegerChromosome;
				float[] scores	= Score(c);
				float total		= 0;
				for(int scoreID = 0; scoreID < scores.Length; scoreID++)
					total	+= scores[scoreID] * ObjectiveWeights[scoreID];
				Console.WriteLine($"Score: {total}");
				return total;
			});
			Algorithm = new GeneticAlgorithm(
				PopulationHistory,
				fitness,
				SelectionType,
				CrossoverType,
				MutationType
			);
			Algorithm.Start();
			Console.WriteLine("========= We made it! =========");
		}
	}
}
