using MathNet.Numerics;
using MathNet.Numerics.Statistics;
using MeesSDK.DataManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// The abstract SBEM inp model object.
	/// 
	/// All objects in SBEM models have the same structure (see example). This object 
	/// defines their shared features. 
	/// 
	/// <para>"SBEM" = COMPLIANCE</para>
	///	<para>	EPC-TYPE            = EPC England</para>
	///	<para>	ENG-HERITAGE        = NO</para>
	///	<para>	BR-STAGE            = As built</para>
	///	<para>	AIR-CON-INSTALLED   = No</para>
	///		..
	/// </summary>
	public abstract class SbemObject : IBuildingEntity
	{
		/// <summary>
		/// Get the associated SBEM object's type. For example, SbemHvacSystem is HVAC-SYSTEM.
		/// </summary>
		/// <returns></returns>
		public abstract string ObjectName();
		/// <summary>
		/// The name of the object. Given as the "<name>" part of the first line of an object's definition.
		/// </summary>
		public string Name { get; }
		/// <summary>
		/// Where the string properties are stored. Can include numbers that aren't treated as numbers by SBEM.
		/// </summary>
		public readonly Dictionary<string, SbemStringProperty> StringProperties	= new Dictionary<string, SbemStringProperty>();	
		/// <summary>
		/// Where the numeric properties are stored.
		/// </summary>
		public readonly Dictionary<string, SbemNumericProperty> NumericProperties = new Dictionary<string, SbemNumericProperty>();
		/// <summary>
		/// Check if a line is a close SBEM object line.
		/// </summary>
		/// <param name="line">A line from an SBEM .inp model</param>
		/// <returns></returns>
		public static bool IsCloseLine(string line)
		{
			return line.Trim() == "..";
		}
		/// <summary>
		/// The standard constructor. Takes the name of the object and the raw text lines
		/// that define the object's properties. Separating numeric and string properties is
		/// done internally.
		/// </summary>
		/// <param name="objectName">Name of the object. E.g "Cavity details" =  CONSTRUCTION</param>
		/// <param name="propertyStrings">The lines from the SBEM .inp model that define the object's properties.</param>
		public SbemObject(string objectName, List<string> propertyStrings)
		{
			Name = objectName;
			// Number matching pattern
			Regex numberPattern = new(@"^-?\d+(\.\d+)?$");

			for (int propertyID = 0; propertyID < propertyStrings.Count; propertyID++)
			{
				string rawProperty	= propertyStrings[propertyID];
				if (string.IsNullOrWhiteSpace(rawProperty))
					continue;

				// Trim full line
				string property = rawProperty.Trim();

				int equalsIndex = property.IndexOf('=');
				if (equalsIndex == -1)
					continue; // no key-value separator

				// Extract key and value
				string key = property.Substring(0, equalsIndex).Trim();
				string value = property.Substring(equalsIndex + 1).Trim();

				// Store based on type
				if (numberPattern.IsMatch(value))
				{
					// float.Parse will throw if it's invalid — in C++ this would blow up too
					SetNumericProperty(key, float.Parse(value));
				}
				else
				{
					SetStringProperty(key, value);
				}
			}
		}
		/// <summary>
		/// Determine if a line from an SBEM .inp is a header line. E.g: "Cavity wall" = CONSTRUCTION or "My project" = SBEM-PROJECT
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		public static (bool isHeader, string name, string type) GetHeaderLine(string line)
		{
			// Skip leading whitespace
			int start = 0;
			while (start < line.Length && char.IsWhiteSpace(line[start]))
				start++;

			if (start >= line.Length || line[start] != '"')
				return (false, "", "");

			int end = line.TrimEnd().Length - 1;
			if (start == end)
				return (false, "", "");

			int closeQuote = line.LastIndexOf('"');
			int equals = line.LastIndexOf('=');
			if (equals == -1 || equals < closeQuote)
				return (false, "", "");

			if (equals == end)
				return (false, "", "");

			// Find start of type name
			int typeStart = -1;
			for (int i = equals + 1; i < line.Length; i++)
			{
				if (!char.IsWhiteSpace(line[i]))
				{
					typeStart = i;
					break;
				}
			}

			if (typeStart == -1)
				return (false, "", "");

			string typeName = line.Substring(typeStart, end - typeStart + 1);
			string upperType = typeName.ToUpperInvariant();

			if (typeName != upperType)
				return (false, "", "");

			string name = line.Substring(start + 1, closeQuote - start - 1);
			return (true, name, typeName);
		}

		/// <summary>
		/// Add a numeric property to the object. Sets value regardless of whether it already existed.
		/// <para>Note: Properties are stored as SbemNumericProperty which have things for working with mixed quotes and mixed values</para>
		/// </summary>
		/// <param name="key">The name of the property</param>
		/// <param name="value">The new value</param>
		public void SetNumericProperty(string key, float value)
		{
			if (!NumericProperties.ContainsKey(key))
				NumericProperties[key] = new SbemNumericProperty(key, value);
			else
				NumericProperties[key].SetValue(value);
		}
		/// <summary>
		/// Add a string property to the object. Sets value regardless of whether it already existed.
		/// <para>Note: Properties are stored as SbemStringProperty which have things for working with mixed quotes and mixed values</para>
		/// </summary>
		/// <param name="key">The name of the property</param>
		/// <param name="value">The new value</param>
		public void SetStringProperty(string key, string value)
		{
			if (!StringProperties.ContainsKey(key))
				StringProperties[key] = new SbemStringProperty(key, value);
			else
				StringProperties[key].SetValue(value);				
		}
		/// <summary>
		/// Set  a numeric property to the object. Only affects existing SbemNumericProperty.
		/// <para>Note: Properties are stored as SbemNumericProperty which have things for working with mixed quotes and mixed values</para>
		/// </summary>
		/// <param name="key">The name of the property</param>
		/// <param name="value">The new value</param>
		public void SetNumericPropertyIfExists(string key, float value)
		{
			if (NumericProperties.ContainsKey(key))
				NumericProperties[key].SetValue(value);
		}
		/// <summary>
		/// Set a string property to the object. Only affects existing SbemNumericProperty.
		/// <para>Note: Properties are stored as SbemStringProperty which have things for working with mixed quotes and mixed values</para>
		/// </summary>
		/// <param name="key">The name of the property</param>
		/// <param name="value">The new value</param>
		public void SetStringPropertyIfExists(string key, string value)
		{
			if (StringProperties.ContainsKey(key))
				StringProperties[key].SetValue(value);
		}
		/// <summary>
		/// Return an SbemStringProperty by its name. E.g. LIGHT-TYPE or EPC-TYPE.
		/// </summary>
		/// <param name="key"></param>
		/// <returns>SbemStringProperty</returns>
		public SbemStringProperty GetStringProperty(string key)
		{
			return StringProperties[key];
		}
		/// <summary>
		/// Return an SbemNumericProperty by its name. E.g U-Value or Area
		/// </summary>
		/// <param name="key"></param>
		/// <returns>SbemNumericProperty</returns>
		public SbemNumericProperty GetNumericProperty(string key)
		{
			return NumericProperties[key];
		}
		/// <summary>
		/// Does the object have a numeric property by with the named passed as key?
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool HasNumericProperty(string key) {  return NumericProperties.ContainsKey(key); }
		/// <summary>
		/// Does the object have a string property by with the named passed as key?
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool HasStringProperty(string key) {return StringProperties.ContainsKey(key); }
		/// <summary>
		/// Does a string property exist and is its value the same as the input?
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool PropertyEquals(string key, string value)
		{
			return HasStringProperty(key) && StringProperties[key].Value == value;
		}
		/// <summary>
		/// 
		/// Does a numeric property exist and is its value the same as the input?
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool PropertyEquals(string key, float value)
		{
			return HasNumericProperty(key) && NumericProperties[key].Value == value;
		}
		/// <summary>
		/// Does a numeric property exist and is its rounded value equal the input?
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool PropertyEquals(string key, int value)
		{
			return HasNumericProperty(key) && NumericProperties[key].Value.Round(0) == value;
		}
		public bool PropertyLessThan(string key, float value)
		{
			return HasNumericProperty(key) && NumericProperties[key].Value < value;
		}
		public bool PropertyGreaterThan(string key, float value)
		{
			return HasNumericProperty(key) && NumericProperties[key].Value > value;
		}
		/// <summary>
		/// Is a string property value in the input array
		/// </summary>
		/// <param name="key"></param>
		/// <param name="values"></param>
		/// <returns></returns>
		public bool PropertyValueIn(string key, string[] values)
		{
			if(!HasStringProperty(key)) 
				return false;
			for (int valueID = 0; valueID < values.Length; valueID++)
				if (StringProperties[key].Value == values[valueID])
					return true;
			return false;
		}
		/// <summary>
		/// Is a numeric property value in the input array
		/// </summary>
		/// <param name="key"></param>
		/// <param name="values"></param>
		/// <returns></returns>
		public bool PropertyValueIn(string key, float[] values)
		{
			if (!HasNumericProperty(key))
				return false;
			for (int valueID = 0; valueID < values.Length; valueID++)
				if (NumericProperties[key].Value == values[valueID])
					return true;
			return false;
		}
		/// <summary>
		/// Rounded Property value is in integer array.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="values"></param>
		/// <returns></returns>
		public bool PropertyValueIn(string key, int[] values)
		{
			if (!HasNumericProperty(key))
				return false;
			int numericValue	= (int) NumericProperties[key].Value.Round(0);
			for (int valueID = 0; valueID < values.Length; valueID++)
				if (numericValue == values[valueID])
					return true;
			return false;
		}

		/// <summary>
		/// Convert the object into an SBEM .inp model format E.g.
		/// <para>"SBEM" = COMPLIANCE</para>
		///	<para>	EPC-TYPE            = EPC England</para>
		///	<para>	ENG-HERITAGE        = NO</para>
		///	<para>	BR-STAGE            = As built</para>
		///	<para>	AIR-CON-INSTALLED   = No</para>
		/// </summary>
		/// <returns></returns>
		public void RemoveStringProperty(string propertyName)
		{
			StringProperties.Remove(propertyName);
		}
		/// <summary>
		/// Remove a numeric property
		/// </summary>
		/// <param name="propertyName"></param>
		public void RemoveNumericProperty(string propertyName)
		{
			NumericProperties.Remove(propertyName);
		}
		/// <summary>
		/// Convert to .inp SBEM object format string
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine($"\"{Name}\" = {ObjectName()}");	
			foreach (SbemStringProperty prop in StringProperties.Values)
				sb.AppendLine(prop.ToString());

			foreach (SbemNumericProperty prop in NumericProperties.Values)
				sb.AppendLine(prop.ToString());
			sb.AppendLine(" ..");
			return sb.ToString();
		}

	}
}
