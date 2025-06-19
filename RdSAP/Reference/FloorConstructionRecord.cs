using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.RdSAP.Reference
{
	namespace MOOSandbox.RdSAP.Reference
	{
		public class FloorConstructionRecord : ReferenceDataRecordBase
		{
			public string Band { get; }
			public float UValueUnknown { get; }
			public float UValue50mm { get; }
			public float UValue100mm { get; }
			public float UValue150mm { get; }

			public FloorConstructionRecord(string band, float unknown, float u50, float u100, float u150)
			{
				Band = band;
				UValueUnknown = unknown;
				UValue50mm = u50;
				UValue100mm = u100;
				UValue150mm = u150;
			}
		}
	}

}
