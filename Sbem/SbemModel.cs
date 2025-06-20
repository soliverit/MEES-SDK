using MeesSDK.Sbem;
using MeesSDK.Sbem.ConsumerCalendar;
using System.Text;
public class SbemModel : SbemModelBase
{
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

	public List<SbemError> Errors = new List<SbemError>();
	public SbemGeneral General { get; set; }
    public SbemCompliance Compliance { get; set; }
    public SbemObjectSet<SbemConstruction> Constructions { get; } = new();
    public SbemObjectSet<SbemGlass> Glasses { get; } = new();
    public SbemObjectSet<SbemHvacSystem> HvacSystems { get; } = new();
    public SbemObjectSet<SbemDhwGenerator> Dhws { get; } = new();
    public SbemObjectSet<SbemShower> Showers { get; } = new();
    public SbemObjectSet<SbemRecUser> RecUsers { get; } = new();
    public SbemObjectSet<SbemImprovementMeasure> ImprovementMeasures { get; } = new();
	public ConsumerConsumptionCalendar EndUseConsumerCalendar { get; protected set; }
	public FuelConsumptionCalendar FuelUseConsumerCalendar { get; protected set; }
	public SbemObjectSet<SbemZone> Zones { get; } = new();
	public SimResult? PairedSimResult { get; protected set; }
    public SbemPvs Pvs { get; set; }
	public bool hasPvs = false;
    public SbemSes Ses { get; set; }
	protected bool hasSes = false;
    public SbemWindGenerator WindGenerator { get; set; }
    public SbemRecProject RecProject { get; set; }

    public bool HasSes { get; set; } = false;
    public bool HasPvs { get; set; } = false;
    public bool HasWindGenerator { get; set; } = false;

	private List<SbemError> errors = new();

	public static SbemModel CreateBasicModel()
	{
		return ParseInpContent(MIN_INP);
	}
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
							model.HasPvs = true;
							model.Pvs = new SbemPvs(currentName, currentProperties);
							break;
						case SbemRecProject.OBJECT_NAME:
							model.RecProject = new SbemRecProject(currentName, currentProperties);
							break;
						case SbemRecUser.OBJECT_NAME:
							model.RecUsers.Add(new SbemRecUser(currentName, currentProperties));
							break;
						case SbemSes.OBJECT_NAME:
							model.HasSes = true;
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
							model.HasWindGenerator = true;
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
				{
					currentProperties.Add(line);
				}
			}
		}
		/*
		 * Do object relations
		 */
		for (int zoneID = 0; zoneID < model.Zones.Length; zoneID++)
		{
			SbemZone zone = model.Zones[zoneID];
			for (int wallID = 0; wallID < zone.Walls.Length; wallID++)
			{
				SbemWall wall = zone.Walls[wallID];
				model.Constructions[wall.GetStringProperty("CONSTRUCTION").QuotelessValue].AddWall(wall);
				for (int doorID = 0; doorID < wall.Doors.Length; doorID++)
					model.Constructions[wall.Doors[doorID].GetStringProperty("CONSTRUCTION").QuotelessValue].AddDoor(wall.Doors[doorID]);
				for (int windowID = 0; windowID < wall.Windows.Length; windowID++)
					model.Glasses[wall.Windows[windowID].GetStringProperty("GLASS").QuotelessValue].AddWindow(wall.Windows[windowID]);

			}
		}
		return model;
	}
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
} // Additional methods (like area calculations) can follow
