using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public class FuelCostReference
	{
		public FuelCostReference(float natGas, float lpg, float bioGas, float oil, float coal, float anthracite, 
			float smokeless, float dualFuel, float biomass, float gridSupElec, float wasteHeat, float dh, float displaced) { 
			NatGas		+= natGas;
			LPG			+= lpg;
			BioGas		+= bioGas;
			Oil			+= oil;
			Coal		+= coal;
			Anthracite	+= anthracite;
			Smokeless	+= smokeless;
			DualFuel	+= dualFuel;
			Biomass		+= biomass;
			GridSupElec	+= gridSupElec;
			WasteHeat	+= wasteHeat;
			DH			+= dh;
			Displaced	+= displaced;
		}
		public float NatGas { get; }
		public float LPG { get; }
		public float BioGas { get; }
		public float Oil { get; }
		public float Coal { get; }
		public float Anthracite { get; }
		public float Smokeless { get; }
		public float DualFuel { get; }
		public float Biomass { get; }
		public float GridSupElec { get; }
		public float WasteHeat { get; }
		public float DH { get; }
		public float Displaced { get; }
	}
}
