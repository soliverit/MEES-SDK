using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// The _epc.inp top-level EPC results object. This contains the BER, TER, TYR, and 
	/// </summary>
	//"SBEM" = EPC
	//	TYPE = England
	//	SER					= 796.195
	//	TYR					= 1179.9
	//	TER					= 294.24
	//	EPC-LANGUAGE		= ENGLISH
	//	BER = 436.121
	//	NOS-LEVEL			= Level 3
	//	TRANSACTION-TYPE	= Mandatory issue(Marketed sale)
	//	MAIN-FUEL-TYPE		= Grid Supplied Electricity
	//	BUILDING-ENVIRONMENT  = Heating and Mechanical Ventilation
	//	..
	public class SbemEpc : SbemObject
	{
		public const string OBJECT_NAME = "EPC";
		public override string ObjectName() { return OBJECT_NAME; }
		public SbemEpc(string name, List<string> currentProperties) : base(name, currentProperties) { }
	}
}
