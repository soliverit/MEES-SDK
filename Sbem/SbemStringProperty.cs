using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public class SbemStringProperty : SbemPropertyBase
	{
		public SbemStringProperty(string name, string value) : base(name)
		{ 
			Value	= value;
		}
		public string Value { get; protected set; }
		public void SetValue(string value)
		{
			Value = value;
		}
		/// <summary>
		/// The Value but with any quotation marks removed. Envelopes associated with Constructions and GLasses
		/// often have their CONSTURCTION / GLASS property in qutes. Sometimes happens in the SbemGeneral object.
		/// </summary>
		public string QuotelessValue { get => Value.Replace("\"", ""); }
		public override string ToString() { return $"{Name} = {Value}"; }
	}
}
