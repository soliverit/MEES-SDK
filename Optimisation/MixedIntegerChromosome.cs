using GeneticSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Chromosome
{
	/// <summary>
	/// GeneticSharp doesn't natively support mixed domain integer problems. For example, 
	/// where one building have 5 retrofits, another has 10. This class makes it possible.
	/// </summary>
	public class MixedIntegerChromosome : ChromosomeBase
	{
		private readonly int[] _minValues;
		private readonly int[] _maxValues;
		/// <summary>
		/// Default constructor. Tell it th lowest values and highest values for genes.
		/// </summary>
		/// <param name="minValues"></param>
		/// <param name="maxValues"></param>
		public MixedIntegerChromosome(int[] minValues, int[] maxValues) : base(minValues.Length)
		{
			_minValues = minValues;
			_maxValues = maxValues;
			for (int i = 0; i < Length; i++)
				ReplaceGene(i, GenerateGene(i));
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
