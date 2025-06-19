using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.RdSAP.Reference
{
	public class GlazingTypeRecord : ReferenceDataRecordBase
	{
		public string Label { get; }
		public string When { get; }
		public float UValue { get; }
		public float GValue { get; }

		public GlazingTypeRecord(string label, string when, float uValue, float gValue)
		{
			Label = label;
			When = when;
			UValue = uValue;
			GValue = gValue;
		}
	}
}


