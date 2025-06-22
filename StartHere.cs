using CsvHelper;
using GeneticSharp;
using MathNet.Numerics.Optimization;
using MeesSDK.Chromosome;
using MeesSDK.DataManagement;
using MeesSDK.ML;
using MeesSDK.Sbem;
using MeesSDK.Sbem.Retrofitting.Measures;
using System.Diagnostics;
using MeesSDK.Examples.RdSAP;
using MeesSDK.Examples.Sbem;
using MeesSDK.Examples;
/*
 *  Math.NET  
 */
MathNet.Numerics.Control.UseNativeMKL();
/*==== 
 * Examples
 ====*/
/*
 * Estate Optimisation
 */
string optimiserData				= "C:\\workspaces\\MeesSDK\\data\\examples\\depc\\stockton\\data_100_rows.csv";
IMeesSDKExample	optimiser			= new EstateRetrofitOptimisation(optimiserData);
optimiser.PrintDescription();
optimiser.RunTheExample();
return;

/*
 * Working with SBEM
 * 
 * This example shows the basics of working with SBEM projects. Loading, processing, pairing results
 */
IMeesSDKExample sbem				= new WorkingWithSBEM();
sbem.PrintDescription();
sbem.RunTheExample();
/*
 * End Use and Fuel Type are disaggregated at HVAC level, except hot water. None of these are at zone level. This
 * example disaggregates it to zone level.
 */
IMeesSDKExample sbemDisaggregation	= new WorkingWithSBEM();
sbemDisaggregation.PrintDescription();
sbemDisaggregation.RunTheExample();
/*
 * Working with the EPC register: RdSAP efficiency estimation.
 */
// Download any D-EPC records from here https://epc.opendatacommunities.org/domestic/search
string certificatesPath				= "c:/workspaces/__shared_data__/depc/domestic-E06000002-Middlesbrough/certificates.csv";
IMeesSDKExample rdSAPEstimation		= new RdSAPEstimation(certificatesPath);
rdSAPEstimation.PrintDescription();
rdSAPEstimation.RunTheExample();
/*
 *  Estate optimisation: Residential retrofit strategy
 */
string retrofitCertificatesPath		= "c:/workspaces/MeesSDK/examples/stockton/data.csv";
IMeesSDKExample estateOptimisation	= new EstateRetrofitOptimisation(certificatesPath);
estateOptimisation.PrintDescription();
estateOptimisation.RunTheExample();