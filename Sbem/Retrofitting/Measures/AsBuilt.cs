using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem.Retrofitting.Measures
{
	public class AsBuilt : RetrofitBase
	{
		public AsBuilt(SbemModel model) : base(model) { }
		public override void Apply()
		{

		}
		public override float Cost { get => 0; }
		public override float Area { get => 0; }
	}
}
