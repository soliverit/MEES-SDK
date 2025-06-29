using MathNet.Numerics.Financial;
using MeesSDK.Sbem.Retrofitting.Measures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem.Retrofitting
{
	public class Analyst
	{
		/// <summary>
		/// The as-built or whatever scenario building. Retrofits are applied to this
		/// </summary>
		public SbemProject Project{ get; protected set; }
		/// <summary>
		/// The SBEM service
		/// </summary>
		public SbemService Sbem { get; protected set; }
		public List<SbemScenario> Scenarios { get; protected set; }
		public Analyst(SbemProject project, SbemService sbem)
		{
			Project	= project;
			Sbem	= sbem;
		}
		/// <summary>
		/// Create a retrofit object, apply the retrofit, then  return it. 
		/// <para>T</para>
		/// </summary>
		/// <param name="model"></param>
		/// <param name="code">The RetrofitBase::MEASURE_REFERENCE_CODE static abstract string</param>
		/// <returns></returns>
		public RetrofitBase DoRetrofit(SbemModel model, string code)
		{
			RetrofitBase retrofit = MakeRetrofit(model, code);
			retrofit.Apply();
			return retrofit;
		}
		/// <summary>
		/// Make a retrofit. Apply it. Run it. Return result struct
		/// </summary>
		/// <param name="model"></param>
		/// <param name="code"></param>
		/// <returns></returns>
		public RetrofitStruct RunRetrofit(SbemModel model, string code)
		{
			RetrofitBase retrofit	= MakeRetrofit(model, code);
			SbemProject project		= Sbem.BuildProject(retrofit.Model);
			return new RetrofitStruct(project, retrofit);
		}
		/// <summary>
		/// Lookup the RetrofitBase type constructor for the passed code. Return
		/// an instance. 
		/// <para>If the code doesn't exist, the as-built retrofit is returned</para>
		/// </summary>
		/// <param name="model"></param>
		/// <param name="code"></param>
		/// <returns></returns>
		public RetrofitBase MakeRetrofit(SbemModel model, string code)
		{
			RetrofitBase retrofit;
			switch (code)
			{
				case NCMCooling3Example.MEASURE_REFERENCE_CODE:
					retrofit    = new NCMCooling3Example(model);
					break;
				case NCMExtnerWallInsulation8Example.MEASURE_REFERENCE_CODE:
					retrofit    = new NCMExtnerWallInsulation8Example(model);
					break;
				case NCMGlazing8example.MEASURE_REFERENCE_CODE:
					retrofit    = new NCMGlazing8example(model);
					break;
				case NCMHeating1Example.MEASURE_REFERENCE_CODE:
					retrofit    = new NCMExtnerWallInsulation8Example(model);
					break;
				case NCMLighting1Example.MEASURE_REFERENCE_CODE:
					retrofit    = new NCMExtnerWallInsulation8Example(model);
					break;
				case NCMHeatPumpR5Example.MEASURE_REFERENCE_CODE:
					retrofit    = new NCMHeatPumpR5Example(model);
					break;
				case NCMLighting2Example.MEASURE_REFERENCE_CODE:
					retrofit    = new NCMLighting2Example(model);
					break;
				case NCMLighting3Example.MEASURE_REFERENCE_CODE:
					retrofit    = new NCMLighting3Example(model);
					break;
				case NCMLighting6Example.MEASURE_REFERENCE_CODE:
					retrofit    = new NCMLighting6Example(model);
					break;
				case NCMRenewables4Example.MEASURE_REFERENCE_CODE:
					retrofit    = new NCMRenewables4Example(model);
					break;
				case NCMRoofE2Example.MEASURE_REFERENCE_CODE:
					retrofit    = new NCMRoofE2Example(model);
					break;
				default:
					retrofit    = new AsBuilt(model);
					break;
			}
			return retrofit;
		}

		public List<RetrofitStruct> ApplyAll()
		{
			List<RetrofitStruct> retrofitStructs = new List<RetrofitStruct>();

			/*
			 * Lighting
			 */
			NCMLighting5Example ncm5	= new NCMLighting5Example();

			return retrofitStructs;
		}
	}
}
