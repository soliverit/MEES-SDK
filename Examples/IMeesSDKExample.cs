using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeesSDK.Sbem;
using MeesSDK.Sbem.Retrofitting.Measures;
namespace MeesSDK.Examples
{
	public interface IMeesSDKExample
	{
		public string GetDescription();
		public void PrintDescription()
		{
			Console.WriteLine("\n====================================");
			Console.WriteLine($"MEES-SDK.\n");
			Console.WriteLine(GetDescription());
			Console.WriteLine("\n====================================");
		}
		public void DoTheExample();
		
	}
}
