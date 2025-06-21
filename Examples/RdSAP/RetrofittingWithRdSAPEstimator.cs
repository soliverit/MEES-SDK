using Npgsql.Replication.PgOutput.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Examples.RdSAP
{
	/// <summary>
	/// This example demonstrates a potential strategy for RdSAP retrofit analysis using
	/// a gradient-boosting regressor to estimate the energy efficiency energy efficiency.
	/// </summary>
	public class RetrofittingWithRdSAPEstimator : IMeesSDKExample
	{
		/// <summary>
		/// Path to the certificates csv for the EstimateEmissionsAndRatings that creates
		/// the estimator used in this example.
		/// </summary>
		public string CertificatesPath { get; protected set; }
		public void RunTheExample() 
		{
			
		}
		public string GetDescription()
		{
			return @" Do some rudimentary retrofitting using the RdSAP estimator.

In this example, we'll use an RdSAP estimator to generate some retrofits.
";
		}
	}
}
