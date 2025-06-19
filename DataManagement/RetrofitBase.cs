using MeesSDK.Sbem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.DataManagement
{
	public abstract class RetrofitBase<T> where T : IBuildingEntity
	{
		public List<T> AffectedObejcts { get; } = new List<T>();
		public abstract float Cost {  get; set; }
		protected float _cost { get; set; } = -1;
		public abstract float Area {  get; set; }
		protected float _area { get; set; } = -1;
		public abstract void Apply();
		public SbemModel Model { get; }
		public RetrofitBase(SbemModel model) 
		{ 
			Model = model; 
		}
	}
}
