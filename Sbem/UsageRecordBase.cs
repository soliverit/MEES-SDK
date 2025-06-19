using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public class UsageRecordBase
	{
		
		public static Dictionary<string, int> MonthIDs = new() {
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
		public static Dictionary<int, string> MonthNames = new()
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
		public UsageRecordBase(int monthID) 
		{
			MonthID = monthID;
			Month	= MonthNames[monthID];
			MonthArrayIndex = MonthID - 1;
		}
		public UsageRecordBase(string month)
		{
			MonthID		= MonthIDs[month];
			Month	= month;
			MonthArrayIndex = MonthID - 1;
		}
		public int MonthID { get; }
		public string Month { get; }
		public int MonthArrayIndex { get; }
	}
}
