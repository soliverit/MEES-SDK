using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// The .err individual error record. Line number, error type, key type, corrupt value.
	/// </summary>
	public struct SbemErrorFileRecord
	{
		/// <summary>
		/// Which line in the .inp is corrupt?
		/// </summary>
		public int LineNumber { get; set; }
		/// <summary>
		/// Is this an Error, warning, or something else?
		/// </summary>
		public string ErrorLevel { get; set; }
		/// <summary>
		/// The error type. E.g (Code unknown) or (Keyword out of range)
		/// </summary>
		public string Key { get; set; }
		/// <summary>
		/// The broken line. Usuall the full line. E.g HEAT-GEN-SEFF = -0.9
		/// </summary>
		public string Value { get; set; }
	}
}
