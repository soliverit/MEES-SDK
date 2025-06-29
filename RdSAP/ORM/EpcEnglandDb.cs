// ---------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by LinqToDB scaffolding tool (https://github.com/linq2db/linq2db).
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
// ---------------------------------------------------------------------------------------------------

using LinqToDB;
using LinqToDB.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable 1573, 1591
#nullable enable

namespace MeesSDK
{
	public partial class EpcEnglandDb : DataConnection
	{
		public EpcEnglandDb()
		{
			InitDataContext();
		}

		public EpcEnglandDb(string configuration)
			: base(configuration)
		{
			InitDataContext();
		}

		public EpcEnglandDb(DataOptions<EpcEnglandDb> options)
			: base(options.Options)
		{
			InitDataContext();
		}

		partial void InitDataContext();

		public ITable<BuildingType>                   BuildingTypes                    => this.GetTable<BuildingType>();
		public ITable<BuildingSetId>                  BuildingSetIds                   => this.GetTable<BuildingSetId>();
		public ITable<BuildingSetResult>              BuildingSetResults               => this.GetTable<BuildingSetResult>();
		public ITable<BuildingSet>                    BuildingSets                     => this.GetTable<BuildingSet>();
		public ITable<Building>                       Buildings                        => this.GetTable<Building>();
		public ITable<BuiltForm>                      BuiltForms                       => this.GetTable<BuiltForm>();
		public ITable<Constituency>                   Constituencies                   => this.GetTable<Constituency>();
		public ITable<ConstituencyPopulation>         ConstituencyPopulations          => this.GetTable<ConstituencyPopulation>();
		public ITable<ConstructionAgeBand>            ConstructionAgeBands             => this.GetTable<ConstructionAgeBand>();
		public ITable<DepcMechanicalVentilation>      DepcMechanicalVentilations       => this.GetTable<DepcMechanicalVentilation>();
		public ITable<DepcMlFeature>                  DepcMlFeatures                   => this.GetTable<DepcMlFeature>();
		public ITable<DepcTransactionType>            DepcTransactionTypes             => this.GetTable<DepcTransactionType>();
		public ITable<Depc>                           Depcs                            => this.GetTable<Depc>();
		public ITable<DeprivationIndexColumnLabel>    DeprivationIndexColumnLabels     => this.GetTable<DeprivationIndexColumnLabel>();
		public ITable<EnergyTariff>                   EnergyTariffs                    => this.GetTable<EnergyTariff>();
		public ITable<EpcRating>                      EpcRatings                       => this.GetTable<EpcRating>();
		public ITable<EpcTransactionType>             EpcTransactionTypes              => this.GetTable<EpcTransactionType>();
		public ITable<Epc>                            Epcs                             => this.GetTable<Epc>();
		public ITable<FloorDescription>               FloorDescriptions                => this.GetTable<FloorDescription>();
		public ITable<GlazedAreaSize>                 GlazedAreaSizes                  => this.GetTable<GlazedAreaSize>();
		public ITable<GlazedType>                     GlazedTypes                      => this.GetTable<GlazedType>();
		public ITable<HotwaterDescription>            HotwaterDescriptions             => this.GetTable<HotwaterDescription>();
		public ITable<IndoorEnvironmentType>          IndoorEnvironmentTypes           => this.GetTable<IndoorEnvironmentType>();
		public ITable<LightingDescription>            LightingDescriptions             => this.GetTable<LightingDescription>();
		public ITable<LocalAuthority>                 LocalAuthorities                 => this.GetTable<LocalAuthority>();
		public ITable<LocalAuthorityDeprivationIndex> LocalAuthorityDeprivationIndices => this.GetTable<LocalAuthorityDeprivationIndex>();
		public ITable<MainFuel>                       MainFuels                        => this.GetTable<MainFuel>();
		public ITable<MainHeatingControlDescription>  MainHeatingControlDescriptions   => this.GetTable<MainHeatingControlDescription>();
		public ITable<MainHeatingDescription>         MainHeatingDescriptions          => this.GetTable<MainHeatingDescription>();
		public ITable<MainHeatingFuel>                MainHeatingFuels                 => this.GetTable<MainHeatingFuel>();
		public ITable<NcmPropertyType>                NcmPropertyTypes                 => this.GetTable<NcmPropertyType>();
		public ITable<NewConstituencyPopulation>      NewConstituencyPopulations       => this.GetTable<NewConstituencyPopulation>();
		public ITable<NondomesticBuilding>            NondomesticBuildings             => this.GetTable<NondomesticBuilding>();
		public ITable<OnsReferenceSet>                OnsReferenceSets                 => this.GetTable<OnsReferenceSet>();
		public ITable<Postcode>                       Postcodes                        => this.GetTable<Postcode>();
		public ITable<PostcodesBackup>                PostcodesBackups                 => this.GetTable<PostcodesBackup>();
		public ITable<PropertyType>                   PropertyTypes                    => this.GetTable<PropertyType>();
		public ITable<RoofDescription>                RoofDescriptions                 => this.GetTable<RoofDescription>();
		public ITable<SbemHvacHeatSource>             SbemHvacHeatSources              => this.GetTable<SbemHvacHeatSource>();
		public ITable<SbemHvacType>                   SbemHvacTypes                    => this.GetTable<SbemHvacType>();
		public ITable<SecondheatDescription>          SecondheatDescriptions           => this.GetTable<SecondheatDescription>();
		public ITable<UprnSource>                     UprnSources                      => this.GetTable<UprnSource>();
		public ITable<WallDescription>                WallDescriptions                 => this.GetTable<WallDescription>();
		public ITable<WindowDescription>              WindowDescriptions               => this.GetTable<WindowDescription>();
	}

