


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// The .inp wall definition. Defines geometry, configurations and relationships.
	/// <code>Relationships
	/// - Has many SbemDoor
	/// - Has many SbemWindow
	/// - Has one SbemConstruction</code>
	/// </summary>
	//"Z0/01/si" = WALL
	//	ORIENTATION = South
	//	TYPE-ENV = Wall
	//	MULTIPLIER = 1
	//	LENGTH = 6.6
	//	AREA = 25.74
	//	CONSTRUCTION = "Int"
	//	PITCH = 90
	//	TYPE = Conditioned adjoining space
	public class SbemWall : SbemSpatialObject
	{
		/// <summary>
		/// C# doesn't have late static binding so we need to add this to all SbemObject.
		/// </summary>
		public const string OBJECT_NAME		= "WALL";
		/// <summary>
		/// SbemWall TYPE property for external walls
		/// </summary>
		public const string EXTERIOR_WALL	= "Exterior";
		/// <summary>
		/// SbemWall TYPE-ENV for roofs
		/// </summary>
		public const string ROOF			= "Roof";
		/// <summary>
		/// Sbemwall TYPE-ENV for walls
		/// </summary>
		public const string WALL			= "Wall";
		/// <summary>
		/// C# doesn't have late static binding so we need to add this to all SbemObject.
		/// </summary>
		public override string ObjectName() { return OBJECT_NAME; }
		/// <summary>
		/// The windows embedded in this wall
		/// </summary>
		public readonly SbemObjectSet<SbemWindow> Windows	= new SbemObjectSet<SbemWindow>();
		/// <summary>
		/// The doors embedded in this wall
		/// </summary>
		public readonly SbemObjectSet<SbemDoor> Doors		= new SbemObjectSet<SbemDoor>();
		
		private float _area									= 0f;
		private float _windowArea							= 0f;
		private float _doorArea								= 0f;
		private bool _gotArea								= false;
		public override float Area { get
			{
				if (!_gotArea)
					return GetArea();
				return _area;
			}
			protected set { _area = value; }
		}
		public SbemConstruction Construction { get; protected set; }
		public SbemWall(string currentName, List<string> currentProperties) : base(currentName, currentProperties) {}
		public void SetConstruction(SbemConstruction construction)
		{
			Construction = construction;
		}
		/// <summary>
		/// Get the wall Area including glazing and doors
		/// </summary>
		/// <returns></returns>
		public float GetArea()
		{
			if (_gotArea)
				return _area;

			// Accumulate window area
			for (int i = 0; i < Windows.Objects.Count; i++)
				_windowArea += Windows.Objects[i].Area;
			for (int i = 0; i < Doors.Objects.Count; i++)
				_windowArea += Windows.Objects[i].Area;
			float baseArea = GetNumericProperty("AREA").Value;

			if (HasNumericProperty("MULTIPLIER"))
			{
				float multiplier = GetNumericProperty("MULTIPLIER").Value;
				_area = baseArea * multiplier;
			}
			else
			{
				_area = baseArea;
			}

			_gotArea = true;
			return _area;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public float SurfaceArea { get => Area - _windowArea;}
		public string Type { get { return StringProperties["TYPE"].Value; } }

		public string TypeEnvironment { get { return StringProperties["TYPE-ENV"].Value; } }
		/// <summary>
		/// Is this an external wall? Normal or heavy ventilation
		/// </summary>
		public bool IsExternalWall { get { return Type == EXTERIOR_WALL && TypeEnvironment == WALL; } }
		/// <summary>
		/// Is this a roof?
		/// </summary>
		public bool IsRoof { get { return TypeEnvironment == ROOF; } }
		/// <summary>
		/// Get the total glazing area
		/// </summary>
		/// <returns></returns>
		public float WindowArea()
		{
			GetArea(); // ensure it's calculated
			return _windowArea;
		}
		/// <summary>
		/// Convert to SBEM-OBJECT formatted string
		/// </summary>
		/// <returns></returns>
		public string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(base.ToString());
			for(int windowID = 0; windowID < Windows.Length; windowID++) 
				sb.AppendLine(Windows[windowID].ToString());
			for(int doorID = 0; doorID < Doors.Length; doorID++)
				sb.AppendLine(Doors[doorID].ToString());
			return sb.ToString();
		}
		
	}
}
