using Microsoft.ML.Data;
using MeesSDK.DataManagement;
using MeesSDK.ML;
using MeesSDK.RdSAP;
using MeesSDK.RdSAP.Reference;
using MeesSDK.RdSAP.Reference.MOOSandbox.RdSAP.Reference;
using System.Text.RegularExpressions;


namespace MeesSDK.RsSAP
{
	/// <summary>
	/// RdSAP Building - A wrapper for D-EPC register records from the 
	/// doemstic EPC register: <a href="https://opendatacommunities.org/"></a>
	/// 
	/// The Building has instance members for all D-EPC register fields. It 
	/// also has extended features inferred from the RdSAP estimator and
	/// and nation-scale retrofit optimisation projects documented
	/// at <a href="https://www.researchgate.net/profile/Stephen-Oliver-6"></a>
	/// </summary>
	public class RdSAPBuilding : IValidatable
	{
		/// <summary>
		/// Strings denoting the ordinal energy or environmental performance of
		/// building element categories. Specifically, floors, walls, windows,
		/// roofs, hot water, and heating. Renewables and mechanical ventilation
		/// are represented in the D-EPC register, but don't have ordinals. Cooling
		/// has no representation in the register 
		/// </summary>
		public readonly static string[] QUALITY_ENUMS = ["N/A", "Very Poor", "Poor", "Average", "Good", "Very Good"];
		/// <summary>
		/// The D-EPC register property that contains the certificate's D-EPC rating
		/// </summary>
		public const string CURRENT_ENERGY_RATING_ABEL = "CURRENT_ENERGY_RATING";
		/// <summary>
		/// The D-EPC register property that contains the certificate's D-EPC potential rating
		/// as calculated by the RdSAP or SAP calculator.
		/// </summary>
		public const string POTENTIAL_ENERGY_RATING_LABEL = "POTENTIAL_ENERGY_RATING";
		/// <summary>
		/// The D-EPC register property that contains the certificate's D-EPC efficiency
		/// as calculated by the RdSAP or SAP calculator.
		/// </summary>
		public const string CURRENT_ENERGY_EFFICIENCY_LABEL = "CURRENT_ENERGY_EFFICIENCY";
		/// <summary>
		/// The D-EPC register property that contains the certificate's potential D-EPC efficiency
		/// as calculated by the RdSAP or SAP calculator.
		/// </summary>
		public const string POTENTIAL_ENERGY_EFFICIENCY_LABEL = "POTENTIAL_ENERGY_EFFICIENCY";
		/// <summary>
		/// Unlabelled lighting kLm.h/yr scaling factor (occupants * area)^0.4714 * this. Ok proxy 
		/// for internal gains contirbution. Section 5 of SAP, from equation L1. TODO: Implemnent L2
		/// and L3.
		/// </summary>
		public const float LIGHTING_GAINS_BASE = 59.73f; // From SAP manual
		/// <summary>
		/// Sensible heat gains from occupants W/occupant. No latent gains in SAP...
		/// ...or HEM when that comes out.
		/// </summary>
		public const float METABOLIC_RATE = 60;
		/// <summary>
		/// Base kWh/m²/annum for electric appliances. Appendix L2(L13)
		/// </summary>
		public const float APPLIANCE_BASE_POWER = 207.8f; // SAP L2
		/// <summary>
		/// D-EPC rating efficiency point brackets. Intended to be 1 to 100, but green design
		/// and operation can bosst the efficiency upwards of 125. The top 20 in the register
		/// are at least 249. The highest,presumably a mistake, is 13,600;
		/// </summary>
		public static readonly Dictionary<string, (int lower, int upper)> RATING_BRACKETS = new()
		{
			["A"] = (92, 999),
			["B"] = (81, 91),
			["C"] = (69, 80),
			["D"] = (55, 68),
			["E"] = (39, 54),
			["F"] = (21, 38),
			["G"] = (-999, 20)
		};
		/// <summary>
		/// Get the lowest score of the given D-EPC rating brackect. E.g, 39 for E
		/// </summary>
		/// <param name="rating">D-EPC rating label</param>
		/// <returns></returns>
		public static int RatingLowerBound(string rating) => RATING_BRACKETS[rating].lower;
		/// <summary>
		/// Get the highest score of the given D-EPC rating brackect. E.g, 54 for E
		/// </summary>
		/// <param name="rating">D-EPC rating label</param>
		/// <returns></returns>
		public static int RatingUpperBound(string rating) => RATING_BRACKETS[rating].upper;

		/// <summary>
		/// The D-EPC register record the Building object is based on. Instance members and
		/// lookup reference data come from here.
		/// </summary>
		[NoColumn]
		public Dictionary<string, string> DEPCRegisterData;
		/// <summary>
		/// The standard constructor. Only populates instance members
		/// with features inferrable from the D-EPC register record alone.
		/// </summary>
		/// <param name="data"></param>
		public RdSAPBuilding(Dictionary<string, string> data)
		{
			// Ordinal efficiencies
			DEPCRegisterData = data;
			MapStandardStringProperties();
			MapUnsafeStandardProperties();
		}
		/// <summary>
		/// <para>The constructor that includes RdSAPReferenceDataSet. Used to extend the D-EPC properties.</para>
		/// <para>Notably:</para>
		///	<para> - Inferring U-values, thermal mass, and envelope geometry</para>
		///	<para> - Indexed heating control definitions. Has TRVs etc.</para>
		/// </summary>
		/// <param name="data">D-EPC register records. Represents a single certificate.</param>
		/// <param name="reference">A collection of references created for the RdSAP estimator</param>
		public RdSAPBuilding(Dictionary<string, string> data, RdSAPReferenceDataSet reference)
		{
			// Ordinal efficiencies
			DEPCRegisterData = data;
			ReferenceData = reference;
			MapStandardStringProperties();
			MapUnsafeStandardProperties();
			AddExtendedProperties(reference);
		}
		

