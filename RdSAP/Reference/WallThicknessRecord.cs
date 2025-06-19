using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MeesSDK.RdSAP.Reference
{
	public class WallThicknessRecord : ReferenceDataRecordBase
	{
		public string Key { get; }
		public Dictionary<string, float> BandValues { get; } = new();

		public WallThicknessRecord(string key, List<string> bands, List<string> values)
		{
			
			Key = key;
			for (int i = 0; i < bands.Count; i++)
			{
				BandValues[bands[i]] = float.Parse(values[i]);
			}
		}

		public float GetThicknessForBand(string band) =>
			BandValues.TryGetValue(band, out var val) ? val : float.NaN;
	}
}
