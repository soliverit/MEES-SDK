


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public class SbemConstruction : SbemSpatialObject
	{
		public const string OBJECT_NAME  = "CONSTRUCTION"; 
		public override string ObjectName() { return OBJECT_NAME; }
		public SbemObjectSet<SbemWall> Walls { get; }	= new();
		public SbemObjectSet<SbemDoor> Doors { get; } = new();
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
		/// <para>Note: There should never be an instance where both SbemDoor and SbemWall are associated with an SbemConstruction</para>
		/// </summary>
		public bool HasAnySurfaces { get => HasDoors || HasWalls; }
		public SbemConstruction(string currentName, List<string> currentProperties) : base(currentName, currentProperties)
		{

		}
		/// <summary>
		/// Associate an SbemWall with this SbemConstruction
		/// </summary>
		/// <param name="wall"></param>
		public void AddWall(SbemWall wall)
		{
			Walls.Add(wall);
			Area	= wall.Area;
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
