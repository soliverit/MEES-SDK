using GeneticSharp;
using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// The complete SBEM input / output model. Everything that goes into and comes out of
	/// SBEM is modelled in here.
	/// <code>- SbemModel[] (as-built, notional, and reference)
	/// - SbemEpcModel[] (as-built)
	/// - SbemErrorFile[] (as-built, notional, and reference)
	/// - SimResult[] (as-built, notional, and reference)</code>
	/// </summary>
	public class SbemProject
	{
		public const string ACTUAL_SIM_FILE_NAME = "model.sim";
		public const string ACTUAL_MODEL_SIM_FILE_NAME = "model_sim.csv";
		public const string ACTUAL_EPC_INP_FILE_NAME = "model_epc.inp";
		public const string ACTUAL_ERROR_FILE_NAME = "model.err";
		public const string NOTIONAL_SIM_FILE_NAME = "model_not.sim";
		public const string NOTIONAL_MODEL_SIM_FILE_ = "model_not_sim.csv";
		public const string NOTIONAL_EPC_INP_FILE_NAME = "model_not.inp";
		public const string NOTIONAL_ERROR_FILE_NAME = "model_not.err";
		public const string REFERENCE_SIM_FILE_NAME = "model_ref.sim";
		public const string REFERENCE_MODEL_SIM_FILE_NAME = "model_ref_sim.csv";
		public const string REFERENCE_EPC_INP_FILE_NAME = "model_ref.inp";
		public const string REFERENCE_ERROR_FILE_NAME = "model_ref.err";

		/// <summary>
		/// The minimum constructor. Use BuildFromDirectory for complete instances.
		/// </summary>
		/// <param name="model"></param>
		public SbemProject(SbemModel model) 
		{
			AsBuiltSbemModel	= model;
		}
		/// <summary>
		/// For factory methods that create SbemProjects from stuff on the filesystem
		/// </summary>
		protected SbemProject()	{ }
		/// <summary>
		/// Attach the associated _epc.inp SbemModel.
		/// </summary>
		/// <param name="model"></param>
		public void AddEpcInpModel(SbemEpcModel model)
		{
			AsBuiltSbemEpcModel	= model;
		}
		/// <summary>
		/// Attach the as-built .sim SimResult that has the energy calendar data.
		/// </summary>
		/// <param name="simResult"></param>
		/// <param name="pairWithInpModel"></param>
		public void AddSimResult(SimResult simResult, bool pairWithInpModel)
		{
			AsBuiltSimResult = simResult;
			if (pairWithInpModel)
				AsBuiltSbemModel.PairWithSimResult(simResult);
		}
		/// <summary>
		/// Attach the notional .sim SimResult that has the energy calendar data.
		/// </summary>
		public void AddNotionalSimResult(SimResult simResult, bool pairWithInpModel)
		{
			NotionalSimResult = simResult;
			if (pairWithInpModel)
				NotionalSbemModel.PairWithSimResult(simResult);
		}
		/// <summary>
		/// Attach the reference .sim SimResult that has the energy calendar data.
		/// </summary>
		public void AddReferenceSimResult(SimResult simResult, bool pairWithInpModel)
		{
			ReferenceSimResult	= simResult;
			if (pairWithInpModel)
				ReferenceSbemModel.PairWithSimResult(simResult);
		}
		/// <summary>
		/// Print errors to the console.
		/// </summary>
		public void PrintErrors()
		{
			Console.WriteLine($"{"Code",-24}{"Heating",50}");
			foreach(SbemError error in AsBuiltSbemModel.Errors)
				Console.WriteLine($"{error.Code,-24}{error.Message,50}");
		}

		/// <summary>
		/// The As-Built .inp SBEM model. The thing that SBEM processes.
		/// </summary>
		public SbemModel AsBuiltSbemModel{ get; set; }
		/// <summary>
		/// The Part L2A "notional" Reference model used to set benchmarks for the As-Built or retrofit scenario.
		/// <para>
		/// Reference models are representations of the As-Built if it were retrofitted based on a
		/// standardised process and building regulations. L2A, for new-builds, uses current standards.
		/// </para>
		/// <para>
		/// The Notional model is more or less a stricter version of the ActualReference model. However, the
		/// Notional model also makes significant changes.
		/// </para>
		/// <para>
		/// In the Notional building, the activity assigned to each zone determines whether it will have
		/// access to daylight through windows, roof-lights, or no glazing at all (i.e., no access to daylight),
		/// regardless of the type of glazing applied to the equivalent zone in the Actual building. The glazing
		/// class assigned to each NCM activity is determined in the “activity” table from the NCM Activity
		/// Database in the “DRIVER2A” field (0 for activity with no daylight, i.e., unlit; 1 for side-lit activity; and
		/// 2 for top-lit activity).
		/// </para>
		/// <para>
		/// Find out more:
		/// <a href="https://www.uk-ncm.org.uk/filelibrary/NCM_Modelling_Guide_2021_Edition_England_26Sep2022.pdf">
		/// NCM Modelling Guide for Buildings Other Than Dwellings in England
		/// </a>
		/// </para>
		/// </summary>
		public SbemModel NotionalSbemModel { get; protected set; }
		/// <summary>
		/// The Part L2B Reference model used to set benchmarks for the As-Built or retrofit scenario.
		/// <para>
		/// Reference models are representations of the As-Built if it were retrofitted based on a
		/// standardised process and building regulations. L2A, for new-builds, uses current standards.
		/// </para>
		/// <para>
		/// <code>
		///	Examples:
		///	- The need for heating or cooling is determined by Part L requirements, not the As-Built.
		///	- Heating and hot water is heat sources consume Natural Gas regardless of whether gas is on site or
		///   if a different fuel is present
		/// - Lighting:
		/// -- 3.75W/m²/100lx in offices/industrial process. Otherwise,5.2W/m²/100lx.
		/// -- No occuancy sensors in general lighting, no auto switch display lighting
		/// </code>
		/// </para>
		/// <para>
		/// Find out more:
		/// <a href="https://www.uk-ncm.org.uk/filelibrary/NCM_Modelling_Guide_2021_Edition_England_26Sep2022.pdf">
		/// NCM Modelling Guide for Buildings Other Than Dwellings in England
		/// </a>
		/// </para>
		/// </summary>
		public SbemModel ReferenceSbemModel { get; protected set; }
		/// <summary>
		/// As-Built SBEM _epc.inp output for EPCGen.exe. Contains BER, SER, and summaries HVAC- and Zone-level consumption
		/// </summary>
		public SbemEpcModel AsBuiltSbemEpcModel { get; protected set; }
		/// <summary>
		/// The As-Built SBEM .sim output data. Part L2B.
		/// </summary>
		public SimResult AsBuiltSimResult {  get; protected set; }
		/// <summary>
		/// The new-build Reference SBEM .sim output data. Part L2A.
		/// </summary>
		public SimResult NotionalSimResult { get; protected set; }
		/// <summary>
		/// The existing building Reference SBEM .sim output data. Part L2B.
		/// </summary>
		public SimResult ReferenceSimResult {  get; protected set; }
		/// <summary>
		/// Errors in the existing building .inp model.
		/// </summary>
		public SbemErrorFile AsBuiltSbemErrors {  get; protected set; }
		/// <summary>
		/// Errors in the new-build EPC Reference .inp model.
		/// </summary>
		public SbemErrorFile NotionalSbemErrors { get; protected set; }
		/// <summary>
		/// Errors in the existing building EPC Reference .inp model
		/// </summary>
		public SbemErrorFile ReferenceSbemErrors { get; protected set; }
		/// <summary>
		/// Errors that occur in activities within the class instance. Nothing to do with SbemErrorFile objects.
		/// </summary>
		public List<SbemError> Errors { get; }	= new List<SbemError>();
		/// <summary>
		/// Are their any instance-specific errors?
		/// </summary>
		public bool HasErrors { get => Errors.Count > 0; }
		/// <summary>
		/// Calculate the Domestic Hot Water demand for each HvacSystem in isolation. Update 
		/// HvacSystem End Use ConsumerConsumptionCalendar.
		/// </summary>
		/// <param name="model"></param>
		/// <param name="sbem"></param>
		public static void DisaggregateHVACHotWater(SbemModel model, SbemService sbem)
		{
			// Enable SBEM Service Mode
			bool inServiceMode	= sbem.IsServiceRunning;
			if (!sbem.IsServiceRunning)
				sbem.StartSbemService();
			// Track the original HVACs of the original input SbemModel
			SbemObjectSet<SbemHvacSystem> hvacs = model.HvacSystems.Copy();
			// Hold SemRequet here
			List<SbemRequest> requests			= new();
			List<SbemRequest> finishedRequests	= new();
			SbemModel baseModel					= model.Clone();
			// Delete the existing HVACs. Cuts back on the Clone time, doesn't really make a difference
			baseModel.HvacSystems.Clear();
			// We're doing every HVAC
			for (int hvacID = 0; hvacID < hvacs.Length; hvacID++)
			{
				// Create new model and add the current HVAC System
				SbemModel singleHvacModel	= baseModel.Clone();
				singleHvacModel.HvacSystems.Add(hvacs[hvacID]);
				// Create, track, and queue the SbemRequest
				SbemRequest request			= new SbemRequest(singleHvacModel);
				requests.Add(request);
				sbem.QueueRequest(request);
			}
			// Restoe the original SbemModel's HVAC set
			for (int hvacID = 0; hvacID < hvacs.Length; hvacID++)
				model.HvacSystems.Add(hvacs[hvacID]);
			// Process requests
			while (requests.Count > 0)
			{
				for (int requestID = 0; requestID < requests.Count; requestID++)
					if (requests[requestID].IsFinished)
					{
						SbemRequest request		= requests[requestID];
						SbemModel requestModel = request.OutputProject.AsBuiltSbemModel;
						SbemHvacSystem hvac		= requestModel.HvacSystems[0];
						baseModel.HvacSystems[hvac.Name].EndUseConsumerCalendar.SetDHW(requestModel.EndUseConsumerCalendar);
						requests.Remove(request);
					}
				Thread.Sleep(50);
			}
			// Do SbemRequest processing. But wait! Why not in the previous loop? The 
			// game plan is to make the SBEM Request Service threaded at some point. This
			// is thread-safe.
			for (int requestID = 0; requestID < requests.Count; requestID++)
			{
				
			}
			// Disable the service if it wasn't on previously.
			if (!inServiceMode)
				sbem.StopService();
		}
		/// <summary>
		/// Calculate the Domestic Hot Water demand for each Zone in isolation. Update 
		/// Set the SbemZone End Use ConsumerConsumptionCalendar.
		/// </summary>
		/// <param name="model"></param>
		/// <param name="sbem"></param>
		public static void CalculateZonalEnergyDemand(SbemModel model, SbemService sbem)
		{
			// Enable SBEM Service Mode
			bool inServiceMode = sbem.IsServiceRunning;
			if (!sbem.IsServiceRunning)
				sbem.StartSbemService();
			// Track the original HVACs of the original input SbemModel
			SbemObjectSet<SbemHvacSystem> hvacs = model.HvacSystems.Copy();
			// Hold SemRequet here
			List<SbemRequest> requests	= new();
			SbemModel baseModel			= model.Clone();
			List<SbemZone> zones		= new();
			// Delete the existing HVACs. Cuts back on the Clone time, doesn't really make a difference
			baseModel.HvacSystems.Clear();
			// We're doing every HVAC
			for (int hvacID = 0; hvacID < hvacs.Length; hvacID++)
			{
				SbemHvacSystem hvac					= hvacs[hvacID];
				SbemObjectSet<SbemZone> tempZones	= hvac.Zones.Copy();
				for(int zoneID = 0; zoneID <  hvac.Zones.Length; zoneID++)
				{
					// The SbemModel for the SbemZone
					SbemModel singleZoneModel	= baseModel.Clone();
					// House cleaning...
					SbemZone zone				= tempZones[zoneID];
					// Add the one HVAC System
					singleZoneModel.HvacSystems.Add(hvac);
					// Make sure there's no Zones then add this one.
					hvac.Zones.Clear();
					hvac.Zones.Add(zone);
					// Track Zones in this method as well. Used for thread-safe calendar mapping later.
					zones.Add(zone);
					// Create the request then send it to the SBEM Service
					SbemRequest request			= new SbemRequest(singleZoneModel);
					requests.Add(request);
					sbem.QueueRequest(request);
				}
			}
			// Restoe the original SbemModel's HVAC set
			for (int hvacID = 0; hvacID < hvacs.Length; hvacID++)
				model.HvacSystems.Add(hvacs[hvacID]);
			// Process requests
			while (requests.Count > 0)
			{
				// Check all the SbemRequest to see if they're still processing. Remove finished from the request tracker
				for (int requestID = 0; requestID < requests.Count; requestID++)
					if (requests[requestID].IsFinished)
					{
						SbemRequest request = requests[requestID];
						SbemZone requestZone = request.OutputProject.AsBuiltSbemModel.Zones[0];
						SbemZone originalZone = model.Zones[requestZone.Name];
						originalZone.SetEndUseConsumerCalendar(request.OutputProject.AsBuiltSbemModel.EndUseConsumerCalendar);
						originalZone.SetFuelUseConsumerCalendar(request.OutputProject.AsBuiltSbemModel.FuelUseConsumerCalendar);
						requests.Remove(request);
					}
				Thread.Sleep(50);
			}

			// Disable the service if it wasn't on previously.
			if (!inServiceMode)
				sbem.StopService();
		}
		/// <summary>
		/// Building an SbemProject from the contents of a directory, where its assumed the directory contains
		/// SBEM input and or output files: 
		///<code>
		/// Parse:
		///		.sim SBEM output files. Fuel and End Use consumption, internal gains and HVAC demands.
		///		.epc SBEM output for EPCGen.exe. Contains the high-level SBEM results. SER, BER, TER, Consumer demand,..
		///		.err SBEM output files with errors. The As-Built, notional, and reference models each have .err files.
		/// </code>
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static SbemProject BuildFromDirectory(string path)
		{
			SbemProject project = new SbemProject();
			if (!Directory.Exists(path))
			{
				project.Errors.Add(new SbemError(SbemError.ErrorCode.CONTENT_FILE_NOT_EXISTS, $"Couldn't find '{path}"));
				return project;
			}
			/*
			 * This method is done in three stages.
			 *	1). Locate files on the fly.
			 *	2) Load found files
			 *	3) Pair .sim results with .inp models
			 *	
			 * This bit loads on the fly. In an ideal world, every SBEM inp that makes it to the filesystem
			 * would have the base name "model": "model.inp", "model_sim.csv" etc. But you can't force it. This
			 * method instead looks for files based on there actual file identifying name: "_ref.inp", "_not.sim" etc.
			 */
			string errorPath				= "";
			string notionalErrorPath		= "";
			string referenceErrorPath		= "";
			string inpPath					= "";
			string notionalInpPath			= "";
			string referenceInpModelPath	= "";
			string epcInpPath				= "";
			string simResultPath			= "";
			string notionalSimResultPath	= "";
			string referenceSimResultPath	= "";
			string[] filePaths				= Directory.GetFiles(path);
			for (int fileID = 0; fileID < filePaths.Length; fileID++)
			{
				string filePath = filePaths[fileID].ToLower();
				// .err error files
				if (filePath.Contains("_ref.err"))
					referenceSimResultPath = filePath;
				else if (filePath.Contains("_not.err"))
					notionalSimResultPath = filePath;
				else if (filePath.Contains(".err"))
					simResultPath = filePath;
				// .inp including _epc.inp
				if (filePath.Contains("_ref.inp"))
					referenceInpModelPath = filePath;
				else if (filePath.Contains("_not.inp"))
					notionalInpPath = filePath;
				else if (filePath.Contains("_epc.inp"))
					epcInpPath = filePath;
				else if (filePath.Contains(".inp"))
					inpPath = filePath;
				// .sim files
				if (filePath.Contains("_ref.sim"))
					referenceSimResultPath = filePath;
				else if (filePath.Contains("_not.sim"))
					notionalSimResultPath = filePath;
				else if (filePath.Contains(".sim"))
					simResultPath = filePath;
			}
			// Actual models and ouputs
			if (inpPath != "")
				project.AsBuiltSbemModel = SbemModel.ParseInpFile(inpPath);
			else
				project.Errors.Add(new SbemError(SbemError.ErrorCode.CONTENT_FILE_NOT_EXISTS, inpPath));
			if (epcInpPath != "")
				project.AsBuiltSbemEpcModel = SbemEpcModel.ParseInpFile(epcInpPath);
			else
				project.Errors.Add(new SbemError(SbemError.ErrorCode.CONTENT_FILE_NOT_EXISTS, epcInpPath));
			if (simResultPath != "")
				project.AsBuiltSimResult = SimResult.ParseSimFile(simResultPath);
			else
				project.Errors.Add(new SbemError(SbemError.ErrorCode.CONTENT_FILE_NOT_EXISTS, simResultPath));
			if (errorPath != "")
				project.AsBuiltSbemErrors = SbemErrorFile.ParseErrorFile(errorPath);
			else
				project.Errors.Add(new SbemError(SbemError.ErrorCode.CONTENT_FILE_NOT_EXISTS, errorPath));
			// Notional models and outputs
			if (notionalInpPath != "")
				project.NotionalSbemModel = SbemModel.ParseInpFile(notionalInpPath);
			else
				project.Errors.Add(new SbemError(SbemError.ErrorCode.CONTENT_FILE_NOT_EXISTS, notionalInpPath));
			if (notionalSimResultPath != "")
				project.NotionalSimResult = SimResult.ParseSimFile(notionalSimResultPath);
			else
				project.Errors.Add(new SbemError(SbemError.ErrorCode.CONTENT_FILE_NOT_EXISTS, notionalSimResultPath));
			if (notionalErrorPath != "")
				project.NotionalSbemErrors = SbemErrorFile.ParseErrorFile(notionalErrorPath);
			else
				project.Errors.Add(new SbemError(SbemError.ErrorCode.CONTENT_FILE_NOT_EXISTS, notionalErrorPath));
			// Reference models and outputs
			if (referenceInpModelPath != "")
				project.ReferenceSbemModel = SbemModel.ParseInpFile(referenceInpModelPath);
			else
				project.Errors.Add(new SbemError(SbemError.ErrorCode.CONTENT_FILE_NOT_EXISTS, referenceInpModelPath));
			if (referenceSimResultPath != "")
				project.ReferenceSimResult = SimResult.ParseSimFile(referenceSimResultPath);
			else
				project.Errors.Add(new SbemError(SbemError.ErrorCode.CONTENT_FILE_NOT_EXISTS, referenceSimResultPath));
			if (referenceErrorPath != "")
				project.ReferenceSbemErrors = SbemErrorFile.ParseErrorFile(referenceErrorPath);
			else
				project.Errors.Add(new SbemError(SbemError.ErrorCode.CONTENT_FILE_NOT_EXISTS, referenceErrorPath));
			// Pair SimResult with counterpart .inp models.
			if (project.AsBuiltSimResult != null)
				project.AsBuiltSbemModel.PairWithSimResult(project.AsBuiltSimResult);
			if (project.NotionalSimResult != null)
				project.NotionalSbemModel.PairWithSimResult(project.NotionalSimResult);
			if (project.ReferenceSimResult != null)
				project.ReferenceSbemModel.PairWithSimResult(project.ReferenceSimResult);
			return project;
		}
	}
}
