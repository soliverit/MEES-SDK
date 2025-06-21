using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeesSDK.Sbem;
using MeesSDK.Sbem.Retrofitting.Measures;
namespace MeesSDK.Examples
{
	/// <summary>
	/// The interface for examples.
	/// </summary>
	public interface IMeesSDKExample
	{
		/// <summary>
		/// Return a string describing the example.
		/// </summary>
		/// <returns></returns>
		public string GetDescription();
		/// <summary>
		/// Print the description
		/// </summary>
		public void PrintDescription()
		{
			Console.WriteLine("\n====================================");
			Console.WriteLine($"MEES-SDK.\n");
			Console.WriteLine(GetDescription());
			Console.WriteLine("\n====================================");
		}
		/// <summary>
		/// Run the example.
		/// </summary>
		public void RunTheExample();
		
	}
}
