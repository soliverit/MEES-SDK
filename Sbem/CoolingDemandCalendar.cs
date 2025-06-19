using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public class CoolingDemandCalendar : UsageCalendarBase<CoolingDemandRecord>
	{
		public CoolingDemandCalendar(string name) : base(name)
		{
			Totals = new(13, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
		}
		protected override void Add(CoolingDemandRecord record)
		{
			Totals.Add(record);
		}
		/// <summary>
		/// Create a safe copy of this Calendar
		/// </summary>
		/// <returns></returns>
		public CoolingDemandCalendar Clone()
		{
			CoolingDemandCalendar output = new CoolingDemandCalendar(Description);
			for (int recordID = 0; recordID < Records.Length; recordID++)
				output.AddRecord(Records[recordID].Clone());
			return output;
		}
		/// <summary>
		/// Subtract Calendar Records from their counterpart in this Calendar.
		/// </summary>
		/// <param name="otherCalendar"></param>
		public void Subtract(CoolingDemandCalendar otherCalendar)
		{
			for (int recordID = 0; recordID < Records.Length; recordID++)
				Records[recordID].Subtract(otherCalendar.Records[recordID]);
		}
		public override void Print()
		{
			Console.WriteLine($"Calendar: {Description}");
			Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
			Console.WriteLine($"{"Month",-8}{"Qi",10}{"Qsun(t)",10}{"Qsun(nt)",10}{"Qsc",10}{"Qtr",10}{"Qvent+inf",12}{"QventNC",10}{"Rb;cool",10}{"Qloss",10}{"glratio",10}{"a-fac",10}{"Tau",10}{"a-red",10}{"Qdem",10}");
			Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");

			foreach (var record in Records)
			{
				Console.WriteLine($"{record.Month,-8}" +
								  $"{record.InternalGains,10:0.0}" +
								  $"{record.SolarGainsTransmitted,10:0.0}" +
								  $"{record.SolarGainsNoTransmission,10:0.0}" +
								  $"{record.SolarControl,10:0.0}" +
								  $"{record.TransmissionLosses,10:0.0}" +
								  $"{record.VentilationLosses,12:0.0}" +
								  $"{record.VentilationNoCooling,10:0.0}" +
								  $"{record.RoomCoolCapacity,10:0.0}" +
								  $"{record.TotalLosses,10:0.0}" +
								  $"{record.GlazingRatio,10:0.0}" +
								  $"{record.AFactor,10:0.0}" +
								  $"{record.CoolTimeConstant,10:0.0}" +
								  $"{record.CoolingReductionFactor,10:0.0}" +
								  $"{record.SpaceCoolingDemand,10:0.0}");
			}

			Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
		}

	}
}
