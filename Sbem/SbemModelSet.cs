using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public class SbemModelSet 
	{
		public SbemModelSet() { }
		public SbemModelSet(string description) 
		{
			Description = description;
		}
		/// <summary>
		/// Number of SbemModel in the set
		/// </summary>
		public int Count { get => SbemModels.Count; }
		/// <summary>
		/// Subscripted accessor
		/// </summary>
		/// <param name="idx"></param>
		/// <returns></returns>
		public SbemModel this[int idx]
		{
			get { return (SbemModel)this[idx]; }
			protected set
			{
				this[idx] = value;
			}
		}
		/// <summary>
		/// Add a model to the set, if it's not already in it
		/// </summary>
		/// <param name="model"></param>
		public void AddModel(SbemModel model)
		{
			if(!SbemModels.Contains(model)) 
				SbemModels.Add(model);
		}
		public string Description { get; set; } = "Default";
		protected List<SbemModel> SbemModels { get; }	= new List<SbemModel>();
	}
}
