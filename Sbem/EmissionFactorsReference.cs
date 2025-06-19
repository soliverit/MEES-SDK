using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// The object that stores Emission factors by SBEM version.
	/// </summary>
	public class EmissionFactorsReference
	{
		/// <summary>
		/// SBEM v6.1.e factors. 
		/// <code>
		/// Note: Grid supplied and displaced electrictiry are average monthly factor. District Heating is when DH-CO2F isn't set.
		/// </code>
		/// </summary>
		public readonly EmissionFactorsReference V6_1_E = new("6.1.e", 0.21f, 0.241f, 0.024f, 0.319f, 0.375f, 0.395f, 0.367f, 0.01415f, 0.029f, 029f, 0.015f, 0.36f, 0.156f); 

		public EmissionFactorsReference(string sbemVersion, float natGas, float lpg, float bioGas, float oil, float coal, float anthracite, 
			float smokeless, float dualFuel, float biomass, float gridSupElec, float wasteHeat, float dh, float displaced) { 
			SbemVersion = sbemVersion;
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
		/// <summary>
		/// SBEM version. E.g. 6.1.e
		/// </summary>
		public string SbemVersion { get; }
		/// <summary>
		/// Natural gas
		/// </summary>
		public float NatGas { get; }
		/// <summary>
		/// LPG
		/// </summary>
		public float LPG { get; }
		/// <summary>
		/// Biogass
		/// </summary>
		public float BioGas { get; }
		/// <summary>
		/// Oil
		/// </summary>
		public float Oil { get; }
		/// <summary>
		/// Coal
		/// </summary>
		public float Coal { get; }
		/// <summary>
		/// Anthracite 
		/// </summary>
		public float Anthracite { get; }
		/// <summary>
		/// Smokeless Fuel (including Coke)
		/// </summary>
		public float Smokeless { get; }
		/// <summary>
		/// Dual Fuel Appliances (Mineral + Wood)
		/// </summary>
		public float DualFuel { get; }
		/// <summary>
		/// Biomass
		/// </summary>
		public float Biomass { get; }
		/// <summary>
		/// Grid Supplied Electricity
		/// </summary>
		public float GridSupElec { get; }
		/// <summary>
		/// Waste Heat
		/// </summary>
		public float WasteHeat { get; }
		/// <summary>
		/// District Heating
		/// </summary>
		public float DH { get; }
		/// <summary>
		/// District Heating
		/// </summary>
		public float DistrictHeating { get => DH; }
		/// <summary>
		/// Grid Displaced Electricity - Combined Heating and Power, Solar, and Wind
		/// </summary>
		public float Displaced { get; }
	}
}
