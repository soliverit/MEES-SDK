


using MeesSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// The .inp metadata. General contains infomration about the building, assessor and 
	/// accreditating body, location, SBEM software interface, building type, inspection, 
	/// power factor and district heating kgCO2/kWh factor.
	/// </summary>
	//"SBEM-PROJECT" = GENERAL
	//	B-TYPE = A1/A2 Retail and Financial/Professional services
	//	WEATHER = LON
	//	B-COUNTRY = England and Wales
	//	P-NAME = "Accreditation test 2"
	//	B-ADDRESS-0 = "42 Colonial Way"
	//	B-CITY = "Watford"
	//	B-POSTCODE = "WD2 4TT"
	//	C-NAME = "Joe Bloggs"
	//	C-TELEPHONE = "99999999999"
	//	C-ADDRESS = "12 Any Street"
	//	C-EMAIL = "joe@bloggs.com"
	//	C-CITY = "Any city"
	//	C-POSTCODE = "AB1 2CD"
	//	C-REG-NUMBER = "ABCD123456"
	//	C-ACCRED-SCHEME = Not accredited
	//	C-REL-PART-DISC = Not related to the owner
	//	TRANSACTION-TYPE = Mandatory issue(Marketed sale)
	//	C-QUALIFICATIONS = NOS3
	//	SOFT-COMP-NAME = CLG
	//	INTERFACE-VAL = iSBEM
	//	INTERFACE = iSBEM
	//	INTERFACE-VERSION = v6.1.e
	//	PATH-FILE-INTERFACE = "C:\NCM\iSBEM_v6.1.e\Projects\Approval Case 2\Approval Case 2.nct"
	//	ACT-NOT = ACT
	//	ELEC-POWER-FACTOR = >0.95
	//	NOS-LEVEL = Level 3
	//	C-INSURER = "Insurance Company Ltd."
	//	C-INS-POL-NUMBER = "0001"
	//	C-INS-EFF-DATE = "2020-08-01"
	//	C-INS-EXP-DATE = "2021-07-31"
	//	C-INS-PI-LIMIT = "500000"
	//	B-INSP-DATE = { 2008, 02, 04 }
	//	UPRN = "UPRN-000000000000"
	//	BUILDING-AREA = 4215
	//	FOUNDATION-AREA = 4215
	//	LIGHT-METERING = 0.95
	//	BUILD-ORIENTATION = 0
	//	MAX-STOREY = 1
	//	..
	public class SbemGeneral : SbemObject
	{
		public const string OBJECT_NAME  = "GENERAL"; 
		public override string ObjectName() { return OBJECT_NAME; }
		public SbemGeneral(string currentName, List<string> currentProperties) : base(currentName, currentProperties)
		{

		}
	}
}
