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
	public abstract class RetrofitBase<T> where T : SbemSpatialObject
	{
		protected RetrofitBase(SbemModel model) 
		{
			Model = model;
		}
		/// <summary>
		/// The SbemObjectSet<T> of the objects modified by the Retrofit.
		/// </summary>
		public SbemObjectSet<T> ModifiedObjects { get; } = new SbemObjectSet<T>();
		public SbemModel Model { get; }
		/// <summary>
		/// The cost of the Retrofit. Whatever currency works for you.
		/// </summary>
		public virtual float Cost
		{
			get
			{
				if (_cost == 0)
					for (int zoneID = 0; zoneID < ModifiedObjects.Length; zoneID++)
						_cost += ModifiedObjects[zoneID].Area * 30;
				return _cost;
			}
			protected set { _cost = value; }
		}
		protected float _cost;
		/// <summary>
		/// The area of the objects modifed by the Retrofit.
		/// </summary>
		public virtual float Area
		{
			get
			{
				if (_area == 0)
					for (int zoneID = 0; zoneID < ModifiedObjects.Length; zoneID++)
						_area += ModifiedObjects[zoneID].Area;
				return _area;
			}
			protected set { _area = value; }
		}
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
		public void AddModifiedObject(T modifiedObject)
		{
			ModifiedObjects.Add(modifiedObject);
			Area	+= modifiedObject.Area;
		}
	}
}
