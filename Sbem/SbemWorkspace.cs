using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public class SbemWorkspace
	{
		public SbemModel AsBultModel { get; }
		public SbemProject AsBuiltProject { get; }
		public SbemService SbemCaller { get; }
		public SbemWorkspace(SbemProject project, SbemService sbem)
		{
			SbemCaller		= sbem;
			AsBuiltProject	= project;;
			AsBultModel		= project.AsBuiltSbemModel;
		}
	}
}
