﻿	using GeneticSharp;
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
		/// The probability of crossover. E.g 0.75 is a 75% chance to mix two chromosome. Otherwise, clone
		/// the current one.
		/// </summary>
		public float CrossoverProbability { get; set; }		= 0.75f;
		/// <summary>
		/// The probability of mutation. E.g. 0.1 is a 10% chance to change a gene's value.using
		/// the MutationType method. Uniform random by default.
		/// </summary>
		public float MutationProbability { get; set; }		= 0.1f;

		/// <summary>
		/// The weight of each score.
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
		/// <summary>
		/// GeneticSharp MutationBase
		/// </summary>
		public MutationBase MutationType { get; set; }			= new UniformMutation(true);
		/// <summary>
		/// GeneticSharp SelectionBase
		/// </summary>
		public SelectionBase SelectionType { get; set; }		= new EliteSelection();
		/// <summary>
		/// IDs of records in Data that are paired with the chromosome
		/// </summary>
		public int[] RecordIDs { get; protected set; }
		/// <summary>
		/// The score struct.
		/// </summary>
		public GeneticSharpScoreSet Scores { get; protected set; }
		public GeneticAlgorithmBase(MathNetRetrofitsTable data, int[] recordIDs)
		{
			Data		= data;
			RecordIDs   = recordIDs;
		}
		/// <summary>
		/// The fitness / score function. This determines how good a chromosome is.
		/// </summary>
		/// <param name="chromosome"></param>
		/// <returns></returns>
		public abstract float[] Score(IChromosome chromosome, int[] recordIDs);
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
			return new int[RecordIDs.Length]; 
		}
		/// <summary>
		/// Get the maximum value for 
		/// </summary>
		/// <returns></returns>
		public abstract int[] GetUpperBounds();
		public void Run()
		{
			// Reset scores
			Scores				= new GeneticSharpScoreSet();
			// Create a fresh population history
			PopulationHistory   = new Population(InitialPopulationSize, MaximumPopulationSize, CreateChromosome());
			// This calls the abstract GenetiAlgorithmBase Score() method.
			FuncFitness fitness	= new FuncFitness(c =>
			{
				MixedIntegerChromosome f	= c as MixedIntegerChromosome;
				float[] scores				= Score(c, RecordIDs);
				float total					= 0;
				// Dot product objectives 
				for(int scoreID = 0; scoreID < scores.Length; scoreID++)
					total	+= scores[scoreID] * ObjectiveWeights[scoreID];
				return total;
			});
			// Create the algorithm. Every can be update before calling this Run() call
			Algorithm = new GeneticAlgorithm(
				PopulationHistory,
				fitness,
				SelectionType,	// How candidates are selected after scoring
				CrossoverType,	// How crossover is done
				MutationType	// How values are mutated
			);
			Algorithm.CrossoverProbability	= CrossoverProbability;
			Algorithm.MutationProbability   = MutationProbability;
			Algorithm.Termination			= new GenerationNumberTermination(Generations);
			Algorithm.Start();
		}
	}
}
