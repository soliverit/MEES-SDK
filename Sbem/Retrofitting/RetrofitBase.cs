using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem.Retrofitting
{
	/// <summary>
	/// Retrofit base class: Build your SbemModel Retrofits with this.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class RetrofitBase
	{
		/// <summary>
		/// HVAC systems modified by the retrofit
		/// </summary>
		public SbemObjectSet<SbemHvacSystem> ModifiedHvacSystems { get; protected set; }
		/// <summary>
		/// Activity Spaces modified by the retrofit
		/// </summary>
		public SbemObjectSet<SbemZone> ModifiedZones { get; protected set; }
		/// <summary>
		/// Constructions modified by the retrofit
		/// </summary>
		public SbemObjectSet<SbemConstruction> ModifiedConstructions { get; protected set; }
		/// <summary>
		/// Glasses modified by the retrofit
		/// </summary>
		public SbemObjectSet<SbemGlass> ModifiedGlasses { get; protected set; }
		/// <summary>
		/// The PVS installed by the retrofit, if it happens
		/// </summary>
		public SbemObjectSet<SbemPvs> InstalledPVS { get; protected set; }
		/// <summary>
		/// The .inp model
		/// </summary>
		public SbemModel Model { get; }
		protected RetrofitBase(SbemModel model) 
		{
			Model = model;
		}
		/// <summary>
		/// The cost of the Retrofit. Whatever currency works for you.
		/// </summary>
		public virtual float Cost { get; protected set; }
		/// <summary>
		/// A placeholder for Cost so it's not necessary to calculate it every time
		/// </summary>
		protected float _cost;
		/// <summary>
		/// The area of the objects modifed by the Retrofit.
		/// </summary>
		public virtual float Area {get; protected set;}
		/// <summary>
		/// A placeholder for Area so it's not necessary to calculate it every time
		/// </summary>
		protected float _area;
		/// <summary>
		/// Apply the Retrofit.
		/// <para>For example:</para>
		/// <code>
		/// For every HVAC with an LTHW boiler:
		///   If SEFF &lt; 0.8:
		///     Set HVAC SEFF = 0.9
		///     Add HVAC to modified objects
		/// </code>
		/// </summary>
		public abstract void Apply();
		/// <summary>
		/// Add an HVAC system to the modified objects 
		/// </summary>
		/// <param name="hvac"></param>
		public void AddModifiedObject(SbemHvacSystem hvac)
		{
			ModifiedHvacSystems.Add(hvac);
			_area += hvac.Area;
		}
		/// <summary>
		/// Add a zone to the modified objects
		/// </summary>
		/// <param name="zone"></param>
		public void AddModifiedObject(SbemZone zone)
		{
			ModifiedZones.Add(zone);
			_area = zone.Area;
		}
		/// <summary>
		/// Add an opaque surface material definition to the modified objects
		/// </summary>
		/// <param name="construction"></param>
		public void AddModifiedObject(SbemConstruction construction)
		{
			ModifiedConstructions.Add(construction);
			_area   += construction.Area;
		}
		/// <summary>
		/// Add a transparent surface material definition to the modified objects
		/// </summary>
		/// <param name="glass"></param>
		public void AddModifiedObject(SbemGlass glass)
		{
			ModifiedGlasses.Add(glass);
			_area   += glass.Area;
		}
		/// <summary>
		/// Add a solar panel system to the modified objects
		/// </summary>
		/// <param name="pvs"></param>
		public void AddModifiedObject(SbemPvs pvs)
		{
			InstalledPVS.Add(pvs);
			_area += pvs.Area;
		}
	}
}
