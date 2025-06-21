using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// A standrd error object. Nothing noteworthy.
	/// </summary>
	public class SbemError
	{
		public enum ErrorCode
		{
			CONTENT_FILE_NOT_EXISTS,
			CONTENT_CORRUPT,
			SBEM_OBJECT_MISSING
		};
		public ErrorCode Code { get; }
		public string Message { get; }
		public SbemError(ErrorCode code, string message)
		{
			Code	= code;
			Message	= message;
		}

	}
}
