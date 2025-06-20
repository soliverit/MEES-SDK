// C# conversion of Buildingset class coming from the original Python logic.
// This assumes the existence of compatible Building and ScoreStruct classes.

using CsvHelper;
using CsvHelper.Configuration;
using MeesSDK.ML;
using MeesSDK.RdSAP;
using MeesSDK.RdSAP.Reference;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.IO;
using System.Linq;
namespace MeesSDK.RsSAP
{
	public class RdSAPBuildingSet : IReadOnlyList<RdSAPBuilding>
	{
		public static string BEST_CHOICE						= "best_choice";
		public static string CURRENT_ENERGY_EFFICIENCY			= "CURRENT_ENERGY_EFFICIENCY";
		public static List<string> INT_KEYS						= new() { CURRENT_ENERGY_EFFICIENCY };
		public int Count { get => Buildings.Count; }
		protected List<RdSAPBuilding> Buildings { get; set; }	= new();
		public double Area										= 0.0;

		public static RdSAPBuildingSet LoadDataSet(string path, RdSAPReferenceDataSet? reference=null)
		{
			RdSAPBuildingSet set = new RdSAPBuildingSet();
			if (path.StartsWith("https"))
			{
				//using var client = new System.Net.Http.HttpClient();
				//var result = client.Send(new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Head, path));
				//if (!result.IsSuccessStatusCode)
				//{
				//	Console.WriteLine($"Error: Can't find file: {path}");
				//	return set;
				//}
			}
			else if (!File.Exists(path))
			{
				Console.WriteLine($"Error: Can't find file: {path}");
				return set;
			}

			List<Dictionary<string, string>> records = new List<Dictionary<string, string>>();

			using (var reader = new StreamReader(path))
			using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
			{
				csv.Read();
				string[] headers	= csv.Context.Parser.Record;
				while (csv.Read())
				{
					var row		= new Dictionary<string, string>();
					var cells	= csv.Context.Parser.Record;
					for (int headerID = 0; headerID < headers.Length; headerID++)
						row[headers[headerID]] = cells[headerID];
					if (reference == null)
						set.AddBuilding(new RdSAPBuilding(row));
					else
						set.AddBuilding(new RdSAPBuilding(row, reference));
				}
			}

			foreach (Dictionary<string, string> row in records)
			{
				var building = new RdSAPBuilding(row);
				set.Append(building);
			}
			return set;
		}
		public static RdSAPBuildingSet FromLinQ2DB(IQueryable<Depc> depcs)
		{
			return FromLinQ2DB(depcs.ToList());
		}
		public static RdSAPBuildingSet FromLinQ2DB(List<Depc> depcs)
		{
			RdSAPBuildingSet	set = new RdSAPBuildingSet();
			for(int depcID = 0; depcID < depcs.Count; depcID++)
			{

			}
			return set;
		}
		public void AddBuilding(RdSAPBuilding building)
		{
			Buildings.Add(building);
			Area += building.TotalFloorArea;
		}
		public static string[] ML_FEATURE_LABELS = new string[]
		{

			"DEPCEnergyEfficiency",
			"ConstructionAgeIndex",
			"ExtensionCount",
			"LowEnergyLighting",
			"LightingPowerW",
			"DEPCEnergyEfficiencyPotential",
			"HeatingCostPotential",
			"FloorOrdinalEnergyEfficiency",
			"HeatingControlOrdinalEnergyEfficiency",
			"HotWaterOrdinalEnergyEfficiency",
			"LightingOrdinalEnergyEfficiency",
			"WindowsOrdinalEnergyEfficiency",
			"WallsOrdinalEnergyEfficiency",
			"FloorUValue",
			"RoofUValue",
			"WallUValue",
			"TotalFloorArea",
			"WindowArea",
			"SolarPanelArea",
			"(float)NumberOfWetRooms",
			"(float)NumberOfHeatedRooms",
			"(float)NumberOfHabitableRooms",
			"Convert.ToSingle(enumeratedRoofDescriptions[D])",
			"MainFuelFactor"
		};
		public EstimatorInputData GetMLData()
		{
			EstimatorInputData data = new EstimatorInputData(ML_FEATURE_LABELS);
			int[] enumeratedRoofDescriptions = GetEnumeratedProperty("ROOF_DESCRIPTION");
			for (int buildingID = 0; buildingID < Buildings.Count; buildingID++)
			{
				RdSAPBuilding building = Buildings[buildingID];
				data.AddRow(new float[]
				{
					// Target
					building.DEPCEnergyEfficiency,	
					// General
					building.ConstructionAgeIndex,
					building.ExtensionCount,
					// Lighting
					building.LowEnergyLighting,
					// Potential states
					building.DEPCEnergyEfficiencyPotential,
					building.HeatingCostPotential,
					// Ordinal efficiencies
					building.FloorOrdinalEnergyEfficiency,
					building.HeatingControlOrdinalEnergyEfficiency,
					building.HotWaterOrdinalEnergyEfficiency,
					building.LightingOrdinalEnergyEfficiency,
					building.WindowsOrdinalEnergyEfficiency,
					building.WallsOrdinalEnergyEfficiency,
					// U-Value
					building.FloorUValue,
					building.RoofUValue,
					building.WallUValue,
					// Areas
					building.TotalFloorArea,
					building.WindowArea,
					building.SolarPanelArea,
					(float)building.NumberOfWetRooms,
					(float)building.NumberOfHeatedRooms,
					(float)building.NumberOfHabitableRooms,
					// Enums etc
					Convert.ToSingle(enumeratedRoofDescriptions[buildingID]),
					building.MainFuelFactor
				});
			}
			return data;
		}
		Dictionary<string, int> map	= new Dictionary<string, int>();
		int counter;
		public int[] GetEnumeratedProperty(string depcRegisterPropertyName)
		{
			int[] enumerated = new int[Length];
			for (int buildingID = 0; buildingID < Length; buildingID++)
			{
				RdSAPBuilding building = Buildings[buildingID];
				if (!map.ContainsKey(building.DEPCRegisterData[depcRegisterPropertyName]))
					map[building.DEPCRegisterData[depcRegisterPropertyName]] = counter++;

				enumerated[buildingID] = map[building.DEPCRegisterData[depcRegisterPropertyName]];
				// use enumId here
			}
			return enumerated;
		}
		public void FilterCorrupt()
		{
			List<RdSAPBuilding> buildings = new List<RdSAPBuilding>();
			for (int buildingID = 0; buildingID < Length; buildingID++)
				if (!Buildings[buildingID].IsCorrupt)
					buildings.Add(Buildings[buildingID]);
			Buildings   = buildings;
		}
		//public void WriteFile(string path)
		//{
		//	using var writer = new StreamWriter(path);
		//	var first = Buildings.First();
		//	var columnNames = first.data.Keys.ToList();
		//	CsvTools.WriteDictionaryList(writer, columnNames, Buildings.Select(b => b.data).ToList());
		//}

