using MeesSDK.Sbem.ConsumerCalendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// Sim Result - where SBEM .sim output data is collated.
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
		public const string PROJECT_ENERGY_PERFORMANCE_REPORT_ABEL	= "REPORT- PEPS Project Energy Performance. Delivered energy consumption";
		/// <summary>
		/// Project-level Fuel Type consumption calendar.
		/// </summary>
		public FuelConsumptionCalendar FuelCalendar { get; protected set; }
		/// <summary>
		/// Project-level End Use consumption calendar.
		/// </summary>
		public ConsumerConsumptionCalendar ConsumerCalendar { get; protected set; }
		/// <summary>
		/// HVAC-level End Use consumption calendars.
		/// </summary>
		public Dictionary<string, ConsumerConsumptionCalendar> HvacConsumerCalendars { get; } = new();
		/// <summary>
		/// HVAC-level Fuel Type consumption calendars.
		/// </summary>
		public Dictionary<string, FuelConsumptionCalendar> HvacFuelCalendars { get; } = new();
		/// <summary>
		/// Zone-level Internal Heat Production calendars.
		/// </summary>
		public Dictionary<string, InternalGainsCalendar> ZoneInternalGainsCalendars{ get; } = new();
		/// <summary>
		/// Zone-level Heating Demand calendars.
		/// </summary>
		public Dictionary<string, HeatingDemandCalendar> ZoneHeatingDemandCalendars { get; } = new();
		/// <summary>
		/// Zone-level Cooling Demand calendars.
		/// </summary>
		public Dictionary<string, CoolingDemandCalendar> ZoneCoolingDemandCalendars { get; } = new();
		/// <summary>
		/// Report any errors with the file here.
		/// </summary>

		public readonly List<string> Errors = new List<string>();
		public SimResult(ConsumerConsumptionCalendar consumption, FuelConsumptionCalendar fuel)
		{ 
			ConsumerCalendar	= consumption;
			FuelCalendar		= fuel;
		}
		public SimResult()
		{

		}
		/// <summary>
		/// Parse a .sim or _sim.csv file.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static SimResult ParseSimFile(string path)
		{
			SimResult simResult					= new SimResult();
			if (!File.Exists(path))
			{
				Console.Write($"SIM reader: Path not found {path}");
				return simResult;
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

			for (int lineID = 0; lineID < lines.Length; lineID++)
			{
				
				string line = lines[lineID].Trim();
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
					lineID			+= 2;
					float area		= float.Parse(lines[lineID].Split(",")[0]);
					string header	= line;
					string[] rows	= new string[12];
					lineID			+=3;
					for (int x = 0; x < 12; x++)
					{
						rows[x] = lines[lineID];
						lineID++;
					}
					if (inConsumer)
					{
						ConsumerConsumptionCalendar consumption			= new ConsumerConsumptionCalendar("Project", area);
						for (int rowID = 0; rowID < 12; rowID++)
							consumption.AddRecord(ConsumerConsumptionRecord.FromLine(rows[rowID]));
						simResult.SetProjectEndUseCalendar(consumption);
					}
					else if (inFuel)
					{
						FuelConsumptionCalendar fuel					= new FuelConsumptionCalendar("Project", area);
						for (int rowID = 0; rowID < 12; rowID++)
							fuel.AddRecord(FuelConsumptionRecord.FromLine(rows[rowID]));
					}else if (inHvacConsumer)
					{
						ConsumerConsumptionCalendar hvacConsumerCalendar = new ConsumerConsumptionCalendar(name, area);
						for (int rowID = 0; rowID < 12; rowID++)
							hvacConsumerCalendar.AddRecord(ConsumerConsumptionRecord.FromHvacLine(rows[rowID]));
						simResult.HvacConsumerCalendars[name]			= hvacConsumerCalendar;
					}
					else if (inHvacFuel)
					{
						FuelConsumptionCalendar hvacFuelConsumerCalendar = new FuelConsumptionCalendar(name, area);
						for (int rowID = 0; rowID < 12; rowID++)
							hvacFuelConsumerCalendar.AddRecord(FuelConsumptionRecord.FromLine(rows[rowID]));
						simResult.HvacFuelCalendars[name] = hvacFuelConsumerCalendar;
					}
					else if (inZoneCooling)
					{
						CoolingDemandCalendar coolingDemandCalendar = new CoolingDemandCalendar(name, area);
						for (int rowID = 0; rowID < 12; rowID++)
							coolingDemandCalendar.AddRecord(CoolingDemandRecord.FromLine(rows[rowID]));
						simResult.ZoneCoolingDemandCalendars[name] = coolingDemandCalendar;
					}
					else if (inZoneHeating)
					{
						HeatingDemandCalendar heatingDemandCalendar = new HeatingDemandCalendar(name, area);
						for (int rowID = 0; rowID < 12; rowID++)
							heatingDemandCalendar.AddRecord(HeatingDemandRecord.FromLine(rows[rowID]));
						simResult.ZoneHeatingDemandCalendars[name] = heatingDemandCalendar;
					}
					else if (inZoneGains)
					{
						InternalGainsCalendar zoneInternalGainsCalendar	= new InternalGainsCalendar(name, area);
						for (int rowID = 0; rowID < 12; rowID++)
							zoneInternalGainsCalendar.AddRecord(InternalGainsRecord.FromLine(rows[rowID]));
						simResult.ZoneInternalGainsCalendars[name] = zoneInternalGainsCalendar;
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
			return simResult;
		}
		/// <summary>
		/// Set the building End Use Calendar. 
		/// <para>Note: A bit dirty but it let's us force area in UsageCalendarBase constructors,
		/// which is a lot more consistent than passing this calendar at SimResult constructor.</para>
		/// </summary>
		/// <param name="calendar"></param>
		public void SetProjectEndUseCalendar(ConsumerConsumptionCalendar calendar)
		{
			ConsumerCalendar    = calendar;
		}
		/// <summary>
		/// Set the building Fuel Type Calendar. 
		/// <para>Note: A bit dirty but it let's us force area in UsageCalendarBase constructors,
		/// which is a lot more consistent than passing this calendar at SimResult constructor.</para>
		/// </summary>
		/// <param name="calendar"></param>
		public void SetProjectFuelTypeCalendar(ConsumerConsumptionCalendar calendar)
		{
			ConsumerCalendar    = calendar;
		}
	}
}
