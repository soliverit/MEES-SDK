using MeesSDK.Sbem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Examples
{
	public abstract  class SbemExampleBase : IMeesSDKExample
	{
		public SbemService SbemHandler { get; }
		public SbemProject Project { get; }
		public abstract string GetDescription();
		public abstract void DoTheExample();
		public string Name { get; set; } = "";
		public SbemExampleBase(SbemProject project, SbemService service)
		{
			Project     = project;
			SbemHandler = service;
		}
	}
}
