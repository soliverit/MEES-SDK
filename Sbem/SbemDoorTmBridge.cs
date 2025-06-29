﻿


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// Thermal bridge information for parent SbemDoor objects. Never seen one with
	/// any properties.
	/// </summary>
	public class SbemDoorTmBridge : SbemObject
	{
		public const string OBJECT_NAME  = "DOOR-TM-BRIDGE"; 
		public override string ObjectName() { return OBJECT_NAME; }

		public SbemDoorTmBridge(string currentName, List<string> currentProperties) : base(currentName, currentProperties)
		{
			
		}
	}
}
