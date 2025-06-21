using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Optimisation
{
	//<summary>
	//  Genetic-algorithm friendly Interface
	//
	//  The Genetic Algorithm uses MathNET for fitness functions. MathNET
	//  is basically NumPy. Like NumPy, everything's fixed size Arrays. This interface
	//  just says the object will be dictionaries of primative classes, namely strings and floats
	// </summary>
	public interface IGeneticAlgorithmFriendly
	{

		public GeneticAlgorithmDataStruct GetGAObjects();
	}
}
