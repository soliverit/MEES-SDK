using GeneticSharp;
using MeesSDK.Chromosome;
using MeesSDK.DataManagement;
using MeesSDK.Optimisation;
namespace MeesSDK.RdSAP
{
	public class RdSAPEstateOptimiser : GeneticAlgorithmBase
	{
		public RdSAPEstateOptimiser(MathNetRetrofitsTable retrofits) : base(retrofits) { }
		public override float[] Score(IChromosome chromosome)
		{
			MixedIntegerChromosome c	= chromosome as MixedIntegerChromosome;
			int[] columns				= c.GetValues();
			float cost					= 0;
			float diff					= 0;
			for (int i = 0; i < columns.Length; i++)
			{
				cost    += Data.Costs[i, columns[i]];
				diff    += Data.Differences[i, columns[i]];
			}
			return new float[1] { -1 * cost };
		}
		public override int[] GetUpperBounds()
		{
			return Enumerable.Repeat(15, Data.Length).ToArray();
		}
	}
}
