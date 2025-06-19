using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// A record that represents a month in the End Use energy consumption calendar.
	/// <para>Source: Calendars are taken from the SBEM .sim output file.</para>
	/// <para> Contains kWh/m² for six consumers: Heating, Cooling, Lighting, Hot Water, Auxiliary, and Equipment.</para>
	/// </summary>
	public class ConsumerConsumptionRecord : UsageRecordBase
	{

		public ConsumerConsumptionRecord(int month, float heating, float cooling, float auxiliary, float lighting, float dhw, float equipment) : base(month)
		{
			Dhw = dhw;
			Lighting = lighting;
			Auxiliary = auxiliary; 
			Heating = heating;
			Cooling = cooling;
			Equipment = equipment;
		}
		public ConsumerConsumptionRecord(string month, float heating, float cooling, float auxiliary, float lighting, float dhw, float equipment) : base(month)
		{
			Dhw = dhw;
			Lighting = lighting;
			Auxiliary = auxiliary;
			Heating = heating;
			Cooling = cooling;
			Equipment = equipment;
		}
		/// <summary>
		/// Parse a line from the .sim 
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		public static ConsumerConsumptionRecord FromLine(string line)
		{
			string[] values	= line.Split(',');
			return new ConsumerConsumptionRecord(
				values[0],
				float.Parse(values[1]),
				float.Parse(values[2]),
				float.Parse(values[3]),
				float.Parse(values[4]),
				float.Parse(values[5]),
				float.Parse(values[6])
				);
		}
		/// <summary>
		/// It's FromLine but for records associated with HVACs The consumer column order is different.
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		public static ConsumerConsumptionRecord FromHvacLine(string line)
		{
			string[] values = line.Split(',');
			return new ConsumerConsumptionRecord(
				values[0],
				float.Parse(values[1]),
				float.Parse(values[2]),
				float.Parse(values[3]),
				float.Parse(values[5]),
				0,	// SBEM doesn't do HVAC-level DHW. We can figure it out by running an SBEM per individual HVAC.
				float.Parse(values[4])
				);
		}
		public float Dhw { get; protected set; }
		public float Heating { get; protected set; }
		public float Cooling { get; protected set; }
		public float Lighting { get; protected set; }
		public float Auxiliary { get; protected set; }
		public float Equipment { get; protected set; }
		public void Add(ConsumerConsumptionRecord record)
		{
			Heating		+= record.Heating;
			Cooling		+= record.Cooling;
			Auxiliary	+= record.Auxiliary;
			Lighting	+= record.Lighting;
			Dhw			+= record.Dhw;
			Equipment	+= record.Equipment;
		}
		public void Subtract(ConsumerConsumptionRecord record)
		{
			Heating		-= record.Heating;
			Cooling		-= record.Cooling;
			Auxiliary	-= record.Auxiliary;
			Lighting	-= record.Lighting;
			Dhw			-= record.Dhw;
			Equipment	-= record.Equipment;
		}
		/// <summary>
		/// Multiply all consumer conumption by the passed factor.
		/// </summary>
		/// <param name="factor"></param>
		public void MultiplyBy(float factor)
		{
			Heating		*= factor;
			Cooling		*= factor;
			Auxiliary	*= factor;
			Lighting	*= factor;
			Dhw			*= factor;
			Equipment	*= factor;
			
		}
		/// <summary>
		/// Update DHW consumption: This exists for DHW disaggregation.
		/// </summary>
		public void SetDHW(float dhw)
		{
			Dhw	= dhw;
		}
		public ConsumerConsumptionRecord Clone()
		{
			return new ConsumerConsumptionRecord(Month, Heating, Cooling, Auxiliary, Lighting, Dhw, Equipment);
		}
	}
}
