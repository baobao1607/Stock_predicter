using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace COP_4365
{
    internal class Training
    {
        List<WaveData> waveData;

        public Training(List<WaveData> waveData)
        {
            this.waveData = waveData;
        }


        public void Train(List<WaveData> newWaves)
        {
            var mlContext = new MLContext();
            IDataView data = mlContext.Data.LoadFromEnumerable(waveData);
            var splitData = mlContext.Data.TrainTestSplit(data, testFraction: 0.2);
            var trainData = splitData.TrainSet;
            var testData = splitData.TestSet;
            ITransformer trainedModel = TrainAndEvaluateModel(mlContext, trainData, testData);
            // Make predictions
            Training.PredictNewData(mlContext, trainedModel, newWaves);
        }


        public ITransformer TrainAndEvaluateModel(MLContext mlContext, IDataView trainData, IDataView testData)
        {
            var pipeline = mlContext.Transforms.Conversion.ConvertType(
               outputColumnName: "LabelBool",
               inputColumnName: nameof(WaveData.Label),
               outputKind: DataKind.Boolean) // Convert Label to Boolean
           .Append(mlContext.Transforms.Conversion.ConvertType(
               outputColumnName: nameof(WaveData.Type),
               inputColumnName: nameof(WaveData.Type),
               outputKind: DataKind.Single)) // Convert Type to float
           .Append(mlContext.Transforms.Conversion.ConvertType(
               outputColumnName: nameof(WaveData.FibonacciHits),
               inputColumnName: nameof(WaveData.FibonacciHits),
               outputKind: DataKind.Single)) // Convert FibonacciHits to float
           .Append(mlContext.Transforms.Conversion.ConvertType(
               outputColumnName: nameof(WaveData.Distance),
               inputColumnName: nameof(WaveData.Distance),
               outputKind: DataKind.Single)) // Convert Distance to float
           .Append(mlContext.Transforms.Conversion.ConvertType(
               outputColumnName: nameof(WaveData.VolumeChange),
               inputColumnName: nameof(WaveData.VolumeChange),
               outputKind: DataKind.Single)) // Convert VolumeChange to float
           .Append(mlContext.Transforms.Conversion.ConvertType(
               outputColumnName: nameof(WaveData.FirstRSI),
               inputColumnName: nameof(WaveData.FirstRSI),
               outputKind: DataKind.Single)) // Convert FirstRSI to float
           .Append(mlContext.Transforms.Conversion.ConvertType(
               outputColumnName: nameof(WaveData.LastRSI),
               inputColumnName: nameof(WaveData.LastRSI),
               outputKind: DataKind.Single)) // Convert LastRSI to float
           .Append(mlContext.Transforms.Conversion.ConvertType(
               outputColumnName: nameof(WaveData.RSI_different),
               inputColumnName: nameof(WaveData.RSI_different),
               outputKind: DataKind.Single)) // Convert RSIDifference to float
           .Append(mlContext.Transforms.Conversion.ConvertType(
               outputColumnName: nameof(WaveData.BullishDivergence),
               inputColumnName: nameof(WaveData.BullishDivergence),
               outputKind: DataKind.Single)) // Convert BullishDivergence to float
           .Append(mlContext.Transforms.Conversion.ConvertType(
               outputColumnName: nameof(WaveData.BearishDivergence),
               inputColumnName: nameof(WaveData.BearishDivergence),
               outputKind: DataKind.Single)) // Convert BearishDivergence to float
           .Append(mlContext.Transforms.Conversion.ConvertType(
               outputColumnName: nameof(WaveData.MACDZeroLinePosition),
               inputColumnName: nameof(WaveData.MACDZeroLinePosition),
               outputKind: DataKind.Single)) // Convert MACDZeroLinePosition to float
           .Append(mlContext.Transforms.Conversion.ConvertType(
               outputColumnName: nameof(WaveData.MACDSignalCrossover),
               inputColumnName: nameof(WaveData.MACDSignalCrossover),
               outputKind: DataKind.Single)) // Convert MACDCrossover to float
           .Append(mlContext.Transforms.Conversion.ConvertType(
               outputColumnName: nameof(WaveData.MACDCrossoverDistance),
               inputColumnName: nameof(WaveData.MACDCrossoverDistance),
               outputKind: DataKind.Single)) // Convert MACDCrossoverDistance to float
           .Append(mlContext.Transforms.Conversion.ConvertType(
               outputColumnName: nameof(WaveData.MACDDistanceFromZero),
               inputColumnName: nameof(WaveData.MACDDistanceFromZero),
               outputKind: DataKind.Single)) // Convert MACDDistanceFromZero to float
           .Append(mlContext.Transforms.Concatenate("Features",
               nameof(WaveData.Type),
               nameof(WaveData.FibonacciHits),
               nameof(WaveData.Distance),
               nameof(WaveData.VolumeChange),
               nameof(WaveData.FirstRSI),
               nameof(WaveData.LastRSI),
               nameof(WaveData.RSI_different),
               nameof(WaveData.BullishDivergence),    // Include BullishDivergence
               nameof(WaveData.BearishDivergence),   // Include BearishDivergence
               nameof(WaveData.MACDZeroLinePosition), // Include MACDZeroLinePosition
               nameof(WaveData.MACDSignalCrossover),       // Include MACDCrossover
               nameof(WaveData.MACDCrossoverDistance), // Include MACDCrossoverDistance
               nameof(WaveData.MACDDistanceFromZero))) // Include MACDDistanceFromZero
           .Append(mlContext.Transforms.NormalizeMinMax("Features")) // Normalize Features
           .Append(mlContext.BinaryClassification.Trainers.LightGbm(
               labelColumnName: "LabelBool",
               featureColumnName: "Features",
               numberOfLeaves: 31,
               minimumExampleCountPerLeaf: 20,
               learningRate: 0.05));



            var model = pipeline.Fit(trainData);

            // Evaluate the model
            var predictions = model.Transform(testData);


            var metrics = mlContext.BinaryClassification.Evaluate(predictions, labelColumnName: "LabelBool");


            string message = $"Accuracy: {metrics.Accuracy:P2}\n" +
                     $"F1 Score: {metrics.F1Score:P2}\n" +
                     $"Area Under ROC Curve (AUC): {metrics.AreaUnderRocCurve:P2}\n" +
                     $"Log Loss: {metrics.LogLoss:F4}";

            // Display metrics in a MessageBox
            MessageBox.Show(message, "Model Evaluation Metrics", MessageBoxButtons.OK, MessageBoxIcon.Information);

            return model;
        }

        public static void PredictNewData(MLContext mlContext, ITransformer model, List<WaveData> newWaves)
        {

            // Convert new data to IDataView
            IDataView newData = mlContext.Data.LoadFromEnumerable(newWaves);

            // Create a prediction engine
            var predictionEngine = mlContext.Model.CreatePredictionEngine<WaveData, WavePrediction>(model);
            StringBuilder resultMessage = new StringBuilder();
            resultMessage.AppendLine("Prediction Results:");
            foreach (var wave in newWaves)
            {
                var prediction = predictionEngine.Predict(wave);
                resultMessage.AppendLine($"Type: {wave.Type}, FibonacciHits: {wave.FibonacciHits}, Number Of Candlesticks: {wave.Distance}, " +
                                         $"Predicted: {ConvertToResult(prediction.PredictedLabel, wave.Type)}, Probability: {ConvertScoreToProbability(prediction.Score):P2}");
            }

            // Display predictions in a MessageBox
            MessageBox.Show(resultMessage.ToString(), "Prediction Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static double ConvertScoreToProbability(float score)
        {
            return 1 / (1 + Math.Exp(-score));
        }


        public static string ConvertToResult (bool source, int type)
        {
            string result = string.Empty;
            if (type == 0)
            {
                if (source)
                {
                    result = "Peak";
                }
                else
                {
                    result = "Not A Peak";
                }
            }
            else
            {
                if (source) 
                {
                    result = "Valley";
                } else
                {
                    result = "Not A Valley";
                }

            }
            return result;
        }

    }

    public class WavePrediction
    {
        [ColumnName("PredictedLabel")]
        public bool PredictedLabel { get; set; } // Match the model's Boolean type

        [ColumnName("Score")]
        public float Score { get; set; }       // Confidence scores for the prediction
    }


}
