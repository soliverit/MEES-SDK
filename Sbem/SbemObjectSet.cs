using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	/// <summary>
	/// A set manager for SBEM objects. 
	/// <para>It's just a collection manager, nothing exciting. Used for retrieving, filtering and modifying SbemObjects.</para>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class SbemObjectSet<T> where T : SbemObject
	{
		/// <summary>
		/// Where the objects are kept
		/// </summary>
		public List<T> Objects { get; protected set; } = new();
		/// <summary>
		/// A name-based dictionary for finding objects by name.
		/// </summary>
		public readonly Dictionary<string, T> ObjectDictionary = new();

		/// <summary>
		/// Add an object to the set
		/// </summary>
		/// <param name="obj"></param>
		public void Add(T obj)
		{
			Objects.Add(obj);
			ObjectDictionary[obj.Name] = obj;
		}
		/// <summary>
		/// The number of objects in the set.
		/// </summary>
		public int Length { get => Objects.Count; }
		/// <summary>
		/// Delete all objects from the set
		/// </summary>
		public void Clear()
		{
			Objects.Clear();
			ObjectDictionary.Clear();
		}
		public SbemObjectSet<T> Copy()
		{
			SbemObjectSet<T> copy	= new SbemObjectSet<T>();
			for(int objectID =0; objectID < Objects.Count; objectID++) 
				copy.Add(Objects[objectID]);
			return copy;
		}
		/// <summary>
		/// Subscripted access to the ObjectDictionary
		/// </summary>
		/// <param name="key">The SBEM object name</param>
		/// <returns></returns>
		public T this[string key]
		{
			get => ObjectDictionary[key]; 
		}
		/// <summary>
		/// Subscripted access to the Objects list.
		/// </summary>
		/// <param name="idx">The position in the list</param>
		/// <returns></returns>
		public T this[int idx]
		{
			get => Objects[idx];
		}
		/// <summary>
		/// Pass a function, return the sum of doing it with every object.
		/// </summary>
		/// <param name="func"></param>
		/// <returns></returns>
		public float Sum(Func<T, float> func) => Objects.Sum(func);
		/// <summary>
		/// Do something with the set but don't bother returning anything
		/// </summary>
		/// <param name="action"></param>
		public void Each(Action<T> action)
		{
			foreach (var obj in Objects)
				action(obj);
		}
		/// <summary>
		/// Select objects from this set and return in a new Set
		/// </summary>
		/// <param name="func"></param>
		/// <returns></returns>
		public SbemObjectSet<T> Select(Func<T, bool> func)
		{
			SbemObjectSet<T> selected = new SbemObjectSet<T>();
			for (int objectID = 0; objectID < Objects.Count; objectID++)
				if (func(Objects[objectID]))
					selected.Add(Objects[objectID]);
			return selected;
		}
		/// <summary>
		/// Remove object that meet criteria by function..
		/// </summary>
		/// <param name="func">The function that determines if it's to be removed.</param>
		public void Filter(Func<T, bool> func)
		{
			List<T> keptObjects = new List<T>();
			for(int objectID = 0;objectID < Objects.Count; objectID++)
				if (!func(Objects[objectID]))
					keptObjects.Add(Objects[objectID]);
			Objects = keptObjects;
		}
		/// <summary>
		/// Remove objects that don't meet criteria by function.
		/// </summary>
		/// <param name="func">The function that determines if it's to be removed.</param>
		public void Keep(Func<T, bool> func)
		{
			List<T> keptObjects = new List<T>();
			for (int objectID = 0; objectID < Objects.Count; objectID++)
				if (func(Objects[objectID]))
					keptObjects.Add(Objects[objectID]);
			Objects = keptObjects;
		}
		/// <summary>
		/// Part of IEnumerable. 
		/// </summary>
		/// <returns></returns>
		public IEnumerator<T> GetEnumerator() => Objects.GetEnumerator();
	}
}