		/// <summary>
		/// Map standard string properties to instance methods. These are string-based properties
		/// which shouldn't be able to break anything if they've corrupt. See MapUnsafeStandarStringProperties()
		/// for handling corrupt values that need to be handled gracefully.
		/// </summary>
		public void MapStandardStringProperties()
		{
			/*
			 * Meta data
			 */
			// Metadata
			Address1					= DEPCRegisterData["ADDRESS1"];
			Address2					= DEPCRegisterData["ADDRESS2"];
			Address3					= DEPCRegisterData["ADDRESS3"];
			BuildingReferenceNumber		= DEPCRegisterData["BUILDING_REFERENCE_NUMBER"];
			BuiltForm					= DEPCRegisterData["BUILT_FORM"];
			Constituency				= DEPCRegisterData["CONSTITUENCY_LABEL"];
			ConstituencyReferenceCode	= DEPCRegisterData["CONSTITUENCY"];
			ConstructionAgeBand			= DEPCRegisterData["CONSTRUCTION_AGE_BAND"];
			County						= DEPCRegisterData["COUNTY"];
			EnergyTariff				= DEPCRegisterData["ENERGY_TARIFF"];
			FloorDescription			= DEPCRegisterData["FLOOR_DESCRIPTION"];
			GlazedType					= DEPCRegisterData["GLAZED_TYPE"];
			HotwaterDescription			= DEPCRegisterData["HOTWATER_DESCRIPTION"];
			LightingDescription			= DEPCRegisterData["LIGHTING_DESCRIPTION"];
			LmkKey						= DEPCRegisterData["LMK_KEY"];
			LocalAuthority				= DEPCRegisterData["LOCAL_AUTHORITY_LABEL"];
			LocalAuthorityReferenceCode	= DEPCRegisterData["LOCAL_AUTHORITY"];
			MainsGasFlag				= DEPCRegisterData["MAINS_GAS_FLAG"].Trim().ToLower() == "y";
			MainFuel					= DEPCRegisterData["MAIN_FUEL"];
			MainheatcontDescription		= DEPCRegisterData["MAINHEATCONT_DESCRIPTION"];
			MainHeatingControls			= DEPCRegisterData["MAIN_HEATING_CONTROLS"];
			MainheatDescription			= DEPCRegisterData["MAINHEAT_DESCRIPTION"];
			Postcode					= DEPCRegisterData["POSTCODE"];
			PostTown					= DEPCRegisterData["POSTTOWN"];
			PropertyType				= DEPCRegisterData["PROPERTY_TYPE"];
			RoofDescription				= DEPCRegisterData["ROOF_DESCRIPTION"];
			SecondheatDescription		= DEPCRegisterData["SECONDHEAT_DESCRIPTION"];
			TransactionType				= DEPCRegisterData["TRANSACTION_TYPE"];
			WallsDescription			= DEPCRegisterData["WALLS_DESCRIPTION"];
			WindowsDescription			= DEPCRegisterData["WINDOWS_DESCRIPTION"];
			HasSolarWater				= DEPCRegisterData["SOLAR_WATER_HEATING_FLAG"].Trim().ToLower() == "y";
			// Ratings
			DEPCRating					= DEPCRegisterData[CURRENT_ENERGY_RATING_ABEL];
			DEPCRatingPotential			= DEPCRegisterData[POTENTIAL_ENERGY_RATING_LABEL];
			/*
			 * Define Ordinal efficiency values. Things with enumerated values."Very poor", "poor",.., "Very good".
			 */
			HeatingOrdinalEnergyEfficiency			= Array.IndexOf(QUALITY_ENUMS, DEPCRegisterData["MAINHEAT_ENERGY_EFF"]);
			LightingOrdinalEnergyEfficiency			= Array.IndexOf(QUALITY_ENUMS, DEPCRegisterData["LIGHTING_ENERGY_EFF"]);
			HotWaterOrdinalEnergyEfficiency			= Array.IndexOf(QUALITY_ENUMS, DEPCRegisterData["HOT_WATER_ENERGY_EFF"]);
			RoofOrdinalEnergyEfficiency				= Array.IndexOf(QUALITY_ENUMS, DEPCRegisterData["ROOF_ENERGY_EFF"]);
			HeatingOrdinalEnergyEfficiency			= Array.IndexOf(QUALITY_ENUMS, DEPCRegisterData["MAINHEAT_ENERGY_EFF"]);
			HeatingControlOrdinalEnergyEfficiency	= Array.IndexOf(QUALITY_ENUMS, DEPCRegisterData["MAINHEATC_ENERGY_EFF"]);
			WallsOrdinalEnergyEfficiency			= Array.IndexOf(QUALITY_ENUMS, DEPCRegisterData["WALLS_ENERGY_EFF"]);
			WindowsOrdinalEnergyEfficiency			= Array.IndexOf(QUALITY_ENUMS, DEPCRegisterData["WINDOWS_ENERGY_EFF"]);
			SecondaryHeatingOrdinalEnergyEfficiency = Array.IndexOf(QUALITY_ENUMS, DEPCRegisterData["SHEATING_ENERGY_EFF"]);
			/*
			 * Verify any dates. Dates tell us which RdSAP/SAP version was used. Doesn't affect RdSAP estimator much but
			 * it's the only proxy for version.
			 */
			DateTime lodgementDate;
			if (!DateTime.TryParse(DEPCRegisterData["LODGEMENT_DATE"], out lodgementDate))
			{
				CorruptMessage	= $"Couldn't parse lodgement date from '{DEPCRegisterData["LODGEMENT_DATE"]}";
				return;
			}
			/*
			 * Fire places
			 */
			int numberOfOpenFireplaces = 0;
			int.TryParse(DEPCRegisterData["NUMBER_OPEN_FIREPLACES"], out numberOfOpenFireplaces);
			NumberOfOpenFireplaces = numberOfOpenFireplaces;
			/*
			 * Building form and property type properties
			 */
			// Convert built forms into categorical values
			switch (DEPCRegisterData["BUILT_FORM"])
			{
				case "Detached":
					IsDetached = true;
					break;
				case "Mid-Terrace":
					IsMidTerrace = true;
					break;
				case "End-Terrace":
					IsEndTerrace = true;
					break;
				case "Semi-Detached":
					IsSemiDetached = true;
					break;
				case "Enclosed Mid-Terrace":
					IsEnclosed = true;
					IsMidTerrace = true;
					break;
				case "Enclosed End-Terrace":
					IsEnclosed = true;
					IsEndTerrace = true;
					break;
			}
			/*
			 * Geometries
			 */
			// Location of the residential property in its constituent buidling
			switch (DEPCRegisterData["FLOOR_LEVEL"].ToString().ToLower())
			{
				case "basement":
					IsBasement = true;
					break;
				case "ground":
					IsGroundFloor = true;
					break;
				case "top":
					IsTopFloor = true;
					break;
			}
			// How much internal surface is shared with the communal corridor?
			switch (DEPCRegisterData["HEAT_LOSS_CORRIDOR"])
			{
				case "NO DATA!":
					HeatLossCorridor = 0;
					break;
				case "no corridor":
					HeatLossCorridor = 0;
					break;
				default:
					HeatLossCorridor = 1 + Array.IndexOf(["heated corridor", "unheated corridor"], DEPCRegisterData["HEAT_LOSS_CORRIDOR"]);
					break;
			}
			/*
			 * Lighting stuff 
			 */
			// Determine percentage of lighting with Tungsten or Halogen lamps
			float lowEnergyLighting	= 0;
			float.TryParse(DEPCRegisterData["LOW_ENERGY_LIGHTING"], out lowEnergyLighting);
			LowEnergyLighting		= lowEnergyLighting;
			/*
			 * Mechanical ventilation stuff
			 */
			// Define mechanical ventilation properties
			switch (DEPCRegisterData["MECHANICAL_VENTILATION"])
			{
				case "mechanical, extract only":
					HasMechanicalExtract	= true;
					break;
				case "mechanical, supply and extract":
					HasMechanicalExtract	= true;
					HasMechanicalSupply		= true;
					break;
				default:
					HasMechanicalExtract	= false;
					HasMechanicalSupply		= false;
					break;
			}
			/*
			 * Windows stuff
			 */
			//Convert GLAZED_AREA enumerated size description to glazing area scaling factor
			string windowSizeText = DEPCRegisterData["GLAZED_AREA"].ToLower();
			switch (windowSizeText)
			{
				case "much less":
					WindowSizeFactor	= -1.4f;
					break;
				case "less":
					WindowSizeFactor	= -1f;
					break;
				case "normal":
					WindowSizeFactor	= 0f;  // WARNING!!!!! Changed from 1. Might affect RdSAP estimator performance
					break;
				case "much more":
					WindowSizeFactor	= 1.4f;
					break;
				case "more":
					WindowSizeFactor	= 1.25f;
					break;
			}
			// Determine the percentage of glazing that is double or triple pane
			float multiGlazeProportion	= 0f;
			float.TryParse(DEPCRegisterData["MULTI_GLAZE_PROPORTION"], out multiGlazeProportion);
			MultiGlazeProportion		= multiGlazeProportion;
		}
		/// <summary>
		/// Update  the RdSAP estimator extended properties reference data. Also updates the extended
		/// properties.
		/// </summary>
		/// <param name="reference"></param>
		public void UpdateReference(RdSAPReferenceDataSet reference)
		{
			ReferenceData	= reference;
			AddExtendedProperties(ReferenceData);
		} 
		/// <summary>
		/// kgCO2/kWh for fuels. A place to store building-specific emission factors.
		/// </summary>
		[NoColumn]
		public Dictionary<string, float> EmissionFactors { get; set; } = new Dictionary<string, float>()
		{
			["electricity"] = 0.519f,
			["lpg"]			= 0.245f,
			["oil"]			= 0.297f,
			["gas"]			= 0.216f,
			["dual"]		= 0.205f,
			["smokeless"]	= 0.392f,
			["coal"]		= 0.291f,
			["no heating"]	= 0f,
			["community"]	= 0.24f
		};
		/// <summary>
		/// <para>Have the D-EPC register properties been extended with RdSAP estimator features?</para>
		/// <para>The RdSAP estimator  defined a bunch of inference data used 
		/// to extended the D-EPC register properties. The Building can be extended by
		/// passing an RdSAP.Rference.ReferenceDataSet to the constructors or call UpdateReference</para>
		/// </summary>
		public bool HasExtendedProperties { get; protected set; }
		/*--- Features and stuff ---*/
		/// <summary>
		/// kgCO2/kWh main heating fuel emissions factor
		/// </summary>
		public float MainFuelFactor { get; protected set; }
		/// <summary>
		/// Index of the Construction in the A to L SAP brackets. E.g "England and Wales: 1950-1966"
		/// = 4, "before 1900" = 0, "after 2007" = 10
		/// </summary>
		public int ConstructionAgeIndex {get; protected set;}
		/// <summary>
		/// Glazing size scaling factor based on GLAZED_AREA enumerated property "Less Than Typical", "Normal", "More Than Typical".
		/// <para>It's rule of thumb factors just kind of winged by seeing how different configurations affected the RdSAP
		/// estimator's performance.</para>
		/// </summary>
		public float WindowSizeFactor {get; protected set;}
		/// <summary>
		/// Is the property in a basement?
		/// </summary>
		public bool IsBasement {get; protected set;} = false;
		/// <summary>
		/// Is the property on the ground floor?
		/// </summary>
		public bool IsGroundFloor {get; protected set;} = false;
		/// <summary>
		/// Is the property on the top floor?
		/// </summary>
		public bool IsTopFloor {get; protected set;} = false;
		/// <summary>
		/// Is the property mid-terrace
		/// </summary>
		public bool IsMidTerrace {get; protected set;} = false;
		/// <summary>
		/// Is the property end-terrace
		/// </summary>
		public bool IsEndTerrace {get; protected set;} = false;
		/// <summary>
		/// Has 3 or more enclosed sides?
		/// </summary>
		public bool IsEnclosed {get; protected set;} = false;
		/// <summary>
		/// Is enclosed and an end-terrace
		/// </summary>
		public bool IsEnclosedEndTerrace { get => IsEnclosed && IsEndTerrace; }
		/// <summary>
		/// Is enclosed and an mid-terrace
		/// </summary>
		public bool IsEnclosedMidTerrace { get => IsEnclosed && IsMidTerrace; }
		/// <summary>
		/// Is semi-detached?
		/// </summary>
		public bool IsSemiDetached {get; protected set;} = false;
		/// <summary>
		/// Is detached? House, park home, bungalow, maisonette
		/// </summary>
		public bool IsDetached {get; protected set;} = false;
		/// <summary>
		/// The D-EPC rating of the certificate this building's defined by
		/// </summary>
		public string DEPCRating { get; protected set; }
		/// <summary>
		/// The potential D-EPC rating. A reference model score akin to Reference or Notional
		/// SBEM inp models. It's the building model upgraded with retrofit measures the calculator
		/// considers cost-effective. It's closer to S63 Scotland Action Plans in that the assessor can disable
		/// measures.
		/// </summary>
		public string DEPCRatingPotential { get; protected set; }
		/// <summary>
		/// The D-EPC efficiency of the certificate this building's defined by
		/// </summary>
		public float DEPCEnergyEfficiency { get; protected set; }
		/// <summary>
		/// The potential D-EPC efficiency. A reference model score akin to Reference or Notional
		/// SBEM inp models. It's the building model upgraded with retrofit measures the calculator
		/// considers cost-effective. It's closer to S63 Scotland Action Plans in that the assessor can disable
		/// measures.
		/// </summary>
		public float DEPCEnergyEfficiencyPotential { get; protected set; }
		/// <summary>
		/// Are there solar panels on site?
		/// </summary>
		public bool HasSolarPanels { get; protected set; }
		/// <summary>
		/// Solar panel surface area (m²)
		/// </summary>
		public float SolarPanelArea { get; protected set; }
		/// <summary>
		/// The ordinal efficieny label of the main heating system. E.g "Very poor", "poor",..., "Very good
		/// </summary>
		public string HeatingEnergyEfficiencyLabel { get => DEPCRegisterData["MAINHEAT_ENERGY_EFF"]; }
		/// <summary>
		/// The ordinal efficieny label of the lighting. E.g "Very poor", "poor",..., "Very good
		/// </summary>
		public string LightingEnergyEfficiencyLabel { get => DEPCRegisterData["LIGHTING_ENERGY_EFF"]; }
		/// <summary>
		/// The ordinal efficieny label of the hot water system. E.g "Very poor", "poor",..., "Very good
		/// </summary>
		public string HotWaterEnergyEfficiencyLabel { get => DEPCRegisterData["HOT_WATER_ENERGY_EFF"]; }
		/// <summary>
		/// The ordinal efficieny label of the roof. E.g "Very poor", "poor",..., "Very good
		/// </summary>
		public string RoofEnergyEfficiencyLabel { get => DEPCRegisterData["ROOF_ENERGY_EFF"]; }
		/// <summary>
		/// The ordinal efficieny label of the heating controls. E.g "Very poor", "poor",..., "Very good
		/// </summary>
		public string HeatingControlEnergyEfficiencyLabel { get => DEPCRegisterData["MAINHEATC_ENERGY_EFF"]; }
		/// <summary>
		/// The ordinal efficieny label of the walls. E.g "Very poor", "poor",..., "Very good
		/// </summary>
		public string WallsEnergyEfficiencyLabel { get => DEPCRegisterData["WALLS_ENERGY_EFF"]; }
		/// <summary>
		/// The ordinal efficieny label of the windows. E.g "Very poor", "poor",..., "Very good
		/// </summary>
		public string WindowsEnergyEfficiencyLabel { get => DEPCRegisterData["WINDOWS_ENERGY_EFF"]; }
		/// <summary>
		/// The ordinal efficieny label of the secondary heating system. E.g "Very poor", "poor",..., "Very good
		/// </summary>
		public string SecondaryHeatingEnergyEfficiencyLabel { get => DEPCRegisterData["SHEATING-ENERGY_EFF"]; }
		/// <summary>
		/// Global unique identifier. Not entirely sure the scope since they appear outwith the Open Data Communites
		/// portal like the Office for National Statistics data.
		/// </summary>
		public string LmkKey { get; protected set; } = string.Empty;
		/// <summary>
		/// The property's unique identifier. It is for all datasets the building is referenced, not just the EPC registers.
		/// </summary>
		public string BuildingReferenceNumber { get; protected set; } = string.Empty;
		/// <summary>
		/// First line of the address.
		/// </summary>
		public string Address1 { get; protected set; } = string.Empty;
		/// <summary>
		/// Second line of the address.
		/// </summary>
		public string Address2 { get; protected set; } = string.Empty;
		/// <summary>
		/// Third line of the address.
		/// </summary>
		public string Address3 { get; protected set; } = string.Empty;
		/// <summary>
		/// The postcode
		/// </summary>
		public string Postcode { get; protected set; } = string.Empty;
		/// <summary>
		/// Property type: Flat, House, Bungalow, Park home, Maisonette
		/// </summary>
		public string PropertyType { get; protected set; } = string.Empty;
		/// <summary>
		/// Built form: Detached, semi-detach, mid-terrace, etc.
		/// </summary>
		public string BuiltForm { get; protected set; } = string.Empty;
		/// <summary>
		/// Name of the constituency the property is in
		/// </summary>
		public string Constituency { get; protected set; } = string.Empty;
		/// <summary>
		/// The local authorities global identifier. E.g Middlesbrough is E06000002.
		/// </summary>
		public string LocalAuthorityReferenceCode { get; protected set; } = string.Empty;
		/// <summary>
		/// Name of the local authority that governs the building's location
		/// </summary>
		public string LocalAuthority { get; protected set; } = string.Empty;
		/// <summary>
		/// The county the building is in.
		/// </summary>
		public string County { get; protected set; } = string.Empty;
		/// <summary>
		/// Date the certificate was lodged
		/// </summary>
		public string LodgementDate { get; protected set; } = string.Empty;
		/// <summary>
		/// Reason for the EPC: For lease, for sale, voluntary, etc.
		/// </summary>
		public string TransactionType { get; protected set; } = string.Empty;
		/// <summary>
		/// As-built enviornmental impact assessment.
		/// <para>Enviornmental impact is a kgCO2-based performance indicator</para>
		/// </summary>
		public float EnvironmentImpactCurrent { get; protected set; }
		/// <summary>
		/// The potential enviornmental impact assessment.
		/// <para>Enviornmental impact is a kgCO2-based performance indicator</para>
		/// </summary>
		public float EnvironmentImpactPotential { get; protected set; }
		/// <summary>
		/// As-built annual energy consumption kWh/m².
		/// </summary>
		public float EnergyConsumptionCurrent { get; protected set; }
		/// <summary>
		/// Potential annual energy consumption kWh/m².
		/// </summary>
		public float EnergyConsumptionPotential { get; protected set; }
		/// <summary>
		/// The as-built CO2 emissions (tCO2)
		/// </summary>
		public float Co2EmissionsCurrent { get; protected set; }
		/// <summary>
		/// The As-built emissions per floor area (kgCO2/m²).
		/// </summary>
		public float Co2EmissCurrPerFloorArea { get; protected set; }
		/// <summary>
		/// The potential CO2 emissions (tCO2)
		/// </summary>
		public float Co2EmissionsPotential { get; protected set; }
		/// <summary>
		/// Total floor area (m²)
		/// </summary>
		public float TotalFloorArea { get; protected set; }
		/// <summary>
		/// Energy tariff: E.g. single or dual.
		/// </summary>
		public string EnergyTariff { get; protected set; } = string.Empty;
		/// <summary>
		/// Is the property connected to the gas network?
		/// </summary>
		public bool MainsGasFlag { get; protected set; }
		/// <summary>
		/// What storey is the flat on? 
		/// </summary>
		public int FloorLevel { get; protected set; }
		/// <summary>
		/// Is this property on the top floor of flats?
		/// </summary>
		public bool FlatTopStorey { get; protected set; }
		/// <summary>
		/// Number of storeys of the building the flat's in.
		/// </summary>
		public int FlatStoreyCount { get; protected set; }
		/// <summary>
		/// Description of main heating controls. E.g. Programmer, room thermostat and TRVs or Automatic charge control.
		/// </summary>
		public string MainHeatingControls { get; protected set; } = string.Empty;
		/// <summary>
		/// Percentage of glazing with two or more panes.
		/// </summary>
		public float MultiGlazeProportion { get; protected set; }
		/// <summary>
		/// Description of glazing. E.g. double glazing installed during or after 2002 or double glazing installed before 2002
		/// </summary>
		public string GlazedType { get; protected set; } = string.Empty;
		/// <summary>
		/// Does the site's ventilation strategy include mechanical supply?
		/// </summary>
		public bool HasMechanicalSupply { get; protected set; }
		/// <summary>
		/// Does the site's ventilation strategy include mechanical extract?
		/// </summary>
		public bool HasMechanicalExtract { get; protected set; }
		/// <summary>
		/// Is there any mechanical ventilation on site?
		/// </summary>
		public bool HasMechanicalVentilation { get => HasMechanicalExtract || HasMechanicalSupply; }
		/// <summary>
		/// Number of building extensions
		/// </summary>
		public int ExtensionCount { get; protected set; }
		/// <summary>
		/// NumberOfHabitableRooms as a float for the LightGBM RdSAP estimator.
		/// </summary>
		public float NumberOfHabitableRoomsFloatForLightGBM { get => NumberOfHabitableRooms; }
		/// <summary>
		/// Number of rooms considered part of the core house. Not necessarily heated.
		/// </summary>
		public int NumberOfHabitableRooms { get; protected set; }
		/// <summary>
		/// NumberOfHeatedRooms as a float for the LightGBM RdSAP estimator.
		/// </summary>
		public float NumberOfHeatedRoomsFloatForLightGBM { get => NumberOfHeatedRooms; }
		/// <summary>
		/// Number of rooms with heating.
		/// </summary>
		public int NumberOfHeatedRooms { get; protected set; }
		/// <summary>
		/// Number of rooms with heated water.
		/// </summary>
		public int NumberOfWetRooms { get; protected set; }

