// ---------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by LinqToDB scaffolding tool (https://github.com/linq2db/linq2db).
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
// ---------------------------------------------------------------------------------------------------

using LinqToDB.Mapping;
using System.Collections.Generic;

#pragma warning disable 1573, 1591
#nullable enable

namespace MeesSDK
{
	[Table("building_sets")]
	public class BuildingSet
	{
		[Column("id"  , IsPrimaryKey = true , IsIdentity = true, SkipOnInsert = true, SkipOnUpdate = true)] public int    Id   { get; set; } // integer
		[Column("name", CanBeNull    = false                                                             )] public string Name { get; set; } = null!; // character varying(128)

		#region Associations
		/// <summary>
		/// fk_building_set_id backreference
		/// </summary>
		[Association(ThisKey = nameof(Id), OtherKey = nameof(BuildingSetId.BuildingSetId1))]
		public IEnumerable<BuildingSetId> BuildingSetIds { get; set; } = null!;

		/// <summary>
		/// fk_building_set_results backreference
		/// </summary>
		[Association(ThisKey = nameof(Id), OtherKey = nameof(BuildingSetResult.BuildingSetId))]
		public IEnumerable<BuildingSetResult> BuildingSetResults { get; set; } = null!;
		#endregion
	}
}
