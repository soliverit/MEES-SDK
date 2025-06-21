using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem.ConsumerCalendar
{
	public class HeatingDemandCalendar : UsageCalendarBase<HeatingDemandRecord>
	{
		public HeatingDemandCalendar(string name, float area) : base(name, area)
		{
			Totals = new HeatingDemandRecord(13, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
		}
		protected override void Add(HeatingDemandRecord record)
		{
			Totals.Add(record);
		}
		/// <summary>
		/// Create a safe copy of this Calendar
		/// </summary>
		/// <returns></returns>
		public HeatingDemandCalendar Clone()
		{
			HeatingDemandCalendar output = new HeatingDemandCalendar(Description, Area);
			for (int recordID = 0; recordID < Records.Length; recordID++)
				output.AddRecord(Records[recordID].Clone());
			return output;
		}
		/// <summary>
		/// Subtract Calendar Records from their counterpart in this Calendar.
		/// </summary>
		/// <param name="otherCalendar"></param>
		public void Subtract(HeatingDemandCalendar otherCalendar)
		{
			for (int recordID = 0; recordID < Records.Length; recordID++)
				Records[recordID].Subtract(otherCalendar.Records[recordID]);
		}
		public override void Print()
		{
			Console.WriteLine($"Calendar: {Description}");
			Console.WriteLine("--------------------------------------------------------------------------------------------------------------------------------------------------------------");
			Console.WriteLine($"{"Month",-8}{"Qi",10}{"Qsun",10}{"Qsun(nt)",10}{"Qsc",10}{"Qscd",10}{"Qtr",10}{"Qvent+inf",12}{"glratio",10}{"Rb;heat",10}{"Qgain",10}{"a-fac",10}{"Tau",10}{"a-red",10}{"Qdem",10}");
			Console.WriteLine("--------------------------------------------------------------------------------------------------------------------------------------------------------------");

			foreach (var record in Records)
			{
				Console.WriteLine($"{record.Month,-8}" +
								  $"{record.InternalGains,10:0.0}" +
								  $"{record.SolarGains,10:0.0}" +
								  $"{record.SolarGainsNoTransmission,10:0.0}" +
								  $"{record.SolarControl,10:0.0}" +
								  $"{record.SolarControlDiffuse,10:0.0}" +
								  $"{record.TransmissionLosses,10:0.0}" +
								  $"{record.VentilationInfiltrationLosses,12:0.0}" +
								  $"{record.GlazingRatio,10:0.0}" +
								  $"{record.RoomHeatCapacity,10:0.0}" +
								  $"{record.TotalGains,10:0.0}" +
								  $"{record.AFactor,10:0.0}" +
								  $"{record.HeatTimeConstant,10:0.0}" +
								  $"{record.HeatReductionFactor,10:0.0}" +
								  $"{record.SpaceHeatingDemand,10:0.0}");
			}

			Console.WriteLine("--------------------------------------------------------------------------------------------------------------------------------------------------------------");
		}

	}
}
