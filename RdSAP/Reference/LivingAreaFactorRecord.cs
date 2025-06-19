using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.RdSAP.Reference
{
	public class LivingAreaFactorRecord: ReferenceDataRecordBase
	{
		public int Index { get; }
		public float Factor { get; }

		public LivingAreaFactorRecord(int index, float factor)
		{
			Index = index;
			Factor = factor;
		}
	}
}
