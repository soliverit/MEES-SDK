using Microsoft.ML;
using Microsoft.ML.Trainers.LightGbm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LgbmModel = Microsoft.ML.Trainers.LightGbm.LightGbmRegressionModelParameters;
using LgbmTransformer = Microsoft.ML.Data.RegressionPredictionTransformer<Microsoft.ML.Trainers.LightGbm.LightGbmRegressionModelParameters>;
using LgbmPipeline = Microsoft.ML.Data.TransformerChain<Microsoft.ML.Data.RegressionPredictionTransformer<Microsoft.ML.Trainers.LightGbm.LightGbmRegressionModelParameters>>;
using Microsoft.ML.Data;
namespace MeesSDK.ML
{
	public class LightGBMEstimator<T> where T : class, IValidatable
	{
		public static float[] ExtractColumnFromIDataView(IDataView data, string columnName)
		{
			float[] values = new float[(int)data.GetRowCount()];

			// 1. Get the column index
			int colIndex = data.Schema.GetColumnOrNull(columnName)?.Index
				?? throw new Exception($"Column '{columnName}' not found.");

			// 2. Get a cursor to iterate the rows
			using var cursor = data.GetRowCursor(new[] { data.Schema[colIndex] });

			// 3. Define a delegate to grab the value from each row
			var getter = cursor.GetGetter<float>(data.Schema[colIndex]);
			float value = default;

			int rowID = 0;
			while (cursor.MoveNext())
			{
				getter.Invoke(ref value);
				values[rowID] = value;
				rowID++;
			}
			return values;
		}
		public static float RMSE(float[] predictions, float[] targets)
		{
			float sum = 0;
			for (int rowID = 0; rowID < predictions.Length; rowID++)
				sum += MathF.Pow((predictions[rowID] - targets[rowID]), 2);
			return MathF.Pow(sum / predictions.Length, 0.5f);
		}
		public static float R2(float[] predictions, float[] targets)
		{
			float mean = 0;
			for (int i = 0; i < targets.Length; i++)
				mean += targets[i];
			mean /= targets.Length;

			float ssTot = 0;
			float ssRes = 0;

			for (int i = 0; i < targets.Length; i++)
			{
				float diff = targets[i] - predictions[i];
				ssRes += diff * diff;

				float deviation = targets[i] - mean;
				ssTot += deviation * deviation;
			}
			return 1 - ssRes / ssTot;
		}
		public float TrainTestSplit {
			get { return _trainTestSplit; }
			set
			{
				if(value != TrainTestSplit)
				{
					_trainingData = null;
					_testData = null;
				}
				_trainTestSplit = value;
			}
		}
		protected float _trainTestSplit { get; set; } = 0.5f;
		public LightGBMInputData<T> Data { get; set; }
		public MLContext MLContextNET { get; protected set; }
		public EstimatorChain<RegressionPredictionTransformer<LightGbmRegressionModelParameters>> Pipeline { get; protected set; }
		public LgbmModel Model { get; protected set; }
		public IDataView DataView { get; protected set; }
		public TransformerChain<RegressionPredictionTransformer<LightGbmRegressionModelParameters>> Transformer { get; private set; }
		protected IDataView? _trainingData { get; set; }
		public IDataView TrainingData
		{ get
			{
				if(_trainingData == null)
					_trainingData = MLContextNET.Data.LoadFromEnumerable(Data.GetFirstRecords((int)(Data.Count * TrainTestSplit)));
				return _trainingData;
			}
		}
		public IDataView TestData
		{
			get
			{
				if (_testData == null)
					_testData = MLContextNET.Data.LoadFromEnumerable(Data.GetLastRecords((int)(Data.Count * (1 - TrainTestSplit))));
				return _testData;
			}
		}
		protected IDataView? _testData { get; set; }	
		/// <summary>
		/// Number of iterations / rounds. The number of trees created.
		/// <code>Common range: 100 - 1,000 </code>
		/// </summary>
		public int NumberOfIterations { get; set; } = 100;	
		/// <summary>
		/// Maximum number of leaf nodes per tree.
		/// <code>Common range: 20 - 128 </code>
		/// </summary>
		public int NumberOfLeaves { get; set; } = 100;			
		/// <summary>
		/// Minimum number of training samples per leaf.
		/// <code>Common range: </code>
		/// </summary>
		public int MinimumExampleCountPerLeaf { get; set; } = 100;
		/// <summary>
		/// Maximum number of bins for continuous and larger categorical features.
		/// </summary>
		public int MaximumBinCountPerFeature { get; set; } = 100;
		/// <summary>
		/// 
		/// </summary>
		public float LearningRate { get; set; } = 0.1f;
		/// <summary>
		/// Has the model been trained?
		/// </summary>
		public bool Trained { get; protected set; } = false;
		/// <summary>
		/// The target feature. What you're trying to predict
		/// </summary>
		public string Target { get; protected set; }
		public LightGbmRegressionTrainer Estimator { get; protected set; }
		public LightGBMEstimator(LightGBMInputData<T> data, string target)
		{
			Data = data;
			Target = target;
		}
		public void SaveModel(string path)
		{
			MLContextNET.Model.Save(Transformer, DataView.Schema, path);
		}
		public void Train()
		{
			Console.WriteLine("TODO: Remove PrintSummary() call from Train() of LightGBMEstimator");
			PrintSummary();
			Trained = true;

			MLContextNET = new MLContext();
			DataView = MLContextNET.Data.LoadFromEnumerable(Data.GetFirstRecords((int)(Data.Count * TrainTestSplit)));

			MeesSDK.DataManagement.DataViewDumper.DumpToCsv(DataView, "c:/workspaces/__sandbox__/__temp_dump__/shoe.csv", MLContextNET, Data.Features);
			// Prepare options
			LightGbmRegressionTrainer.Options options	= new LightGbmRegressionTrainer.Options();
			options.NumberOfIterations					= NumberOfIterations;
			options.NumberOfLeaves						= NumberOfLeaves;
			options.MinimumExampleCountPerLeaf          = MinimumExampleCountPerLeaf;
			options.LearningRate						= LearningRate;
			options.LabelColumnName						= Target;
			Pipeline									= MLContextNET.Transforms
				.Concatenate("Features", Data.Features) // ← dynamic feature column list
				.Append(MLContextNET.Regression.Trainers.LightGbm(options));
			Transformer = Pipeline.Fit(DataView);
		}
		/// <summary>
		/// Make predictions on the input samples where T is an IValidatable like RdSAPBuilding or SbemModel.
		/// </summary>
		/// <param name="inputs"></param>
		/// <returns></returns>
		public PredictionSet Predict(List<T> inputs)
		{
			IDataView inputObjectsView = MLContextNET.Data.LoadFromEnumerable(inputs);
			IDataView predictions = Transformer.Transform(inputObjectsView);
			return new PredictionSet(MLContextNET.Data.CreateEnumerable<LightGBMScore>(predictions, reuseRowObject: false).Select(p => p.Score).ToArray<float>());
		}
		/// <summary>
		/// Make predictions on the input samples where T is an IValidatable like RdSAPBuilding or SbemModel.
		/// </summary>
		/// <param name="inputs"></param>
		/// <returns></returns>
		public PredictionSet Predict(IDataView inputObjectsView)
		{
			IDataView predictions = Transformer.Transform(inputObjectsView);
			return new PredictionSet(MLContextNET.Data.CreateEnumerable<LightGBMScore>(predictions, reuseRowObject: false).Select(p => p.Score).ToArray<float>());
		}
		/// <summary>
		/// Get the results in PredictionSet for the Test data. Determined by Data and TrainTestSpllit 
		/// </summary>
		/// <returns></returns>
		public PredictionSet Test()
		{
			IDataView testData = TestData;
			float[] targets = ExtractColumnFromIDataView(testData, Target);
			PredictionSet predictions = Predict(testData);
			predictions.SetTargets(targets);
			return predictions;
		}
		/// <summary>
		/// Print some information about the model. If trained, includes test data results
		/// </summary>
		public void PrintSummary()
		{
			Console.WriteLine("--- Data ---");
			Console.WriteLine($"Length:           {Data.Count}");
			Console.WriteLine($"Train/test split: {TrainTestSplit}");
			Console.WriteLine("--- Model ---");
			Console.WriteLine($"No. rounds:       {Data.Count}");
			Console.WriteLine($"Learning rate:    {Data.Count}");
			if (Trained)
			{
				PredictionSet testResults = Test();
				Console.WriteLine("--- Test results ---");
				Console.WriteLine($"RMSE: {testResults.RMSE}");
				Console.WriteLine($"R2:   {testResults.R2}");
				//Console.WriteLine($"No. rounds:       {Data.Count}");
				//Console.WriteLine($"Learning rate:    {Data.Count}");
			}
		}
	}
}
