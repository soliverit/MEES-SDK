using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.RdSAP.Reference
{
	public class ConstructionAgeRecord : ReferenceDataRecordBase
	{
		public string Label { get; }
		public string Band { get; }
		public int Index { get; }

		public ConstructionAgeRecord(string label, string band, int index)
		{
			Label = label;
			Band = band;
			Index = index;
		}
	}
}
