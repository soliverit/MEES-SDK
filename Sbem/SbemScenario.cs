using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public class SbemScenario
	{
		public SbemScenario() { }
		public SbemModel BaseModel { get; protected set; }
		public SbemEpcModel BaseEpcInpModel { get; protected set; }
		public Dictionary<string, SbemProject> Projects { get; protected set; }

	}
}
