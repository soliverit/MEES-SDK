using MeesSDK.Sbem;
using MeesSDK.Sbem.Retrofitting.Measures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Examples
{
	public class Retrofitting : IMeesSDKExample
	{
		public string Name { get; set; } = "EPC Lighting 5 - T8 lamp replacement";
		public void DoTheExample(SbemProject project, SbemService sbem)
		{
			/* 
			 * Once you've create your retrofit method, it's straightforward
			 * to apply.
			 * 
			 * Check out MeesSDK.Sbem.Retrofitting.Measures: NCMLighting5Example
			 * 
			 * NCMLighting5Example explains how retrofitting works
			 */
			// Create the Retrofit
			NCMLighting5Example retrofit = new NCMLighting5Example(project.AsBuiltSbemModel);
			// Apply the retrofit
			retrofit.Apply();
			// Process and get the resulting SbemProject
			SbemProject retrofittedProject = sbem.BuildProject(retrofit.Model);
			/*
			 * The difference model(tm).
			 * 
			 * It's helpful to know the difference in demand and production of a 
			 * retrofitted SbemModel against another, typically the as-built model. 
			 * This model substracts all energy calendars from the counter part to 
			 * to leave difference in demand or production.
			 * 
			 * In a later example, we'll use this for occupancy correction.
			 */
			SbemModel differenceModel = project.AsBuiltSbemModel.GetDifferenceModel(retrofittedProject.AsBuiltSbemModel);
			// Print old and new End Use
			project.AsBuiltSbemModel.EndUseConsumerCalendar.Print();
			differenceModel.EndUseConsumerCalendar.Print();
		}
		//===================
		public string GetDescription()
		{
			return @""" 
Lighting Retrofit:

	We want to see how efficient our building would be if T8 lamps, halphosphate / triphosphor and 
	low- / high-efficiency ballasts, were replaced with LEDs at 60lm/W.

1: Load the SbemModel
2: Prepare and run the Retorift
3: Get Retrofit results
4: Get the consumption difference SbemModel
""";
		}
	}
}