		/// <summary>
		/// Percentage of fixed lighting that are energy efficient (%).
		/// </summary>
		public float LowEnergyLighting { get; protected set; }
		/// <summary>
		/// Number of open fireplaces.
		/// </summary>
		public int NumberOfOpenFireplaces { get; protected set; }
		/// <summary>
		/// Description of the hot water system. E.g. From main system or Electric instantaneous at point of use
		/// </summary>
		public string HotwaterDescription { get; protected set; } = string.Empty;
		/// <summary>
		/// The hot water system's ordinal energy efficiency.
		/// /// <para>Ordinal efficiencies are enumerated values from the sorted QUALITY_ENUMS array. E.g. "Very poor" = 1, "Very good" = 5.</para>
		/// </summary>
		public int HotWaterOrdinalEnergyEfficiency { get; protected set; }
		/// <summary>
		/// The hot water systems's emissions-based ordinal environmental efficiency.
		/// <para>Ordinal efficiencies are enumerated values from the sorted QUALITY_ENUMS array. E.g. "Very poor" = 1, "Very good" = 5.</para>
		/// </summary>
		public int HotWaterOrdinalEnvironmentalEfficiency{ get; protected set; }
		/// <summary>
		/// Description of the ground floor. E.g. Solid, no insulation (assumed) or (other premises below).
		/// </summary>
		public string FloorDescription { get; protected set; } = string.Empty;
		/// <summary>
		/// The bottom floors' ordinal energy efficiency.
		/// /// <para>Ordinal efficiencies are enumerated values from the sorted QUALITY_ENUMS array. E.g. "Very poor" = 1, "Very good" = 5.</para>
		/// </summary>
		public int FloorOrdinalEnergyEfficiency { get; protected set; }
		/// <summary>
		/// The floors' emissions-based ordinal environmental efficiency.
		/// <para>Ordinal efficiencies are enumerated values from the sorted QUALITY_ENUMS array. E.g. "Very poor" = 1, "Very good" = 5.</para>
		/// </summary>
		public int FloorOrdinalEnvironmentalEfficiency{ get; protected set; }
		/// <summary>
		/// Description of the windows. E.g. Mostly double glazing or Fully double glazed.
		/// </summary>
		public string WindowsDescription { get; protected set; } = string.Empty;
		/// <summary>
		/// The windows' ordinal energy efficiency.
		/// /// <para>Ordinal efficiencies are enumerated values from the sorted QUALITY_ENUMS array. E.g. "Very poor" = 1, "Very good" = 5.</para>
		/// </summary>
		public int WindowsOrdinalEnergyEfficiency { get; protected set; }
		/// <summary>
		/// The windows' emissions-based ordinal environmental efficiency.
		/// <para>Ordinal efficiencies are enumerated values from the sorted QUALITY_ENUMS array. E.g. "Very poor" = 1, "Very good" = 5.</para>
		/// </summary>
		public int WindowsOrdinalEnvironmentalEfficiency{ get; protected set; }
		/// <summary>
		/// Description of the external walls. E.g. Cavity wall, filled cavity or Solid brick, as built, no insulation (assumed)
		/// </summary>
		public string WallsDescription { get; protected set; } = string.Empty;
		/// <summary>
		/// The external walls' ordinal energy efficiency.
		/// /// <para>Ordinal efficiencies are enumerated values from the sorted QUALITY_ENUMS array. E.g. "Very poor" = 1, "Very good" = 5.</para>
		/// </summary>
		public int WallsOrdinalEnergyEfficiency { get; protected set; }
		/// <summary>
		/// The external walls' emissions-based ordinal environmental efficiency.
		/// <para>Ordinal efficiencies are enumerated values from the sorted QUALITY_ENUMS array. E.g. "Very poor" = 1, "Very good" = 5.</para>
		/// </summary>
		public int WallsOrdinalEnvironmentalEfficiency{ get; protected set; }
		/// <summary>
		/// Description of the secondary heating system. E.g. Room heaters, electric or Portable electric heaters (assumed).
		/// </summary>
		public string SecondheatDescription { get; protected set; } = string.Empty;
		/// <summary>
		/// The main heating system's ordinal energy efficiency.
		/// /// <para>Ordinal efficiencies are enumerated values from the sorted QUALITY_ENUMS array. E.g. "Very poor" = 1, "Very good" = 5.</para>
		/// </summary>
		public int HeatingOrdinalEnergyEfficiency {get; protected set;}
		/// <summary>
		/// The main heating system controls' ordinal energy efficiency.
		/// /// <para>Ordinal efficiencies are enumerated values from the sorted QUALITY_ENUMS array. E.g. "Very poor" = 1, "Very good" = 5.</para>
		/// </summary>
		public int HeatingControlOrdinalEnergyEfficiency { get; protected set; }
		/// <summary>
		/// The secondary heating system's ordinal energy efficiency.
		/// /// <para>Ordinal efficiencies are enumerated values from the sorted QUALITY_ENUMS array. E.g. "Very poor" = 1, "Very good" = 5.</para>
		/// </summary>
		public int SecondaryHeatingOrdinalEnergyEfficiency {get; protected set;}
		/// <summary>
		/// Description of the roof. E.g. Pitched, 100 mm loft insulation or (another dwelling above).
		/// </summary>
		public string RoofDescription { get; protected set; } = string.Empty;
		/// <summary>
		/// The roof's ordinal energy efficiency.
		/// /// <para>Ordinal efficiencies are enumerated values from the sorted QUALITY_ENUMS array. E.g. "Very poor" = 1, "Very good" = 5.</para>
		/// </summary>
		public int RoofOrdinalEnergyEfficiency {get; protected set;}
		/// <summary>
		/// The roofs's emissions-based ordinal environmental efficiency.
		/// <para>Ordinal efficiencies are enumerated values from the sorted QUALITY_ENUMS array. E.g. "Very poor" = 1, "Very good" = 5.</para>
		/// </summary>
		public int RoofOrdinalEnvironmentalEfficiency{ get; protected set; }
		/// <summary>
		/// Description of the main heating system. E.g. Boiler and radiators, mains gas or Room heaters, electric.
		/// </summary>
		public string MainheatDescription { get; protected set; } = string.Empty;
		/// <summary>
		/// Main heating controls description. E.g. Boiler and radiators, mains gas or 
		/// </summary>
		public string MainheatcontDescription { get; protected set; } = string.Empty;
		/// <summary>
		/// MainheatControlOrdinalEfficiency as float for LightGBM RdSAP estimator.
		/// </summary>
		public int MainheatControlOrdinalEnergyEfficiency {get; protected set;}
		/// <summary>
		/// Main heating control's emissions-based ordinal environmental efficiency.
		/// <para>Ordinal efficiencies are enumerated values from the sorted QUALITY_ENUMS array. E.g. "Very poor" = 1, "Very good" = 5.</para>
		/// </summary>
		public int MainheatControlOrdinalEnvironmentalEfficiency{ get; protected set; }
		/// <summary>
		/// Description of the fixed lighting. E.g. Low energy lighting in 75% of fixed outlets or No low energy lighting.
		/// </summary>
		public string LightingDescription { get; protected set; } = string.Empty;
		/// <summary>
		/// The fixed lighting's ordinal energy efficiency.
		/// /// <para>Ordinal efficiencies are enumerated values from the sorted QUALITY_ENUMS array. E.g. "Very poor" = 1, "Very good" = 5.</para>
		/// </summary>
		public int LightingOrdinalEnergyEfficiency { get; protected set; }
		/// <summary>
		/// Lighting's emissions-based ordinal environmental efficiency.
		/// <para>Ordinal efficiencies are enumerated values from the sorted QUALITY_ENUMS array. E.g. "Very poor" = 1, "Very good" = 5.</para>
		/// </summary>
		public int LightingOrdinalEnvironmentalEfficiency{ get; protected set; }
		/// <summary>
		/// Main fuel description. Type and is community. E.g mains gas (no community) or electricity (no community)
		/// </summary>
		public string MainFuel { get; protected set; } = string.Empty;
		/// <summary>
		/// Number of wind turbines on site.
		/// </summary>
		public int WindTurbineCount { get; protected set; }
		/// <summary>
		/// Heat loss corridor category: no corridor, unheated, or heated
		/// </summary>
		public float HeatLossCorridor { get; protected set; }
		/// <summary>
		/// Length of internal envelopes adjacent to communal corridors
		/// </summary>
		public float UnheatedCorridorLength { get; protected set; }
		/// <summary>
		/// Average floor height (m). Not consistently available.
		/// </summary>
		public float FloorHeight { get; protected set; }
		/// <summary>
		/// Are there solar panels on site?
		/// </summary>
		public string PhotoSupply { get; protected set; } = string.Empty;
		/// <summary>
		/// Is there solar hot water on site?
		/// </summary>
		public bool HasSolarWater { get; protected set; }
		/// <summary>
		/// Description of the ventilation system. Natural, extract-only, or supply and extract
		/// </summary>
		public string MechanicalVentilation { get; protected set; } = string.Empty;
		/// <summary>
		/// The associated constituency's government unique identifier.
		/// </summary>
		public string ConstituencyReferenceCode { get; protected set; } = string.Empty;
		/// <summary>
		/// The postal town name. E.g. MIDDLESBROUGH.
		/// </summary>
		public string PostTown { get; protected set; } = string.Empty;
		/// <summary>
		/// Who owns and who lives. Owner-occupied,  rental (private), rental (social), etc.
		/// </summary>
		public string Tenure { get; protected set; } = string.Empty;
		/// <summary>
		/// Total number of light fixture
		/// </summary>
		public float FixedLightingOutletsCount { get; protected set; }
		/// <summary>
		/// Number of light fixtures with low-energy lighting
		/// </summary>
		public float LowEnergyFixedLightCount { get; protected set; }
		/// <summary>
		/// Unique property reference number. The building's EPC reference number
		/// </summary>
		public string Uprn { get; protected set; } = string.Empty;
		/// <summary>
		/// Where the uprn came from: Address match on register, entered by the assessor, or "" unkknown.
		/// </summary>
		public string UprnSource { get; protected set; } = string.Empty;
		/// <summary>
		/// Pass. Turned up 2025, couldn't find the definition, its only values are 100 and 101, tt's not related to
		/// transaction type.
		/// </summary>
		public string ReportType { get; protected set; } = string.Empty;
		/// <summary>
		/// Estimated annual lighting cost (£).
		/// </summary>
		public float LightingCostCurrent { get; protected set; }
		// <summary>
		/// Estimated potential annual lighting cost (£).
		/// </summary>
		public float LightingCostPotential { get; protected set; }
		/// <summary>
		/// Estimated annual hot water cost (£).
		/// </summary>
		public float HeatingCostCurrent{ get; protected set; }
		/// <summary>
		/// Estimated potential annual heating cost (£).
		/// </summary>
		public float HeatingCostPotential { get; protected set; }
		/// <summary>
		/// Estimated annual hot water cost (£).
		/// </summary>
		public float HotWaterCostCurrent { get; protected set; }
		/// <summary>
		/// Estimated potential annual hot water cost (£).
		/// </summary>
		public float HotWaterCostPotential { get; protected set; }
		/// <summary>
		/// The descriptive age band label. E.g. England and Wales: 1950-1966 or England and Wales: before 1900.
		/// </summary>
		public string ConstructionAgeBand {get; protected set; } = string.Empty;
		/// <summary>
		/// Total floor are of habitable spaces (m²).
		/// </summary>
		public float FloorUValue {get; protected set;}
		/// <summary>
		/// Thermal bridging factor (W/mK).
		/// </summary>
		public float ThermalBridgingFactor {get; protected set;}
		/// <summary>
		/// Calculated window area (m²) This is NOT as-built area
		/// <para> See S3.7.1 of SAP 2012 <a href="https://files.bregroup.com/SAP/SAP-2012_9-92.pdf?"></a></para>
		/// </summary>
		public float WindowArea {get; protected set;}
		/// <summary>
		/// Ratio of glaizng to floor area.
		/// <para>Note: Wall to floor would've been preferrable but I couldn't figure out how to 
		/// wrangle it form the D-EPC register properties.</para>
		/// </summary>
		public float WindowTofloorRatio {get; protected set;}
		/// <summary>
		/// Glazing G-Value: Total solar transmittance
		/// </summary>
		public float GlassGValue {get; protected set;}
		/// <summary>
		/// Glazing average U-Value W/m²K
		/// </summary>
		public float GlassUValue {get; protected set;}
		/// <summary>
		/// Roof average U-Value W/m²K
		/// </summary>
		public float RoofUValue {get; protected set;}
		/// <summary>
		/// External wall average U-Value W/m²K
		/// </summary>
		public float WallUValue {get; protected set;}
		/// <summary>
		/// Wall thickness (mm)
		/// </summary>
		public float WallThickness {get; protected set;}

