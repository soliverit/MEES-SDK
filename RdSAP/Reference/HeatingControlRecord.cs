using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.RdSAP.Reference
{
	namespace MOOSandbox.RdSAP.Reference
	{
		public class HeatingControlRecord : ReferenceDataRecordBase
		{
			public string Measure { get; }
			public int AutoChange { get; }
			public int ApplianceThermostat { get; }
			public int CommunityControls { get; }
			public int TRV { get; }
			public int Programmer { get; }
			public int RoomThermostat { get; }
			public int FlatRate { get; }
			public string Comment { get; }

			public HeatingControlRecord(string measure, int auto, int appThermo, int commCtrl,
										int trv, int prog, int roomThermo, int flatRate, string comment)
			{
				Measure = measure;
				AutoChange = auto;
				ApplianceThermostat = appThermo;
				CommunityControls = commCtrl;
				TRV = trv;
				Programmer = prog;
				RoomThermostat = roomThermo;
				FlatRate = flatRate;
				Comment = comment;
			}
		}
	}
}
