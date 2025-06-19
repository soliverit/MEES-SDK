using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public class SbemEpc : SbemObject
	{
		public const string OBJECT_NAME = "EPC";
		public override string ObjectName() { return OBJECT_NAME; }
		public SbemEpc(string name, List<string> currentProperties) : base(name, currentProperties) { }
	}
}
