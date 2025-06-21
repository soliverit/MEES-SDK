using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem.ConsumerCalendar
{
	/// <summary>
	/// Fuel type consumption calendar.
	/// <para>Stores monthly Fuel consumption</para>
	/// <para>Sample from: model.sim. Created by SBEM.</para>
	/// <code>
	///Month | NatGas | LPG | BioGas | Oil | Coal | Anthr | Smokel | Dual | Biomass | GridEl | WHeat | DH  | All    | Displ | Net
	///------|--------|-----|--------|-----|------|-------|--------|------|---------|--------|-------|-----|--------|-------|--------
	///JAN   | 7.39   | 0   | 0      | 0   | 0    | 0     | 0      | 0    | 0       | 268.01 | 0     | 0   | 275.40 | 0     | 275.40
	///FEB   | 6.01   | 0   | 0      | 0   | 0    | 0     | 0      | 0    | 0       | 242.08 | 0     | 0   | 248.09 | 0     | 248.09
	///</code>
	/// </summary>
	public class FuelConsumptionCalendar : UsageCalendarBase<FuelConsumptionRecord>
	{

		public FuelConsumptionCalendar(string name, float area) : base(name, area)
		{
			Totals = new FuelConsumptionRecord(13, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
		}
		/// <summary>
		/// Add UsageCalendarBase<T> to the totals. Bit dirty, still learning how to work wihout late static binding.
		/// </summary>
		/// <param name="record"></param>
		protected override void Add(FuelConsumptionRecord record)
		{
			Totals.Add(record);
		}
		/// <summary>
		/// Subtract Calendar Records from their counterpart in this Calendar.
		/// </summary>
		/// <param name="otherCalendar"></param>
		public void Subtract(FuelConsumptionCalendar otherCalendar)
		{
			for (int recordID = 0; recordID < Records.Length; recordID++)
				Records[recordID].Subtract(otherCalendar.Records[recordID]);
		}
		/// <summary>
		/// Create a safe copy of this Calendar
		/// </summary>
		/// <returns></returns>
		public FuelConsumptionCalendar Clone()
		{
			FuelConsumptionCalendar output = new FuelConsumptionCalendar(Description, Area);
			for (int recordID = 0; recordID < Records.Length; recordID++)
				output.AddRecord(Records[recordID].Clone());
			return output;
		}
		public float CalculateEnergyCostPerM2(FuelCostReference costReference)
		{
			return Totals.NatGas* costReference.NatGas + Totals.LPG * costReference.LPG + Totals.BioGas * costReference.BioGas + 
					Totals.Oil * costReference.Oil + Totals.Coal * costReference.Coal + Totals.Anthracite * costReference.Anthracite + 
					Totals.Smokeless * costReference.Smokeless + Totals.DualFuel * costReference.DualFuel + Totals.Biomass * costReference.Biomass + 
					Totals.GridSupElec * costReference.GridSupElec + Totals.WasteHeat * costReference.WasteHeat + Totals.DH * costReference.DH + 
					Totals.Displaced * costReference.Displaced;
		}
		public float CalculateEmissionsPerM2(EmissionFactorsReference emissionsFactorReference)
		{
			return Totals.NatGas * emissionsFactorReference.NatGas + Totals.LPG * emissionsFactorReference.LPG + Totals.BioGas * emissionsFactorReference.BioGas +
					Totals.Oil * emissionsFactorReference.Oil + Totals.Coal * emissionsFactorReference.Coal + Totals.Anthracite * emissionsFactorReference.Anthracite +
					Totals.Smokeless * emissionsFactorReference.Smokeless + Totals.DualFuel * emissionsFactorReference.DualFuel + Totals.Biomass * emissionsFactorReference.Biomass +
					Totals.GridSupElec * emissionsFactorReference.GridSupElec + Totals.WasteHeat * emissionsFactorReference.WasteHeat + Totals.DH * emissionsFactorReference.DH +
					Totals.Displaced * emissionsFactorReference.Displaced;
		}
		public override void Print()
		{
			Console.WriteLine($"Calendar: {Description}");
			Console.WriteLine("--------------------------------------------------------------------------------------------------------------------------------------------------------------------");
			Console.WriteLine($"{"Month",-8}{"NatGas",12}{"LPG",12}{"BioGas",12}{"Oil",12}{"Coal",12}{"Anthracite",12}{"Smokeless",12}{"DualFuel",12}{"Biomass",12}{"GridSupElec",12}{"WasteHeat",12}{"DH",12}{"All",12}");
			Console.WriteLine("--------------------------------------------------------------------------------------------------------------------------------------------------------------------");
			foreach (var record in Records)
			{
				Console.WriteLine($"{record.Month,-8}{record.NatGas,12}{record.LPG,12}{record.BioGas,12}{record.Oil,12}{record.Coal,12}{record.Anthracite,12}{record.Smokeless,12}{record.DualFuel,12}{record.Biomass,12}{record.GridSupElec,12}{record.WasteHeat,12}{record.DH,12}{record.All,12}");
			}
			Console.WriteLine("--------------------------------------------------------------------------------------------------------------------------------------------------------------------");
		}
	}
}
