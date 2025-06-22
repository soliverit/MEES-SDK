using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Optimisation
{
	public class GeneticSharpScoreSet
	{
		public List<GeneticSharpScoreStruct> Scores { get; set; } = new List<GeneticSharpScoreStruct>();
		public GeneticSharpScoreSet() { }
		public void AddScore(GeneticSharpScoreStruct score)
		{
			Scores.Add(score);
		}
		public GeneticSharpScoreStruct this[int scoreID] { 
			get => Scores[scoreID];
		}
	}
}
