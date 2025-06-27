using MeesSDK.DataManagement;
using MeesSDK.Optimisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.RdSAP.Optimisation
{
	public class RdSAPSubsetOptimiser : JAEMOO<RdSAPEstateOptimiser>
	{
		public RdSAPSubsetOptimiser(MathNetRetrofitsTable data) : base(data) { }
		public override RdSAPEstateOptimiser CreateAlgorithm(int[] recordIDs)
		{
			return new RdSAPEstateOptimiser(Data, recordIDs);
		}
	}
}
