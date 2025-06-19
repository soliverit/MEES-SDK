using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.ML
{
	public class PredictionSet
	{
		public static float GetRMSE(float[] predictions, float[] targets)
		{
			float sum = 0;
			for (int rowID = 0; rowID < predictions.Length; rowID++)
				sum += MathF.Pow((predictions[rowID] - targets[rowID]), 2);
			return MathF.Pow(sum / predictions.Length, 0.5f);
		}
		public static float GetR2(float[] predictions, float[] targets)
		{
			float mean = 0;
			for (int i = 0; i < targets.Length; i++)
				mean += targets[i];
			mean /= targets.Length;

			float ssTot = 0;
			float ssRes = 0;

			for (int i = 0; i < targets.Length; i++)
			{
				float diff = targets[i] - predictions[i];
				ssRes += diff * diff;

				float deviation = targets[i] - mean;
				ssTot += deviation * deviation;
			}
			return 1 - ssRes / ssTot;
		}
		public float[] Predictions { get; protected set; }
		public float[]? Targets { get; protected set; }
		public bool HasTargets { get; protected set; } = false;
		public float RMSE { get {
				_doneRMSE = true;
				return GetRMSE(Predictions, Targets); 
			}}
		protected bool _doneRMSE { get; set; } = false;
		public float R2
		{
			get
			{
				_doneR2 = true;
				return GetR2(Predictions, Targets);
			}
		}
		protected bool _doneR2 { get; set; } = false;
		public PredictionSet(float[] predictions, float[] targets) 
		{
			Predictions = predictions;
			Targets = targets;
			HasTargets = true;
		}
		public PredictionSet(float[] predictions)
		{
			Predictions = predictions;
		}
		public void SetTargets(float[] targets)
		{
			Targets = targets;
			HasTargets = true;
		}
	}
}
