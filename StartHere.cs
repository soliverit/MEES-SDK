using CsvHelper;
using GeneticSharp;
using MathNet.Numerics.Optimization;
using MeesSDK.Chromosome;
using MeesSDK.DataManagement;
using MeesSDK.ML;
using MeesSDK.RdSAP;
using MeesSDK.RdSAP.Reference;
using MeesSDK.RsSAP;
using MeesSDK.Sbem;
using MeesSDK.Sbem.Retrofitting.Measures;
using System.Diagnostics;
/*
 *  Math.NET  
 */
MathNet.Numerics.Control.UseNativeMKL();

// Define locations
string SBEM_DIRECTORY = "c:\\ncm\\6.1.e\\";
string SBEM_TARGET_DIRECTORY = SBEM_DIRECTORY + "project\\";
string TEST_INP_MODEL_PATH = "C:\\workspaces\\__shared_data__\\sbem_model_sandbox\\model.inp";
string TEST_INP_MODEL_EPC_PATH = "C:\\workspaces\\__shared_data__\\sbem_model_sandbox\\model_epc.inp";
string TEST_SIM_OUTPUT_PATH = SBEM_TARGET_DIRECTORY + "model.sim";


/*====================================================================================
 * SBEM - Nondoemstic analysis 
==================================================================================== */
// Load SBEM inp model
SbemModel model					= SbemModel.ParseInpFile(TEST_INP_MODEL_PATH);
// Create an SBEM Service
SbemService sbem				= new SbemService(SBEM_DIRECTORY, SBEM_TARGET_DIRECTORY);
// Enable async mode. Let's you do other things while SbemService works through requests
sbem.StartSbemService();
// Process the SbemModel with SBEM
sbem.RunSBEM(model);
// Load the SBEM .sim results data
SimResult simResult				= SimResult.ParseSimFile(TEST_SIM_OUTPUT_PATH);
// Pair .sim results with the SBEM model, HVACs, and Zones.
model.PairWithSimResult(simResult);
// Load the entire SBEM input/output
SbemProject project				= SbemProject.BuildFromDirectory(SBEM_TARGET_DIRECTORY);
SbemModel actualModel			= project.AsBuiltSbemModel;		// The as-built .inp model
SbemEpcModel epcModel			= project.AsBuiltSbemEpcModel;	// The _epc.inp model containing the results summary
SbemModel notionalModel			= project.NotionalSbemModel;	// The reference model for new-build EPCs
SbemModel referenceModel		= project.ReferenceSbemModel;	// The reference model for existing building EPCs
SimResult actualSimResults		= project.AsBuiltSimResult;		// The full .sim results for the as-built building
SimResult notionalSimResults	= project.NotionalSimResult;	// The Notional model's .sim results
SimResult referenceSimResults	= project.ReferenceSimResult;	 // The Reference model's .sim results
SbemErrorFile actualErrors		= project.AsBuiltSbemErrors;	// The .err report from SBEM. As-built model errors and warnings
SbemErrorFile notionalErrors	= project.NotionalSbemErrors;	// The .err report from SBEM. Notional model errors and warnings
SbemErrorFile referenceErrors	= project.ReferenceSbemErrors;	// The .err report from SBEM. Reference model errors and warnings
// Print the _epc.inp EPC block.
// "SBEM" = EPC
//   TYPE                  = England
//   SER                   = 796.195
//   TYR                   = 1179.9
//   TER                   = 294.24
//   EPC-LANGUAGE          = ENGLISH
//   BER                   = 436.121
//   NOS-LEVEL             = Level 3
//   TRANSACTION-TYPE      = Mandatory issue (Marketed sale)
//   MAIN-FUEL-TYPE        = Grid Supplied Electricity
//   BUILDING-ENVIRONMENT  = Heating and Mechanical Ventilation
//   ..
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
/*====================================================================================
 * Create your own retrofit measures
==================================================================================== */
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
SbemProject retrofitProject = sbem.BuildProject(lightingRetrofit.Model);
/*
 * Working with the Results
 */
