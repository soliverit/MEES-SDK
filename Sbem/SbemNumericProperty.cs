using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// Numeric SBEM object prperties.
	/// </summary>
	public class SbemNumericProperty : SbemPropertyBase
	{
		/// <summary>
		/// The numeric value assumed to be a float by default.
		/// </summary>
		public float Value { get; protected set; }
		/// <summary>
		/// The integer form of the number.
		/// </summary>
		public int Integer { get => (int)Value; }
		public SbemNumericProperty(string name, float value) : base(name)
		{
			SetValue(value);
		}
		/// <summary>
		/// The method used to change the value. Keeps updates consistent and properties protected.
		/// </summary>
		/// <param name="value"></param>
		public void SetValue(float value)
		{
			Value = value;
		}
		/// <summary>
		/// Convert property to string. E.g U-VALUE = 0.28.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return $"{Name} = {Value}";
		}
	}
	
}
