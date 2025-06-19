using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.RdSAP.Reference
{
	public class DoorConstructionRecord : ReferenceDataRecordBase
	{
		public string Band { get; }
		public float Factor { get; }

		public DoorConstructionRecord(string band, float factor)
		{
			Band = band;
			Factor = factor;
		}
	}
}

