


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public class SbemGlass : SbemSpatialObject
	{
		public const string OBJECT_NAME  = "GLASS"; 
		public override string ObjectName() { return OBJECT_NAME; }
		public SbemGlass(string currentName, List<string> currentProperties) : base(currentName, currentProperties)	{ }
		public SbemObjectSet<SbemWindow> Windows { get; }	= new();
		public void AddWindow(SbemWindow window)
		{
			Windows.Add(window);
			Area += window.Area;
		}
	}
}
