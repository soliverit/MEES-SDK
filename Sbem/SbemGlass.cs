


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// The .inp  transparent surfaces construction properties. U-Value (W/m²K), visible light
	/// and total solar transmittance
	/// <code>Relationships:
	/// - Has many SbemWindow
	/// </code>
	//"Default glazing" = GLASS
	//	U-VALUE-CORR = NO
	//	U-VALUE = 5.279
	//	TOT-SOL-TRANS = 0.858
	//	LIG-SOL-TRANS = 0.898
	//	..
	public class SbemGlass : SbemSpatialObject
	{
		public const string OBJECT_NAME  = "GLASS"; 
		public override string ObjectName() { return OBJECT_NAME; }
		public SbemGlass(string currentName, List<string> currentProperties) : base(currentName, currentProperties)	{ }
		public SbemObjectSet<SbemWindow> Windows { get; }	= new();
		public void AddWindow(SbemWindow window)
		{
			Windows.Add(window);
			Area += window.Area;
		}
	}
}
