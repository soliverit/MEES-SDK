

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// The .inp solar panel (PhotoVoltaic System) definition. Defines area, composition, 
	/// </summary>
	public class SbemPvs : SbemSpatialObject
	{
		public const string OBJECT_NAME  = "PVS"; 
		public override string ObjectName() { return OBJECT_NAME; }
		public SbemPvs(string currentName, List<string> currentProperties) : base(currentName, currentProperties)
		{

		}
	}
}
