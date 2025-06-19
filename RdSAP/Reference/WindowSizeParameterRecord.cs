using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MeesSDK.RdSAP.Reference
{
	public class WindowSizeParametersRecord : ReferenceDataRecordBase
	{
		public string Label { get; }
		public float House { get; }
		public float Bungalow { get; }
		public float Flat { get; }
		public float Maisonette { get; }

		public float HousePlus { get; }
		public float BungalowPlus { get; }
		public float FlatPlus { get; }
		public float MaisonettePlus { get; }

		public WindowSizeParametersRecord(string label, float house, float bungalow, float flat, float maisonette,
										  float housePlus, float bungalowPlus, float flatPlus, float maisonettePlus)
		{
			Label = label;
			House = house;
			Bungalow = bungalow;
			Flat = flat;
			Maisonette = maisonette;
			HousePlus = housePlus;
			BungalowPlus = bungalowPlus;
			FlatPlus = flatPlus;
			MaisonettePlus = maisonettePlus;
		}
	}
}