		//public void Append(Building building) => Buildings.Add(building);
		//public void Shuffle() => Buildings.Shuffle();

		//public Buildingset GetByRating(string rating)
		//{
		//	var set = new Buildingset();
		//	foreach (var building in Buildings)
		//		if (building.toRating() == rating)
		//			set.Append(building);
		//	return set;
		//}

		//public Buildingset GetByRatings(List<string> ratings)
		//{
		//	var set = new Buildingset();
		//	foreach (var building in Buildings)
		//		if (ratings.Contains(building.toRating()))
		//		{
		//			set.Append(building);
		//			break;
		//		}
		//	return set;
		//}

		//public int RetrofitCount() => Buildings.Sum(b => b.retrofitCount);

		//public List<Buildingset> Partition(int count)
		//{
		//	var sets = Enumerable.Range(0, count).Select(_ => new Buildingset()).ToList();
		//	int counter = count;
		//	foreach (var building in Buildings)
		//	{
		//		sets[counter % count].Append(building);
		//		counter++;
		//	}
		//	return sets;
		//}

		//public Dictionary<string, double> GetCheapestToRating(string rating)
		//{
		//	double cost = 0.0, points = 0.0, metPoints = 0.0;
		//	foreach (var building in Buildings)
		//	{
		//		double pointDiff = building.toRating(rating);
		//		if (pointDiff <= 0) continue;
		//		points += pointDiff;
		//		var retrofit = building.getCheapestRetrofitToEfficiency(Building.ratingLowerBound(rating));
		//		if (retrofit != null)
		//		{
		//			cost += retrofit.cost;
		//			metPoints += retrofit.difference;
		//		}
		//	}
		//	return new Dictionary<string, double> { { "metPoints", metPoints }, { "cost", cost }, { "points", points } };
		//}