		// Properties for LightGBM
		/// <summary>
		/// ConstructionAgeIndex as float for LightGBM RdSAP estimator.
		/// </summary>
		public float ConstructionAgeIndexFloatForLightGBM { get => ConstructionAgeIndex; }
		/// <summary>
		/// ExtensionCount as float for LightGBM RdSAP estimator.
		/// </summary>
		public float ExtensionCountFloatForLightGBM { get => ExtensionCount; }
		/// <summary>
		/// NumberOfWetRooms as float for LightGBM RdSAP estimator.
		/// </summary>
		public float NumberOfWetRoomsFloatForLightGBM { get => NumberOfWetRooms; }
		/// <summary>
		/// NumberOfOpenFireplaces as float for LightGBM RdSAP estimator.
		/// </summary>
		public float NumberOfOpenFireplacesFloatForLightGBM { get => NumberOfOpenFireplaces; }
		/// <summary>
		/// HotWaterOrdinalEnergyEfficiency as float for LightGBM RdSAP estimator.
		/// </summary>
		public float HotWaterOrdinalEnergyEfficiencyFloatForLightGBM { get => HotWaterOrdinalEnergyEfficiency; }
		/// <summary>
		/// HotWaterOrdinalEnevironmentalEfficiency as float for LightGBM RdSAP estimator.
		/// </summary>
		public float HotWaterOrdinalEnvironmentalEfficiencyFloatForLightGBM { get => HotWaterOrdinalEnvironmentalEfficiency; }
		/// <summary>
		/// FloorOrdinalEnergyEfficiency as float for LightGBM RdSAP estimator.
		/// </summary>
		public float FloorOrdinalEnergyEfficiencyFloatForLightGBM { get => FloorOrdinalEnergyEfficiency; }
		/// <summary>
		/// FloorOrdinalEnvironmentalEfficiency as float for LightGBM RdSAP estimator.
		/// </summary>
		public float FloorOrdinalEnvironmentalEfficiencyFloatForLightGBM { get => FloorOrdinalEnvironmentalEfficiency; }
		/// <summary>
		/// WindowsOrdinalEnvironmentalEfficiency as float for LightGBM RdSAP estimator.
		/// </summary>
		public float WindowsOrdinalEnergyEfficiencyFloatForLightGBM { get => WindowsOrdinalEnergyEfficiency; }
		/// <summary>
		/// WindowsOrdinalEnvironmentalEfficiency as float for LightGBM RdSAP estimator.
		/// </summary>
		public float WindowsOrdinalEnvironmentalEfficiencyFloatForLightGBM { get => WindowsOrdinalEnvironmentalEfficiency; }
		/// <summary>
		/// WallsOrdinalEnvironmentalEfficiency as float for LightGBM RdSAP estimator.
		/// </summary>
		public float WallsOrdinalEnergyEfficiencyFloatForLightGBM { get => WallsOrdinalEnergyEfficiency; }
		/// <summary>
		/// WallsOrdinalEnvironmentalEfficiency as float for LightGBM RdSAP estimator.
		/// </summary>
		public float WallsOrdinalEnvironmentalEfficiencyFloatForLightGBM { get => WallsOrdinalEnvironmentalEfficiency; }
		/// <summary>
		/// HeatingOrdinalEnvironmentalEfficiency as float for LightGBM RdSAP estimator.
		/// </summary>
		public float HeatingOrdinalEnergyEfficiencyFloatForLightGBM { get => HeatingOrdinalEnergyEfficiency; }
		/// <summary>
		/// HeatingControlOrdinalEnergyEfficiency as float for LightGBM RdSAP estimator.
		/// </summary>
		public float HeatingControlOrdinalEnergyEfficiencyFloatForLightGBM { get => HeatingControlOrdinalEnergyEfficiency; }
		/// <summary>
		/// SecondaryHeatingControlOrdinalEnergyEfficiency as float for LightGBM RdSAP estimator.
		/// </summary>
		public float SecondaryHeatingOrdinalEnergyEfficiencyFloatForLightGBM { get => SecondaryHeatingOrdinalEnergyEfficiency; }
		/// <summary>
		/// RoofOrdinalEnergyEfficiency as float for LightGBM RdSAP estimator.
		/// </summary>
		public float RoofOrdinalEnergyEfficiencyFloatForLightGBM { get => RoofOrdinalEnergyEfficiency; }
		/// <summary>
		/// RoofOrdinalEnvironmentalEfficiency as float for LightGBM RdSAP estimator.
		/// </summary>
		public float RoofOrdinalEnvironmentalEfficiencyFloatForLightGBM { get => RoofOrdinalEnvironmentalEfficiency; }
		/// <summary>
		/// MainHeatingControlOrdinalEnergyEfficiency as float for LightGBM RdSAP estimator.
		/// </summary>
		public float MainheatControlOrdinalEnergyEfficiencyFloatForLightGBM { get => MainheatControlOrdinalEnergyEfficiency; }
		/// <summary>
		/// MainheatControlOrdinalEnvironmentalEfficiency as float for LightGBM RdSAP estimator.
		/// </summary>
		public float MainheatControlOrdinalEnvironmentalEfficiencyFloatForLightGBM { get => MainheatControlOrdinalEnvironmentalEfficiency; }

