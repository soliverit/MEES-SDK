using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// A modified version of the as-built model that break the link between the
	/// as-built and other starting states. 
	/// <para>
	/// For example, you might want to compare a heating retrofit of the as-built against
	/// the model with heating defined by the NCM conventions (2018), 6.11. 
	/// <code>NCM conventions issue 9, 6.11:
	/// Zones must be assessed as seen on the day of the inspection. However, for the purposes of this convention
	///	the SBEM activity types where zones may not be conditioned(i.e.where no HVAC systems are present)
	///	are as follows:
	///		a.Circulation Areas
	///		b.Plant rooms
	///		c.Store rooms and Warehouse Storage
	///		d.Industrial process area
	///		e.Car park
	///		f.B1 Workshops
	///		g.Toilet (only if there are no other zones in the building that have an expectation of conditioning)
	/// </code>
	/// </para>
	/// </summary>
	public class SbemScenario
	{
		public SbemScenario() { }
		public SbemModel BaseModel { get; protected set; }
		public SbemEpcModel BaseEpcInpModel { get; protected set; }
		public Dictionary<string, SbemProject> Projects { get; protected set; }

	}
}
