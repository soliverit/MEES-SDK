// ---------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by LinqToDB scaffolding tool (https://github.com/linq2db/linq2db).
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
// ---------------------------------------------------------------------------------------------------

using LinqToDB.Mapping;

#pragma warning disable 1573, 1591
#nullable enable

namespace MeesSDK
{
	[Table("depc_ml_features")]
	public class DepcMlFeature
	{
		[Column("id"                            , IsPrimaryKey = true, IsIdentity = true, SkipOnInsert = true, SkipOnUpdate = true)] public int      Id                          { get; set; } // integer
		[Column("current_energy_efficiency"                                                                                       )] public decimal  CurrentEnergyEfficiency     { get; set; } // numeric(5,2)
		[Column("age_index"                                                                                                       )] public int      AgeIndex                    { get; set; } // integer
		[Column("energy_consumption_potential"                                                                                    )] public decimal? EnergyConsumptionPotential  { get; set; } // numeric(8,2)
		[Column("extension_count"                                                                                                 )] public int?     ExtensionCount              { get; set; } // integer
		[Column("floor_u_value"                                                                                                   )] public decimal  FloorUValue                 { get; set; } // numeric(10,4)
		[Column("heating_cost_potential"                                                                                          )] public decimal? HeatingCostPotential        { get; set; } // numeric(8,1)
		[Column("hot_water_energy_efficiency"                                                                                     )] public int?     HotWaterEnergyEfficiency    { get; set; } // integer
		[Column("internal_gains"                                                                                                  )] public decimal? InternalGains               { get; set; } // numeric(12,4)
		[Column("is_detached"                                                                                                     )] public int?     IsDetached                  { get; set; } // integer
		[Column("is_end"                                                                                                          )] public int?     IsEnd                       { get; set; } // integer
		[Column("lighting"                                                                                                        )] public decimal? Lighting                    { get; set; } // numeric(10,5)
		[Column("low_energy_lighting"                                                                                             )] public int?     LowEnergyLighting           { get; set; } // integer
		[Column("main_heating_controls"                                                                                           )] public int?     MainHeatingControls         { get; set; } // integer
		[Column("main_fuel_factor"                                                                                                )] public decimal? MainFuelFactor              { get; set; } // numeric(4,3)
		[Column("main_heating_energy_efficiency"                                                                                  )] public int?     MainHeatingEnergyEfficiency { get; set; } // integer
		[Column("multi_glaze_proportion"                                                                                          )] public int?     MultiGlazeProportion        { get; set; } // integer
		[Column("number_heated_rooms"                                                                                             )] public int?     NumberHeatedRooms           { get; set; } // integer
		[Column("number_open_fireplaces"                                                                                          )] public int?     NumberOpenFireplaces        { get; set; } // integer
		[Column("photosupply"                                                                                                     )] public int?     Photosupply                 { get; set; } // integer
		[Column("potential_energy_efficiency"                                                                                     )] public int?     PotentialEnergyEfficiency   { get; set; } // integer
		[Column("roof_energy_efficiency"                                                                                          )] public int?     RoofEnergyEfficiency        { get; set; } // integer
		[Column("roof_u_value"                                                                                                    )] public decimal? RoofUValue                  { get; set; } // numeric(4,3)
		[Column("secondheatdescription"                                                                                           )] public int?     Secondheatdescription       { get; set; } // integer
		[Column("temperature_adjustment"                                                                                          )] public decimal? TemperatureAdjustment       { get; set; } // numeric(5,3)
		[Column("wall_thickness"                                                                                                  )] public decimal? WallThickness               { get; set; } // numeric(6,2)
		[Column("wall_u_value"                                                                                                    )] public decimal? WallUValue                  { get; set; } // numeric(10,4)
		[Column("wall_energy_efficiency"                                                                                          )] public int?     WallEnergyEfficiency        { get; set; } // integer
		[Column("war"                                                                                                             )] public decimal? War                         { get; set; } // numeric(10,4)
		[Column("wet_rooms"                                                                                                       )] public int?     WetRooms                    { get; set; } // integer
		[Column("window_energy_efficiency"                                                                                        )] public int?     WindowEnergyEfficiency      { get; set; } // integer
		[Column("lmk_key"                                                                                                         )] public string?  LmkKey                      { get; set; } // character varying(256)
		[Column("building_reference_number"                                                                                       )] public long?    BuildingReferenceNumber     { get; set; } // bigint
		[Column("lodgment_date"                                                                                                   )] public string?  LodgmentDate                { get; set; } // character varying(128)
		[Column("building_id"                                                                                                     )] public int?     BuildingId                  { get; set; } // integer
		[Column("depc_id"                                                                                                         )] public int      DepcId                      { get; set; } // integer
		[Column("roof_description_id"                                                                                             )] public int?     RoofDescriptionId           { get; set; } // integer

		#region Associations
		/// <summary>
		/// depc_ml_features_building_id_fkey
		/// </summary>
		[Association(ThisKey = nameof(BuildingId), OtherKey = nameof(MeesSDK.Building.Id))]
		public Building? Building { get; set; }
		#endregion
	}
}
