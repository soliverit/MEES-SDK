using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MeesSDK.RdSAP.Reference
{
	public class RoofConstructionRecord : ReferenceDataRecordBase
	{
		public string Band { get; }
		public float UValuePitched { get; }
		public float UValueFlat { get; }
		public float UValueRoom { get; }

		public RoofConstructionRecord(string band, float pitched, float flat, float room)
		{
			Band = band;
			UValuePitched = pitched;
			UValueFlat = flat;
			UValueRoom = room;
		}
	}
}
