using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem.Retrofitting
{
	public class Analyst
	{
		public SbemProject Project{ get; protected set; }
		public List<SbemScenario> Scenarios { get; protected set; }
		public Analyst(SbemProject project, SbemService sbem)
		{
			Project = project;
		}
		
	}
}
