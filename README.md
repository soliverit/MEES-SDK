# MEES-SDK
A software development kit for developing Part L analysis tools

## Highlights
- SBEM modelling
- Retrofit analysis
- Estate optimisation tools (GeneticSharpBase, RdSAPEstimator
- Partitioned and threaded subset optimisation
- Normalised DEPC-Register database with extensions
- D-EPC register analysis (RdSAPBuilding, RdSAPBuildingSet. RdSAP.ORM
- D-EPC register extensions. Thermal properties, geometries,
- Natural language processing for RdSAP.ORM

## Getting started

1. Install Visual Studio 2022.
2. During installation or via the add/remove program entry for Visual Studio, select .NET desktop development
3. Open MeesSDK.sln. Open Nuget manager (package manager) and install the depndencies. Currently, GeneticSharp, ML.NET,
4. MathNetCore, CsvHelper (being deprecated), and LightGBM for ML.NET.
5. Build and run. This should step through the examples in StartHere.cs in the root directory

### SBEM (working with)
```csharp
/*
 * File locations: Example values are valid for development computer.
 */
// The path to the SBEM.exe executable. This library's tailored to 6.1.e, but 
// should work with 4.1.e onwards.
string SBEM_DIRECTORY              = "c:\\ncm\\6.1.e\\";
// The directory where SBEM projects will be processed. SBEM outputare are written here.
string SBEM_TARGET_DIRECTORY       = SBEM_DIRECTORY + "project\\";
// The .inp file that's being used for the example. 
string TEST_INP_MODEL_PATH         = "C:\\workspaces\\__shared_data__\\sbem_model_sandbox\\model.inp";
// The .sim output path.
string TEST_SIM_OUTPUT_PATH        = SBEM_TARGET_DIRECTORY + "model.sim";

/*
 * Loading SbemModel and running SBEM.
 */
// Load SBEM inp model
SbemModel model                    = SbemModel.ParseInpFile(TEST_INP_MODEL_PATH);
// Create an SBEM Service
SbemService sbem                   = new SbemService(SBEM_DIRECTORY, SBEM_TARGET_DIRECTORY);
// Turn off SBEM console logs
sbem.SilentMode                    = true;		
// Enable async mode. Lets you do other things while SbemService works through requests
sbem.StartSbemService();
// Process the SbemModel with SBEM
sbem.RunSBEM(model);
// Wait for the SBEM Service (if in async mode) Timeout in 20 seconds.
sbem.WaitForService(20000);
// Stop the SBEM Service. Return to serial processing.
sbem.StopService();
// Load the SBEM .sim results data
SimResult simResult                = SimResult.ParseSimFile(TEST_SIM_OUTPUT_PATH);
// Pair .sim results with the SBEM model, HVACs, and Zones.
model.PairWithSimResult(simResult);

// Load the entire SBEM input/output
SbemProject project                = SbemProject.BuildFromDirectory(SBEM_TARGET_DIRECTORY);
SbemModel actualModel              = project.AsBuiltSbemModel;          // The as-built .inp model
SbemEpcModel epcModel              = project.AsBuiltSbemEpcModel;       // The _epc.inp model containing the results summary
SbemModel notionalModel            = project.NotionalSbemModel;         // The reference model for new-build EPCs
SbemModel referenceModel           = project.ReferenceSbemModel;        // The reference model for existing building EPCs
SimResult actualSimResults         = project.AsBuiltSimResult;          // The full .sim results for the as-built building
SimResult notionalSimResults       = project.NotionalSimResult;         // The Notional model's .sim results
SimResult referenceSimResults      = project.ReferenceSimResult;        // The Reference model's .sim results
SbemErrorFile actualErrors         = project.AsBuiltSbemErrors;         // The .err report from SBEM. As-built model errors and warnings
SbemErrorFile notionalErrors       = project.NotionalSbemErrors;        // The .err report from SBEM. Notional model errors and warnings
SbemErrorFile referenceErrors      = project.ReferenceSbemErrors;       // The .err report from SBEM. Reference model errors and warnings

// Print the _epc.inp EPC block.
// "SBEM" = EPC
//   SER                   = 796.195
//   TER                   = 294.24
//   BER                   = 436.121
//   TRANSACTION-TYPE      = Mandatory issue (Marketed sale)
//   MAIN-FUEL-TYPE        = Grid Supplied Electricity
//   BUILDING-ENVIRONMENT  = Heating and Mechanical Ventilation
Console.WriteLine(project.AsBuiltSbemEpcModel.Epc.ToString());

// Print the Project End Use consumer calendar to the console
project.AsBuiltSbemModel.EndUseConsumerCalendar.Print();
// Print an example HVAC Fuel Type consumption calendar
project.AsBuiltSbemModel.HvacSystems[0].FuelUseConsumerCalendar.Print();
// Print an example Zone internal heat gains calendar
project.AsBuiltSbemModel.HvacSystems[0].Zones[0].InternalHeatGainsCalendar.Print();
// Print an example Zone heating demand calendar
project.AsBuiltSbemModel.HvacSystems[0].Zones[0].HeatingEnergyDemandCalendar.Print();
// Print an example Zone cooling demand calendar
project.AsBuiltSbemModel.HvacSystems[0].Zones[0].CoolingEnergyDemandCalendar.Print();

// Disaggregate DHW and Fuel Type consumption to zone-level
SbemProject.CalculateZonalEnergyDemand(project.AsBuiltSbemModel, sbem);
// Print an example of zone-level End Use disaggregation, including DHW
project.AsBuiltSbemModel.HvacSystems[0].Zones[0].EndUseConsumerCalendar.Print();
// Print an example of zone-level Fuel Type consumption calendar
project.AsBuiltSbemModel.HvacSystems[0].Zones[0].FuelUseConsumerCalendar.Print();

/*
 *  Creating and running Retrofits 
 */
// Clone the SbemModel and create a new NCM Lighting 5 (T8 lamp replacement) Retrofit
NCMLighting5Example lightingRetrofit = new NCMLighting5Example(model.Clone());
// Apply the Retrofit to the SbemModel
lightingRetrofit.Apply();

/*
 *  Applying the retrofit and retrieving the results.
 */
// Run the new model through SBEM and get the SbemProject
SbemProject retrofitProject        = sbem.BuildProject(lightingRetrofit.Model);

/*
 * Working with the Results
 */
// Build the difference model - A model whose calendars are the difference of the as-built and retrofit SbemModel
SbemModel differenceModel          = model.GetDifferenceModel(retrofitProject.AsBuiltSbemModel);

// Print the as-built End Use calendar
project.AsBuiltSbemModel.EndUseConsumerCalendar.Print();
// Print the retrofitted End Use calendar
retrofitProject.AsBuiltSbemModel.EndUseConsumerCalendar.Print();
// Print the difference End Use calendar
differenceModel.EndUseConsumerCalendar.Print();
```
### SBEM (build your own retrfit example)
```c#
public class NCMLighting5Example : RetrofitBase<SbemZone>
{
	public NCMLighting5Example(SbemModel sbemModel) : base(sbemModel) { }
	/// <summary>
	/// Apply the retrofit, track the modified objects.
	/// </summary>
	public override void Apply()
	{
		/*
		 * There's nothing complicated. Iterate over the zones and
		 * update the ones that have T8s.
		 * 
		 * Important: make sure to AddModifiedObject(modifiedZone) for costs
		 * and schedules later
		 */
		// Check all Zones for T8s
		for (int zoneID = 0; zoneID < Model.Zones.Length; zoneID++)
		{
			SbemZone zone = Model.Zones[zoneID];
			// Skip zones with lighting not defined by template
			if (zone.GetStringProperty("LIGHT-CASE").Value != "UNKNOWN")
				continue;
			// Replace T8 lamps
			if (zone.GetStringProperty("LIGHT-TYPE").Value.StartsWith("T8"))
			{
				// Tell SBEM we're using efficacy then set the efficacy
				zone.SetNumericProperty("LAMP-BALLAST-EFF", 60);
				zone.SetStringProperty("LIGHT-CASE", "CHOSEN");
				AddModifiedObject(zone);
			}
		}
	}
}
```
### SBEM zone-level disaggregation of End Use consumption calendars, including DHW.
```c#
public class ZonalConsumerDisaggregation : SbemExampleBase
{
	public ZonalConsumerDisaggregation(SbemProject project, SbemService service) : base(project, service) { }
	public override void RunTheExample()
	{
		/*
		 * It's just two lines. What's happening under the hood?
		 * 
		 * For every SbemZone in the input SbemModel, create a Single-HVAC, Single-Zone SbemModel
		 * and get the results using the SbemService. After all SbemRequest have been processed by
		 * the SbemService, attach Single-HVAC, Single-Zone SbemModel amd attach its Project End 
		 * Use and Fuel consmption calendars to the associated Zone in the input SbemModel
		 */
		// Disaggregate Hot Water to HVAC-level
		SbemProject.DisaggregateHVACHotWater(Project.AsBuiltSbemModel, SbemHandler);
		// Disaggregate End Use and Fuel consumption to Zone-level
		SbemProject.CalculateZonalEnergyDemand(Project.AsBuiltSbemModel, SbemHandler);
	}
```

### Estate optimisation (residential example)
```csharp
public class EstateRetrofitOptimisation : IMeesSDKExample
{
	public EstateRetrofitOptimisation(string inputDataPath) { InputDataPath = inputDataPath; }
	/// <summary>
	/// The path to the certificates.csv retrofits. This csv has three columns per retrofit label
	/// of all combinations of four heating, glazing, wall, and roof retrofit measures.
	/// </summary>
	public string InputDataPath { get; protected set; }
	/// <summary>
	/// Generating an optimal residential estate retrofit strategy using a genetic algorithm
	/// </summary>
	public void RunTheExample()
	{

		// Load data
		CsvHandler csvHandler = CsvHandler.ParseCSV(InputDataPath);
		// Print any errors
		csvHandler.PrintErrors();
		// Build the data set.	
		MathNetRetrofitsTable data		= new MathNetRetrofitsTable(csvHandler, RdSAPRetrofitOption.ALL_RETROFIT_OPTION_KEYS.ToArray());
		// Create a genetic algorithm
		RdSAPEstateOptimiser optimiser	= new RdSAPEstateOptimiser(data, Enumerable.Range(0, data.Length).ToArray());
		// Set the number of agents in the initial population
		optimiser.InitialPopulationSize = 50;
		// Set the maximum number of agents in generation 2 onwards
		optimiser.MaximumPopulationSize = 100;
		// The probability that the secondary parent's gene(s) will be replaced by the second's
		optimiser.CrossoverProbability	= 0.75f;
		// The probability that genes will mutate after the crossover round.
		optimiser.MutationProbability	= 0.02f;
		// Maximum number of generations (Called Termination in GeneticSharp)
		optimiser.Generations           = 500;
		// Run it.
		optimiser.Run();
	}
}
```
### Massive estate optimisation with JAE-MOO - just-add-estimates multi-objective optimisation (Residential example)
### RdSAP (estimator example)
```csharp
/*
 * Prepare the reference data used to extend the register's features. 
 * 
 * Reference data was created from Part L, NCM(SBEM) Database, and 
 * educated guesses.
 */
string DEPC_REFERENCE_PATH       = "c://workspaces/depc_emulator/data/";
RdSAPReferenceDataSet reference  = RdSAPReferenceDataSet.Build(
    DEPC_REFERENCE_PATH + "age_band_lookup.csv",         // Primary key
    DEPC_REFERENCE_PATH + "floor_constructions.csv",     // Floor U-Value
    DEPC_REFERENCE_PATH + "glazing_types.csv",           // Windows U-Value
    DEPC_REFERENCE_PATH + "heating_controls.csv",        // Heating controls
    DEPC_REFERENCE_PATH + "roof_constructions.csv",      // Roof U-Value
    DEPC_REFERENCE_PATH + "wall_constructions.csv",      // Wall U-Value
    DEPC_REFERENCE_PATH + "wall_thickness.csv",          // Wall thickness 
    DEPC_REFERENCE_PATH + "window_parameters.csv"        // SAP window size built-form parameters
);

/*
 * Get the data
 * 
 * Download dataset from the OpenDataCommunities D-EPC register and set
 * the DEPC_REGISTER_DATA variable to the csv's path
 */
string DEPC_REGISTER_DATA        = "C:\\workspaces\\__shared_data__\\depc\\domestic-E06000002-Middlesbrough\\certificates.csv";
// Load the D-EPC register certificates.csv. The constructor with reference tells it to add some properties like U-Values
RdSAPBuildingSet buildings       = RdSAPBuildingSet.LoadDataSet(DEPC_REGISTER_DATA, reference);
// Remove corrupt and incomplete records
buildings.FilterCorrupt();

/*
 * Create a LightGBM estimator	
 */
// Define the name of the RdSAPBuilding property that we're trying to estimate. 
string RDSAP_ESTIMATOR_TARGET_FEATURE = "DEPCEnergyEfficiency";

// Define the list of RdSAPBuilding properties that'll be used for training.
string[] RDSP_ESTIMATOR_FEATURES = new string[]
{
    "ConstructionAgeIndexFloatForLightGBM",                         // A to L as 0 to 10
    "DEPCEnergyEfficiencyPotential",                                // Potential energy efficiency
    "ExtensionCountFloatForLightGBM",                               // Number of extensions
    "NumberOfWetRoomsFloatForLightGBM",                             // Number of wet rooms
    "NumberOfOpenFireplacesFloatForLightGBM",                       // Number of open fireplaces
    "HotWaterOrdinalEnergyEfficiencyFloatForLightGBM",              // Hot water system efficiency 0 - 5
    "FloorOrdinalEnergyEfficiencyFloatForLightGBM",                 // Floor construction efficiency 0 - 5
    "WindowsOrdinalEnergyEfficiencyFloatForLightGBM",               // Windows efficiency 0 - 5
    "WallsOrdinalEnergyEfficiencyFloatForLightGBM",                 // External walls efficiency 0 - 5
    "HeatingOrdinalEnergyEfficiencyFloatForLightGBM",               // Main heating system efficiency 0 - 5
    "HeatingControlOrdinalEnergyEfficiencyFloatForLightGBM",        // Heating controls efficiency 0 - 5
    "RoofOrdinalEnergyEfficiencyFloatForLightGBM",                  // Roof efficiency 0 - 5
    "LightingOrdinalEnergyEfficiencyFloatForLightGBM",              // Fixed lighting efficiency 0 - 5
    "LowEnergyLighting",                                            // Percent lighting is low energy
    "HeatingCostPotential",                                         // Heating cost potential determined by RdSAP
    "GlassUValue", "FloorUValue",  "RoofUValue", "WallUValue",      // U-Values W/m²K
    "TotalFloorArea",                                               // Net internal area m²
    "WindowArea",                                                   // Window area (Calculated by SAP, not from assessor)
    "SolarPanelArea",                                               // Solar panel area m²
    "MainFuelFactor"                                                // Main heating fuel kgCO2/m²
};

// Build a LightGBMEstimator-friendly dataset from the D-EPC certificates
LightGBMInputData<RdSAPBuilding> mlData = LightGBMInputData<RdSAPBuilding>.FromList(RDSP_ESTIMATOR_FEATURES, buildings);
// You can remove corrupt results from this data, too. Anything IValidatable (Thing that has a public HasError getter)
mlData.RemoveCorruptObjects();

// Create the estimator
Estimator                        = new LightGBMEstimator<RdSAPBuilding>(mlData, RDSAP_ESTIMATOR_TARGET_FEATURE);
// Configure the learner
Estimator.LearningRate           = 0.075f;
Estimator.NumberOfIterations     = 100;
// Train it
Estimator.Train();
// Print the results: A trained estimator's PrintSummary will include the Test Buildings RMSE
Estimator.PrintSummary();

/*
 * The features, data, and process can be applied to anything else that can be estimated.
 * 
 * NOTE: There's a large gap between capable and useful, but you can add features
 *       tailored to the project at hand.
 */
// kgCO2/m²
LightGBMEstimator<RdSAPBuilding> co2Estimator       = new LightGBMEstimator<RdSAPBuilding>(mlData, "CO2_PER_FLOOR_AREA");
co2Estimator.Train();
co2Estimator.PrintSummary();

// Potential energy efficiency
LightGBMEstimator<RdSAPBuilding> potentialEstimator = new LightGBMEstimator<RdSAPBuilding>(mlData, RdSAPBuilding.POTENTIAL_ENERGY_EFFICIENCY_LABEL);
potentialEstimator.Train();
potentialEstimator.PrintSummary();
```


