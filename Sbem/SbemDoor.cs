


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public class SbemDoor : SbemSpatialObject
	{
		public const string OBJECT_NAME  = "DOOR"; 
		public override string ObjectName() { return OBJECT_NAME; }
		public SbemDoorTmBridge TmBridge;
		public SbemDoor(string currentName, List<string> currentProperties) : base(currentName, currentProperties)
		{

		}
		public void SetThermalBridge(SbemDoorTmBridge tmBridge)
		{
			TmBridge = tmBridge;
		}
	}
}
