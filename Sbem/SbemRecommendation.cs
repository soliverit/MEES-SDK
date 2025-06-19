using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public class SbemRecommendation : SbemObject
	{
		public SbemRecommendation(string name, List<string> currentProperties) : base(name, currentProperties) { }
		public const string OBJECT_NAME = "RECOMMENDATION";
		public override string ObjectName() { return OBJECT_NAME; }

	}
}
