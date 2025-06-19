using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.ML
{
	public class LightGBMInputData<T> where T : class, IValidatable
	{
		public LightGBMInputData(string[] features) 
		{
			Features = features;
		}
		public string[] Features { get; protected set; }
		public List<T> Objects { get; protected set; }	= new List<T>();
		public int Count { get => Objects.Count; }
		public void AddObject(T obj)
		{
			Objects.Add(obj);
		}
		public void RemoveCorruptObjects()
		{
			List<T> objects = new List<T>();
			for (int objectID = 0; objectID < Objects.Count; objectID++)
				if (!Objects[objectID].HasError)
					objects.Add(Objects[objectID]);
			Objects = objects;
		}
		public List<T> GetFirstRecords(int count)
		{
			List<T> objects = new List<T>();
			for (int objectID = 0; objectID < count; objectID++)
				objects.Add(Objects[objectID]);
			return objects;
		}
		public List<T> GetLastRecords(int count)
		{
			List<T> objects = new List<T>();
			for (int objectID = Count - count; objectID < Count; objectID++)
				objects.Add(Objects[objectID]);
			return objects;
		}
		public List<T> GetRecordsFrom(int start, int count)
		{
			List<T> objects = new List<T>();
			count = count < Count ? count : Count;
			for (int objectID = start; objectID < count ; objectID++)
				objects.Add(Objects[objectID]);
			return objects;
		}
		public LightGBMInputData<T> CloneFirstRecords(int count)
		{
			LightGBMInputData<T> objects = new LightGBMInputData<T>(Features.ToArray());

			for (int objectID = 0; objectID < count; objectID++)
				objects.AddObject(Objects[objectID]);
			return objects;
		}
		public LightGBMInputData<T> CloneLastRecords(int count)
		{
			LightGBMInputData<T> objects = new LightGBMInputData<T>(Features.ToArray());
			for (int objectID = Count - count; objectID < Count; objectID++)
				objects.AddObject(Objects[objectID]);
			return objects;
		}
		public LightGBMInputData<T> CloneRecordsFrom(int start, int count)
		{
			LightGBMInputData<T> objects = new LightGBMInputData<T>(Features.ToArray());
			count = count < Count ? count : Count;
			for (int objectID = start; objectID < count; objectID++)
				objects.AddObject(Objects[objectID]);
			return objects;
		}
	}
}
