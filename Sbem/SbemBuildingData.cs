using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public class SbemBuildingData : SbemObject
	{
		public SbemBuildingData(string name, List<string> currentProperties) : base(name, currentProperties) { }
		public const string OBJECT_NAME = "BUILDING-DATA";
		public List<SbemHvacSystemData> HvacSystems { get; } = new List<SbemHvacSystemData>();
		public override string ObjectName() { return OBJECT_NAME; }
		public override string ToString()
		{
			StringBuilder content = new StringBuilder();
			content.AppendLine(base.ToString());
			foreach(SbemHvacSystemData systemData in HvacSystems)
				content.AppendLine(systemData.ToString());
			return content.ToString();
		}
	}
}
