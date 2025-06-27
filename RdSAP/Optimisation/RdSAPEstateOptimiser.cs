using GeneticSharp;
using MeesSDK.Chromosome;
using MeesSDK.DataManagement;
using MeesSDK.Optimisation;
namespace MeesSDK.RdSAP.Optimisation
{
	public class RdSAPEstateOptimiser : GeneticAlgorithmBase
	{
		public RdSAPEstateOptimiser(MathNetRetrofitsTable retrofits, int[] recordIDs) : base(retrofits, recordIDs) { }
		public override float[] Score(IChromosome chromosome, int[] recordIDs)
		{
			MixedIntegerChromosome c	= chromosome as MixedIntegerChromosome;
			int[] columns				= c.GetValues();
			float cost					= 0;
			float diff					= 0;
			for (int i = 0; i < columns.Length; i++)
			{
				cost    += Data.Costs[recordIDs[i], columns[i]];
				diff    += Data.Differences[recordIDs[i], columns[i]];
			}
			return new float[1] { -1 * cost };
		}
		public override int[] GetUpperBounds()
		{
			return Enumerable.Repeat(15, RecordIDs.Length).ToArray();
		}
	}
}
