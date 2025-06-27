using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// SbemObjects with areas (m²)
	/// </summary>
	public abstract class SbemSpatialObject : SbemObject
	{
		/// <summary>
		/// Area (m²). Not specific. Can be floor space, wall, window, solar panel,...
		/// </summary>
		public virtual float Area { get; protected set; }
		/// <summary>
		/// Area (m²) excluding embedded windows and doors
		/// </summary>
		public virtual float SurfaceArea{ get; protected set; }
		public SbemSpatialObject(string name, List<string> properties) : base(name, properties) 
		{
			// Some objects have an area in their definition
			if (HasNumericProperty("AREA"))
				Area = GetNumericProperty("AREA").Value;
			// Others are aggregates of other objects' area. For example, SbemHvacSystem is SbemZone[] Area
			else
				Area = 0;
		}
	}
}