		/// <summary>
		/// LightingOrdinalEnergyEfficiency as float for LightGBM RdSAP estimator.
		/// </summary>
		public float LightingOrdinalEnergyEfficiencyFloatForLightGBM { get => LightingOrdinalEnergyEfficiency; }
		/// <summary>
		/// LightingOrdinalEnvironmentalEfficiency as float for LightGBM RdSAP estimator.
		/// </summary>
		public float LightingOrdinalEnvironmentalEfficiencyFloatForLightGBM { get => LightingOrdinalEnvironmentalEfficiency; }
		/*
		 * Reference data sets. The stuff used to populate the extended properties used by the RdSAP estimator.
		 */
		/// <summary>
		/// The complete reference data set. Includes all reference sets and sources for all *Data instance members
		/// </summary>
		[NoColumn]
		public RdSAPReferenceDataSet? ReferenceData {get; protected set;}
		/// <summary>
		/// Shoe.
		/// </summary>
		[NoColumn]
		public ConstructionAgeRecord  ConstructionAgeData {get; protected set;}
		/// <summary>
		/// Information about the external door construction. Only band and index. 
		/// </summary>
		[NoColumn]
		public DoorConstructionRecord? DoorConstructionData {get; protected set;}
		[NoColumn]
		public FloorConstructionRecord? FloorConstructionData {get; protected set;}
		[NoColumn]
		public GlazingTypeRecord? GlazingTypeData {get; protected set;}
		[NoColumn]
		public HeatingControlRecord? HeatingControlData {get; protected set;}
		[NoColumn]
		public LivingAreaFactorRecord? LivingAreaFactorData {get; protected set;}
		[NoColumn]
		public RoofConstructionRecord? RoofConstructionData {get; protected set;}
		[NoColumn]
		public ThermalBridgingRecord? ThermalBridgingData {get; protected set;}
		[NoColumn]
		public WallConstructionRecord? WallConstructionData {get; protected set;}
		[NoColumn]
		public WallThicknessRecord? WallThicknessData {get; protected set;}
		[NoColumn]
		public WindowSizeParametersRecord? WindowSizeParametersData {get; protected set;}
		/// <summary>
		/// The first fault found in the D-EPC register data. Fualts typically breaks parsing down the line.
		/// </summary>
		public string CorruptMessage {  get; protected set; } = string.Empty;
		/// <summary>
		/// Is there at least one fault in the D-EPC register data? At least one because we can only guarantee parsing works up to that point.
		/// </summary>
		public bool IsCorrupt { get => CorruptMessage != ""; }
		/// <summary>
		/// Does corrupt D-EPC register data prevent the building from being populated properly?
		/// </summary>
		public bool HasError { get => IsCorrupt; }
		/// <summary>
		/// Part of the SAP lighting calc around L13. A correction to account for the percentage of low-energy fixtures.
		/// </summary>
		public float LightingCorrectionFactor { get; protected set; }
		/// <summary>
		/// Annual energy consumption of appliances. See SAP 2012, L13.
		/// </summary>
		public float AnnualApplianceEnergyConsumption { get; protected set; } // SAP L13
		/// <summary>
		/// Number of occupants. SAP 2012, Table 1b - Occupancy
		/// <para>N = 1 + 1.76 * [1-exp (-0.000349 * (TFA-13.9)² )] + 0.0013 * (NetInternalArea-13.9)</para>
		/// </summary>
		public float Occupants { get; protected set; }
		/// <summary>
		/// Map D-EPC register properties to instance members. These properties are unsafe in that parsing failures
		/// can invalidate the entire building, maybe throw an unhandled exception.
		/// </summary>
		public void MapUnsafeStandardProperties()
		{
			try
			{
				
				// Renewables
				int pvsArea					= 0;
				bool pvsIsInt				= int.TryParse(DEPCRegisterData["PHOTO_SUPPLY"], out pvsArea);
				if (pvsIsInt)
				{
					HasSolarPanels	= true;
					SolarPanelArea	= pvsArea;
				}
				else
				{
					HasSolarPanels	= false;
					SolarPanelArea	= 0;
				}
			}
			catch (Exception exception)
			{
				CorruptMessage	= $"Failed to map string features. Something must be very wrong with the input D-EPC certificates data. Error message: {exception.Message}";
				return;
			}
			// Try cost and efficiency values
			try
			{
				EnvironmentImpactCurrent	= float.Parse(DEPCRegisterData["ENVIRONMENT_IMPACT_CURRENT"]);
				EnvironmentImpactPotential	= float.Parse(DEPCRegisterData["ENVIRONMENT_IMPACT_POTENTIAL"]);
				EnergyConsumptionCurrent	= float.Parse(DEPCRegisterData["ENERGY_CONSUMPTION_CURRENT"]);
				EnergyConsumptionPotential	= float.Parse(DEPCRegisterData["ENERGY_CONSUMPTION_POTENTIAL"]);
				Co2EmissionsCurrent			= float.Parse(DEPCRegisterData["CO2_EMISSIONS_CURRENT"]);
				Co2EmissCurrPerFloorArea	= float.Parse(DEPCRegisterData["CO2_EMISS_CURR_PER_FLOOR_AREA"]);
				Co2EmissionsPotential		= float.Parse(DEPCRegisterData["CO2_EMISSIONS_POTENTIAL"]);
				LightingCostCurrent			= float.Parse(DEPCRegisterData["LIGHTING_COST_CURRENT"]);
				LightingCostPotential		= float.Parse(DEPCRegisterData["LIGHTING_COST_POTENTIAL"]);
				HeatingCostCurrent			= float.Parse(DEPCRegisterData["HEATING_COST_CURRENT"]);
				HeatingCostPotential		= float.Parse(DEPCRegisterData["HEATING_COST_POTENTIAL"]);
				HotWaterCostCurrent			= float.Parse(DEPCRegisterData["HOT_WATER_COST_CURRENT"]);
				HotWaterCostPotential		= float.Parse(DEPCRegisterData["HOT_WATER_COST_POTENTIAL"]);
			}
			catch
			{
				CorruptMessage = "RdSAPBuilding::MapUnsfeStandardProperties: Emissions/Cost/Impact (e.g HEATING_COST_CURRENT) property with non-numeric value";
				return;
			}
			try
			{
				float floatPlaceHolder;
				int intPlaceHolder;
				bool boolPlaceHolder;

				FlatStoreyCount			= int.TryParse(DEPCRegisterData["FLAT_STOREY_COUNT"], out intPlaceHolder) ? intPlaceHolder : -1;
				FlatTopStorey			= DEPCRegisterData["FLAT_TOP_STOREY"].ToLower() == "y";
				FloorHeight				= float.TryParse(DEPCRegisterData["FLOOR_HEIGHT"], out floatPlaceHolder) ? floatPlaceHolder : -1;
				FloorLevel				= int.TryParse(DEPCRegisterData["FLOOR_LEVEL"], out intPlaceHolder) ? intPlaceHolder : -1;
				MultiGlazeProportion	= float.TryParse(DEPCRegisterData["MULTI_GLAZE_PROPORTION"], out floatPlaceHolder) ? floatPlaceHolder : 0;
				
				UnheatedCorridorLength	= float.TryParse(DEPCRegisterData["UNHEATED_CORRIDOR_LENGTH"], out floatPlaceHolder) ? floatPlaceHolder : 0;
				TotalFloorArea			= float.Parse(DEPCRegisterData["TOTAL_FLOOR_AREA"]);
				ExtensionCount			= int.TryParse(DEPCRegisterData["EXTENSION_COUNT"], out intPlaceHolder) ? intPlaceHolder : -1;
				// Rooms
				NumberOfHabitableRooms	= int.TryParse(DEPCRegisterData["NUMBER_HABITABLE_ROOMS"], out intPlaceHolder) ? intPlaceHolder : -1;
				NumberOfHeatedRooms		= int.TryParse(DEPCRegisterData["NUMBER_HABITABLE_ROOMS"], out intPlaceHolder) ? intPlaceHolder : -1;
				if (NumberOfHabitableRooms == -1 || NumberOfHeatedRooms == -1)
				{
					CorruptMessage = "RdSAPBuilding::MapUnsafeStandardProperties: One or more 'Number of rooms' values are missing";
					return;
				}
				if (NumberOfHabitableRooms < 3)
					NumberOfWetRooms = 1;
				else if (NumberOfHabitableRooms < 5)
					NumberOfWetRooms = 2;
				else if (NumberOfHabitableRooms < 7)
					NumberOfWetRooms = 3;
				else if (NumberOfHabitableRooms < 9)
					NumberOfWetRooms = 4;
				else if (NumberOfHabitableRooms < 11)
					NumberOfWetRooms = 5;
				else
					NumberOfWetRooms = 6;
				// Misc
				
				// Occupants 
				if (TotalFloorArea < 13.69)
					Occupants = 1;
				else
					Occupants = (float)(1  + 1.76 * (1 - Math.Exp(-0.000349 * Math.Pow(TotalFloorArea - 13.9, 2))) + 0.0013 * (TotalFloorArea - 13.69));
				// Appliances
				AnnualApplianceEnergyConsumption	= APPLIANCE_BASE_POWER * MathF.Pow(TotalFloorArea * Occupants, 0.4714f);
				// Lighting
				LowEnergyLighting					= float.TryParse(DEPCRegisterData["LOW_ENERGY_LIGHTING"], out floatPlaceHolder) ? floatPlaceHolder : 0;
				LowEnergyFixedLightCount			= float.TryParse(DEPCRegisterData["LOW_ENERGY_FIXED_LIGHT_COUNT"], out floatPlaceHolder) ? floatPlaceHolder	: 0;
				FixedLightingOutletsCount			= float.TryParse(DEPCRegisterData["FIXED_LIGHTING_OUTLETS_COUNT"], out floatPlaceHolder) ? floatPlaceHolder : 0;
				if (LowEnergyFixedLightCount > 0)
					LightingCorrectionFactor = 1 - 0.5f * (LowEnergyFixedLightCount / FixedLightingOutletsCount);
				else
					LightingCorrectionFactor = 1;
				// Hot water
				HotWaterOrdinalEnergyEfficiency					= Array.IndexOf(QUALITY_ENUMS, DEPCRegisterData["HOT_WATER_ENERGY_EFF"]);
				HotWaterOrdinalEnvironmentalEfficiency			= Array.IndexOf(QUALITY_ENUMS, DEPCRegisterData["HOT_WATER_ENV_EFF"]);
				// Ordinals
				FloorOrdinalEnergyEfficiency					= Array.IndexOf(QUALITY_ENUMS, DEPCRegisterData["FLOOR_ENERGY_EFF"]);
				FloorOrdinalEnvironmentalEfficiency				= Array.IndexOf(QUALITY_ENUMS, DEPCRegisterData["FLOOR_ENV_EFF"]);

				WindowsOrdinalEnergyEfficiency					= Array.IndexOf(QUALITY_ENUMS, DEPCRegisterData["WINDOWS_ENERGY_EFF"]);
				WindowsOrdinalEnvironmentalEfficiency			= Array.IndexOf(QUALITY_ENUMS, DEPCRegisterData["WINDOWS_ENV_EFF"]);

				WallsOrdinalEnergyEfficiency					= Array.IndexOf(QUALITY_ENUMS, DEPCRegisterData["WALLS_ENERGY_EFF"]);
				WallsOrdinalEnvironmentalEfficiency				= Array.IndexOf(QUALITY_ENUMS, DEPCRegisterData["WALLS_ENV_EFF"]);

				RoofOrdinalEnergyEfficiency						= Array.IndexOf(QUALITY_ENUMS, DEPCRegisterData["ROOF_ENERGY_EFF"]);
				RoofOrdinalEnvironmentalEfficiency				= Array.IndexOf(QUALITY_ENUMS, DEPCRegisterData["ROOF_ENV_EFF"]);

				MainheatControlOrdinalEnergyEfficiency			= Array.IndexOf(QUALITY_ENUMS, DEPCRegisterData["MAINHEATC_ENERGY_EFF"]);
				MainheatControlOrdinalEnvironmentalEfficiency	= Array.IndexOf(QUALITY_ENUMS, DEPCRegisterData["MAINHEATC_ENV_EFF"]);
				LightingOrdinalEnergyEfficiency					= Array.IndexOf(QUALITY_ENUMS, DEPCRegisterData["LIGHTING_ENERGY_EFF"]);
				LightingOrdinalEnvironmentalEfficiency			= Array.IndexOf(QUALITY_ENUMS, DEPCRegisterData["LIGHTING_ENV_EFF"]);
				
				
				

				string mainFuelDescription = DEPCRegisterData["MAIN_FUEL"].ToLower();
				if (mainFuelDescription.Contains("electricity"))
					MainFuelFactor = EmissionFactors["electricity"];
				else if (mainFuelDescription.Contains("lpg"))
					MainFuelFactor = EmissionFactors["electricity"];
				else if (mainFuelDescription.Contains("oil"))
					MainFuelFactor = EmissionFactors["electricity"];
				else if (mainFuelDescription.Contains("mains gas"))
					MainFuelFactor = EmissionFactors["electricity"];
				else if (mainFuelDescription.Contains("dual"))
					MainFuelFactor = EmissionFactors["electricity"];
				else if (mainFuelDescription.Contains("smokeless"))
					MainFuelFactor = EmissionFactors["electricity"];
				else if (mainFuelDescription.Contains("coal"))
					MainFuelFactor = EmissionFactors["electricity"];
				else if (!mainFuelDescription.Contains("no heating"))
					MainFuelFactor = EmissionFactors["electricity"];
				else if (!mainFuelDescription.Contains("community"))
					MainFuelFactor = EmissionFactors["electricity"];
				else
					CorruptMessage = $"RdSAPBuidling::MapUnsafeStandardProperties: Couldn't find emissions factor for  {DEPCRegisterData["MAIN_FUEL"]}";
			}
			catch (Exception exception)
			{
				CorruptMessage =  $"RdSAPBuilding::MapUnsafeStandardProperties: failed to cast one or more standard properties for building with reference {BuildingReferenceNumber}. Error message {exception.Message}";
				return;
			}
		}
		/// <summary>
		/// The CONSTRUCTION_AGE_BAND that denotes the construction era of existing buildings. "England and Wales: 2007 onwards".
		/// <para>Note: Some building have only define the year of construction.</para>
		/// </summary>
		public const string CONSTRUCTION_AGE_BAND_2007_ONWARDS = "England and Wales: 2007 onwards";
		/// <summary>
		/// Extend the building definition from reference data.
		/// <para>There are D-EPC properties that either don't exist on the register or
		/// aren't in a suitable format inferring unknown properties. The RdSAPReferenceDataSet contains
		/// table-friendly versions of features.  For example, main heating controls description is broken
		/// into features: has TRVs, has thermostat, has programmer etc.</para>
		/// <para>U-Values are inferred from the building regs associated with the construction age. Values
		/// aren't necessarily the real-world values and older building values are somewhat educated guesses. That said,
		/// buildings with retrofitted envelopes tend to have descriptions "Average thermal transittance 0.x(W/m²K).</para>
		/// <para>U-values</para>
		/// </summary>
		/// <param name="reference"></param>
		public void AddExtendedProperties(RdSAPReferenceDataSet reference)
		{
			/*
			 * Extended Building stuff
			 * 
			 * Features that reference reference data created for the RdSAP estimator.
			 */
			ReferenceData = reference;
			// Construction age stuff
			// Patch construction age
			int constructionAge;
			if (DEPCRegisterData["CONSTRUCTION_AGE_BAND"] == "NO DATA!" || DEPCRegisterData["CONSTRUCTION_AGE_BAND"] == "")
			{
				
				if (DEPCRegisterData["TRANSACTION_TYPE"] == "new dwelling")
				{
					DEPCRegisterData["CONSTRUCTION_AGE_BAND"] = CONSTRUCTION_AGE_BAND_2007_ONWARDS;
				}
				else
				{
					CorruptMessage = "RdSAPBuilding::AddExtendedProperties: CONSTRUCTION_AGE_BAND = NO DATA! or ''";
					return;
				}
			}
			else if (int.TryParse(DEPCRegisterData["CONSTRUCTION_AGE_BAND"], out constructionAge) && constructionAge > 2006)
			{
				DEPCRegisterData["CONSTRUCTION_AGE_BAND"] = CONSTRUCTION_AGE_BAND_2007_ONWARDS;
			}

			ConstructionAgeData = reference.ConstructionAge.FindFirst(record =>
			{
				return DEPCRegisterData["CONSTRUCTION_AGE_BAND"].Contains(record.Label);
			});
			if(ConstructionAgeData == null)
			{
				CorruptMessage = $"RdSAPBuilding::AddExtendedProperties: CONSTRUCTION_AGE_BAND =  {DEPCRegisterData["CONSTRUCTION_AGE_BAND"]}!";
				return;
			}
			ConstructionAgeBand = ConstructionAgeData.Band;
			ConstructionAgeIndex = char.ToUpperInvariant(ConstructionAgeBand[0]) - 'A';
			// Glazed type
			string glazedType = DEPCRegisterData["GLAZED_TYPE"].ToLower();
			if (glazedType.Contains("double"))
			{
				GlazingTypeData = reference.GlazingType.FindFirst(record =>
				{
					return glazedType.Contains(record.When);
				});
			}
			else if(glazedType == "" || glazedType.Contains("not defined") || glazedType.Contains("invalid") || glazedType.Contains("no data"))
			{
				CorruptMessage = "Couldn't match GLAZED_TYPE to GlazedTypeReference";
				return;
			}
			else
			{
				GlazingTypeData = reference.GlazingType.FindFirst(record =>
				{
					
					return glazedType.Contains(record.Label.ToLower());
				});
			}
			GlassGValue = GlazingTypeData.GValue;
			GlassUValue = GlazingTypeData.UValue;
			// Windows function parameters stuff
			if (!reference.WindowSizeParameter.LabelsDictionary.ContainsKey(ConstructionAgeBand))
			{
				CorruptMessage = "Couldn't find windows size parameter reference (RdSAP.WindowSizeParameterReferece)";
				return;
			}

			WindowSizeParametersData = reference.WindowSizeParameter.LabelsDictionary[ConstructionAgeBand];

			// Property Type
			string propertyTypeText = DEPCRegisterData["PROPERTY_TYPE"].ToLower();
			if (propertyTypeText == "house" || propertyTypeText == "bungalow")
				WindowArea = TotalFloorArea * WindowSizeParametersData.House + WindowSizeParametersData.HousePlus * WindowSizeFactor;
			else
				WindowArea = TotalFloorArea * WindowSizeParametersData.Flat + WindowSizeParametersData.FlatPlus * WindowSizeFactor;
			// Roof stuff
			RoofConstructionData = reference.RoofConstruction.BandsDictionary[ConstructionAgeData.Band];
			string roofDescription = DEPCRegisterData["ROOF_DESCRIPTION"].ToLower();
			if (roofDescription.Contains("pitch"))
				RoofUValue = RoofConstructionData.UValuePitched;
			else if (roofDescription.Contains("flat"))
				RoofUValue = RoofConstructionData.UValueFlat;
			else if (roofDescription.Contains("room"))
				RoofUValue = RoofConstructionData.UValueRoom;
			else if (roofDescription.Contains("other dwelling"))
				RoofUValue = 0;
			// Floor stuff
			FloorConstructionData = reference.FloorConstruction.BandsDictionary[ConstructionAgeBand];
			string floorDescription = DEPCRegisterData["FLOOR_DESCRIPTION"].ToLower();
			if (floorDescription.Contains("50 mm"))
				FloorUValue = FloorConstructionData.UValue50mm;
			else if (floorDescription.Contains("100 mm"))
				FloorUValue = FloorConstructionData.UValue100mm;
			else if (floorDescription.Contains("150 mm"))
				FloorUValue = FloorConstructionData.UValue150mm;
			else if (floorDescription.Contains("other dwelling"))
				FloorUValue = 0;
			else if (floorDescription.Contains("Average thermal transmittance "))
			{

				Match floorMatch = Regex.Match(DEPCRegisterData["FLOOR_DESCRIPTION"], @"0\.\d+");

				if (!floorMatch.Success)
				{
					CorruptMessage = $"Couldn't find floor U-Value for '{DEPCRegisterData["FLOOR_DESCRIPTION"]}'";
					return;
				}
				FloorUValue = float.Parse(floorMatch.Value);
			}
			else
			{
				FloorUValue = FloorConstructionData.UValueUnknown;
			}
			// Wall thickness stuff
			string wallsDescription = DEPCRegisterData["WALLS_DESCRIPTION"].ToLower();
			WallThicknessData = reference.WallThickness.FindFirst(record =>
			{
				return wallsDescription.Contains(record.Key.ToLower());
			});
			
			try
			{
				// TODO: Why doesn't think work? Not a big problem.
				WallThickness = WallThicknessData.BandValues[ConstructionAgeBand];
			}
			catch(Exception exception)
			{
				
				CorruptMessage = "Something building just testing";
				return;
			}
			// Walls U-Value stuff
			
			Match wallMatch = Regex.Match(DEPCRegisterData["ROOF_DESCRIPTION"], @"\d+");
			if (wallMatch.Success)
			{
				WallUValue = float.Parse(wallMatch.Value);
				Dictionary<string, float> bandsDictionary = new Dictionary<string, float>()
				{
					["A"] = WallUValue,
					["B"] = WallUValue,
					["C"] = WallUValue,
					["D"] = WallUValue,
					["E"] = WallUValue,
					["F"] = WallUValue,
					["G"] = WallUValue
				};
				WallConstructionData = new WallConstructionRecord("custom", "custom", bandsDictionary);
			}
			else
			{
				WallConstructionData = reference.WallConstruction.FindFirst(record =>
				{
					return wallsDescription.Contains(record.WallType.ToLower()) && wallsDescription.Contains(record.Insulation.ToLower());
				});
				if (WallConstructionData == null)
				{

					CorruptMessage = $"Couldn't find wall U-Value from '{DEPCRegisterData["WALLS_DESCRIPTION"]}";
					return;
				}
				WallUValue = WallConstructionData.BandValues[ConstructionAgeBand];
			}
			WindowTofloorRatio = WindowArea / TotalFloorArea;
			// Thermal brdiging factor
			ThermalBridgingData = reference.ThermalBrdige.BandsDictionary[ConstructionAgeBand];
			ThermalBridgingFactor = ThermalBridgingData.Factor;
			// Set the I've been done flag
			HasExtendedProperties = true;
		}
		public float ToRating(string targetRating)
		{
			int target = RATING_BRACKETS[targetRating].lower;
			if (DEPCEnergyEfficiency < target)
			{
				return (target - DEPCEnergyEfficiency);
			}
			return 0;
		}
	}
}
