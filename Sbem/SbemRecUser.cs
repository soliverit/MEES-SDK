


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// 
	/// </summary>
	public class SbemRecUser : SbemObject
	{
		public const string OBJECT_NAME  = "REC-USER"; 
		public override string ObjectName() { return OBJECT_NAME; }
		public SbemRecUser(string currentName, List<string> currentProperties) : base(currentName, currentProperties)
		{

		}
	}
}
