


using MathNet.Numerics;
using MeesSDK.Sbem.ConsumerCalendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// The .inp activity space definition. These were the model area comes from. The 
	/// NCM database activity defines hot water demand, fresh air rates, occupancy patterns, and plug and
	/// small load consumption. It also defines th Part L checks that are necessary for the 
	/// activity.
	/// /// </summary>
	//"Z0/01" = ZONE
	//	JNCT-JAMB-MC = 1.27
	//	HEAT-REC-SEFF = 0
	//	F-CTRL-VENT-DEM = No demand-controlled ventilation
	//	JNCT-SILL = 0.04
	//	JNCT-WALL-GRND-MC = 1.15
	//	Q50-INF = 25
	//	ACTIVITY = 1071
	//	JNCT-ROOF-WALL = 0.12
	//	LIGHT-CONTROL = MANUAL
	//	HEPA-FILTER = NO
	//	HEAT-REC-VAR-EFF = NO
	//	JNCT-JAMB = 0.05
	//	JNCT-WALL-WALL-MC = 0.25
	//	DHW-PIPE-LEN = 0
	//	JNCT-WALL-GRND = 0.16
	//	LIGHT-DL-AUT-ZONE = YES
	//	VENT-MECH-EXH = NO
	//	JNCT-ACCR-DETAIL = { 0, 0, 0, 0, 0, 0, 0, 0, 0 }
	//	JNCT-WALL-FLR-MC = 0
	//	LIGHT-CASE = UNKNOWN
	//	JNCT-WALL-WALL = 0.09
	//	LIGHT-OCC-SENS-T = NONE
	//	DESTRAT-FAN = NO
	//	MULTIPLIER = 1
	//	JNCT-ACCR-DETAIL-MC = { 0, 0, 0, 0, 0, 0, 0, 0, 0 }
	//	JNCT-LINTEL-MC = 1.27
	//	LIGHT-TYPE = Fluorescent - compact
	//	VENT-ZONE-HVAC = ZONE
	//	ISBEM-ID = 11
	//	JNCT-WALL-FLR = 0.07
	//	DHW-GENERATOR = "from combi"
	//	VENT-SFP = 1.5
	//	AREA = 102.36
	//	NIGHT-COOLING = NO
	//	JNCT-SILL-MC = 1.27
	//	LIGHT-DISPLAY-EFF = NO
	//	HEAT-REC-SYSTEM = No heat recovery
	//	Q-V-TYPE = Natural Supply
	//	JNCT-LINTEL = 0.3
	//	JNCT-ROOF-WALL-MC = 0.28
	//	SFP-TU = 0.8
	//	HEIGHT = 3.9
	//	ACTIVITY-NAME = Small Shop Unit Sales area - general
	//	..

	public class SbemZone : SbemSpatialObject
	{
		public const string OBJECT_NAME = "ZONE";
		public override string ObjectName() { return OBJECT_NAME; }
		public SbemObjectSet<SbemWall> Walls = new SbemObjectSet<SbemWall>();
		public SbemDhwGenerator DhwGenerator { get; protected set; }
		/// <summary>
		/// Energy consumption by End Use calendar. 
		/// <para>Note: Zone calendars don't exist, so we have to do a bit of legwork by creating 
		/// SbemModel with only one SbemHvacSystem and one SbemZone.</para>
		/// <para>See SbemProject.CalculateZonalEnergyDemand()</para>
		/// </summary>
		public ConsumerConsumptionCalendar EndUseConsumerCalendar { get; protected set; }
		public FuelConsumptionCalendar FuelUseConsumerCalendar { get; protected set; }
		public SbemZone(string currentName, List<string> currentProperties) : base(currentName, currentProperties)
		{

		}
		public InternalGainsCalendar InternalHeatProductionCalendar { get; protected set; }
		public HeatingDemandCalendar HeatingEnergyDemandCalendar { get; protected set; }
		public CoolingDemandCalendar CoolingEnergyDemandCalendar { get; protected set; }
		private float _wallArea = 0f;
		private float _wallSurfaceArea = 0f;
		private float _windowArea = 0f;
		private bool _gotWallArea = false;
		public void SetCoolingEnergyDemandCalendar(CoolingDemandCalendar coolingCalendar)
		{
			CoolingEnergyDemandCalendar	= coolingCalendar;
		}
		public void SetHeatingEnergyDemandCalendar(HeatingDemandCalendar heatingCalendar)
		{
			HeatingEnergyDemandCalendar = heatingCalendar;
		}
		public void SetInternalGainsCalendar(InternalGainsCalendar internalGainsCalendar)
		{
			InternalHeatProductionCalendar	= internalGainsCalendar;
		}
		public void SetEndUseConsumerCalendar(ConsumerConsumptionCalendar consumerCalendar)
		{
			EndUseConsumerCalendar = consumerCalendar;
		}
		public void SetFuelUseConsumerCalendar(FuelConsumptionCalendar fuelConsumptionCalendar)
		{
			FuelUseConsumerCalendar = fuelConsumptionCalendar;
		}
		public void SetDhwGenerator(SbemDhwGenerator dhw)
		{
			DhwGenerator = dhw;
		}
		public float WallArea()
		{
			if (_gotWallArea)
				return _wallArea;

			for (int wallID = 0; wallID < Walls.Objects.Count; wallID++)
			{
				var wall = Walls.Objects[wallID];
				_wallArea += wall.Area;
				_wallSurfaceArea += wall.SurfaceArea();
				_windowArea += wall.WindowArea();
			}

			_gotWallArea = true;
			return _wallArea;
		}

		public float WallSurfaceArea()
		{
			WallArea(); // triggers calculation if needed
			return _wallSurfaceArea;
		}

		public float WindowArea()
		{
			WallArea(); // triggers calculation if needed
			return _windowArea;
		}
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(base.ToString());
			for (int wallID = 0; wallID < Walls.Length; wallID++)
				sb.AppendLine(Walls[wallID].ToString());
			return sb.ToString();
		}
	}
}
