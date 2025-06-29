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
	[Table("building_set_results")]
	public class BuildingSetResult
	{
		[Column("id"             , IsPrimaryKey = true, IsIdentity = true, SkipOnInsert = true, SkipOnUpdate = true)] public int Id            { get; set; } // integer
		[Column("building_set_id"                                                                                  )] public int BuildingSetId { get; set; } // integer
		[Column("A"                                                                                                )] public int A             { get; set; } // integer
		[Column("B"                                                                                                )] public int B             { get; set; } // integer
		[Column("C"                                                                                                )] public int C             { get; set; } // integer
		[Column("D"                                                                                                )] public int D             { get; set; } // integer
		[Column("E"                                                                                                )] public int E             { get; set; } // integer
		[Column("F"                                                                                                )] public int F             { get; set; } // integer
		[Column("G"                                                                                                )] public int G             { get; set; } // integer
		[Column("houses"                                                                                           )] public int Houses        { get; set; } // integer
		[Column("flats"                                                                                            )] public int Flats         { get; set; } // integer
		[Column("bungalows"                                                                                        )] public int Bungalows     { get; set; } // integer
		[Column("maisonettes"                                                                                      )] public int Maisonettes   { get; set; } // integer
		[Column("heating_cost"                                                                                     )] public int HeatingCost   { get; set; } // integer
		[Column("hotwater_cost"                                                                                    )] public int HotwaterCost  { get; set; } // integer
		[Column("lighting_cost"                                                                                    )] public int LightingCost  { get; set; } // integer

		#region Associations
		/// <summary>
		/// fk_building_set_results
		/// </summary>
		[Association(CanBeNull = false, ThisKey = nameof(BuildingSetId), OtherKey = nameof(MeesSDK.BuildingSet.Id))]
		public BuildingSet BuildingSet { get; set; } = null!;
		#endregion
	}
}
