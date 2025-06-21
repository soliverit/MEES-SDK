using MeesSDK.Sbem;
using MeesSDK.Sbem.ConsumerCalendar;
using System.Text;
public class SbemModel : SbemModelBase
{
	/// <summary>
	/// The .inp model - the centre piece of all things SBEM.
	/// <para>
	/// There's three versions of this file: As-Built/Actual, Notional,
	/// and Reference. These are where the BER, TER, and SER are calculated from.
	/// </para>
	/// </summary>
	enum ErrorCodes
	{
		CONTENT_FILE_NOT_EXISTS,
		CONTENT_CORRUPT,
		SBEM_OBJECT_MISSING
	};
	/// <summary>
	/// The bare minimum meta data to make SBEM run.
	/// </summary>
	public static readonly string MIN_INP = @"""PROJECT"" = GENERAL
B-TYPE = B8 Storage or Distribution
WEATHER = MAN
B-COUNTRY = England and Wales
P-NAME = ""8 BUSINESS PARK""
B-ADDRESS-0 = ""Unit (lol)""
B-ADDRESS-1 = ""Business Centre""
B-ADDRESS-2 = ""Man Road""
B-CITY = ""Portford""
B-POSTCODE = ""PF1 3AS""
C-NAME = ""Rab C Nisbit""
C-TELEPHONE = ""01324 555 555""
C-ADDRESS = ""124 Business Road""
C-EMAIL = ""info@business.ac.uk""
C-CITY = ""Business city""
C-POSTCODE = ""BN1 1NB""
C-REG-NUMBER = ""EES-007""
C-ACCRED-SCHEME = Elmhurst Energy Systems
C-EMP-TRAD-NAME = ""Business Energy""
C-EMP-TRAD-ADDRESS = ""Business Energy trade address""
C-REL-PART-DISC = Not related to the owner
TRANSACTION-TYPE = Voluntary (No legal requirement for an EPC)
C-QUALIFICATIONS = NOS4
SOFT-COMP-NAME = G-ISBEM Ltd
INTERFACE-VAL = iSBEM
INTERFACE = G-ISBEM
INTERFACE-VERSION = v25.3
PATH-FILE-INTERFACE = ""./BUSINESS PARK.nct""
ACT-NOT = ACT
BUILDING-AREA = 175.85
FOUNDATION-AREA = 169.38
ELEC-POWER-FACTOR = <0.9
NOS-LEVEL = Level 3
C-INSURER = ""Hillson LTD""
C-INS-POL-NUMBER = ""PLL3334234-7""
C-INS-EFF-DATE = ""2022-07-31""
C-INS-EXP-DATE = ""2023-07-30""
C-INS-PI-LIMIT = ""5000000""
B-INSP-DATE = { 2023, 06, 15 }
UPRN = ""UPRN-12345676""
LIGHT-METERING = 1
BUILD-ORIENTATION = 0
MAX-STOREY = 2
 ..

""SBEM"" = COMPLIANCE
EPC-TYPE = EPC England
ENG-HERITAGE = NO
BR-STAGE = As built
AIR-CON-INSTALLED = No
 ..";
	/// <summary>
	/// A list of Generic error structs used to track problems with the model found
	/// internally before sending to SBEM.
	/// </summary>
	public List<SbemError> Errors = new List<SbemError>();
	/// <summary>
	/// The .inp metadata object. Accrediting body, Assessor, Building, Location, 
	/// District heat CO2 fact, power factor, SBEM interface information.
	/// </summary>
	public SbemGeneral General { get; set; }
	/// <summary>
	/// The .inp information about the calulcation, the building construction phase, and
	/// regulations
	/// </summary>
    public SbemCompliance Compliance { get; set; }
	/// <summary>
	/// List<SbemConstruction> The .inp objects for opaque surface construction properties. SbemDoor
	/// and SbemWall.
	/// </summary>
    public SbemObjectSet<SbemConstruction> Constructions { get; } = new();
	/// <summary>
	/// List<SbemGlass> The .inp objects for transparent surface construction properties. Window
	/// </summary>
    public SbemObjectSet<SbemGlass> Glasses { get; } = new();
	/// <summary>
	/// The .inp HVAC definitions. HVACs define heating, cooling, and central ventilation strategy.
	/// </summary>
    public SbemObjectSet<SbemHvacSystem> HvacSystems { get; } = new();
	/// <summary>
	/// The .inp Domestic Hot Water system definitions. They define systems providing domestic hot water
	/// or hot water and heating.
	/// </summary>
    public SbemObjectSet<SbemDhwGenerator> Dhws { get; } = new();
	/// <summary>
	/// The .inp shower definitions. They define SbemShower.
	/// </summary>
    public SbemObjectSet<SbemShower> Showers { get; } = new();
	/// <summary>
	/// The .inp assessor recommendation comments and selections. These are objects added by
	/// the assessor or the SBEM interface that tell SBEM whether measures should be included 
	/// and their potential impact if they were. Potential in ordinal efficiency labels (poor, good).
	/// </summary>
    public SbemObjectSet<SbemRecUser> RecUsers { get; } = new();
	/// <summary>
	/// Th .inp seemingly unused struct for defining retrofit impacts in real monetary and efficiency
	/// terms.
	/// </summary>
    public SbemObjectSet<SbemImprovementMeasure> ImprovementMeasures { get; } = new();
	/// <summary>
	/// The .sim output Project End Use consumption calendar. Monthly consumption for heating, cooling,
	/// hot water, lighting, auxiliary systems, and equipment.
	/// <para>Note: Must be attached separately using SetEndUseConsumerCalendar() from the SBEM
	/// .sim or _sim.csv results.</para>
	/// consumption.
	/// </summary>
	public ConsumerConsumptionCalendar EndUseConsumerCalendar { get; protected set; }
	/// <summary>
	/// The .sim output Project Fuel Type consumption calendar. Monthly consumption for: natural gas,
	/// lpg, oil, coal, anthracite, dual fuel, smokeless, grid supplied electricity, biomass, biogass,
	/// waste heat, and grid displaced electricity. It also has District Heating, but that's not a fuel
	/// type directly and its instance-specific fuel type isn't defined in the .sim.
	/// <para>Note: Must be attached separately using SetEndUseConsumerCalendar() from the SBEM
	/// .sim or _sim.csv results.</para>
	/// consumption.
	/// </summary>
	public FuelConsumptionCalendar FuelUseConsumerCalendar { get; protected set; }
	/// <summary>
	/// The .inp Activity Space definitions. These are one ore more physical spaces that share HVAC and
	/// activity space type. They aren't necessarily a single physical space or even adjacent spaces. 
	/// For example, there's nothing in the SBEM interface certification that say communal storage on
	/// floors 1 and 12 of a 15 storey office can't be merged. Merging was preferable around 2008 due to
	/// hardware limitations. The problem is confound by the MULTIPLIER or number of instances property,
	/// that can group storage spaces 1 with 12, 2 with 5, and 3 with 8, as one ZONE.
	/// </summary>
	public SbemObjectSet<SbemZone> Zones { get; } = new();
	/// <summary>
	/// The .sim SimReult data created by SBEM. Attached using PairWithSimResult(). This contains End Use
	/// demand, heat production, and renewables annual calendars. 12 month calendars 
	/// </summary>
	public SimResult? PairedSimResult { get; protected set; } 
	/// <summary>
	/// The .inp for on-site solar panels.
	/// </summary>
    public SbemPvs Pvs { get; set; }
	/// <summary>
	/// Is there solar panels on-site?
	/// </summary>
	public bool hasPvs = false;
	/// <summary>
	/// The .in solar energy system for hot water.
	/// </summary>
    public SbemSes Ses { get; set; }
	public bool HasSes { get => Ses != null; }
	/// <summary>
	/// has solar hot water?
	/// </summary>
	public bool HasPvs { get => Pvs != null; }
	/// <summary>
	/// The .inp wind turbine definition.
	/// </summary>
	public SbemWindGenerator WindGenerator { get; set; }
	/// <summary>
	/// The mysterious .inp object that hasn't had a single property
	/// since before 2008.
	/// </summary>
    public SbemRecProject RecProject { get; set; }

	/// <summary>
	/// Are there SbemWindGenerator on-site?
	/// </summary>
    public bool HasWindGenerator { get => WindGenerator != null; }
	/// <summary>
	/// Parse a .inp model into SbemModel from the filesystem.
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static SbemModel ParseInpFile(string path)
	{
		if (!File.Exists(path))
		{
			var model = new SbemModel();
			model.AddError((int)SbemError.ErrorCode.CONTENT_FILE_NOT_EXISTS, $"Inp file '{path}' doesn't exist.");
			return model;
		} 
		string content = File.ReadAllText(path);
		return ParseInpContent(content);
	}
	/// <summary>
	/// Parse a .inp model into SbemModel from .inp file contents
	/// </summary>
	/// <param name="content"></param>
	/// <returns></returns>
	public static SbemModel ParseInpContent(string content)
	{
		var model = new SbemModel();
		using var reader = new StringReader(content);

		string line;
		string currentName = null;
		string currentType = null;
		var currentProperties = new List<string>();
		bool inObject = false;

		SbemDoor currentDoor = null;
		SbemHvacSystem currentHvac = null;
		SbemWall currentWall = null;
		SbemWindow currentWindow = null;
		SbemZone currentZone = null;
		int lineNumber = 0;
		while ((line = reader.ReadLine()) != null)
		{
			lineNumber++;
			line = line.Trim();
			if (string.IsNullOrWhiteSpace(line) || line.StartsWith("$")) 
				continue;
			else if (!inObject)
			{
				var header = SbemObject.GetHeaderLine(line);
				if (header.isHeader)
				{
					inObject = true;
					currentName = header.name;
					currentType = header.type;
					currentProperties.Clear();
				}
				else
				{
					model.AddError(SbemError.ErrorCode.CONTENT_CORRUPT, $"Line {lineNumber} was expected to be a header line. '{line}'");
				}
			}
			else
			{
				if (SbemObject.IsCloseLine(line))
				{
					inObject = false;
					var objectType = currentType;
					/*
					 * There's not really a clean way to do this with C# because it's strict typed
					 * and it doesn't have Late Static Binding. So, we add an OBJECT_NAME and Object()
					 * method to each class, even though it's not in the abstract SbemObject.
					 */ 
					switch (objectType)
					{
						case SbemCompliance.OBJECT_NAME:
							model.Compliance = new SbemCompliance(currentName, currentProperties);
							break;
						case SbemConstruction.OBJECT_NAME:
							model.Constructions.Add(new SbemConstruction(currentName, currentProperties));
							break;
						case SbemDhwGenerator.OBJECT_NAME:
							model.Dhws.Add(new SbemDhwGenerator(currentName, currentProperties));
							break;
						case SbemDoor.OBJECT_NAME:
							currentWall.Doors.Add(new SbemDoor(currentName, currentProperties));
							break;
						case SbemDoorTmBridge.OBJECT_NAME:
							currentDoor.SetThermalBridge(new SbemDoorTmBridge(currentName, currentProperties));
							break;
						case SbemGeneral.OBJECT_NAME:
							model.General = new SbemGeneral(currentName, currentProperties);
							break;
						case SbemGlass.OBJECT_NAME:
							model.Glasses.Add(new SbemGlass(currentName, currentProperties));
							break;
						case SbemHvacSystem.OBJECT_NAME:
							currentHvac = new SbemHvacSystem(currentName, currentProperties);
							model.HvacSystems.Add(currentHvac);
							break;
						case SbemImprovementMeasure.OBJECT_NAME:
							model.ImprovementMeasures.Add(new SbemImprovementMeasure(currentName, currentProperties));
							break;
						case SbemPvs.OBJECT_NAME:
							model.Pvs = new SbemPvs(currentName, currentProperties);
							break;
						case SbemRecProject.OBJECT_NAME:
							model.RecProject = new SbemRecProject(currentName, currentProperties);
							break;
						case SbemRecUser.OBJECT_NAME:
							model.RecUsers.Add(new SbemRecUser(currentName, currentProperties));
							break;
						case SbemSes.OBJECT_NAME:
							model.Ses = new SbemSes(currentName, currentProperties);
							break;
						case SbemShower.OBJECT_NAME:
							model.Showers.Add(new SbemShower(currentName, currentProperties));
							break;
						case SbemWall.OBJECT_NAME:
							currentWall = new SbemWall(currentName, currentProperties);
							string f = currentWall.GetStringProperty("CONSTRUCTION").QuotelessValue;
							currentWall.SetConstruction(model.Constructions[f]);
							currentZone.Walls.Add(currentWall);
							break;
						case SbemWindGenerator.OBJECT_NAME:
							model.WindGenerator = new SbemWindGenerator(currentName, currentProperties);
							break;
						case SbemWindow.OBJECT_NAME:
							currentWindow = new SbemWindow(currentName, currentProperties);
							currentWindow.SetGlass(model.Glasses[currentWindow.GetStringProperty("GLASS").QuotelessValue]);
							currentWall.Windows.Add(currentWindow);
							break;
						case SbemWindowTmBridge.OBJECT_NAME:
							currentWindow.SetThermalBridge(new SbemWindowTmBridge(currentName, currentProperties));
							break;
						case SbemZone.OBJECT_NAME:
							currentZone = new SbemZone(currentName, currentProperties);
							currentHvac.AddZone(currentZone);
							model.Zones.Add(currentZone);
							break;
					}
				}
				else
					currentProperties.Add(line);
			}
		}
		/*
		 * Do object relations
		 */
		for (int zoneID = 0; zoneID < model.Zones.Length; zoneID++)
		{
			SbemZone zone = model.Zones[zoneID];
			// Do DHW
			zone.SetDhwGenerator(model.Dhws[zone.GetStringProperty("DHW-GENERATOR").QuotelessValue]);
			// Do relationships for walls, doors, and windows.
			for (int wallID = 0; wallID < zone.Walls.Length; wallID++)
			{
				SbemWall wall = zone.Walls[wallID];
				model.Constructions[wall.GetStringProperty("CONSTRUCTION").QuotelessValue].AddWall(wall);
				// Do SbemDoor
				for (int doorID = 0; doorID < wall.Doors.Length; doorID++)
				{
					string constructionName = wall.Doors[doorID].GetStringProperty("CONSTRUCTION").QuotelessValue;
					model.Constructions[constructionName].AddDoor(wall.Doors[doorID]);
					wall.SetConstruction(model.Constructions[constructionName]);
				}
				// Do SbemWindow
				for (int windowID = 0; windowID < wall.Windows.Length; windowID++)
				{
					string glassName = wall.Windows[windowID].GetStringProperty("GLASS").QuotelessValue;
					model.Glasses[glassName].AddWindow(wall.Windows[windowID]);
					wall.Windows[windowID].SetGlass(model.Glasses[glassName]);
				}
			}
		}
		return model;
	}
	/// <summary>
	/// Get the calendar difference SbemModel. The difference model is the base model
	/// with its internal heat production, End Use, Fuel Type, and renewables energy
	/// calendar set to the difference between the existing values and those in the 
	/// calendars' counterparts another model. For example, you might retrofit the 
	/// lighting then get the difference model to see how it affected End Use.
	/// </summary>
	/// <param name="inputModel"></param>
	/// <returns></returns>
	public SbemModel GetDifferenceModel(SbemModel inputModel)
	{
		SbemModel outputModel = Clone();
		outputModel.EndUseConsumerCalendar.Subtract(inputModel.EndUseConsumerCalendar);
		for(int hvacID = 0; hvacID < HvacSystems.Length; hvacID++)
		{
			SbemHvacSystem hvacSystem	= outputModel.HvacSystems[hvacID];
			SbemHvacSystem otherHvac	= inputModel.HvacSystems[hvacSystem.Name];
			hvacSystem.FuelUseConsumerCalendar.Subtract(hvacSystem.FuelUseConsumerCalendar);
			hvacSystem.EndUseConsumerCalendar.Subtract(hvacSystem.EndUseConsumerCalendar);
			for(int zoneID = 0; zoneID < hvacSystem.Zones.Length; zoneID++)
			{
				SbemZone zone		= outputModel.Zones[zoneID];
				SbemZone otherZone	= inputModel.Zones[zone.Name];
				zone.HeatingEnergyDemandCalendar.Subtract(otherZone.HeatingEnergyDemandCalendar);
				zone.CoolingEnergyDemandCalendar.Subtract(otherZone.CoolingEnergyDemandCalendar);
				zone.InternalHeatGainsCalendar.Subtract(otherZone.InternalHeatGainsCalendar);
			}
		}
		return outputModel;
	}
	public void PairWithSimResult(SimResult simResult)
	{
		EndUseConsumerCalendar	= simResult.ConsumerCalendar;
		FuelUseConsumerCalendar = simResult.FuelCalendar;
		// Do HVACs
		for(int hvacID = 0; hvacID < HvacSystems.Length; hvacID++)
		{
			SbemHvacSystem hvac	= HvacSystems[hvacID];
			// HVAC calendars
			if(simResult.HvacConsumerCalendars.ContainsKey(hvac.Name))
				hvac.SetEndUseConsumerCalendar(simResult.HvacConsumerCalendars[hvac.Name]);
			if(simResult.HvacFuelCalendars.ContainsKey(hvac.Name))
				hvac.SetFuelTypConsumerCalendar(simResult.HvacFuelCalendars[hvac.Name]);
			// Do zones
			for(int zoneID = 0; zoneID < hvac.Zones.Length; zoneID++)
			{
				SbemZone zone	= hvac.Zones[zoneID];
				if(simResult.ZoneInternalGainsCalendars.ContainsKey(zone.Name))
					zone.SetInternalGainsCalendar(simResult.ZoneInternalGainsCalendars[zone.Name]);
				if (simResult.ZoneHeatingDemandCalendars.ContainsKey(zone.Name))
					zone.SetHeatingEnergyDemandCalendar(simResult.ZoneHeatingDemandCalendars[zone.Name]);
				if(simResult.ZoneCoolingDemandCalendars.ContainsKey(zone.Name))
					zone.SetCoolingEnergyDemandCalendar(simResult.ZoneCoolingDemandCalendars[zone.Name]);
			}
		}
	}
	public override string ToString()
    {
        var content = new StringBuilder();
        content.AppendLine(General.ToString());
        content.AppendLine(Compliance.ToString());

        foreach (var obj in Constructions.Objects)
            content.AppendLine(obj.ToString());

        foreach (var obj in Glasses.Objects)
            content.AppendLine(obj.ToString());

        if (HasSes) content.AppendLine(Ses.ToString());
        if (HasPvs) content.AppendLine(Pvs.ToString());
        if (HasWindGenerator) content.AppendLine(WindGenerator.ToString());

        foreach (var obj in Showers.Objects)
            content.AppendLine(obj.ToString());
        foreach (var obj in Dhws.Objects)
            content.AppendLine(obj.ToString());
        foreach (var obj in HvacSystems.Objects)
            content.AppendLine(obj.ToString());
        foreach (var obj in RecUsers.Objects)
            content.AppendLine(obj.ToString());
        foreach (var obj in ImprovementMeasures.Objects)
            content.AppendLine(obj.ToString());
        return content.ToString();
    }
	/// <summary>
	/// Clone this SbemModel
	/// </summary>
	/// <returns></returns>
	public SbemModel Clone()
	{
		// Make sure we don't mess with the as-built model
		SbemModel clone	= ParseInpContent(ToString());
		// Do project-level End Use and Fuel Type calendars
		if (EndUseConsumerCalendar != null)
			clone.EndUseConsumerCalendar = EndUseConsumerCalendar.Clone();
		if (FuelUseConsumerCalendar != null)
			clone.FuelUseConsumerCalendar = FuelUseConsumerCalendar.Clone();

		// Do all HVACS
		for(int hvacID = 0; hvacID < HvacSystems.Length; hvacID++)
		{
			SbemHvacSystem hvacSystem	= HvacSystems[hvacID];
			// Do HVAC End Use and Fuel Type calendars
			if (hvacSystem.EndUseConsumerCalendar != null)
				clone.HvacSystems[hvacID].SetEndUseConsumerCalendar(hvacSystem.EndUseConsumerCalendar.Clone());
			if (hvacSystem.FuelUseConsumerCalendar != null)
				clone.HvacSystems[hvacID].SetFuelTypConsumerCalendar(hvacSystem.FuelUseConsumerCalendar.Clone());
		}
		// Do all Zones
		// NOTE: SbemModel has an SbemObjectSet<SbemZone> for convenience which is why we 
		// can do this outside the HVAC loop.
		for (int zoneID = 0; zoneID < Zones.Length; zoneID++)
		{
			SbemZone zone = Zones[zoneID];
			// Demand calendars
			if (zone.HeatingEnergyDemandCalendar != null)
				clone.Zones[zoneID].SetHeatingEnergyDemandCalendar(zone.HeatingEnergyDemandCalendar.Clone());
			if (zone.CoolingEnergyDemandCalendar != null)
				clone.Zones[zoneID].SetCoolingEnergyDemandCalendar(zone.CoolingEnergyDemandCalendar.Clone());
			// Internal heat production calendar
			if (zone.InternalHeatGainsCalendar != null)
				clone.Zones[zoneID].SetInternalGainsCalendar(zone.InternalHeatGainsCalendar.Clone());
			// End Use and Fuel Type calendars
			if (zone.EndUseConsumerCalendar!= null)
				clone.Zones[zoneID].SetEndUseConsumerCalendar(zone.EndUseConsumerCalendar.Clone());
			if (zone.FuelUseConsumerCalendar!= null)
				clone.Zones[zoneID].SetFuelUseConsumerCalendar(zone.FuelUseConsumerCalendar.Clone());
		}
		return clone;
	}
	public void PrintHvacSystems(bool withZones)
	{ 
		List<List<string>> table = new List<List<string>>();
		table.Add(new List<string>() { "Type", "Heat source", "Area (m²)", "SEFF", "SSEFF", "C-SEER", "C-SSEER", "SFP (W/m²)" });
		foreach (SbemHvacSystem hvac in HvacSystems)
		{
			table.Add(new List<string>()
				{
					hvac.GetStringProperty("TYPE").Value,
					(hvac.HasStringProperty("HEAT-SOURCE") ?  hvac.GetStringProperty("HEAT-SOURCE").Value : "N/A"),
					hvac.Area.ToString(),
					(hvac.HasNumericProperty("HEAT-GEN-SEFF") ? hvac.NumericProperties["HEAT-GEN-SEFF"].Value.ToString() : "NA"),
					(hvac.HasNumericProperty("HEAT-SSEFF") ? hvac.NumericProperties["HEAT-SSEFF"].Value.ToString() : "NA"),
					(hvac.HasNumericProperty("COOL-GEN-SEER") ? hvac.NumericProperties["COOL-GEN-SEER"].Value.ToString() : "NA"),
					(hvac.HasNumericProperty("COOL-SSEER") ? hvac.NumericProperties["COOL-SSEER"].Value.ToString() : "NA"),
					(hvac.HasNumericProperty("SFP") ? hvac.NumericProperties["SFP"].Value.ToString() : "NA"),
				});
			table.Add(new List<string>() {  });

		}
		PHelper.PrintTable(table);
	}
	public void DropRecUsers() => RecUsers.Clear();
