using MeesSDK.RdSAP.Reference;
using MeesSDK.RsSAP;
using MeesSDK.ML;


namespace MeesSDK.Examples.RdSAP
{
	public class RdSAPEstimation : IMeesSDKExample
	{
		/// <summary>
		/// The path to the input certificates
		/// </summary>
		public string CertificatesPath { get; protected set; }
		public RdSAPEstimation(string path) { CertificatesPath  = path; }
		/// <summary>
		/// The estimator RunTheExample creates.
		/// </summary>
		public LightGBMEstimator<RdSAPBuilding> Estimator { get; protected set; }
		/// <summary>
		/// Using the DEPC register and RdSAPBuilding extended features to create RdSAP estimators
		/// </summary>
		/// <param name="proejct"></param>
		/// <param name="sbem"></param>
		public void RunTheExample()
		{
			/*
			 * Prepare the reference data used to extend the register's features. 
			 * 
			 * Reference data was created from Part L, NCM(SBEM) Database, and 
			 * educated guesses.
			 */
			string DEPC_REFERENCE_PATH = "c://workspaces/depc_emulator/data/";
			RdSAPReferenceDataSet reference = RdSAPReferenceDataSet.Build(
				DEPC_REFERENCE_PATH + "age_band_lookup.csv",		// Primary key
				DEPC_REFERENCE_PATH + "floor_constructions.csv",    // Floor U-Value
				DEPC_REFERENCE_PATH + "glazing_types.csv",          // Windows U-Value
				DEPC_REFERENCE_PATH + "heating_controls.csv",       // Heating controls
				DEPC_REFERENCE_PATH + "roof_constructions.csv",     // Roof U-Value
				DEPC_REFERENCE_PATH + "wall_constructions.csv",     // Wall U-Value
				DEPC_REFERENCE_PATH + "wall_thickness.csv",         // Wall thicknes 
				DEPC_REFERENCE_PATH + "window_parameters.csv"       // SAP window size built-form parameters
			);
			/*
			 * Get the data
			 * 
			 * Download dataset from the OpenDataCommunities D-EPC register and set
			 * the DEPC_REGISTER_DATA variable to the csv's path
			 */
			string DEPC_REGISTER_DATA = "C:\\workspaces\\__shared_data__\\depc\\domestic-E06000002-Middlesbrough\\certificates.csv";
			// Load the D-EPC register certificates.csv. The constructor with reference tells it to add some properties like U-Values
			RdSAPBuildingSet buildings = RdSAPBuildingSet.LoadDataSet(DEPC_REGISTER_DATA, reference);
			// Remove corrupt and incomplete records
			buildings.FilterCorrupt();
			/*
			 * Create a LightGBM estimator	
			 */
			// Define the name of the RdSAPBuilding property that we're trying to estimate. 
			string RDSAP_ESTIMATOR_TARGET_FEATURE = "DEPCEnergyEfficiency";
			//  Define the list of RdSAPBuilding properties that'll be used for training.
			string[] RDSP_ESTIMATOR_FEATURES = new string[]
			{
				"ConstructionAgeIndexFloatForLightGBM",								// A to L as 0 to 10
				"DEPCEnergyEfficiencyPotential",									// Potential energy efficiency
				"ExtensionCountFloatForLightGBM",									// Number of extensions
				"NumberOfWetRoomsFloatForLightGBM",									// Number of wet rooms
				"NumberOfOpenFireplacesFloatForLightGBM",							// Number of open fireplaces
				"HotWaterOrdinalEnergyEfficiencyFloatForLightGBM",					// Hot water system efficiency 0 - 5
				"FloorOrdinalEnergyEfficiencyFloatForLightGBM",						// Floor construction efficiency 0 - 5
				"WindowsOrdinalEnergyEfficiencyFloatForLightGBM",					// Windows efficiency 0 - 5
				"WallsOrdinalEnergyEfficiencyFloatForLightGBM",						// External walls efficiency 0 - 5
				"HeatingOrdinalEnergyEfficiencyFloatForLightGBM",					// Main heating system efficiency 0 - 5
				"HeatingControlOrdinalEnergyEfficiencyFloatForLightGBM",			// Heating controls efficiency 0 - 5
				"RoofOrdinalEnergyEfficiencyFloatForLightGBM",						// Roof efficiency 0 - 5
				"LightingOrdinalEnergyEfficiencyFloatForLightGBM",					// Fixed lighting efficiency 0 - 5
				"LowEnergyLighting",												// Percent lighting is low energy
				"HeatingCostPotential",												// Heating cost potential determined by RdSAP
				"GlassUValue", "FloorUValue",  "RoofUValue",   "WallUValue",		// U-Values W/m²K
				"TotalFloorArea",													// Net internal area m²
				"WindowArea",														// Window area (Calculated by SAP, not from assessor	
				"SolarPanelArea",													// Solar panel area m²
				"MainFuelFactor"													// Main heating fuel kgCO2/m²
			};
			// Build a LightGBMEstimator-friendly dataset from the D-EPC certificates
			LightGBMInputData<RdSAPBuilding> mlData = LightGBMInputData<RdSAPBuilding>.FromList(RDSP_ESTIMATOR_FEATURES, buildings);
			// You can remove corrupt results from this data, too. Anything IValidatable (Thing that has a public HasError getter
			mlData.RemoveCorruptObjects();
			// Create the estimator
			Estimator					= new LightGBMEstimator<RdSAPBuilding>(mlData, RDSAP_ESTIMATOR_TARGET_FEATURE);
			// Configure the learner
			Estimator.LearningRate			= 0.075f;
			Estimator.NumberOfIterations    = 100;
			// Train it
			Estimator.Train();
			// Print the results: A trained estimator's PrintSummary will include the Test Buildings RMSE
			Estimator.PrintSummary();
			/*
			 * The features, data, and process can be applied to anything else that can be estimated.
			 * 
			 * NOTE: There's a large gap between capable and useful, but you can add features
			 *		 tailored to the project at hand.
			 */
			// kgCO2/m²
			LightGBMEstimator<RdSAPBuilding> co2Estimator    = new LightGBMEstimator<RdSAPBuilding>(mlData, "CO2_PER_FLOOR_AREA");
			co2Estimator.Train();
			co2Estimator.PrintSummary();
			// Potential energy efficiency
			LightGBMEstimator<RdSAPBuilding> potentialEstimator = new LightGBMEstimator<RdSAPBuilding>(mlData, RdSAPBuilding.POTENTIAL_ENERGY_EFFICIENCY_LABEL);
			co2Estimator.Train();
			co2Estimator.PrintSummary();
		}

		//============ Admin ============//
		public string GetDescription()
		{
			return @"""Estimating D-EPC ratings using Gradient-boosting decision trees

We can make a reasonable guess at a of a D-EPC rating from the associated certificate's D-EPC register entry 
and inference through RdSAPBuilding. 

This example demonstrates how to create a simple LightGBMEstimator D-EPC rating prediction.

	To download your own example set visit: https://epc.opendatacommunities.org/domestic/search and use
	any of the files named 'certificates.csv' E.g. ./domestic-E06000002-Middlesbrough/certificates.csv

Challenge: The related study's accuracy on the Stockton-on-Tees dataset was 2.37 on the test data, 3.1 on the holdout.
				""";
		}
	}
}
