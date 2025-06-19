using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.RdSAP
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	public class RdSAPRetrofitOption
	{
		public string Description { get; }
		public int MeasureCount { get; }
		public string CostKey { get; }
		public string EfficiencyKey { get; }
		public string DifferenceKey { get; }
		public string[] Measures { get; }

		private RdSAPRetrofitOption(string description, int measureCount)
		{
			Description = description;
			MeasureCount = measureCount;
			CostKey = description + "-Cost";
			EfficiencyKey = description + "-Eff";
			DifferenceKey = description + "-Diff";
			Measures = description.Split('_');
		}

		public bool HasRoof => Measures.Contains("roof");
		public bool HasEnvelopes => Measures.Contains("envelopes");
		public bool HasWindows => Measures.Contains("windows");
		public bool HasHotwater => Measures.Contains("hotwater");

		public static readonly RdSAPRetrofitOption ENVELOPES_HOTWATER_ROOF_WINDOWS = new("envelopes_hotwater_roof_windows", 4);
		public static readonly RdSAPRetrofitOption HOTWATER_ROOF_WINDOWS = new("hotwater_roof_windows", 3);
		public static readonly RdSAPRetrofitOption HOTWATER_ROOF = new("hotwater_roof", 2);
		public static readonly RdSAPRetrofitOption ROOF = new("roof", 1);
		public static readonly RdSAPRetrofitOption HOTWATER = new("hotwater", 1);
		public static readonly RdSAPRetrofitOption ROOF_WINDOWS = new("roof_windows", 2);
		public static readonly RdSAPRetrofitOption WINDOWS = new("windows", 1);
		public static readonly RdSAPRetrofitOption HOTWATER_WINDOWS = new("hotwater_windows", 2);
		public static readonly RdSAPRetrofitOption ENVELOPES_HOTWATER_ROOF = new("envelopes_hotwater_roof", 3);
		public static readonly RdSAPRetrofitOption ENVELOPES_ROOF = new("envelopes_roof", 2);
		public static readonly RdSAPRetrofitOption ENVELOPES = new("envelopes", 1);
		public static readonly RdSAPRetrofitOption ENVELOPES_HOTWATER = new("envelopes_hotwater", 2);
		public static readonly RdSAPRetrofitOption ENVELOPES_ROOF_WINDOWS = new("envelopes_roof_windows", 3);
		public static readonly RdSAPRetrofitOption ENVELOPES_WINDOWS = new("envelopes_windows", 2);
		public static readonly RdSAPRetrofitOption ENVELOPES_HOTWATER_WINDOWS = new("envelopes_hotwater_windows", 3);
		public static readonly RdSAPRetrofitOption AS_BUILT = new("as_built", 0);

		public static readonly Dictionary<string, RdSAPRetrofitOption> RETROFIT_OPTION_DICTIONARY = new()
		{
			{ "envelopes_hotwater_roof_windows", ENVELOPES_HOTWATER_ROOF_WINDOWS },
			{ "hotwater_roof_windows", HOTWATER_ROOF_WINDOWS },
			{ "hotwater_roof", HOTWATER_ROOF },
			{ "roof", ROOF },
			{ "hotwater", HOTWATER },
			{ "roof_windows", ROOF_WINDOWS },
			{ "windows", WINDOWS },
			{ "hotwater_windows", HOTWATER_WINDOWS },
			{ "envelopes_hotwater_roof", ENVELOPES_HOTWATER_ROOF },
			{ "envelopes_roof", ENVELOPES_ROOF },
			{ "envelopes", ENVELOPES },
			{ "envelopes_hotwater", ENVELOPES_HOTWATER },
			{ "envelopes_roof_windows", ENVELOPES_ROOF_WINDOWS },
			{ "envelopes_windows", ENVELOPES_WINDOWS },
			{ "envelopes_hotwater_windows", ENVELOPES_HOTWATER_WINDOWS },
			{ "as_built", AS_BUILT },
		};

		public static readonly List<string> RETROFIT_OPTION_KEYS = new()
		{
			"envelopes_hotwater_roof_windows",
			"hotwater_roof_windows",
			"hotwater_roof",
			"roof",
			"hotwater",
			"roof_windows",
			"windows",
			"hotwater_windows",
			"envelopes_hotwater_roof",
			"envelopes_roof",
			"envelopes",
			"envelopes_hotwater",
			"envelopes_roof_windows",
			"envelopes_windows",
			"envelopes_hotwater_windows",
		};

		public static readonly List<string> ALL_RETROFIT_OPTION_KEYS = new()
		{
			"envelopes_hotwater_roof_windows",
			"hotwater_roof_windows",
			"hotwater_roof",
			"roof",
			"hotwater",
			"roof_windows",
			"windows",
			"hotwater_windows",
			"envelopes_hotwater_roof",
			"envelopes_roof",
			"envelopes",
			"envelopes_hotwater",
			"envelopes_roof_windows",
			"envelopes_windows",
			"envelopes_hotwater_windows",
			"as_built"
		};
	}
}


