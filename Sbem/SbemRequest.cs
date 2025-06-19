using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public class SbemRequest
	{
		public SbemRequest(SbemModel model) 
		{ 
			Model	= model;
		}
		public bool IsFinished { get; protected set; }
		public bool WasSuccessful {  get; protected set; }
		public SbemModel Model { get; protected set; }
		public SbemProject OutputProject { get; protected set; }
		public Action<string> OnComplete { get; set; } // or Func<Task> if async
		public void Close(SbemProject project, bool success)
		{
			OutputProject	= project;
			WasSuccessful	= success;
			IsFinished		= true;
		}
	}
}
