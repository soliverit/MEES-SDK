using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.ML
{
	public class LightGBMScore
	{
		public float Score {  get; set; }

		public float RealError { get; protected set; }
		public float AbsoluteError { get => MathF.Abs(RealError); }
		public float SquaredError { get => MathF.Pow(RealError, 2); }
	}
}