		//	public List<int> GetCheapestToRatingState(string rating)
		//	{
		//		var state = new List<int>();
		//		foreach (var building in Buildings)
		//		{
		//			double pointDiff = building.toRating("D");
		//			if (pointDiff == 0)
		//				state.Add(0);
		//			else
		//				state.Add(building.getCheapestRetrofitToEfficiencyID(Building.ratingLowerBound("D")));
		//		}
		//		return state;
		//	}

		//	public Dictionary<string, double> ScoreState(List<string> state)
		//	{
		//		double cost = 0, points = 0;
		//		int bCount = 0;
		//		for (int i = 0; i < Buildings.Count; i++)
		//		{
		//			var retrofit = Buildings[i].retrofitHash[state[i]];
		//			cost += retrofit.cost;
		//			points += retrofit.difference;
		//			if (retrofit.cost > 0) bCount++;
		//		}
		//		return new Dictionary<string, double> {
		//	{ "cost", cost },
		//	{ "points", points },
		//	{ "Buildings", Buildings.Count },
		//	{ "affected_Buildings", bCount }
		//};
		//	}

		//	public Dictionary<string, double> GetCurrentStateResults()
		//	{
		//		double cost = 0, points = 0;
		//		int count = 0, mCount = 0;
		//		foreach (var building in Buildings)
		//		{
		//			var retrofit = building.retrofitHash[building.currentState];
		//			if (!retrofit.isAsBuilt)
		//			{
		//				cost += retrofit.cost;
		//				points += retrofit.difference;
		//				count++;
		//				mCount += retrofit.measureCount;
		//			}
		//		}
		//		return new Dictionary<string, double> {
		//	{ "cost", cost },
		//	{ "points", points },
		//	{ "Buildings", Buildings.Count },
		//	{ "affected_Buildings", count },
		//	{ "totalRetrofitCount", RetrofitCount() },
		//	{ "retrofitCount", count },
		//	{ "measureCount", mCount }
		//};
		//	}

		//	public bool HasCurrentState() => Buildings.All(b => b.data.ContainsKey(BEST_CHOICE));

		//	public int ToRatingDifference(string rating)
		//	{
		//		int total = 0;
		//		foreach (var building in Buildings)
		//		{
		//			int diff = Building.ratingLowerBound(rating) - (int)building.efficiency;
		//			if (diff > 0) total += diff;
		//		}
		//		return total;
		//	}

		//	public ScoreStruct GetScoreStruct(string key)
		//	{
		//		var scoreStruct = new ScoreStruct();
		//		foreach (var building in Buildings)
		//		{
		//			var retrofit = building.getRetrofit(Convert.ToInt32(building.data[key]));
		//			scoreStruct.cost += retrofit.cost;
		//			scoreStruct.points += retrofit.efficiency;
		//			scoreStruct.difference += retrofit.difference;
		//		}
		//		return scoreStruct;
		//	}

		//	public void Merge(Buildingset otherSet)
		//	{
		//		foreach (var building in otherSet)
		//			Append(building);
		//	}

		//	public Buildingset Clone()
		//	{
		//		var newSet = new Buildingset();
		//		foreach (var building in Buildings)
		//			newSet.Append(building);
		//		return newSet;
		//	}

		//	public void FilterByFunction(Func<Building, bool> predicate)
		//	{
		//		Buildings.RemoveAll(b => !predicate(b));
		//	}

		//	public void FilterHarderMeasures() => Buildings.ForEach(b => b.filterHarderMeasures());
		//	public void FilterRetrofitsByM2Rate(double rate) => Buildings.ForEach(b => b.filterRetrofitsByM2Rate(rate));
		//	public void FilterRetrofitsByImpactRatio(double threshold) => Buildings.ForEach(b => b.filterRetrofitsByImpactRatio(threshold));
		//	public void FilterRetrofitsByCostAndRatio(double cost, double ratio) => Buildings.ForEach(b => b.filterRetrofitsByCostAndRatio(cost, ratio));

		//	public void FilterZeroOptionBuildings()
		//	{
		//		Buildings.RemoveAll(b => b.retrofitCount <= 1);
		//	}

		public int Length => Buildings.Count;
		public IEnumerator<RdSAPBuilding> GetEnumerator() => Buildings.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		public RdSAPBuilding this[int index] => Buildings[index];
	}
}
