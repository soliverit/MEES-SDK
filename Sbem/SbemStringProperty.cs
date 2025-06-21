using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// The .inp and _epc.inp string property object. Numbers can technically be string properties.
	/// </summary>
	public class SbemStringProperty : SbemPropertyBase
	{
		public SbemStringProperty(string name, string value) : base(name)
		{ 
			Value	= value;
		}
		/// <summary>
		/// The property's urrent value.
		/// </summary>
		public string Value { get; protected set; }
		/// <summary>
		/// Update the property's value.
		/// </summary>
		/// <param name="value"></param>
		public void SetValue(string value)
		{
			Value = value;
		}
		/// <summary>
		/// The Value but with any quotation marks removed. Envelopes associated with Constructions and GLasses
		/// often have their CONSTURCTION / GLASS property in qutes. Sometimes happens in the SbemGeneral object.
		/// </summary>
		public string QuotelessValue { get => Value.Replace("\"", ""); }
		/// <summary>
		/// Convert the property to string. E.g HEAT-SOURCE = LTHW boiler
		/// </summary>
		/// <returns></returns>
		public override string ToString() { return $"{Name} = {Value}"; }
	}
}
