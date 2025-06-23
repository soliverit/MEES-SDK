using MeesSDK.DataManagement;
using MeesSDK.RdSAP;
using MeesSDK.RdSAP.Optimisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Examples.RdSAP
{
	public class JAEMOOEstateOptimisation : IMeesSDKExample
	{
		public string InputDataPath;
		public JAEMOOEstateOptimisation(string path) 
		{
			InputDataPath = path;
		}
		public void RunTheExample()
		{
			Console.WriteLine("Committed this a bit early. Might fail 23/06/25");
			// Load the data
			CsvHandler csvHandler			= CsvHandler.ParseCSV(InputDataPath);
			csvHandler.PrintErrors();
			// Build the data set.	
			MathNetRetrofitsTable data		= new MathNetRetrofitsTable(csvHandler, RdSAPRetrofitOption.ALL_RETROFIT_OPTION_KEYS.ToArray());
			// Create the subset optimiser
			RdSAPSubsetOptimiser optimiser	= new RdSAPSubsetOptimiser(data);
			// Run it.
			optimiser.Run();
		}
		public string GetDescription()
		{
			return @"

";
		}
	}
}
