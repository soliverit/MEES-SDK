using GeneticSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Chromosome
{
	public class MixedIntegerChromosome : ChromosomeBase
	{
		private readonly int[] _minValues;
		private readonly int[] _maxValues;

		public MixedIntegerChromosome(int[] minValues, int[] maxValues) : base(minValues.Length)
		{
			_minValues = minValues;
			_maxValues = maxValues;

			for (int i = 0; i < Length; i++)
			{
				ReplaceGene(i, GenerateGene(i));
			}
		}

		public override Gene GenerateGene(int index)
		{
			int val = RandomizationProvider.Current.GetInt(_minValues[index], _maxValues[index] + 1);
			return new Gene(val);
		}

		public override IChromosome CreateNew()
		{
			return new MixedIntegerChromosome(_minValues, _maxValues);
		}

		public int[] GetValues()
		{
			return GetGenes().Select(g => (int)g.Value).ToArray();
		}
	}

}
