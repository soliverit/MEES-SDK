﻿using System.Diagnostics;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// The SBEM interface. When you interact with SBEM, BRUKL, S63GEN, it's through this class.
	/// <para></para>
	/// </summary>
	public class SbemService
	{
		public const string INP_MODEL_NAME = "model.inp";
		/// <summary>
		/// Determines whether the SBEM.exe output is printed to the console.
		/// </summary>
		public bool SilentMode {  get; set; }
		public SbemService(string sbemPath, string processingPath) 
		{ 
			SbemPath			= sbemPath; 
			ProcessingPath		= processingPath;
			ProcessingInpPath	= Path.Combine(ProcessingPath, INP_MODEL_NAME);
		}
		/// <summary>
		/// Path to the SBEM executable.
		/// </summary>
		public string SbemPath { get; protected set; }
		/// <summary>
		/// Path to where SbemModel are to be processed. 
		/// <para>This is the directory of what /F SBEM.exe command line parameter.<code>E.g. "SBEM.exe /F project/model.inp"</code></para>
		/// </summary>
		public string ProcessingPath { get; protected set; }
		/// <summary>
		/// The model file 
		/// </summary>
		public string ProcessingInpPath { get; protected set; }
		/// <summary>
		/// Call SBEM. Doesn't return anything. See BuildProject(project, true)
		/// </summary>
		/// <param name="inpPath"></param>
		public void RunSBEM(string inpPath)
		{
			if (!File.Exists(inpPath))
				return;
			CallSBEM(inpPath);
		}
		protected void CallSBEM(string path)
		{
			var psi = new ProcessStartInfo
			{
				FileName = Path.Combine(SbemPath, "sbem.exe"),
				Arguments = $" /F \"{ProcessingInpPath}\"",
				WorkingDirectory = SbemPath,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true
			};

			var proc = new Process { StartInfo = psi };

			if(!SilentMode)
				proc.OutputDataReceived += (s, e) => { if (e.Data != null) Console.WriteLine(e.Data); };
			if(!SilentMode)
				proc.OutputDataReceived += (s, e) => { if (e.Data != null) Console.WriteLine(e.Data); };
			proc.ErrorDataReceived += (s, e) => { if (e.Data != null) Console.Error.WriteLine(e.Data); };

			proc.Start();
			proc.BeginOutputReadLine();
			proc.BeginErrorReadLine();
			proc.WaitForExit();
		}
		protected void FlushProcessingDirectory()
		{
			if (Directory.Exists(ProcessingPath))
			{
				foreach (var file in Directory.GetFiles(ProcessingPath))
				{
					try
					{
						File.Delete(file);
					}
					catch (IOException ex)
					{
						// File might be in use or locked
						Console.WriteLine($"Could not delete {file}: {ex.Message}");
					}
					catch (UnauthorizedAccessException ex)
					{
						// Permissions issue
						Console.WriteLine($"Access denied for {file}: {ex.Message}");
					}
				}
			}
		}
		public void RunSBEM(SbemModel sbemModel)
		{
			FlushProcessingDirectory();
			File.WriteAllText(ProcessingInpPath, sbemModel.ToString());
			CallSBEM(ProcessingInpPath);
		}
		public SbemProject BuildProject(string processingDirectory, bool runSBEM)
		{
			FlushProcessingDirectory();
			if (runSBEM)
				RunSBEM(processingDirectory);
			return SbemProject.BuildFromDirectory(processingDirectory);
		}
		public SbemProject BuildProject(SbemModel model)
		{
			FlushProcessingDirectory();
			RunSBEM(model);
			return SbemProject.BuildFromDirectory(ProcessingPath);
		}
		/*
		 * The SBEM service
		 */ 
		public bool IsInServiceMode { get; protected set; }
		protected Queue<SbemRequest> Requests { get; }	= new Queue<SbemRequest>();
		/// <summary>
		/// Number of requests queued up. Only relevant in Service mode.
		/// </summary>
		public int RequestCount { get => Requests.Count; }
		protected AutoResetEvent QueueSignal = new AutoResetEvent(false);
		public bool IsServiceRunning { get; protected set; }
		protected Thread ServiceThread { get; set; }
		public void StartSbemService()
		{
			
			ServiceThread = new Thread(Run)
			{
				IsBackground	= true,
				Name			= "SBEM Worker"
			};
			IsServiceRunning = true;
			ServiceThread.Start();
		}
		public void QueueRequest(SbemRequest request)
		{
			lock (Requests)
			{
				Requests.Enqueue(request);
				QueueSignal.Set(); // wake the thread
			}
		}
		/// <summary>
		/// Wait for the service to process the entire Queue or for a timeout.
		/// </summary>
		/// <param name="timeout"></param>
		/// <returns>True if processing finished before the time out.</returns>
		public bool WaitForService(int timeout)
		{
			long ms = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + timeout;
			while (Requests.Count > 0)
			{
				if (ms < DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
					return false;
				Thread.Sleep(10);
			}
			return true;
		}
		/// <summary>
		/// Turn of the SBEM service if it's running.
		/// </summary>
		public void StopService()
		{
			IsServiceRunning	= false;
		}
		private void Run()
		{

			while (IsServiceRunning)
			{
				SbemRequest request = null;
				lock (Requests)
					if (Requests.Count > 0)
						request = Requests.Dequeue();
				if (request != null)
				{
					
					SbemProject newProject;
					try
					{
						Console.WriteLine($"Doing SBEM Process for 1 of {Requests.Count} requests");
						newProject	= BuildProject(request.Model);
						request.Close(newProject, true);
						request.OnComplete?.Invoke("shoe");	// TODO: Set up 
					}
					catch (Exception ex)
					{
						Console.WriteLine($"SBEM call failed: {ex.Message}");
						request.Close(null, false);
					}
				}
				else
				{
					QueueSignal.WaitOne(); // wait until signal to check again
				}
			}
		}

		public void Dispose()
		{
			IsServiceRunning = false;
			QueueSignal.Set(); // wake thread to let it exit
			ServiceThread.Join();
			QueueSignal.Dispose();
		}
	}
}
