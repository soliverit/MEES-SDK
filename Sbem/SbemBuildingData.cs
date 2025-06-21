using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LinqToDB.Common.Configuration;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// A _epc.inp SbemEpcModel object that contains the high level results from SBEM. There are
	/// three in every file for the as-built, notional, and reference .inp SbemModel simulations
	/// </summary>
	/// 
	//	"BUILDING_DATA" = BUILDING-DATA
	//  ANALYSIS = NOTIONAL
	//  AREA                       = 4215
	//  AREA-EXT                   = 11050
	//  WEATHER                    = LON
	//  Q50-INF                    = 3
	//  BUILDING-W/K               = 1971.4
	//  BUILDING-W/M2K             = 0.178407
	//  BUILDING-ALPHA             = 7.5032
	//  KWH/M2-HEAT                = 14.8604
	//  KWH/M2-COOL                = 0
	//  KWH/M2-AUX                 = 3.96641
	//  KWH/M2-LIGHT               = 12.4312
	//  KWH/M2-DHW                 = 2167.84
	//  KWH/M2-EQUP                = 147.939
	//  KWH/M2-NATGAS              = 14.8604
	//  KWH/M2-LPG                 = 0
	//  KWH/M2-BIOGAS              = 0
	//  KWH/M2-OIL                 = 0
	//  KWH/M2-COAL                = 0
	//  KWH/M2-ANTHRACITE          = 0
	//  KWH/M2-SMOKELESS           = 0
	//  KWH/M2-DUELFUEL            = 0
	//  KWH/M2-BIOMASS             = 0
	//  KWH/M2-SUPELEC             = 2184.24
	//  KWH/M2-WASTEHEAT           = 0
	//  KWH/M2-DISTRICT-HEATING    = 0
	//  KWH/M2-DISP                = 33.3718
	//  KWH/M2-PVS                 = 33.3718
	//  KWH/M2-WIND                = 0
	//  KWH/M2-CHP                 = 0
	//  KWH/M2-SES                 = 0
	//  KWH/M2-HEAT-PUMP           = 0
	//  KWH/M2-HEAT-DEMAND         = 12.7799
	//  KWH/M2-COOL-DEMAND         = 131.741
	//  KWH/M2-DEMAND-ALL          = 144.521
	//  KWH/M2-CONSUM-ALL          = 2199.1
	//  KG/M2-CO2                  = 294.24
	//  PRIM-KWH/M2                = 3189.35
	//  ACT-AREA                   = { 1074, 1320, 1012, 2695, 1297, 125, 1078, 50, 1077, 25 }
	//..
	public class SbemBuildingData : SbemObject
	{
		public SbemBuildingData(string name, List<string> currentProperties) : base(name, currentProperties) { }
		public const string OBJECT_NAME = "BUILDING-DATA";
		public List<SbemHvacSystemData> HvacSystems { get; } = new List<SbemHvacSystemData>();
		public override string ObjectName() { return OBJECT_NAME; }
		public override string ToString()
		{
			StringBuilder content = new StringBuilder();
			content.AppendLine(base.ToString());
			foreach(SbemHvacSystemData systemData in HvacSystems)
				content.AppendLine(systemData.ToString());
			return content.ToString();
		}
	}
}