// Build the difference model - A model whose calendars are the difference of the as-built and retrofitte SbemModel
SbemModel differenceModel = model.GetDifferenceModel(retrofitProject.AsBuiltSbemModel);
//Print the as-built End Use calendar
project.AsBuiltSbemModel.EndUseConsumerCalendar.Print();
//Print the retrofitted End Use calendar
retrofitProject.AsBuiltSbemModel.EndUseConsumerCalendar.Print();
//Print the difference End Use calendar
differenceModel.EndUseConsumerCalendar.Print();











/*
 *  RdSAP
 */
// Build reference 
//string DEPC_REFERENCE_PATH = "c://workspaces/depc_emulator/data/";
//RdSAPReferenceDataSet reference = RdSAPReferenceDataSet.Build(
//	DEPC_REFERENCE_PATH + "age_band_lookup.csv",
//	DEPC_REFERENCE_PATH + "door_constructions.csv",
//	DEPC_REFERENCE_PATH + "floor_constructions.csv",
//	DEPC_REFERENCE_PATH + "glazing_types.csv",
//	DEPC_REFERENCE_PATH + "heating_controls.csv",
//	DEPC_REFERENCE_PATH + "living_area_factors.csv",
//	DEPC_REFERENCE_PATH + "roof_constructions.csv",
//	DEPC_REFERENCE_PATH + "thermal_bridging.csv",
//	DEPC_REFERENCE_PATH + "wall_constructions.csv",
//	DEPC_REFERENCE_PATH + "wall_thickness.csv",
//	DEPC_REFERENCE_PATH + "window_parameters.csv"
//);
//// Reference Data loading
//string JUST_TEN_DEPC_CERTIFICATES_PATH = "c:/workspaces/__sandbox__/small_depc_certificates.csv";
//string MIDDLESBROUGH_DEPC_PATH = "C:\\workspaces\\__sandbox__\\moo_sharp_sandbox\\MOOSandbox\\examples\\1000_depc_certificates.csv";
///*
// * Let's build a LightGBM estimator for D-EPC
// */
//// First, let's define the features list. The properties the estimator is trained on.
//string RDSAP_ESTIMATOR_TARGET_FEATURE = "DEPCEnergyEfficiency";
//string[] RDSP_ESTIMATOR_FEATURES = new string[]
//{
//	"ConstructionAgeIndexFloatForLightGBM",
//	"DEPCEnergyEfficiencyPotential",
//	"ExtensionCountFloatForLightGBM",
//	"NumberOfWetRoomsFloatForLightGBM",
//	"NumberOfOpenFireplacesFloatForLightGBM",
//	"HotWaterOrdinalEnergyEfficiencyFloatForLightGBM",
//	"FloorOrdinalEnergyEfficiencyFloatForLightGBM",
//	"WindowsOrdinalEnergyEfficiencyFloatForLightGBM",
//	"WallsOrdinalEnergyEfficiencyFloatForLightGBM",
//	"HeatingOrdinalEnergyEfficiencyFloatForLightGBM",
//	"HeatingControlOrdinalEnergyEfficiencyFloatForLightGBM",
//	"SecondaryHeatingOrdinalEnergyEfficiencyFloatForLightGBM",
//	"RoofOrdinalEnergyEfficiencyFloatForLightGBM",
//	"MainheatOrdinalEnergyEfficiencyFloatForLightGBM",
//	"MainheatControlOrdinalEnergyEfficiencyFloatForLightGBM",
//	"LightingOrdinalEnergyEfficiencyFloatForLightGBM",
//	"WindTurbineCountFloatForLightGBM",
//	"LowEnergyLighting", "LightingPowerW",
//	"HeatingCostPotential", 
//	"FloorUValue",  "RoofUValue",   "WallUValue",   
//	"TotalFloorArea",
//	"WindowArea",   "GlassUValue", "GlassGValue",
//	"SolarPanelArea",
//	//"enumeratedRoofDescriptions)",	TODO: Strings probably work fine with LightGBM
//	"MainFuelFactor"
//};
//// Load data
//RdSAPBuildingSet buildings = RdSAPBuildingSet.LoadDataSet(MIDDLESBROUGH_DEPC_PATH, reference);
//// Create an ML dataset and load records
//LightGBMInputData<RdSAPBuilding> mlData = new LightGBMInputData<RdSAPBuilding>(RDSP_ESTIMATOR_FEATURES);
//for (int buildingID = 0; buildingID < buildings.Length; buildingID++)
//	mlData.AddObject(buildings[buildingID]);