	public static partial class ExtensionMethods
	{
		#region Table Extensions
		public static BuildingSetId? Find(this ITable<BuildingSetId> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<BuildingSetId?> FindAsync(this ITable<BuildingSetId> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static BuildingSetResult? Find(this ITable<BuildingSetResult> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<BuildingSetResult?> FindAsync(this ITable<BuildingSetResult> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static BuildingSet? Find(this ITable<BuildingSet> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<BuildingSet?> FindAsync(this ITable<BuildingSet> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static Building? Find(this ITable<Building> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<Building?> FindAsync(this ITable<Building> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static BuiltForm? Find(this ITable<BuiltForm> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<BuiltForm?> FindAsync(this ITable<BuiltForm> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static Constituency? Find(this ITable<Constituency> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<Constituency?> FindAsync(this ITable<Constituency> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static ConstructionAgeBand? Find(this ITable<ConstructionAgeBand> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<ConstructionAgeBand?> FindAsync(this ITable<ConstructionAgeBand> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static DepcMechanicalVentilation? Find(this ITable<DepcMechanicalVentilation> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<DepcMechanicalVentilation?> FindAsync(this ITable<DepcMechanicalVentilation> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static DepcMlFeature? Find(this ITable<DepcMlFeature> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<DepcMlFeature?> FindAsync(this ITable<DepcMlFeature> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static DepcTransactionType? Find(this ITable<DepcTransactionType> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<DepcTransactionType?> FindAsync(this ITable<DepcTransactionType> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static Depc? Find(this ITable<Depc> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<Depc?> FindAsync(this ITable<Depc> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static DeprivationIndexColumnLabel? Find(this ITable<DeprivationIndexColumnLabel> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<DeprivationIndexColumnLabel?> FindAsync(this ITable<DeprivationIndexColumnLabel> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static EnergyTariff? Find(this ITable<EnergyTariff> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<EnergyTariff?> FindAsync(this ITable<EnergyTariff> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static EpcTransactionType? Find(this ITable<EpcTransactionType> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<EpcTransactionType?> FindAsync(this ITable<EpcTransactionType> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static Epc? Find(this ITable<Epc> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<Epc?> FindAsync(this ITable<Epc> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static FloorDescription? Find(this ITable<FloorDescription> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<FloorDescription?> FindAsync(this ITable<FloorDescription> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static GlazedAreaSize? Find(this ITable<GlazedAreaSize> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<GlazedAreaSize?> FindAsync(this ITable<GlazedAreaSize> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static GlazedType? Find(this ITable<GlazedType> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<GlazedType?> FindAsync(this ITable<GlazedType> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static HotwaterDescription? Find(this ITable<HotwaterDescription> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<HotwaterDescription?> FindAsync(this ITable<HotwaterDescription> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static IndoorEnvironmentType? Find(this ITable<IndoorEnvironmentType> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<IndoorEnvironmentType?> FindAsync(this ITable<IndoorEnvironmentType> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static LightingDescription? Find(this ITable<LightingDescription> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<LightingDescription?> FindAsync(this ITable<LightingDescription> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static LocalAuthority? Find(this ITable<LocalAuthority> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<LocalAuthority?> FindAsync(this ITable<LocalAuthority> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static LocalAuthorityDeprivationIndex? Find(this ITable<LocalAuthorityDeprivationIndex> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<LocalAuthorityDeprivationIndex?> FindAsync(this ITable<LocalAuthorityDeprivationIndex> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static MainFuel? Find(this ITable<MainFuel> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<MainFuel?> FindAsync(this ITable<MainFuel> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static MainHeatingControlDescription? Find(this ITable<MainHeatingControlDescription> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<MainHeatingControlDescription?> FindAsync(this ITable<MainHeatingControlDescription> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static MainHeatingDescription? Find(this ITable<MainHeatingDescription> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<MainHeatingDescription?> FindAsync(this ITable<MainHeatingDescription> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static MainHeatingFuel? Find(this ITable<MainHeatingFuel> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<MainHeatingFuel?> FindAsync(this ITable<MainHeatingFuel> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static NcmPropertyType? Find(this ITable<NcmPropertyType> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<NcmPropertyType?> FindAsync(this ITable<NcmPropertyType> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static NewConstituencyPopulation? Find(this ITable<NewConstituencyPopulation> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<NewConstituencyPopulation?> FindAsync(this ITable<NewConstituencyPopulation> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static OnsReferenceSet? Find(this ITable<OnsReferenceSet> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<OnsReferenceSet?> FindAsync(this ITable<OnsReferenceSet> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static Postcode? Find(this ITable<Postcode> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<Postcode?> FindAsync(this ITable<Postcode> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static PropertyType? Find(this ITable<PropertyType> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<PropertyType?> FindAsync(this ITable<PropertyType> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static RoofDescription? Find(this ITable<RoofDescription> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<RoofDescription?> FindAsync(this ITable<RoofDescription> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static SbemHvacHeatSource? Find(this ITable<SbemHvacHeatSource> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<SbemHvacHeatSource?> FindAsync(this ITable<SbemHvacHeatSource> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static SbemHvacType? Find(this ITable<SbemHvacType> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<SbemHvacType?> FindAsync(this ITable<SbemHvacType> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static SecondheatDescription? Find(this ITable<SecondheatDescription> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<SecondheatDescription?> FindAsync(this ITable<SecondheatDescription> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static UprnSource? Find(this ITable<UprnSource> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<UprnSource?> FindAsync(this ITable<UprnSource> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static WallDescription? Find(this ITable<WallDescription> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<WallDescription?> FindAsync(this ITable<WallDescription> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static WindowDescription? Find(this ITable<WindowDescription> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<WindowDescription?> FindAsync(this ITable<WindowDescription> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}
		#endregion
	}
}
