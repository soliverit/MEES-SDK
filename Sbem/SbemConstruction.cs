


using MeesSDK.Sbem.Exceptions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// The .inp  opaque surfaces construction properties. U-Value (W/m²K), internal 
	/// heat capacity kJ/m²K, Has metal cladding?, Has U-Value correction.
	/// <code>Relationships:
	/// - Has many SbemWall
	/// - Has many SbemDoor
	/// </code>
	/// </summary>
	//"Default construction for walls" = CONSTRUCTION
	//	TYPE = U_VALUE
	//	U-VALUE-CORR = NO
	//	CM = { 51, 51 }
	//	METAL-CLADDING = NO
	//	U-VALUE = 0.32
	// ..
	public class SbemConstruction : SbemSpatialObject
	{
		public const string NORTH = "North";
		public const string NORTH_EAST = "North-East";
		public const string EAST = "East";
		public const string SOUTH_EAST = "South-East";
		public const string SOUTH = "South";
		public const string SOUTH_WEST = "South-West";
		public const string WEST = "West";
		public const string NORTH_WEST = "North-West";	
		public const string OBJECT_NAME  = "CONSTRUCTION"; 
		public override string ObjectName() { return OBJECT_NAME; }
		public SbemObjectSet<SbemWall> Walls { get; }	= new();
		public SbemObjectSet<SbemDoor> Doors { get; } = new();
		protected float _northArea { get; set; }
		protected float _northEastArea { get; set; }
		protected float _eastArea { get; set; }
		protected float _southEastArea { get; set; }
		protected float _southArea { get; set; }
		protected float _southWestArea { get; set; }
		protected float _westArea { get; set; }
		protected float _northWestArea { get; set; }
		/// <summary>
		/// Total north-facing surface area including glass
		/// </summary>
		public float NorthArea
		{
			get => _northArea;
			protected set => _northArea = value;
		}
		/// <summary>
		/// Total north-east facing surface area including glass
		/// </summary>
		public float NorthEastArea
		{
			get => _northEastArea;
			protected set => _northEastArea = value;
		}
		/// <summary>
		/// Total east facing surface area including glass
		/// </summary>
		public float EastArea
		{
			get => _eastArea;
			protected set => _eastArea = value;
		}
		/// <summary>
		/// Total south-east facing surface area including glass
		/// </summary>
		public float SouthEastArea
		{
			get => _southEastArea;
			protected set => _southEastArea = value;
		}
		/// <summary>
		/// Total south facing surface area including glass
		/// </summary>
		public float SouthArea
		{
			get => _southArea;
			protected set => _southArea = value;
		}
		/// <summary>
		/// Total south-west facing surface area including glass
		/// </summary>
		public float SouthWestArea
		{
			get => _southWestArea;
			protected set => _southWestArea = value;
		}
		/// <summary>
		/// Total west facing surface area including glass
		/// </summary>
		public float WestArea
		{
			get => _westArea;
			protected set => _westArea = value;
		}
		/// <summary>
		/// Total north-west facing surface area including glass
		/// </summary>
		public float NorthWestArea
		{
			get => _northWestArea;
			protected set => _northWestArea = value;
		}
		protected float _northSurfaceArea { get; set; }
		protected float _northEastSurfaceArea { get; set; }
		protected float _eastSurfaceArea { get; set; }
		protected float _southEastSurfaceArea { get; set; }
		protected float _southSurfaceArea { get; set; }
		protected float _southWestSurfaceArea { get; set; }
		protected float _westSurfaceArea { get; set; }
		protected float _northWestSurfaceArea { get; set; }
		/// <summary>
		/// Total north-facing surface area excluding glass
		/// </summary>
		public float NorthSurfaceArea
		{
			get => _northSurfaceArea;
			protected set => _northSurfaceArea = value;
		}
		/// <summary>
		/// Total north-east facing surface area excluding glass
		/// </summary>
		public float NorthEastSurfaceArea
		{
			get => _northEastSurfaceArea;
			protected set => _northEastSurfaceArea = value;
		}
		/// <summary>
		/// Total east-facing surface area excluding glass
		/// </summary>
		public float EastSurfaceArea
		{
			get => _eastSurfaceArea;
			protected set => _eastSurfaceArea = value;
		}
		/// <summary>
		/// Total south-east facing surface area excluding glass
		/// </summary>
		public float SouthEastSurfaceArea
		{
			get => _southEastSurfaceArea;
			protected set => _southEastSurfaceArea = value;
		}
		/// <summary>
		/// Total south-facing surface area excluding glass
		/// </summary>
		public float SouthSurfaceArea
		{
			get => _southSurfaceArea;
			protected set => _southSurfaceArea = value;
		}
		/// <summary>
		/// Total south-west facing surface area excluding glass
		/// </summary>
		public float SouthWestSurfaceArea
		{
			get => _southWestSurfaceArea;
			protected set => _southWestSurfaceArea = value;
		}
		/// <summary>
		/// Total west-facing surface area excluding glass
		/// </summary>
		public float WestSurfaceArea
		{
			get => _westSurfaceArea;
			protected set => _westSurfaceArea = value;
		}
		/// <summary>
		/// Total north-west facing surface area excluding glass
		/// </summary>
		public float NorthWestSurfaceArea
		{
			get => _northWestSurfaceArea;
			protected set => _northWestSurfaceArea = value;
		}
		/// <summary>
		/// Does this construction have any mapped SbemWall associated with it?
		/// </summary>
		public bool HasWalls { get => Walls.Length > 0; }
		/// <summary>
		/// Does this construction have any mapped SbemDoor associated with it?
		/// <para>Note: There should never be an instance where both SbemDoor and SbemWall are associated with an SbemConstruction</para>
		/// </summary>
		public bool HasDoors { get => Doors.Length > 0; }
		/// <summary>
		/// Does this construction have any mapped associated surfaces?
		/// <para>Note: There should never be an instance where both SbemDoor and SbemWall Kare associated with an SbemConstruction</para>
		/// </summary>
		public bool HasAnySurfaces { get => HasDoors || HasWalls; }
		public SbemConstruction(string currentName, List<string> currentProperties) : base(currentName, currentProperties){}
		public bool IsRoof
		{
			get
			{
				// We can't tell if there's no walls to check
				if (Walls.Length == 0)
					return false;
				// We could test for mixed surface exceptions, but no really the place for it
				return Walls[0].IsRoof;
			}
		}
		/// <summary>
		/// Does this construction represent an external wall?
		/// </summary>
		public bool IsExternalWall { get
		{
			// We can't tell if there's no walls to check
			if (Walls.Length == 0)
				return false;
			// We could test for mixed surface exceptions, but no really the place for it
			return Walls[0].IsExternalWall;
		}}
		/// <summary>
		/// Associate an SbemWall with this SbemConstruction
		/// </summary>
		/// <param name="wall"></param>
		public void AddWall(SbemWall wall)
		{
			Walls.Add(wall);
			Area		+= wall.Area;
			SurfaceArea += wall.SurfaceArea;
			if (wall.PropertyEquals("ORIENTATION", NORTH))
			{
				NorthArea += wall.Area;
				NorthSurfaceArea += wall.SurfaceArea;
			}
			else if (wall.PropertyEquals("ORIENTATION", NORTH_EAST))
			{
				NorthEastArea += wall.Area;
				NorthEastSurfaceArea += wall.SurfaceArea;
			}
			else if (wall.PropertyEquals("ORIENTATION", EAST))
			{
				EastArea += wall.Area;
				EastSurfaceArea += wall.SurfaceArea;
			}
			else if (wall.PropertyEquals("ORIENTATION", SOUTH_EAST))
			{
				SouthEastArea += wall.Area;
				SouthEastSurfaceArea += wall.SurfaceArea;
			}
			else if (wall.PropertyEquals("ORIENTATION", SOUTH))
			{
				SouthArea += wall.Area;
				SouthSurfaceArea += wall.SurfaceArea;
			}
			else if (wall.PropertyEquals("ORIENTATION", SOUTH_WEST))
			{
				SouthWestArea += wall.Area;
				SouthWestSurfaceArea += wall.SurfaceArea;
			}
			else if (wall.PropertyEquals("ORIENTATION", WEST))
			{
				WestArea += wall.Area;
				WestSurfaceArea += wall.SurfaceArea;
			}
			else if (wall.PropertyEquals("ORIENTATION", NORTH_WEST))
			{
				NorthWestArea += wall.Area;
				NorthWestSurfaceArea += wall.SurfaceArea;
			}
		}

		/// <summary>
		/// Associate an SbemDoor with this SbemConstruction
		/// </summary>
		/// <param name="door"></param>
		public void AddDoor(SbemDoor door)
		{
			Doors.Add(door);
			Area	= door.Area;
		}
	}
}
