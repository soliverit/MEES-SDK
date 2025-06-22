using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Optimisation
{
	public struct GeneticSharpScoreStruct
	{ 
		public float[] Scores { get; }
		public GeneticSharpScoreStruct(float[] scores)
		{
			Scores	= scores;
		}
		/// <summary>
		/// Get the multi-objective score.
		/// </summary>
		/// <param name="weights"></param>
		/// <returns></returns>
		public float WeightedScore(float[] weights)
		{
			float score = 0;
			for(int scoreID = 0;  scoreID < Scores.Length; scoreID++) 
				score += Scores[scoreID] * weights[scoreID];
			return score;
		}
		public float WeightedAverageScore(float[] weights)
		{
			return WeightedScore(weights);
		}
	}
}
