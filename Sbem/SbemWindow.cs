


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// The .inp window definition.
	/// <code>Relationships
	/// - Has one SbemWindowTmBridge</code>
	/// </summary>
	public class SbemWindow : SbemSpatialObject
	{
		/// <summary>
		/// C# doesn't have late static binding so we need to add this to all SbemObject.
		/// </summary>
		public const string OBJECT_NAME  = "WINDOW";
		/// <summary>
		/// C# doesn't have late static binding so we need to add this to all SbemObject.
		/// </summary>
		public override string ObjectName() { return OBJECT_NAME; }
		/// <summary>
		/// The .inp SbemGlass that defines the windows thermal properties.
		/// </summary>
		public SbemGlass Glass { get; protected set; }
		/// <summary>
		/// 
		/// </summary>
		public SbemWindowTmBridge TMBridge { get; protected set; }
		public SbemWindow(string currentName, List<string> currentProperties) : base(currentName, currentProperties)
		{

		}
		/// <summary>
		/// Set the SbemGlass
		/// </summary>
		/// <param name="glass"></param>
		public void SetGlass(SbemGlass glass)
		{
			Glass = glass;
		}
		/// <summary>
		/// Set the SbemWindowThermalBridge
		/// </summary>
		/// <param name="thermalBridge"></param>
		public void SetThermalBridge(SbemWindowTmBridge thermalBridge)
		{
			TMBridge = thermalBridge;
		}
	}
}
