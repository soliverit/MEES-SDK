using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// An object for the SbemService Queue used to ensure thread-safe SBEM 
	/// calls.
	/// processing. 
	/// </summary>
	public class SbemRequest
	{
		public SbemRequest(SbemModel model) 
		{ 
			Model	= model;
		}
		/// <summary>
		/// Has the request been processed? Regardless of success
		/// </summary>
		public bool IsFinished { get; protected set; }
		/// <summary>
		/// Was the SBEM call successful? Regardless of whether SBEM was able to process
		/// the input model.
		/// </summary>
		public bool WasSuccessful {  get; protected set; }
		/// <summary>
		/// The model that's processed by SBEM
		/// </summary>
		public SbemModel Model { get; protected set; }
		/// <summary>
		/// The complete inputs and outputs of SBEM.
		/// </summary>
		public SbemProject OutputProject { get; protected set; }
		/// <summary>
		/// Something to do after the process is complete.
		/// </summary>
		public Action<string> OnComplete { get; set; } // or Func<Task> if async
		/// <summary>
		/// Update the request to being complete. Intended for SbemService only.
		/// </summary>
		/// <param name="project"></param>
		/// <param name="success"></param>
		public void Close(SbemProject project, bool success)
		{
			OutputProject	= project;
			WasSuccessful	= success;
			IsFinished		= true;
		}
	}
}
