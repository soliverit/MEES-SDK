using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Optimisation
{
	public struct GeneticAlgorithmDataStruct
	{
		public GeneticAlgorithmDataStruct() { }
		public Dictionary<string, string[]> StringFeatures { get; } = new Dictionary<string, string[]>();
		public Dictionary<string, float[]> FloatFeatures { get; } = new Dictionary<string, float[]>();
	}
}
