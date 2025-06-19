

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public class SbemSes : SbemObject
	{
		public const string OBJECT_NAME  = "SES"; 
		public override string ObjectName() { return OBJECT_NAME; }
		public SbemSes(string currentName, List<string> currentProperties) : base(currentName, currentProperties)
		{

		}
	}
}
