using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// Sim Result - where SBEM .sim output data is collated.
	/// 
	/// </summary>
	public class SimResult
	{
		/*
		 *  Report headers - Used to determine if the next however many lines contains useful information and what kind of
		 *  information is there. For now, we're only interested in UsageCalendarBase-friendly consumptio calendars.
		 */
		public const string PROJECT_FUEL_CALENDAR_LABEL				= "REPORT- Energy consumption by Fuel Type (kWh/m2): PROJECT";
		public const string PROJECT_CONSUMER_CALENDAR_LABEL			= "REPORT- Energy consumption by End Uses (kWh/m2):";
		public const string HVAC_CONSUMER_CALENDAR_LABEL			= "REPORT- HVAC Energy consumption by End Uses (kWh/m2):";
		public const string HVAC_FUEL_CONSUMPTION_CALENDAR_LABEL	= "REPORT- HVAC Energy consumption by Fuel Type (kWh/m2):";
		public const string ZONE_HEATING_DEMAND_REPORT_LABEL		= "REPORT- LD-H Details of Heating demand for zone";
		public const string ZONE_COOLING_DEMAND_REPORT_LABEL		= "REPORT- LD-C Details of Cooling demand for zone";
		public const string ZONE_INTERNAL_GAINS_REPORT_LABEL		= "REPORT- LD-I Details of Internal heat production for zone";
		public const string PROJECT_RENEWWABLES_CO2_REPORT_LABEL	= "REPORT- CO2 emissions and primary energy equivalent to renewables-generated electricity";
		public const string PROJECT_ENERGY_PERFORMANCE_REPORT_ABEL = "REPORT- PEPS Project Energy Performance. Delivered energy consumption";
		public FuelConsumptionCalendar FuelCalendar { get; }
		public ConsumerConsumptionCalendar ConsumerCalendar { get; }
		public Dictionary<string, ConsumerConsumptionCalendar> HvacConsumerCalendars { get; } = new();
		public Dictionary<string, FuelConsumptionCalendar> HvacFuelCalendars { get; } = new();
		public Dictionary<string, InternalGainsCalendar> ZoneInternalGainsCalendars{ get; } = new();
		public Dictionary<string, HeatingDemandCalendar> ZoneHeatingDemandCalendars { get; } = new();
		public Dictionary<string, CoolingDemandCalendar> ZoneCoolingDemandCalendars { get; } = new();

		public readonly List<string> Errors = new List<string>();
		public SimResult(ConsumerConsumptionCalendar consumption, FuelConsumptionCalendar fuel)
		{ 
			ConsumerCalendar	= consumption;
			FuelCalendar		= fuel;
		}
		public static SimResult ParseSimFile(string path)
		{
			ConsumerConsumptionCalendar consumption	= new ConsumerConsumptionCalendar("Project");
			FuelConsumptionCalendar fuel			= new FuelConsumptionCalendar("Project");
			SimResult simResults					= new SimResult(consumption, fuel);
			if (!File.Exists(path))
			{
				Console.Write($"SIM reader: Path not found {path}");
				return simResults;
			}
			bool inConsumer		= false;
			bool inHvacConsumer	= false;
			bool inHvacFuel		= false;
			bool inZoneHeating	= false;
			bool inZoneCooling	= false;
			bool inZoneGains	= false;
			bool inFuel			= false;
			bool foundSomething	= false;
			string name			= "";
			string[] lines = File.ReadAllLines(path);

			for (int i = 0; i < lines.Length; i++)
			{
				
				string line = lines[i].Trim();
				// Skip non-calendar reports.

				if (line == PROJECT_CONSUMER_CALENDAR_LABEL)
				{
					inConsumer		= true;
					foundSomething	= true;
				}
				else if (line == PROJECT_FUEL_CALENDAR_LABEL)
				{
					inFuel			= true;
					foundSomething	= true;
				} else if (line.StartsWith(HVAC_CONSUMER_CALENDAR_LABEL))
				{
					inHvacConsumer	= true;
					foundSomething	= true;
					name			= line.Split(':')[^1].Trim();
				}
				else if (line.StartsWith(HVAC_FUEL_CONSUMPTION_CALENDAR_LABEL))
				{
					inHvacFuel		= true;
					foundSomething	= true;
					name			= line.Split(':')[^1].Trim();
				}
				else if(line.StartsWith(ZONE_HEATING_DEMAND_REPORT_LABEL))
				{
					inZoneHeating	= true;
					foundSomething	= true;
					name			= line.Split(':')[^1].Trim();
				}else if (line.StartsWith(ZONE_COOLING_DEMAND_REPORT_LABEL))
				{
					inZoneCooling	= true;
					foundSomething	= true;
					name			= line.Split(':')[^1].Trim();
				}else if(line.StartsWith(ZONE_INTERNAL_GAINS_REPORT_LABEL))
				{
					inZoneGains		= true;
					foundSomething	= true;
					name			= line.Split(':')[^1].Trim();
				}
				// If we're processing a calendar
				if (foundSomething)
				{
					string header	= line;
					string[] rows	= new string[12];
					i				+= 5;
					for (int x = 0; x < 12; x++)
					{
						rows[x] = lines[i];
						i++;
					}
					if (inConsumer)
					{
						for (int rowID = 0; rowID < 12; rowID++)
							consumption.AddRecord(ConsumerConsumptionRecord.FromLine(rows[rowID]));
					}
					else if (inFuel)
					{
						for (int rowID = 0; rowID < 12; rowID++)
							fuel.AddRecord(FuelConsumptionRecord.FromLine(rows[rowID]));
					}else if (inHvacConsumer)
					{
						ConsumerConsumptionCalendar hvacConsumerCalendar = new ConsumerConsumptionCalendar(name);
						for (int rowID = 0; rowID < 12; rowID++)
							hvacConsumerCalendar.AddRecord(ConsumerConsumptionRecord.FromHvacLine(rows[rowID]));
						simResults.HvacConsumerCalendars[name]	= hvacConsumerCalendar;
					}
					else if (inHvacFuel)
					{
						FuelConsumptionCalendar hvacFuelConsumerCalendar = new FuelConsumptionCalendar(name);
						for (int rowID = 0; rowID < 12; rowID++)
							hvacFuelConsumerCalendar.AddRecord(FuelConsumptionRecord.FromLine(rows[rowID]));
						simResults.HvacFuelCalendars[name] = hvacFuelConsumerCalendar;
					}
					else if (inZoneCooling)
					{
						CoolingDemandCalendar coolingDemandCalendar = new CoolingDemandCalendar(name);
						for (int rowID = 0; rowID < 12; rowID++)
							coolingDemandCalendar.AddRecord(CoolingDemandRecord.FromLine(rows[rowID]));
						simResults.ZoneCoolingDemandCalendars[name] = coolingDemandCalendar;
					}
					else if (inZoneHeating)
					{
						HeatingDemandCalendar heatingDemandCalendar = new HeatingDemandCalendar(name);
						for (int rowID = 0; rowID < 12; rowID++)
							heatingDemandCalendar.AddRecord(HeatingDemandRecord.FromLine(rows[rowID]));
						simResults.ZoneHeatingDemandCalendars[name] = heatingDemandCalendar;
					}
					else if (inZoneGains)
					{
						InternalGainsCalendar zoneInternalGainsCalendar	= new InternalGainsCalendar(name);
						for (int rowID = 0; rowID < 12; rowID++)
							zoneInternalGainsCalendar.AddRecord(InternalGainsRecord.FromLine(rows[rowID]));
						simResults.ZoneInternalGainsCalendars[name] = zoneInternalGainsCalendar;
					}
					// Don't worry about figuring out what was found, just reset everything.
					inConsumer		= false;
					inFuel			= false;
					inZoneHeating	= false;
					inZoneCooling	= false;
					inZoneGains		= false;
					inHvacConsumer	= false;
					inHvacFuel		= false;
					foundSomething	= false;
				}
			}
			return simResults;
		}
	}
}
