using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public class SbemPropertyBase
	{
		public string Name { get; protected set; }
		public SbemPropertyBase(string name) { Name = name; }
	}
}
