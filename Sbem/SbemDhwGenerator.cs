


using GeneticSharp;
using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// The .inp hot water systems for domestic services including showers
	/// <para>Note: Showers are represented by SbemShower.</para>
	/// <code>Relationships:
	/// - Has many SbemShower
	/// - Has many dependent SbemHvacSystem.
	/// </code>
	/// </summary>
	//"HWS" = DHW-GENERATOR
	//	HEAT-GEN-TYPE = Instantaneous hot water only
	//	FUEL-TYPE = Grid Supplied Electricity
	//	STORE-SYSTEM = NO
	//	SHOWERS-SERVED-REF = { "Default" }
	//	SHOWERS-SERVED-NUM = { 1 }
	//	ISBEM-ID = 11
	//	DHW-GEN-SEFF = 0.7
	//	..
	public class SbemDhwGenerator : SbemObject
	{
		public const string OBJECT_NAME  = "DHW-GENERATOR"; 
		public override string ObjectName() { return OBJECT_NAME; }
		public SbemDhwGenerator(string currentName, List<string> currentProperties) : base(currentName, currentProperties)
		{

		}
	}
}
