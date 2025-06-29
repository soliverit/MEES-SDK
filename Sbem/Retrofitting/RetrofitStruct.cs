using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem.Retrofitting
{
	public struct RetrofitStruct
	{
		public SbemProject Project { get; set; }
		public RetrofitBase Retrofit { get; }
		public RetrofitStruct(SbemProject project, RetrofitBase retrofit)
		{
			Project		= project;
			Retrofit	= retrofit;
		}
	}
}
