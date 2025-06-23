using MeesSDK.DataManagement;
using MeesSDK.Examples.RdSAP;
using System.Threading.Tasks;

namespace MeesSDK.Optimisation
{
	/*
	 *  Threaded! Iterative! Partitioning! Not yet ported over from Python..
	 *  
	 *  This class will eventually be ported from JAE-MOO python. The library
	 *  does efficient subset optimisation to make searching the decision space
	 *  more effective in earlier rounds.
	 */ 
	public abstract class JAEMOO<T> where T : GeneticAlgorithmBase
	{
		/// <summary>
		/// Number of subsets.
		/// </summary>
		public int PartitionCount { get; set; } = 1;
		/// <summary>
		/// Number of threads. N - 2 cores is probably as efficient as it gets.
		/// </summary>
		public int ThreadCount { get; set; }	= 1;
		/// <summary>
		/// Initial population size
		/// </summary>
		public int InitialPopulationSize { get; set; } = 50;
		/// <summary>
		/// Maximum population size
		/// </summary>
		public int MaximumPopulationSize { get; set; } = 50;
		/// <summary>
		/// Maximum number of generation/rounds
		/// </summary>
		public int Generations { get; set; } = 50;
		/// <summary>
		/// The probability of crossover. E.g 0.75 is a 75% chance to mix two chromosome. Otherwise, clone
		/// the current one.
		/// </summary>
		public float CrossoverProbability { get; set; } = 0.75f;
		/// <summary>
		/// The probability of mutation. E.g. 0.1 is a 10% chance to change a gene's value.using
		/// the MutationType method. Uniform random by default.
		/// </summary>
		public float MutationProbability { get; set; } = 0.1f;

		/// <summary>
		/// The weight of each score.
		/// </summary>
		public float[] ObjectiveWeights { get; set; } = new float[1] { 1.0f };
		/// <summary>
		/// Indices in the MatNetR
		/// </summary>
		public int[] RecordIDs { get; set; }
		public JAEMOO(MathNetRetrofitsTable data) 
		{
			Data		= data;
			RecordIDs	= Enumerable.Range(0, Data.Length).ToArray();
		}
		public MathNetRetrofitsTable Data { get; protected set; }
		public abstract T CreateAlgorithm(int[] recordIDs);
		/// <summary>
		/// Run the optimiser
		/// </summary>
		public void Run()
		{
			_run().GetAwaiter().GetResult(); // Wait for async method to complete
		}
		/// <summary>
		/// The main method. The RecordIDs are split into Partitions with PartitionData(). Each
		/// is processed on ThreadCount threads until everything's done. Exit wait loop is
		/// in the public Run().
		/// </summary>
		/// <returns></returns>
		protected async Task _run()
		{
			List<Task> tasks		= new List<Task>();	
			SemaphoreSlim semaphore	= new SemaphoreSlim(ThreadCount);
			int[][] partitions		= PartitionRecordIDs(); 
			for (int i = 0; i < partitions.Length ; i++)
			{
				await semaphore.WaitAsync(); //
				int threadNum = i; // capture loop variable
				tasks[i] = Task.Run(() =>
				{
					Console.WriteLine($"Thread {threadNum} started.{partitions.Length} - {threadNum}");
					Console.WriteLine(partitions.Length);
					Console.WriteLine(partitions[threadNum].Length);
					T algorithm						= CreateAlgorithm(partitions[threadNum]);
					algorithm.InitialPopulationSize	= InitialPopulationSize;
					algorithm.MaximumPopulationSize = MaximumPopulationSize;
					algorithm.Generations           = Generations;
					algorithm.Run();
					Console.WriteLine($"Thread {threadNum} finished.");
				});
			}
			Task.WaitAll(tasks.ToArray()); // Wait for all threads
			Console.WriteLine("All threads complete.");
		}
		/// <summary>
		/// Split RecordIDs into subsets.
		/// <para>Note: Usually, this should return PartitionCount subsets, but 
		/// it's not an obligation.</para>
		/// </summary>
		/// <returns></returns>
		public virtual int[][] PartitionRecordIDs() 
		{ 
			return RecordIDs.Select((val, i) => new { val, group = i * PartitionCount / RecordIDs.Length })
							.GroupBy(x => x.group)
							.Select(g => g.Select(x => x.val).ToArray())
							.ToArray();
		}
	}
}
