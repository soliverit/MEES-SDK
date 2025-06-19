using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// A record that represents a month in the Fuel Type consumption calendar.
	/// <para>Source: Calendars are taken from the SBEM .sim output file.</para>
	/// <para> Contains kWh/m² for the common fuel types including natural gas, oil, lpg, coal, electricity.</para>
	/// </summary>
	public class FuelConsumptionRecord : UsageRecordBase
	{
		/// <summary>
		/// Create a new Record using the month index (JAN = 1,.. DEC = 13) and consumption.
		/// </summary>
		/// <param name="month"></param>
		/// <param name="NatGas"></param>
		/// <param name="LPG"></param>
		/// <param name="BioGas"></param>
		/// <param name="Oil"></param>
		/// <param name="Coal"></param>
		/// <param name="Anthracite"></param>
		/// <param name="Smokeless"></param>
		/// <param name="DualFuel"></param>
		/// <param name="Biomass"></param>
		/// <param name="GridSupElec"></param>
		/// <param name="WasteHeat"></param>
		/// <param name="DH"></param>
		/// <param name="All"></param>
		/// <param name="Displaced"></param>
		public FuelConsumptionRecord(int month, float NatGas, float LPG, float BioGas, float Oil, float Coal, float Anthracite, float Smokeless, float DualFuel, float Biomass, float GridSupElec, float WasteHeat, float DH, float All, float Displaced) : base(month)
		{
			this.NatGas = NatGas;
			this.LPG = LPG;
			this.BioGas = BioGas;
			this.Oil = Oil;
			this.Coal = Coal;
			this.Anthracite = Anthracite;
			this.Smokeless = Smokeless;
			this.DualFuel = DualFuel;
			this.Biomass = Biomass;
			this.GridSupElec = GridSupElec;
			this.Displaced = Displaced;
			this.WasteHeat = WasteHeat;
			this.DH = DH;
			this.All = All;
		}
		/// <summary>
		/// Create a new Record using the month index (JAN = 1,.. DEC = 13) and consumption.
		/// </summary>
		/// <param name="month"></param>
		/// <param name="NatGas"></param>
		/// <param name="LPG"></param>
		/// <param name="BioGas"></param>
		/// <param name="Oil"></param>
		/// <param name="Coal"></param>
		/// <param name="Anthracite"></param>
		/// <param name="Smokeless"></param>
		/// <param name="DualFuel"></param>
		/// <param name="Biomass"></param>
		/// <param name="GridSupElec"></param>
		/// <param name="WasteHeat"></param>
		/// <param name="DH"></param>
		/// <param name="All"></param>
		/// <param name="Displaced"></param>
		public FuelConsumptionRecord(string month, float NatGas, float LPG, float BioGas, float Oil, float Coal, float Anthracite, float Smokeless, float DualFuel, float Biomass, float GridSupElec, float WasteHeat, float DH, float All, float Displaced) : base(month)
		{
			this.NatGas = NatGas;
			this.LPG = LPG;
			this.BioGas = BioGas;
			this.Oil = Oil;
			this.Coal = Coal;
			this.Anthracite = Anthracite;
			this.Smokeless = Smokeless;
			this.DualFuel = DualFuel;
			this.Biomass = Biomass;
			this.GridSupElec = GridSupElec;
			this.Displaced = Displaced;
			this.WasteHeat = WasteHeat;
			this.DH = DH;
			this.All = All;
		}
		/// <summary>
		/// Parse line from SBEM .sim output file to Fuel Consumption Record.
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		public static FuelConsumptionRecord FromLine(string line)
		{
			string[] values = line.Split(',');
			return new FuelConsumptionRecord(
				values[0],
				float.Parse(values[1]),
				float.Parse(values[2]),
				float.Parse(values[3]),
				float.Parse(values[4]),
				float.Parse(values[5]),
				float.Parse(values[6]),
				float.Parse(values[7]),
				float.Parse(values[8]),
				float.Parse(values[9]),
				float.Parse(values[10]),
				float.Parse(values[11]),
				float.Parse(values[12]),
				float.Parse(values[13]),
				values.Length > 15 ? float.Parse(values[15]) : 0
				);
		}
		/// <summary>
		/// Natural gas (kWh/m²)
		/// </summary>
		public float NatGas { get; protected set; }
		/// <summary>
		/// LPG  (kWh/m²)
		/// </summary>
		public float LPG { get; protected set; }
		/// <summary>
		/// Biogas (kWh/m²)
		/// </summary>
		public float BioGas { get; protected set; }
		/// <summary>
		/// Oil (kWh/m²)
		/// </summary>
		public float Oil { get; protected set; }
		/// <summary>
		/// Coal (kWh/m²)
		/// </summary>
		public float Coal { get; protected set; }
		/// <summary>
		/// Anthracite (kWh/m²)
		/// </summary>
		public float Anthracite { get; protected set; }
		/// <summary>
		/// Smokeless (kWh/m²)
		/// </summary>
		public float Smokeless { get; protected set; }
		/// <summary>
		/// Dual Fuel (kWh/m²)
		/// </summary>
		public float DualFuel { get; protected set; }
		/// <summary>
		/// Biomass (kWh/m²)
		/// </summary>
		public float Biomass { get; protected set; }
		/// <summary>
		/// Grid supplied electricity (kWh/m²)
		/// </summary>
		public float GridSupElec { get; protected set; }
		/// <summary>
		/// Waste Heat (kWh/m²)
		/// </summary>
		public float WasteHeat { get; protected set; }
		/// <summary>
		/// District heating (kWh/m²)
		/// </summary>
		public float DH { get; protected set; }
		/// <summary>
		/// All energy consumption (kWh/m²)
		/// </summary>
		public float All { get; protected set; }
		/// <summary>
		/// Grid displaced. Solar and wind turbines (kWh/m²)
		/// </summary>
		public float Displaced { get; protected set; }

		/// <summary>
		/// Add the values of another FuelConsumptionRecords to this one's.
		/// </summary>
		/// <param name="record"></param>
		public void Add(FuelConsumptionRecord record)
		{
			NatGas		+= record.NatGas;
			LPG			+= record.LPG;
			BioGas		+= record.BioGas;
			Oil			+= record.Oil;
			Coal		+= record.Coal;
			Anthracite	+= record.Anthracite;
			Smokeless	+= record.Smokeless;
			DualFuel	+= record.DualFuel;
			Biomass		+= record.Biomass;
			GridSupElec	+= record.GridSupElec;
			WasteHeat	+= record.WasteHeat;
			DH			+= record.DH;
			All			+= record.All;
			Displaced	+= record.Displaced;
		}
		/// <summary>
		/// Subtract another FuelConsumptionRecord from this one.
		/// </summary>
		/// <param name="record"></param>
		public void Subtract(FuelConsumptionRecord record)
		{
			NatGas		-= record.NatGas;
			LPG			-= record.LPG;
			BioGas		-= record.BioGas;
			Oil			-= record.Oil;
			Coal		-= record.Coal;
			Anthracite	-= record.Anthracite;
			Smokeless	-= record.Smokeless;
			DualFuel	-= record.DualFuel;
			Biomass		-= record.Biomass;
			GridSupElec	-= record.GridSupElec;
			WasteHeat	-= record.WasteHeat;
			DH			-= record.DH;
			All			-= record.All;
			Displaced	-= record.Displaced;
		}
		
		/// <summary>
		/// Multiply all values in this Record by the given factor.
		/// </summary>
		/// <param name="factor"></param>
		public void MultiplyBy(float factor)
		{
			NatGas += factor;
			LPG += factor;
			BioGas += factor;
			Oil += factor;
			Coal += factor;
			Anthracite += factor;
			Smokeless += factor;
			DualFuel += factor;
			Biomass += factor;
			GridSupElec += factor;
			WasteHeat += factor;
			DH += factor;
			All += factor;
			Displaced += factor;
		}
		/// <summary>
		/// Create a copy of this Record.
		/// </summary>
		/// <returns></returns>
		public FuelConsumptionRecord Clone()
		{
			return new FuelConsumptionRecord(Month, NatGas, LPG, BioGas, Oil, Coal, Anthracite, Smokeless, DualFuel, Biomass, GridSupElec, WasteHeat, DH, All, Displaced);
		}
		
	}
}
