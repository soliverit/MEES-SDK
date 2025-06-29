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
	[Table("postcodes")]
	public class Postcode
	{
		[Column("id"                , IsPrimaryKey = true , IsIdentity = true, SkipOnInsert = true, SkipOnUpdate = true)] public int      Id               { get; set; } // integer
		[Column("postcode"          , CanBeNull    = false                                                             )] public string   Postcode1        { get; set; } = null!; // character varying(32)
		[Column("local_authority_id"                                                                                   )] public int?     LocalAuthorityId { get; set; } // integer
		[Column("constituency_id"                                                                                      )] public int?     ConstituencyId   { get; set; } // integer
		[Column("latitude"                                                                                             )] public decimal? Latitude         { get; set; } // numeric(8,5)
		[Column("longitude"                                                                                            )] public decimal? Longitude        { get; set; } // numeric(8,5)

		#region Associations
		/// <summary>
		/// fk_postcode_constituency_id
		/// </summary>
		[Association(ThisKey = nameof(ConstituencyId), OtherKey = nameof(MeesSDK.Constituency.Id))]
		public Constituency? Constituency { get; set; }

		/// <summary>
		/// fk_postcode_local_authority_id
		/// </summary>
		[Association(ThisKey = nameof(LocalAuthorityId), OtherKey = nameof(MeesSDK.LocalAuthority.Id))]
		public LocalAuthority? LocalAuthority { get; set; }
		#endregion
	}
}
