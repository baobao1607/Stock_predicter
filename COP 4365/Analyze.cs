using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COP_4365
{
    public class Analyze
    {
        public List<int> get_all_valley_candlestick(List<SmartCandlestick> stock_data)
        {
            List<int> valley_position = new List<int>();
            Recognizer_Valley recognize = new Recognizer_Valley();
            for (int i = 0; i < stock_data.Count; i++)
            {
                SmartCandlestick temp = stock_data[i];
                if (recognize.Recognize(stock_data, i) == true)
                {
                    valley_position.Add(i);
                }
            }
            return valley_position;
        }



        public List<int> get_all_peak_candlestick(List<SmartCandlestick> stock_data)
        {
            List<int> peak_position = new List<int>();
            Recognizer_Peak recognize = new Recognizer_Peak();
            for (int i = 0; i < stock_data.Count; i++)
            {
                SmartCandlestick temp = stock_data[i];
                if (recognize.Recognize(stock_data, i) == true)
                {
                    peak_position.Add(i);
                }
            }
            return peak_position;
        }


        public List<WaveData> Analyze_File(string directory_path)
        {
            List<WaveData> allResults = new List<WaveData>();
            string[] stockFiles = Directory.GetFiles(directory_path, "*.csv");
            foreach (string stockFile in stockFiles)
            {
                // Load the stock file into a suitable data structure
                List<Candlestick> cs_stockData = goReadFile(stockFile);
                List<SmartCandlestick> stockData = convert_to_smart(cs_stockData);

                // Analyze the stock data
                List<WaveData> stockAnalysis = Analyze_Stock(stockData);

                // Add the results to the aggregated list
                allResults.AddRange(stockAnalysis);

            }

            WriteToCsv(allResults, "C:\\Users\\baolam\\Desktop\\SPRING 2024\\COP_4365_project_3\\Stock_Data\\WaveDataResults.csv");

            return allResults;
        }


        private void WriteToCsv(List<WaveData> waveDataList, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                // Write the CSV header, including the required features
                writer.WriteLine("Type,FibonacciHits,Distance,VolumeChange,Label,FirstRSI,SecondRSI,RSIDifference," +
                                 "BullishDivergence,BearishDivergence,MACDZeroLinePosition,MACDSignalCrossover," +
                                 "MACDCrossoverDistance,MACDDistanceFromZero");

                // Write each WaveData as a CSV row
                foreach (var wave in waveDataList)
                {
                    writer.WriteLine($"{wave.Type},{wave.FibonacciHits},{wave.Distance},{wave.VolumeChange}," +
                                     $"{wave.Label},{wave.FirstRSI},{wave.LastRSI},{wave.RSI_different}," +
                                     $"{wave.BullishDivergence},{wave.BearishDivergence},{wave.MACDZeroLinePosition}," +
                                     $"{wave.MACDSignalCrossover},{wave.MACDCrossoverDistance},{wave.MACDDistanceFromZero}");
                }
            }
        }




        public List<SmartCandlestick> convert_to_smart(List<Candlestick> source)
        {
            List<SmartCandlestick> result = new List<SmartCandlestick>();
            foreach (Candlestick cs in source)
            {
                SmartCandlestick scs = new SmartCandlestick(cs);
                result.Add(scs);
            }
            return result;
        }


        public List<WaveData> Analyze_Stock(List<SmartCandlestick> stockData)
        {
            List<int> valley_positions = get_all_valley_candlestick(stockData);
            List<int> peak_positions = get_all_peak_candlestick(stockData);
            List<double> closing_price = get_closing_price(stockData);
            var (macdLine, signalLine, histogram) = MCAD.CalculateMACD_DataSet(closing_price);
            
            // Analyze valleys and peaks
            List<WaveData> valleyResult = AnalyzeDataSet(valley_positions, peak_positions, 0, stockData, macdLine, signalLine, histogram);
            List<WaveData> peakResult = AnalyzeDataSet(peak_positions, valley_positions, 1, stockData, macdLine, signalLine, histogram);

            // Combine results
            List<WaveData> result = new List<WaveData>();
            result.AddRange(valleyResult);
            result.AddRange(peakResult);

            return result;
        }


        public List<double> get_closing_price(List<SmartCandlestick> source)
        {
            var result = new List<double>();
            foreach (SmartCandlestick cs in source)
            {
                result.Add((double)cs.close);

            }
            return result;
        }


        public List<WaveData> AnalyzeDataSet(List<int> list_position,List<int> list_of_next, int type, List<SmartCandlestick> stock_data, List<double> macdLine, List<double>signalLine, List<double> Histogram)
        {
            List<WaveData> result = new List<WaveData>();
            foreach (int index in list_position)
            {

                for (int second_index = index + 1; second_index <= stock_data.Count-1; second_index++)
                {

                    if (!IsValidWave(index, second_index, stock_data, type))
                    {
                        continue;
                    }


                    int distance = second_index - index + 1;
                    int label;
                    if (type == 0)
                    {
                        label = validate_peak(second_index, stock_data);
                    }
                    else
                    {
                        label = validate_valley(second_index, stock_data);
                    }

                    float volume = Calculate_Volume(index, second_index, stock_data);


                    int hit_count = get_fibonnaci_hit(index, second_index, stock_data,type);

                    decimal first_rsi = Calculate_RSI(stock_data, index);

                    decimal second_rsi = Calculate_RSI(stock_data, second_index);

                    decimal rsi_difference = second_rsi - first_rsi;

                    int crossover = calculate_crossover(second_index, macdLine, signalLine);

                    int bullish_divergence = calculate_bullish(stock_data, macdLine, second_index);
                    int bearish_divergence = calculate_bearish(stock_data, macdLine, second_index);

                    int zero_line_position = calculate_zero_line_position(macdLine[second_index]);
                    int MACD_distance = CalculateMACDCrossoverDistance(macdLine, signalLine, second_index);


                    float MACD_distance_zero = CalculateMACDDistanceFromZero(macdLine[second_index]);
                    result.Add(new WaveData
                    {
                        Type = type,
                        FibonacciHits = hit_count,
                        VolumeChange = volume,
                        Distance = distance,
                        Label = label,
                        FirstRSI = (float)(first_rsi),
                        LastRSI = (float)(second_rsi),
                        RSI_different = (float)(rsi_difference),
                        MACDLine = (float)(macdLine[second_index]),
                        SignalLine = (float)(signalLine[second_index]),
                        MACDHistogram = (float)(Histogram[second_index]),
                        MACDSignalCrossover = crossover,
                        BearishDivergence = bearish_divergence,
                        BullishDivergence = bullish_divergence,
                        MACDZeroLinePosition = zero_line_position,
                        MACDCrossoverDistance = MACD_distance,
                        MACDDistanceFromZero = MACD_distance_zero
                    });
                }

            }
            return result;
        }


        public float CalculateMACDDistanceFromZero(double macdLineValue)
        {
            if (double.IsNaN(macdLineValue))
                return float.NaN; // Invalid state

            return (float)Math.Abs(macdLineValue); // Absolute distance from zero
        }



        public int CalculateMACDCrossoverDistance(List<double> macdLine, List<double> signalLine, int currentIndex)
        {
            int lastCrossoverIndex = -1;

            // Find the last crossover before the current index
            for (int i = currentIndex - 1; i >= 0; i--)
            {
                if ((macdLine[i] > signalLine[i] && macdLine[i - 1] <= signalLine[i - 1]) || // Bullish crossover
                    (macdLine[i] < signalLine[i] && macdLine[i - 1] >= signalLine[i - 1]))   // Bearish crossover
                {
                    lastCrossoverIndex = i;
                    break;
                }
            }

            if (lastCrossoverIndex == -1)
            {
                return -1;
            } else
            {
                return currentIndex - lastCrossoverIndex;
            }
        }



        public int calculate_zero_line_position(double value)
        {
            if (value < 0) return -1;
            else if (value > 0) return 1;
            else return 0;
        }

        public int calculate_bullish(List<SmartCandlestick> stock_data, List<Double>macdLine, int index)
        {
            if (index < 2) return 0; // Not enough data to compare two valleys

            // Check if price is making a lower low
            bool priceLowerLow = stock_data[index].close < stock_data[index - 2].close && stock_data[index - 1].close > stock_data[index].close;

            // Check if MACD is making a higher low
            bool macdHigherLow = macdLine[index] > macdLine[index - 2] && macdLine[index - 1] < macdLine[index];

            // Return 1 for bullish divergence, otherwise 0
            return (priceLowerLow && macdHigherLow) ? 1 : 0;
        }


        public int calculate_bearish(List<SmartCandlestick> stock_data, List<Double> macdLine, int index)
        {
            if (index < 2) return 0; // Not enough data to compare two peaks

            // Check if price is making a higher high
            bool priceHigherHigh = stock_data[index].close > stock_data[index - 2].close && stock_data[index - 1].close < stock_data[index].close;

            // Check if MACD is making a lower high
            bool macdLowerHigh = macdLine[index] < macdLine[index - 2] && macdLine[index - 1] > macdLine[index];

            // Return 1 for bearish divergence, otherwise 0
            return (priceHigherHigh && macdLowerHigh) ? 1 : 0;
        }


        public int calculate_crossover(int i, List<double> macdLine, List<Double> signalLine)
        {
            int result = 0;
            if (double.IsNaN(macdLine[i]) || double.IsNaN(signalLine[i]) ||
            double.IsNaN(macdLine[i - 1]) || double.IsNaN(signalLine[i - 1]))
            {
                result = 0; // No valid crossover due to NaN
            }
            else if (macdLine[i] > signalLine[i] && macdLine[i - 1] <= signalLine[i - 1])
            {
                result = 1; // Bullish Crossover
            }
            else if (macdLine[i] < signalLine[i] && macdLine[i - 1] >= signalLine[i - 1])
            {
                result = -1; // Bearish Crossover
            }
            else
            {
                result = 0; // No Crossover
            }
            return result;
        }


        public float Calculate_Volume(int first, int second, List<SmartCandlestick> stock_data)
        {
            float first_volume = stock_data[first].volume;
            float second_volume = stock_data[second].volume;
            if (first_volume == 0)
            {
                return 0;
            }
            return ((second_volume - first_volume) / first_volume) * 100;
        }

        public bool IsValidWave(int index, int second_index, List<SmartCandlestick> stock_data, int type)
        {
            Decimal min_price = Math.Min(stock_data[index].low, stock_data[second_index].low);
            Decimal max_price = Math.Max(stock_data[index].high, stock_data[second_index].high);
            if (type == 0) // Valley case
            {
                // Check if the second candlestick's low is lower than the first candlestick's low
                if (stock_data[second_index].low < stock_data[index].low)
                {
                    return false; // Invalid wave
                }
            }
            else if (type == 1) // Peak case
            {
                // Check if the second candlestick's high is higher than the first candlestick's high
                if (stock_data[second_index].high > stock_data[index].high)
                {
                    return false; // Invalid wave
                }
            }
            for (int i = index; i < second_index; i++)
            {
                SmartCandlestick temp = stock_data[i];
                if (temp.low < min_price)
                {
                    return false;
                }
                if (temp.high > max_price) { return false; }
            }
            return true;
        }

        public int validate_peak(int index, List<SmartCandlestick> stock_data)
        {
            Recognizer_Peak recognizer_Peak = new Recognizer_Peak();
            if (!recognizer_Peak.Recognize(stock_data, index))
            {
                return 0;
            }
            return 1;
        }


        public int validate_valley(int index, List<SmartCandlestick> stock_data)
        {
            Recognizer_Valley recognizer_Valley = new Recognizer_Valley();
            if (!recognizer_Valley.Recognize(stock_data, index))
            {
                return 0;
            }
            return 1;
        }


        public int get_fibonnaci_hit(int index, int second_index, List<SmartCandlestick> stock_data, int type)
        {
            int result = 0;

            List<Decimal> Fibonacci_levels = new List<Decimal>();

            if (type == 0)
            {
                Fibonacci_levels = get_levels(stock_data[index].low, stock_data[second_index].high);
            } else
            {
                Fibonacci_levels = get_levels(stock_data[index].high, stock_data[second_index].low);
            }

            Decimal threshold = (Fibonacci_levels[Fibonacci_levels.Count - 1] - Fibonacci_levels[0]) * 0.015m;

            for (int i = index; i <= second_index; i++)
            {
                SmartCandlestick temp = stock_data[i];
                foreach (Decimal levels in Fibonacci_levels)
                {
                    if (Math.Abs(levels - temp.low) < threshold)
                    {
                        result++;
                    }
                    if (Math.Abs(levels - temp.high) < threshold)
                    {
                        result++;
                    }
                    if (Math.Abs(levels - temp.close) < threshold)
                    {
                        result++;
                    }
                    if (Math.Abs(levels - temp.open) < threshold)
                    {
                        result++;
                    }
                }
            }
            return result;
        }


        public List<Decimal> get_levels(decimal first, decimal second)
        {
            List<Decimal> levels = new List<Decimal>();

            Decimal low = Math.Min(first, second);
            Decimal high = Math.Max(second, first);
            Decimal range = high - low;
            levels.Add(low);
            levels.Add(low + range * (0.24m));
            levels.Add(low + range * (0.38m));
            levels.Add(low + range * (0.5m));
            levels.Add(low + range * (0.62m));
            levels.Add(low + range * (0.76m));
            levels.Add(high);
            return levels;
        }


        private List<Candlestick> goReadFile(string filename)
        {
            // declare a local variable that takes the form of List that store Candlestick object
            List<Candlestick> list_of_candlesticks = new List<Candlestick>(1024);
            // declare a string to compare with the first line in the file 
            const string referenceString = "Date,Open,High,Low,Close,Volume";
            // declare a object that abstract from StreamReader class that can read the file
            using (StreamReader sr = new StreamReader(filename))
            {
                // clear the list_of_candlesticks obtained from the previos function call
                list_of_candlesticks.Clear();
                // assign the first line read from the file to variable "line"
                string line = sr.ReadLine();
                // if condition to check if the two strings declared above is the same or not
                if (line == referenceString)
                {
                    // if they are the same, run the while loop until read all the lines in the file
                    while ((line = sr.ReadLine()) != null)
                    {
                        // create a new Candlestick object taking parameters from the read line
                        Candlestick cs = new Candlestick(line);
                        // adding the object to the list of candlestick
                        list_of_candlesticks.Add(cs);
                        // create a new smart candelstick object taking parameters from the read line 
                    }
                }// if the condition failes, print Bad File to the user
            }// return the list_of_candlesticks that hold all the candlesticks object
            return list_of_candlesticks;
        }


        public decimal Calculate_RSI (List<SmartCandlestick> source, int index)
        {
            decimal rsi_values = 0;

            if (index <= 14)
            {
                return 0;
            }
            decimal averageGain = 0, averageLoss = 0;

            // Calculate initial average gain and loss
            for (int i = 1; i <= 14; i++)
            {
                decimal change = source[i].close - source[i - 1].close;
                if (change > 0)
                {
                    averageGain += change;
                }
                else
                {
                    averageLoss -= change; // Loss is negative, so subtract
                }
            }
            averageGain /= 14;
            averageLoss /= 14;

            decimal rs = averageGain / averageLoss;
            rsi_values = 100 - (100 / (1 + rs));

            for (int i = 15; i <= index; i++)
            {
                decimal change = source[i].close - source[i - 1].close;

                decimal gain = (change > 0) ? change : 0;
                decimal loss = (change < 0) ? -change : 0;

                averageGain = ((averageGain * (14 - 1)) + gain) / 14;
                averageLoss = ((averageLoss * (14 - 1)) + loss) / 14;

                if (averageLoss == 0)
                {
                    return 100; // RSI is 100 when there are no losses (overbought condition)
                }
                if (averageGain == 0)
                {
                    return 0; // RSI is 0 when there are no gains (oversold condition)
                }

                rs = averageGain / averageLoss;
                rsi_values = (100 - (100 / (1 + rs)));
            }

            return rsi_values;
        }
    }
}