//Console.Write(">O length: " + mlData.Count);
//mlData.RemoveCorruptObjects();
//Console.Write(">R length: " + mlData.Count);
//LightGBMEstimator<RdSAPBuilding> estimator = new LightGBMEstimator<RdSAPBuilding>(mlData, RDSAP_ESTIMATOR_TARGET_FEATURE);
//estimator.Train();
//estimator.PrintSummary();

//PredictionSet predictions = estimator.Predict(mlData.GetFirstRecords(10));









//return;
///-----
//string EXAMPLE_CSV_PATH = "c:/workspaces/__sandbox__/moo_sharp_sandbox/MOOSandbox/examples/stockton/data.csv";


//// Load data
//CsvHandler csvHandler		= CsvHandler.ParseCSV(EXAMPLE_CSV_PATH);
//csvHandler.PrintErrors();

//MathNetRetrofitsTable data = new MathNetRetrofitsTable(csvHandler, RetrofitOption.ALL_RETROFIT_OPTION_KEYS.ToArray() );
//Console.WriteLine(data.SumCosts(new int[] { 1, 2 }, new int[] { 1, 2 }));

//// Define the chromosome: four genes representing x1, y1, x2, y2
//var chromosome = new MixedIntegerChromosome(
//	new int[N_VARIABLES],       // Lower bounds
//	Enumerable.Repeat<int>(15, N_VARIABLES).ToArray()  // All values = 42// Upper bounds
//);

//int[] ids = Enumerable.Range(0, N_VARIABLES).ToArray();
//// Define the fitness function: maximize Euclidean distance
//var fitness = new FuncFitness(c =>
//{
//	MixedIntegerChromosome fc	= (MixedIntegerChromosome)c;
//	int[] values				= fc.GetValues();

//	//for (int i = 0; i < N_VARIABLES; i++)
//	//	for (int j = 0; j < N_VARIABLES; j++)
//	//		score += 1; // TODO: This is where the data
//	//return data.Sum()
//	return data.Score(ids, values);
//});

// Create the population
//var population = new Population(100, 100, chromosome);

//// Configure the genetic algorithm
//var ga = new GeneticAlgorithm(
//	population,
//	fitness,
//	new EliteSelection(),
//	new UniformCrossover(),
//	new UniformMutation(true)
//);

//ga.Termination = new FitnessStagnationTermination(500);

//// Subscribe to the GenerationRan event to output progress
//ga.GenerationRan += (sender, e) =>
//{
//	var bestChromosome = ga.BestChromosome as MixedIntegerChromosome;
//	var bestValues = bestChromosome.GetValues();
//	var bestFitness = bestChromosome.Fitness.Value * -1;
//	//Console.WriteLine($"Generation {ga.GenerationsNumber}: Best Fitness = {bestFitness:F2}");
//	//Console.WriteLine($"Points: ({bestValues[0]:F2}, {bestValues[1]:F2}) to ({bestValues[2]:F2}, {bestValues[3]:F2})");
//};

//Stopwatch watch	= Stopwatch.StartNew();
//// Start the genetic algorithm
//ga.Start();
//watch.Stop();
//Console.WriteLine($"{watch.Elapsed.TotalSeconds} ms");
