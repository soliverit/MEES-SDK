using MeesSDK.Sbem;
using MeesSDK.Sbem.Retrofitting.Measures;
using MeesSDK.Examples.Sbem;


namespace MeesSDK.Examples.Sbem
{
	public class WorkingWithSBEM : IMeesSDKExample
	{

		public void RunTheExample()
		{
			/*
			 * File locations: Example values are valid for development computer.
			 */
			// The path to the SBEM.exe executable. This library's tailored to 6.1.e, but 
			// should work with 4.1.e onwards.
			string SBEM_DIRECTORY			= "c:\\ncm\\6.1.e\\";
			// The directory where SBEM projects will be processed. SBEM outputare are written here.
			string SBEM_TARGET_DIRECTORY	= SBEM_DIRECTORY + "project\\";
			// The .inp file that's being used for the example. 
			string TEST_INP_MODEL_PATH		= "C:\\workspaces\\__shared_data__\\sbem_model_sandbox\\model.inp";
			// The .sim output path.
			string TEST_SIM_OUTPUT_PATH		= SBEM_TARGET_DIRECTORY + "model.sim";
			/*
			 * Loading SbemModel and running SBEM.
			 */
			// Load SBEM inp model
			SbemModel model					= SbemModel.ParseInpFile(TEST_INP_MODEL_PATH);
			// Create an SBEM Service
			SbemService sbem				= new SbemService(SBEM_DIRECTORY, SBEM_TARGET_DIRECTORY);
			// Turn of SBEM console logs
			sbem.SilentMode					= true;		
			// Enable async mode. Let's you do other things while SbemService works through requests
			sbem.StartSbemService();
			// Process the SbemModel with SBEM
			sbem.RunSBEM(model);
			// Wait for the SBEM Service (if in async mode) Timeout in 20 seconds.
			sbem.WaitForService(20000);
			// Stop the SBEM Service. Return to serial processing.
			sbem.StopService();
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
			SimResult referenceSimResults	= project.ReferenceSimResult;	// The Reference model's .sim results
			SbemErrorFile actualErrors		= project.AsBuiltSbemErrors;	// The .err report from SBEM. As-built model errors and warnings
			SbemErrorFile notionalErrors	= project.NotionalSbemErrors;	// The .err report from SBEM. Notional model errors and warnings
			SbemErrorFile referenceErrors	= project.ReferenceSbemErrors;	// The .err report from SBEM. Reference model errors and warnings
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
		}
		public string GetDescription()
		{
			return @"Working with SBEM data.

This example shows the basics of working with SBEM.";
		}
	}
}
