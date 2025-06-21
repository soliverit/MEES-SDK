using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// A place to work with the as-built project and other base states.
	/// <para>
	/// Some situations break the relationship between the as-built model
	/// and alternative starting scenarios. In those instances, it's 
	/// necessary to branch into scenarios:
	/// </para>
	/// <para>
	/// For example, you might want to compare a heating retrofit of the as-built against
	/// the model with heating defined by the NCM conventions (2018), 6.11. Migrating zones can cause changes in the
	/// reference and notional model, resulting in a different SER or TER. Since the EPC rating's BER / SER * 50, only 
	/// models with consistent SERs are comparable.
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
	public class SbemWorkspace
	{
		/// <summary>
		/// The as-built model. The base for all operations.
		/// </summary>
		public SbemModel AsBultModel { get; }
		/// <summary>
		/// The as-built project. All inputs and outputs
		/// </summary>
		public SbemProject AsBuiltProject { get; }
		/// <summary>
		/// The thing that handles interactoins between the library and SBEM.exe
		/// </summary>
		public SbemService SbemCaller { get; }
		public SbemWorkspace(SbemProject project, SbemService sbem)
		{
			SbemCaller		= sbem;
			AsBuiltProject	= project;;
			AsBultModel		= project.AsBuiltSbemModel;
		}
	}
}
