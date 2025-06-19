using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MeesSDK.RdSAP.Reference
{
	public class WallConstructionRecord : ReferenceDataRecordBase
	{

		public string WallType { get; }
		public string Insulation { get; }
		public Dictionary<string, float> BandValues { get; } = new();

		public WallConstructionRecord(string wallType, string insulation, Dictionary<string, float> bandsDict)
		{
			WallType = wallType;
			Insulation = insulation;
			BandValues = bandsDict;
		}

		public float GetValueForBand(string band) =>
			BandValues.TryGetValue(band, out var val) ? val : float.NaN;
	}
}
