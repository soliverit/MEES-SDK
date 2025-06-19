using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public class SbemNumericProperty : SbemPropertyBase
	{
		public float Value { get; protected set; }
		public int Integer { get; protected set; }
		public SbemNumericProperty(string name, float value) : base(name)
		{
			SetValue(value);
		}
		public void SetValue(float value)
		{
			Value = value;
			Integer = (int)value;
		}
		public string ToString()
		{
			return $"{Name} = {Value}";
		}
	}
	
}
