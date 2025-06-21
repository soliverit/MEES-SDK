


using MeesSDK.Sbem.ConsumerCalendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// The .inp HVAC sytem.
	/// <para>IMPORTANT NOTE: In most respects, HVAC-SYSTEM can be thought of
	/// as fully qualified .inp models themselves. Accounting for Hot water and renewables
	/// separately, you can rebuild EPC ratings from the individual HVAC-SYSTEM .inps </para>
	/// </summary> 
	//"Main System" = HVAC-SYSTEM
	//	TYPE = Flued forced-convection air heaters
	//	HEAT-SOURCE = Air heater
	//	CHP = NO
	//	FUEL-TYPE = Natural Gas
	//	HEAT-REC-SYSTEM = No heat recovery
	//	HEAT-REC-VAR-EFF = NO
	//	VARIABLE-SPEED-PUMP = NO
	//	SFP-CHECK = NO
	//	ISBEM-ID = 11
	//	HEAT-REC-SEFF = 0
	//	HEAT-SSEFF = 0.821
	//	AUX-ENERGY-CORR = 0
	//	AUX-ENERGY/KWH = 0.02
	//	HEAT-GEN-SEFF = 0.9
	//	METERING = 0.95
	//	..
	public class SbemHvacSystem : SbemSpatialObject
	{
		/*
		 *  HVAC Type and Heat Source constants.
		 */
		public const string NO_HEATING_OR_COOLING		= "No Heating or Cooling";
		public const string CENTRAL_WATER_RADIATORS		= "Central heating using water: radiators";
		public const string CENTRAL_WATER_CONVECTORS	= "Central heating using water: convectors";
		public const string CENTRAL_WATER_FLOOR_HEATING = "Central heating using water: floor heating";
		public const string CENTRAL_AIR_DISTRIBUTION	= "Central heating using air distribution";
		public const string ROOM_HEATER_FANNED			= "Other local room heater - fanned";
		public const string RADIANT_HEATER_UNFLUED		= "Unflued radiant heater";
		public const string RADIANT_HEATER_FLUED		= "Flued radiant heater";
		public const string RADIANT_HEATERS_MULTIBURNER = "Multiburner radiant heaters";
		public const string AIR_HEATER_FLUED_FORCED		= "Flued forced-convection air heaters";
		public const string AIR_HEATER_UNFLUED_FORCED	= "Unflued forced-convection air heaters";
		public const string VAV_SINGLE_DUCT				= "Single-duct VAV";
		public const string VAV_DUAL_DUCT				= "Dual-duct VAV";
		public const string CABINET_INDOOR_PACKAGED		= "Indoor packaged cabinet (VAV)";
		public const string FAN_COIL_SYSTEMS			= "Fan coil systems";
		public const string INDUCTION_SYSTEM			= "Induction system";
		public const string VOLUME_FIXED_AIR			= "Constant volume system (fixed fresh air rate)";
		public const string VOLUME_VARIABLE_AIR			= "Constant volume system (variable fresh air rate)";
		public const string MULTIZONE_HOT_COLD			= "Multizone (hot deck/cold deck)";
		public const string TERMINAL_REHEAT				= "Terminal reheat (constant volume)";
		public const string DUAL_DUCT_CONSTANT			= "Dual duct (constant volume)";
		public const string CHILLED_DISP_VENT			= "Chilled ceilings or passive chilled beams and displacement ventilation";
		public const string CHILLED_BEAMS_ACTIVE		= "Active chilled beams";
		public const string WATER_LOOP_HEAT_PUMP		= "Water loop heat pump";
		public const string VARIABLE_REFRIGERANT_FLOW	= "Variable refrigerant flow";
		public const string SPLIT_SYSTEM				= "Split or multi-split system";
		public const string SPLIT_SYSTEM_VENT			= "Split or multi-split system with ventilation";
		public const string SINGLE_ROOM_COOLING			= "Single room cooling system";
		public const string ROOM_HEATER_UNFANNED		= "Other local room heater - unfanned";
		public const string CHILLED_MIX_VENT			= "Chilled ceilings or passive chilled beams and mixing ventilation";
		/*
		 * HVAC Type type getters
		 */
		/// <summary>
		/// Determine if the HVAC's Type matches the input value.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool TypeIs(string value) =>	GetStringProperty("TYPE").Value == value;
		public bool IsNoHeatingOrCooling() => TypeIs(NO_HEATING_OR_COOLING);
		public bool IsCentralWaterRadiators() => TypeIs(CENTRAL_WATER_RADIATORS);
		public bool IsCentralWaterConvectors() => TypeIs(CENTRAL_WATER_CONVECTORS);
		public bool IsCentralWaterFloorHeating() => TypeIs(CENTRAL_WATER_FLOOR_HEATING);
		public bool IsCentralAirDistribution() => TypeIs(CENTRAL_AIR_DISTRIBUTION);
		public bool IsRoomHeaterFanned() => TypeIs(ROOM_HEATER_FANNED);
		public bool IsRadiantHeaterUnflued() => TypeIs(RADIANT_HEATER_UNFLUED);
		public bool IsRadiantHeaterFlued() => TypeIs(RADIANT_HEATER_FLUED);
		public bool IsRadiantHeatersMultiburner() => TypeIs(RADIANT_HEATERS_MULTIBURNER);
		public bool IsAirHeaterFluedForced() => TypeIs(AIR_HEATER_FLUED_FORCED);
		public bool IsAirHeaterUnfluedForced() => TypeIs(AIR_HEATER_UNFLUED_FORCED);
		public bool IsVavSingleDuct() => TypeIs(VAV_SINGLE_DUCT);
		public bool IsVavDualDuct() => TypeIs(VAV_DUAL_DUCT);
		public bool IsCabinetIndoorPackaged() => TypeIs(CABINET_INDOOR_PACKAGED);
		public bool IsFanCoilSystems() => TypeIs(FAN_COIL_SYSTEMS);
		public bool IsInductionSystem() => TypeIs(INDUCTION_SYSTEM);
		public bool IsVolumeFixedAir() => TypeIs(VOLUME_FIXED_AIR);
		public bool IsVolumeVariableAir() => TypeIs(VOLUME_VARIABLE_AIR);
		public bool IsMultizoneHotCold() => TypeIs(MULTIZONE_HOT_COLD);
		public bool IsTerminalReheat() => TypeIs(TERMINAL_REHEAT);
		public bool IsDualDuctConstant() => TypeIs(DUAL_DUCT_CONSTANT);
		public bool IsChilledDispVent() => TypeIs(CHILLED_DISP_VENT);
		public bool IsChilledBeamsActive() => TypeIs(CHILLED_BEAMS_ACTIVE);
		public bool IsWaterLoopHeatPump() => TypeIs(WATER_LOOP_HEAT_PUMP);
		public bool IsVariableRefrigerantFlow() => TypeIs(VARIABLE_REFRIGERANT_FLOW);
		public bool IsSplitSystem() => TypeIs(SPLIT_SYSTEM);
		public bool IsSplitSystemVent() => TypeIs(SPLIT_SYSTEM_VENT);
		public bool IsSingleRoomCooling() => TypeIs(SINGLE_ROOM_COOLING);
		public bool IsRoomHeaterUnfanned() => TypeIs(ROOM_HEATER_UNFANNED);
		public bool IsChilledMixVent() => TypeIs(CHILLED_MIX_VENT);


		// Heat Source
		public const string LTHW_BOILER									= "LTHW boiler";
		public const string MTHW_BOILER									= "MTHW boiler";
		public const string HTHW_BOILER									= "HTHW boiler";
		public const string AIR_HEATER									= "Air heater";
		public const string UNFLUED_GAS_WARM_AIR_HEATER					= "Unflued gas warm air heater";
		public const string UNITARY_RADIANT_HEATER						= "Unitary radiant heater";
		public const string RADIANT_HEATER								= "Radiant heater";
		public const string UNFLUED_RADIANT_HEATER						= "Unflued radiant heater";
		public const string DIRECT_OR_STORAGE_ELECTRIC_HEATER			= "Direct or storage electric heater";
		public const string HEAT_PUMP_GAS_OIL_AIR_SOURCE				= "Heat pump (gas/oil): air source";
		public const string HEAT_PUMP_GAS_OIL_GROUND_OR_WATER_SOURCE	= "Heat pump (gas/oil): ground or water source";
		public const string ROOM_HEATER									= "Room heater";
		public const string DISTRICT_HEATING							= "District heating";
		public const string CHP											= "CHP";
		public const string DIRECT_GAS_FIRING							= "Direct gas firing";
		public const string HEAT_PUMP_ELECTRIC_AIR_SOURCE				= "Heat pump (electric): air source";
		public const string HEAT_PUMP_ELECTRIC_GROUND_OR_WATER_SOURCE	= "Heat pump (electric): ground or water source";

		/// <summary>
		/// Determine if the HVAC system's Type matches the input value
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool HeatSourceIs(string value) =>	HasStringProperty("HEAT-SOURCE") && GetStringProperty("HEAT-SOURCE").Value == value;
		public bool IsLthwBoiler() => HeatSourceIs(LTHW_BOILER);
		public bool IsMthwBoiler() => HeatSourceIs(MTHW_BOILER);
		public bool IsHthwBoiler() => HeatSourceIs(HTHW_BOILER);
		public bool IsAirHeater() => HeatSourceIs(AIR_HEATER);
		public bool IsUnfluedGasWarmAirHeater() => HeatSourceIs(UNFLUED_GAS_WARM_AIR_HEATER);
		public bool IsUnitaryRadiantHeater() => HeatSourceIs(UNITARY_RADIANT_HEATER);
		public bool IsRadiantHeater() => HeatSourceIs(RADIANT_HEATER);
		public bool IsUnfluedRadiantHeater() => HeatSourceIs(UNFLUED_RADIANT_HEATER);
		public bool IsDirectOrStorageElectricHeater() => HeatSourceIs(DIRECT_OR_STORAGE_ELECTRIC_HEATER);
		public bool IsHeatPumpGasOilAirSource() => HeatSourceIs(HEAT_PUMP_GAS_OIL_AIR_SOURCE);
		public bool IsHeatPumpGasOilGroundOrWaterSource() => HeatSourceIs(HEAT_PUMP_GAS_OIL_GROUND_OR_WATER_SOURCE);
		public bool IsRoomHeater() => HeatSourceIs(ROOM_HEATER);
		public bool IsDistrictHeating() => HeatSourceIs(DISTRICT_HEATING);
		public bool IsChp() => HeatSourceIs(CHP);
		public bool IsDirectGasFiring() => HeatSourceIs(DIRECT_GAS_FIRING);
		public bool IsHeatPumpElectricAirSource() => HeatSourceIs(HEAT_PUMP_ELECTRIC_AIR_SOURCE);
		public bool IsHeatPumpElectricGroundOrWaterSource() => HeatSourceIs(HEAT_PUMP_ELECTRIC_GROUND_OR_WATER_SOURCE);
		/// <summary>
		/// SBEM Inp object name. All SBEM objects have one. 
		/// </summary>
		public const string OBJECT_NAME  = "HVAC-SYSTEM"; 
		public override string ObjectName() { return OBJECT_NAME; }
		public SbemObjectSet<SbemZone> Zones { get; protected set; } = new SbemObjectSet<SbemZone>();
		public ConsumerConsumptionCalendar EndUseConsumerCalendar { get; protected set; }
		public FuelConsumptionCalendar FuelUseConsumerCalendar { get; protected set; }
		public SbemHvacSystem(string currentName, List<string> currentProperties) : base(currentName, currentProperties)
		{

		}
		public void SetEndUseConsumerCalendar(ConsumerConsumptionCalendar endUseConsumerCalendar)
		{
			EndUseConsumerCalendar = endUseConsumerCalendar;
		}
		public void SetFuelTypConsumerCalendar(FuelConsumptionCalendar fuelConsumptionCalendar)
		{
			FuelUseConsumerCalendar = fuelConsumptionCalendar;
		}
		/// <summary>
		/// The SSEFF adjusted to replacing the heating element with another with GEN-SEFF = newSeff.
		/// <para>This exists because SBEM because the user enters HEAT-GEN-SEFF and the SBEM interface
		/// calculated the HEAT-SSEFF. This is "much better than a guess".</para>
		/// </summary>
		/// <param name="newSeff"></param>
		/// <returns></returns>
		public float GetRelativeSSEFF(float newSeff)
		{
			return newSeff /  GetNumericProperty("HEAT-GEN-SEFF").Value * GetNumericProperty("HEAT-SSEFF").Value;
		}
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(base.ToString());
			for(int zoneID = 0; zoneID < Zones.Length; zoneID++)
				sb.AppendLine(Zones[zoneID].ToString());
			return sb.ToString();	
		}
		public void AddZone(SbemZone zone)
		{
			Zones.Add(zone);
			Area += zone.Area;
		}
	}
}
