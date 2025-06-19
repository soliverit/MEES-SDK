using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// The base class for SBEM .sim output file Consumer Calendards. 
	/// <para>These calendars have monthly kWh/m² for either End Uses
	/// or Fuel Types. It looks something like these:</para>
	/// <code>
	/// Month | Heating | Cooling | Aux    | Lighting | Hot Water | All    | Equip  | All+Eq | CHP_H | CHP_C | CHP_HW | CHP_All | CHP_Displaced
	/// ------|---------|---------|--------|----------|-----------|--------|--------|--------|--------|--------|---------|----------|----------------
	/// JAN   | 7.39    | 0       | 0.438  | 4.55     | 263.03    | 275.40 | 12.56  | 287.96 | 0      | 0      | 0       | 0        | 0
	/// FEB   | 6.01    | 0       | 0.396  | 4.11     | 237.57    | 248.09 | 11.35  | 259.44 | 0      | 0      | 0       | 0        | 0
	///
	/// Month | NatGas | LPG | BioGas | Oil | Coal | Anthr | Smokel | Dual | Biomass | GridEl | WHeat | DH  | All    | Displ | Net
	/// ------|--------|-----|--------|-----|------|-------|--------|------|---------|--------|-------|-----|--------|-------|--------
	/// JAN   | 7.39   | 0   | 0      | 0   | 0    | 0     | 0      | 0    | 0       | 268.01 | 0     | 0   | 275.40 | 0     | 275.40
	/// FEB   | 6.01   | 0   | 0      | 0   | 0    | 0     | 0      | 0    | 0       | 242.08 | 0     | 0   | 248.09 | 0     | 248.09
	/// </code>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class UsageCalendarBase<T> where T : UsageRecordBase
	{
		/// <summary>
		/// Calendar month indices index by SBEM .sim output month names.
		/// <para>Note: Month 13 isn't included in calendar Records arrays, but since total Records use "SUM", it's
		/// added for consistency.</para>
		/// </summary>
		public static readonly Dictionary<string, int> RECORD_INDEX_BY_MONTH_NAME = new()
		{
			["JAN"] = 1,
			["FEB"] = 2,
			["MAR"] = 3,
			["APR"] = 4,
			["MAY"] = 5,
			["JUN"] = 6,
			["JUL"] = 7,
			["AUG"] = 8,
			["SEP"] = 9,
			["OCT"] = 10,
			["NOV"] = 11,
			["DEC"] = 12,
			["SUM"] = 13
		};
		/// <summary>
		/// Calendar month SBEM .sim output names indexed by position in Records array.
		/// <para>Note: Month 13 isn't included in calendar Records arrays, but since total Records use "SUM", it's
		/// added for consistency.</para>
		/// </summary>
		public static readonly Dictionary<int, string> MONTH_NAMES_BY_RECORD_INDEX = new()
		{
			[1] = "JAN",
			[2] = "FEB",
			[3] = "MAR",
			[4] = "APR",
			[5] = "MAY",
			[6] = "JUN",
			[7] = "JUL",
			[8] = "AUG",
			[9] = "SEP",
			[10] = "OCT",
			[11] = "NOV",
			[12] = "DEC",
			[13] = "SUM"
		};
		/// <summary>
		/// The base constructor. Set the Description
		/// </summary>
		public UsageCalendarBase(string description)
		{
			Description	= description;
		}
		/// <summary>
		/// The header description that flagged the calendar's parse.
		/// </summary>
		public string Description { get; protected set; }
		/// <summary>
		/// Structs that store the Month and consumer kWh/m². Depending on the calendar, they're Fuel Type or End use consumption.
		/// </summary>
		public T[] Records { get; }	= new T[12];
		/// <summary>
		/// The sum of all months in Records.
		/// </summary>
		public T Totals;
		/// <summary>
		/// Subscripted access by SBEM .sim output Month names. E.g. JAN, FEB,... DEC.
		/// </summary>
		/// <param name="month"></param>
		/// <returns></returns>
		public T this[string monthName]
		{
			get => Records[RECORD_INDEX_BY_MONTH_NAME[monthName]];
		}
		/// <summary>
		/// Subscripted access by SBEM .sim output Month index. E.g. 1 = JAN, 12 = DEC.
		/// </summary>
		/// <param name="month"></param>
		/// <returns></returns>
		public T this[int index]
		{
			get => Records[index];
		}
		/// <summary>
		/// Add a Record to the Calendar. Added by the record's MonthArrayIndex.
		/// </summary>
		/// <param name="record"></param>
		public void AddRecord(T record)
		{
			Records[record.MonthArrayIndex]	= record;
			Add(record);
		}
		public IEnumerable<T> AllRecords => Records;
		/// <summary>
		/// A method for adding a T Record to the Totals T Record.. It exists because there's no late static binding
		/// in C#. Probably a better way to handle it.
		/// </summary>
		/// <param name="record"></param>
		protected abstract void Add(T record);
		/// <summary>
		/// Print the calendar to the console. Informative and it looks cool.
		/// </summary>
		public abstract void Print();
	}
}
