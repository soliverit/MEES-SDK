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
 * Working with SBEM
 * 
 * This example shows the basics of working with SBEM projects. Loading, processing, pairing results
 */
IMeesSDKExample sbemExample = new WorkingWithSBEM();
sbemExample.PrintDescription();
sbemExample.RunTheExample();
/*
 * End Use and Fuel Type are disaggregated at HVAC level, except hot water. None of these are at zone level. This
 * example disaggregates it to zone level.
 */