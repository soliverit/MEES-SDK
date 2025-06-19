using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// End use energy consumption calendar.
	/// <para>Stores monthly end use energy consumption for: Heating, Cooling, Lighting, Auxiliary, Hot Water, and Equipment.</para>
	/// <para>Sample from: model.sim. Created by SBEM.</para>
	/// <code>
	/// Month | Heating | Cooling | Aux    | Lighting | Hot Water | All    | Equip  |
	/// ------|---------|---------|--------|----------|-----------|--------|--------|
	/// JAN   | 7.39    | 0       | 0.438  | 4.55     | 263.03    | 275.40 | 12.56  |
	/// FEB   | 6.01    | 0       | 0.396  | 4.11     | 237.57    | 248.09 | 11.35  |
	/// </code>
	/// </summary>
	public class ConsumerConsumptionCalendar : UsageCalendarBase<ConsumerConsumptionRecord>
	{
		/// <summary>
		/// The constructor. Nothing exciting, just creates the Totals Record
		/// 
		/// </summary>
		/// <param name="name"></param>
		public ConsumerConsumptionCalendar(string name) : base(name)
		{
			Totals = new ConsumerConsumptionRecord(13, 0, 0, 0, 0, 0, 0); 
		}
		/// <summary>
		/// Subtract Calendar Records from their counterpart in this Calendar.
		/// </summary>
		/// <param name="otherCalendar"></param>
		public void Subtract(ConsumerConsumptionCalendar otherCalendar)
		{
			for (int recordID = 0; recordID < Records.Length; recordID++)
				Records[recordID].Subtract(otherCalendar.Records[recordID]);
		}
		protected override void Add(ConsumerConsumptionRecord record)
		{
			Totals.Add(record);
		}
		/// <summary>
		/// Create a safe copy of this Calendar
		/// </summary>
		/// <returns></returns>
		public ConsumerConsumptionCalendar Clone()
		{
			ConsumerConsumptionCalendar output = new ConsumerConsumptionCalendar(Description);
			for (int recordID = 0; recordID < Records.Length; recordID++)
				output.AddRecord(Records[recordID].Clone());
			return output;
		}
		public void SetDHW(ConsumerConsumptionCalendar sourceCalendar)
		{
			for (int recordID = 0; recordID < Records.Length; recordID++)
				Records[recordID].SetDHW(sourceCalendar.Records[recordID].Dhw);
		}
		/// <summary>
		/// Print the Calendar table to the console. Looks cool if nothing else.
		/// </summary>
		public override void Print()
		{
			Console.WriteLine($"Calendar: {Description}");
			Console.WriteLine("-------------------------------------------------------------------------------");
			Console.WriteLine($"{"Month",-8}{"Heating",10}{"Cooling",10}{"Auxiliary",10}{"Lighting",10}{"DHW",10}{"Equipment",10}");
			Console.WriteLine("-------------------------------------------------------------------------------");
			foreach (var record in Records)
			{
				Console.WriteLine(record.Dhw);
				Console.WriteLine($"{record.Month,-8}{record.Heating,10:0.000}{record.Cooling,10:0.000}{record.Auxiliary,10:0.000}{record.Lighting,10:0.000}{record.Dhw,10:0.000}{record.Equipment,10:0.000}");
			}
			Console.WriteLine("-------------------------------------------------------------------------------");
		}
	}
}
