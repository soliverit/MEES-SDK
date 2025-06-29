// ---------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by LinqToDB scaffolding tool (https://github.com/linq2db/linq2db).
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
// ---------------------------------------------------------------------------------------------------

using LinqToDB.Mapping;
using System;

#pragma warning disable 1573, 1591
#nullable enable

namespace MeesSDK
{
	[Table("depcs")]
	public class Depc
	{
		[Column("id"                                 , IsPrimaryKey = true, IsIdentity = true, SkipOnInsert = true, SkipOnUpdate = true)] public int       Id                              { get; set; } // integer
		[Column("current_energy_efficiency"                                                                                            )] public short?    CurrentEnergyEfficiency         { get; set; } // smallint
		[Column("potential_energy_efficiency"                                                                                          )] public short?    PotentialEnergyEfficiency       { get; set; } // smallint
		[Column("energy_consumption_current"                                                                                           )] public int?      EnergyConsumptionCurrent        { get; set; } // integer
		[Column("energy_consumption_potential"                                                                                         )] public int?      EnergyConsumptionPotential      { get; set; } // integer
		[Column("co2_emissions_current"                                                                                                )] public int?      Co2EmissionsCurrent             { get; set; } // integer
		[Column("co2_emiss_curr_per_floor_area"                                                                                        )] public decimal?  Co2EmissCurrPerFloorArea        { get; set; } // numeric(16,3)
		[Column("co2_emissions_potential"                                                                                              )] public int?      Co2EmissionsPotential           { get; set; } // integer
		[Column("lighting_cost_current"                                                                                                )] public int?      LightingCostCurrent             { get; set; } // integer
		[Column("lighting_cost_potential"                                                                                              )] public int?      LightingCostPotential           { get; set; } // integer
		[Column("heating_cost_current"                                                                                                 )] public int?      HeatingCostCurrent              { get; set; } // integer
		[Column("heating_cost_potential"                                                                                               )] public int?      HeatingCostPotential            { get; set; } // integer
		[Column("hot_water_cost_current"                                                                                               )] public int?      HotWaterCostCurrent             { get; set; } // integer
		[Column("hot_water_cost_potential"                                                                                             )] public int?      HotWaterCostPotential           { get; set; } // integer
		[Column("area"                                                                                                                 )] public decimal?  Area                            { get; set; } // numeric(16,4)
		[Column("multi_glaze_proportion"                                                                                               )] public decimal?  MultiGlazeProportion            { get; set; } // numeric(6,3)
		[Column("extension_count"                                                                                                      )] public short?    ExtensionCount                  { get; set; } // smallint
		[Column("number_habitable_rooms"                                                                                               )] public short?    NumberHabitableRooms            { get; set; } // smallint
		[Column("number_heated_rooms"                                                                                                  )] public int?      NumberHeatedRooms               { get; set; } // integer
		[Column("low_energy_lighting"                                                                                                  )] public short?    LowEnergyLighting               { get; set; } // smallint
		[Column("number_open_fireplaces"                                                                                               )] public decimal?  NumberOpenFireplaces            { get; set; } // numeric(16,4)
		[Column("hot_water_energy_eff"                                                                                                 )] public int?      HotWaterEnergyEff               { get; set; } // integer
		[Column("floor_energy_eff"                                                                                                     )] public char?     FloorEnergyEff                  { get; set; } // character(1)
		[Column("windows_energy_eff"                                                                                                   )] public int?      WindowsEnergyEff                { get; set; } // integer
		[Column("walls_energy_eff"                                                                                                     )] public short?    WallsEnergyEff                  { get; set; } // smallint
		[Column("roof_energy_eff"                                                                                                      )] public int?      RoofEnergyEff                   { get; set; } // integer
		[Column("mainheat_energy_eff"                                                                                                  )] public char?     MainheatEnergyEff               { get; set; } // character(1)
		[Column("mainheatc_energy_eff"                                                                                                 )] public short?    MainheatcEnergyEff              { get; set; } // smallint
		[Column("lighting_energy_eff"                                                                                                  )] public int?      LightingEnergyEff               { get; set; } // integer
		[Column("wind_turbine_count"                                                                                                   )] public string?   WindTurbineCount                { get; set; } // character varying(256)
		[Column("floor_height"                                                                                                         )] public float?    FloorHeight                     { get; set; } // real
		[Column("photo_supply"                                                                                                         )] public string?   PhotoSupply                     { get; set; } // character varying(256)
		[Column("mechanical_ventilation"                                                                                               )] public string?   MechanicalVentilation           { get; set; } // character varying(256)
		[Column("fixed_lighting_outlets_count"                                                                                         )] public short?    FixedLightingOutletsCount       { get; set; } // smallint
		[Column("low_energy_fixed_light_count"                                                                                         )] public decimal?  LowEnergyFixedLightCount        { get; set; } // numeric(16,4)
		[Column("current_energy_rating"                                                                                                )] public char?     CurrentEnergyRating             { get; set; } // character(1)
		[Column("potential_energy_rating"                                                                                              )] public char?     PotentialEnergyRating           { get; set; } // character(1)
		[Column("tenure"                                                                                                               )] public string?   Tenure                          { get; set; } // character varying(256)
		[Column("uprn"                                                                                                                 )] public string?   Uprn                            { get; set; } // character varying(256)
		[Column("top_storey"                                                                                                           )] public bool?     TopStorey                       { get; set; } // boolean
		[Column("hotwater_description_id"                                                                                              )] public int?      HotwaterDescriptionId           { get; set; } // integer
		[Column("has_solar_water"                                                                                                      )] public bool?     HasSolarWater                   { get; set; } // boolean
		[Column("building_id"                                                                                                          )] public int?      BuildingId                      { get; set; } // integer
		[Column("floor_description_id"                                                                                                 )] public int?      FloorDescriptionId              { get; set; } // integer
		[Column("main_heating_control_description_id"                                                                                  )] public int?      MainHeatingControlDescriptionId { get; set; } // integer
		[Column("main_heating_description_id"                                                                                          )] public int?      MainHeatingDescriptionId        { get; set; } // integer
		[Column("construction_age_band_id"                                                                                             )] public int?      ConstructionAgeBandId           { get; set; } // integer
		[Column("roof_description_id"                                                                                                  )] public int?      RoofDescriptionId               { get; set; } // integer
		[Column("window_description_id"                                                                                                )] public int?      WindowDescriptionId             { get; set; } // integer
		[Column("wall_description_id"                                                                                                  )] public int?      WallDescriptionId               { get; set; } // integer
		[Column("lighting_description_id"                                                                                              )] public int?      LightingDescriptionId           { get; set; } // integer
		[Column("secondheat_description_id"                                                                                            )] public int?      SecondheatDescriptionId         { get; set; } // integer
		[Column("property_type_id"                                                                                                     )] public int?      PropertyTypeId                  { get; set; } // integer
		[Column("_deprecated_main_heating_control_id"                                                                                  )] public int?      DeprecatedMainHeatingControlId  { get; set; } // integer
		[Column("glazed_type_id"                                                                                                       )] public int?      GlazedTypeId                    { get; set; } // integer
		[Column("built_form_id"                                                                                                        )] public int?      BuiltFormId                     { get; set; } // integer
		[Column("postcode_id"                                                                                                          )] public int?      PostcodeId                      { get; set; } // integer
		[Column("lodgement_date"                                                                                                       )] public DateTime? LodgementDate                   { get; set; } // date
		[Column("main_fuel_id"                                                                                                         )] public int?      MainFuelId                      { get; set; } // integer
		[Column("energy_tariff_id"                                                                                                     )] public int?      EnergyTariffId                  { get; set; } // integer
		[Column("glazed_area_size_id"                                                                                                  )] public int?      GlazedAreaSizeId                { get; set; } // integer
		[Column("active"                                                                                                               )] public bool?     Active                          { get; set; } // boolean

		#region Associations
		/// <summary>
		/// depcs_building_id_fkey
		/// </summary>
		[Association(ThisKey = nameof(BuildingId), OtherKey = nameof(MeesSDK.Building.Id))]
		public Building? Building { get; set; }

		/// <summary>
		/// depcs_glazed_area_size_id_fkey
		/// </summary>
		[Association(ThisKey = nameof(GlazedAreaSizeId), OtherKey = nameof(MeesSDK.GlazedAreaSize.Id))]
		public GlazedAreaSize? GlazedAreaSize { get; set; }

		/// <summary>
		/// fk_property_type_id
		/// </summary>
		[Association(ThisKey = nameof(PropertyTypeId), OtherKey = nameof(MeesSDK.PropertyType.Id))]
		public PropertyType? PropertyType { get; set; }
		#endregion
	}
}
