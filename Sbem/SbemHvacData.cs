using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// The _epc.inp Hvac System-level data object.
	/// </summary>
	//"Main System" = HVAC-SYSTEM-DATA
	//	AREA                       = 4140
	//	TYPE                       = Flued forced-convection air heaters
	//	HEAT-SOURCE                = Air heater
	//	FUEL-TYPE                  = Natural Gas
	//	FUEL-TYPE-COOL             = Grid Supplied Electricity
	//	MJ/M2-HEAT-DEM             = 132.203
	//	MJ/M2-COOL-DEM             = 609.254
	//	KWH/M2-HEAT                = 44.7297
	//	KWH/M2-COOL                = 0
	//	KWH/M2-AUX                 = 5.13983
	//	HEAT-SSEFF                 = 0.821
	//	HEAT-GEN-SEFF              = 0.9
	//	ACT-AREA                   = { 1074, 1320, 1012, 2695, 1297, 125 }
	//	..
public class SbemHvacSystemData : SbemObject
	{
		public SbemHvacSystemData(string name, List<string> currentProperties) : base(name, currentProperties) { }
		public const string OBJECT_NAME = "HVAC-SYSTEM-DATA";
		public override string ObjectName() { return OBJECT_NAME; }

	}
}
