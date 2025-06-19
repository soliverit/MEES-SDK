using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.RdSAP.Reference
{
	public class ReferenceDataBase<T> where T : ReferenceDataRecordBase
	{
		public List<T> Records { get; } = new List<T>();
		public Dictionary<string, T> BandsDictionary { get; } = new Dictionary<string, T>();
		public Dictionary<string, T> LabelsDictionary { get; } = new Dictionary<string, T>();
		public T FindFirst(Func<T, bool> selector)
		{
			return Records.FirstOrDefault(selector);
		}
	}
}
