


using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public class SbemZone : SbemSpatialObject
	{
		public const string OBJECT_NAME = "ZONE";
		public override string ObjectName() { return OBJECT_NAME; }
		public SbemObjectSet<SbemWall> Walls = new SbemObjectSet<SbemWall>();
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
		public InternalGainsCalendar InternalHeatGainsCalendar { get; protected set; }
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
			InternalHeatGainsCalendar	= internalGainsCalendar;
		}
		public void SetEndUseConsumerCalendar(ConsumerConsumptionCalendar consumerCalendar)
		{
			EndUseConsumerCalendar = consumerCalendar;
		}
		public void SetFuelUseConsumerCalendar(FuelConsumptionCalendar fuelConsumptionCalendar)
		{
			FuelUseConsumerCalendar = fuelConsumptionCalendar;
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
