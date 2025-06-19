


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public class SbemWindow : SbemSpatialObject
	{
		public const string OBJECT_NAME  = "WINDOW"; 
		public override string ObjectName() { return OBJECT_NAME; }
		public SbemGlass Glass { get; protected set; }
		public SbemWindowTmBridge TMBridge { get; protected set; }
		public SbemWindow(string currentName, List<string> currentProperties) : base(currentName, currentProperties)
		{

		}
		public void SetGlass(SbemGlass glass)
		{
			Glass = glass;
		}
		public void SetThermalBridge(SbemWindowTmBridge thermalBridge)
		{
			TMBridge = thermalBridge;
		}
	}
}
