using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// The .err output object. Stores records from SBEM .err files for the as-built, notional,
	/// and reference SbemModel.
	/// </summary>
	// Example .err
	// * [151]  *** ERROR 1 *** : (Code unknown): SFP-CHECK = NOPEs;l
	//*[157]*** ERROR 2 *** : (Keyword out of range): HEAT-GEN-SEFF = -0.9
	//
	// *** ERRORS     *** Number of errors:    2
	// *** WARNINGS   *** Number of warnings:  0
	// *** CWARNINGS  *** Number of cwarnings: 0
	// END

	public class SbemErrorFile
	{
		public List<SbemErrorFileRecord> Errors { get; }	= new List<SbemErrorFileRecord>();
		public static SbemErrorFile ParseErrorFile(string path)
		{
			SbemErrorFile errorFile	= new SbemErrorFile();

			// Matches lines like: * [172]  **  WARN  1  ** : (Other warnings): LAMP-BALLAST-EFF = 200
			var regex = new Regex(@"\[\s*(\d+)\]\s+\*+\s+(\w+)\s+\d+\s+\*+\s*:\s*\(([^)]+)\):\s+(\S+)\s*=\s*(.+)");

			foreach (var line in File.ReadLines(path))
			{
				var match = regex.Match(line);
				if (!match.Success)
					continue;

				errorFile.AddRecord(new SbemErrorFileRecord
				{
					LineNumber	= int.Parse(match.Groups[1].Value),
					ErrorLevel	= match.Groups[3].Value.Trim(),
					Key			= match.Groups[4].Value.Trim(),
					Value		= match.Groups[5].Value.Trim()
				});
			}

			return errorFile;
		}
		public void AddRecord(SbemErrorFileRecord record)
		{
			Errors.Add(record);
		}
	}
}
