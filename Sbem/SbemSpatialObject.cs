using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public abstract class SbemSpatialObject : SbemObject
	{
		public float Area { get; protected set; }
		public SbemSpatialObject(string name, List<string> properties) : base(name, properties) 
		{
			if (HasNumericProperty("AREA"))
				Area = GetNumericProperty("AREA").Value;
			else
				Area = 0;
		}
	}
}
