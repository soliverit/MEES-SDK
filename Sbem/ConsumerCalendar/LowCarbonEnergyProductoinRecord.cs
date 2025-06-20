using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem.ConsumerCalendar
{
	/// <summary>
	/// A UsageCalendarBase calendar for Energy Production from renewabls. It includes solar panels and hot water,
	/// wind turbines and combined heating and power.
	/// </summary>
	public class LowCarbonEnergyProductoinRecord : UsageRecordBase
	{
		/// <summary>
		/// Energy products by solar panels (kWh/m²)
		/// </summary>
		public float Pvs { get; protected set; }
		/// <summary>
		/// Energy products by wind turbines (kWh/m²)
		/// </summary>
		public float Wind { get; protected set; }
		/// <summary>
		/// Energy products by combined heating and power systems (kWh/m²)
		/// </summary>
		public float CombinedHeatingAndPower { get; protected set; }
		/// <summary>
		/// Energy products by all renewables systems (kWh/m²)
		/// </summary>
		public float Total {  get; protected set; }
		/// <summary>
		/// Grid supplied electricity displaced by renewables systems (kWh/m²)
		/// </summary>
		public float Displaced { get; protected set; }
		/// <summary>
		/// The default construcotr
		/// </summary>
		/// <param name="month"></param>
		/// <param name="pvs"></param>
		/// <param name="wind"></param>
		/// <param name="chp"></param>
		/// <param name="total"></param>
		/// <param name="displaced"></param>
		public LowCarbonEnergyProductoinRecord(int month, float pvs, float wind, float chp, float total, float displaced) : base(month)
		{
			Pvs						= pvs;
			Wind					= wind;	
			CombinedHeatingAndPower	= chp;
			Total					= total;
			Displaced				= displaced;
		}
		/// <summary>
		/// Add another record's values to this instance's.
		/// </summary>
		/// <param name="record"></param>
		public void Add(LowCarbonEnergyProductoinRecord record) 
		{
			Pvs						+= record.Pvs;
			Wind					+= record.Wind;
			CombinedHeatingAndPower	+= record.CombinedHeatingAndPower;
			Total					+= record.Total;
			Displaced               += record.Displaced;
		
		}
		/// <summary>
		/// Subtract another record's values to this instance's.
		/// </summary>
		/// <param name="record"></param>
		public void Subtract(LowCarbonEnergyProductoinRecord record)
		{
			Pvs                     -= record.Pvs;
			Wind                    -= record.Wind;
			CombinedHeatingAndPower -= record.CombinedHeatingAndPower;
			Total                   -= record.Total;
			Displaced               -= record.Displaced;
		}
		/// <summary>
		/// Multiply all values of this record by the passed factor.
		/// </summary>
		/// <param name="factor"></param>
		public void MultiplyBy(float factor)
		{
			Pvs                     *= factor;
			Wind                    *= factor;
			CombinedHeatingAndPower *= factor;
			Total                   *= factor;
			Displaced               *= factor;
		}
	}
}
