using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public class InternalGainsCalendar : UsageCalendarBase<InternalGainsRecord>
	{
		public InternalGainsCalendar(string name) : base(name)
		{
			Totals = new InternalGainsRecord(13, 0, 0, 0, 0, 0, 0, 0, 0, 0);
		}
		protected override void Add(InternalGainsRecord record)
		{
			Totals.Add(record);
		}
		/// <summary>
		/// Subtract Calendar Records from their counterpart in this Calendar.
		/// </summary>
		/// <param name="otherCalendar"></param>
		public void Subtract(InternalGainsCalendar otherCalendar)
		{
			for (int recordID = 0; recordID < Records.Length; recordID++)
				Records[recordID].Subtract(otherCalendar.Records[recordID]);
		}
		/// <summary>
		/// Create a safe copy of this Calendar
		/// </summary>
		/// <returns></returns>
		public InternalGainsCalendar Clone()
		{
			InternalGainsCalendar output = new InternalGainsCalendar(Description);
			for (int recordID = 0; recordID < Records.Length; recordID++)
				output.AddRecord(Records[recordID].Clone());
			return output;
		}
		public override void Print()
		{
			Console.WriteLine("--------------------------------------------------------------------------------------------------------------");
			Console.WriteLine($"{"Month",-8}{"People",10}{"Appliances",12}{"LightingInt",12}{"Ventilation",12}{"TotalGains",12}{"LPD",10}{"DL%",10}{"LFE",10}{"W/F",10}");
			Console.WriteLine("--------------------------------------------------------------------------------------------------------------");

			foreach (var record in Records)
			{
				Console.WriteLine($"{record.Month,-8}" +
								  $"{record.People,10:0.0}" +
								  $"{record.Appliances,12:0.0}" +
								  $"{record.LightingInternal,12:0.0}" +
								  $"{record.Ventilation,12:0.0}" +
								  $"{record.TotalGains,12:0.0}" +
								  $"{record.LightingPowerDensity,10:0.0}" +
								  $"{record.DaylightingPercent,10:0.0}" +
								  $"{record.LightingEnergyFactor,10:0.0}" +
								  $"{record.WallFraction,10:0.0}");
			}

			Console.WriteLine("--------------------------------------------------------------------------------------------------------------");
		}

	}
}
