using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public class SbemAirConQuestionaire : SbemObject
	{
		public SbemAirConQuestionaire(string name, List<string> currentProperties) : base(name, currentProperties) { }
		public const string OBJECT_NAME = "AIR-CON-QUESTIONNAIRE";
		public override string ObjectName() { return OBJECT_NAME; }

	}
}
