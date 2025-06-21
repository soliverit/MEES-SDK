


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// The .inp SbemModel object that defines the EPC and building regulations 
	/// properties. 
	/// </summary>
	// "SBEM" = COMPLIANCE
	//	EPC-TYPE = EPC England
	//	ENG-HERITAGE = NO
	//	BR-STAGE = As built
	//	AIR-CON-INSTALLED = No
	//	..
	public class SbemCompliance : SbemObject
	{
		public const string OBJECT_NAME  = "COMPLIANCE"; 
		public override string ObjectName() { return OBJECT_NAME; }
		public SbemCompliance(string currentName, List<string> currentProperties) : base(currentName, currentProperties)
		{

		}
	}
}
