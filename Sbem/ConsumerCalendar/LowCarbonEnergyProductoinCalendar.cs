using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem.ConsumerCalendar
{
	public class LowCarbonEnergyProductoinCalendar : UsageCalendarBase<LowCarbonEnergyProductoinRecord>
	{
		public LowCarbonEnergyProductoinCalendar(string description) : base(description) { }
		public override void Print()
		{

		}
		protected override void Add(LowCarbonEnergyProductoinRecord record)
		{
			Totals.Add(record);
		}
	}
}
