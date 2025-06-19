using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MeesSDK.RdSAP.Reference
{
	public class ThermalBridgingRecord : ReferenceDataRecordBase
	{
		public string Band { get; }
		public float Factor { get; }

		public ThermalBridgingRecord(string band, float factor)
		{
			Band = band;
			Factor = factor;
		}
	}
}
