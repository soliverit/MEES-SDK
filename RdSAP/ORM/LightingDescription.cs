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
	[Table("lighting_descriptions")]
	public class LightingDescription
	{
		[Column("id"                , IsPrimaryKey = true, IsIdentity = true, SkipOnInsert = true, SkipOnUpdate = true)] public int      Id               { get; set; } // integer
		[Column("name"                                                                                                )] public string?  Name             { get; set; } // character varying(256)
		[Column("percent_low_energy"                                                                                  )] public decimal? PercentLowEnergy { get; set; } // numeric(10,7)
	}
}
