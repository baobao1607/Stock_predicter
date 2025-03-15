using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COP_4365
{
    public class MCAD
    {
        public static (List<double> MACDLine, List<double> SignalLine, List<double> Histogram) CalculateMACD(
        List<double> closingPrices,
        int fastPeriod = 12,
        int slowPeriod = 26,
        int signalPeriod = 9)
        {
            var fastEMA = CalculateEMA(closingPrices, fastPeriod);
            var slowEMA = CalculateEMA(closingPrices, slowPeriod);

            // Step 2: Calculate MACD Line (Fast EMA - Slow EMA)
            var macdLine = fastEMA.Zip(slowEMA, (fast, slow) => fast - slow).ToList();

            // Step 3: Filter out NaN values from the MACD Line
            var validMacdLine = macdLine.Where(x => !double.IsNaN(x)).ToList();

            // Step 4: Calculate Signal Line (9-day EMA of valid MACD Line)
            var signalLine = CalculateEMA(validMacdLine, signalPeriod);

            // Step 5: Calculate Histogram (Valid MACD Line - Signal Line)
            var histogram = validMacdLine.Zip(signalLine, (macd, signal) => macd - signal).ToList();

            signalLine = signalLine.Skip(10).ToList();
            validMacdLine = validMacdLine.Skip(10).ToList();
            histogram = histogram.Skip(10).ToList();

            return (validMacdLine, signalLine, histogram);
        }


        public static (List<double> MACDLine, List<double> SignalLine, List<double> Histogram) CalculateMACD_DataSet(
        List<double> closingPrices,
        int fastPeriod = 12,
        int slowPeriod = 26,
        int signalPeriod = 9)
        {
            var fastEMA = CalculateEMA(closingPrices, fastPeriod);
            var slowEMA = CalculateEMA(closingPrices, slowPeriod);

            // Step 2: Calculate MACD Line (Fast EMA - Slow EMA)
            var macdLine = fastEMA.Zip(slowEMA, (fast, slow) => fast - slow).ToList();

            // Step 3: Filter out NaN values from the MACD Line
            var validMacdLine = macdLine.Where(x => !double.IsNaN(x)).ToList();

            // Step 4: Calculate Signal Line (9-day EMA of valid MACD Line)
            var signalLine = CalculateEMA(validMacdLine, signalPeriod);

            var alignedSignalLine = Enumerable.Repeat(double.NaN, slowPeriod - 1)
                                   .Concat(signalLine)
                                   .ToList();
            // Step 5: Calculate Histogram (Aligned MACD Line - Aligned Signal Line)
            var histogram = macdLine.Zip(alignedSignalLine, (macd, signal) =>
                !double.IsNaN(signal) ? macd - signal : double.NaN
            ).ToList();

            return (macdLine, alignedSignalLine, histogram);
        }

        private static List<double> CalculateEMA(List<double> prices, int period)
        {
            List<double> ema = new List<double>();
            double multiplier = 2.0 / (period + 1);
            double? previousEMA = null;

            for (int i = 0; i < prices.Count; i++)

            {
                if (double.IsNaN(prices[i]))
                {
                    ema.Add(double.NaN); // Propagate NaN if input contains NaN
                    continue;
                }

                if (i < period - 1)
                {
                    ema.Add(double.NaN); // Not enough data for EMA
                    continue;
                }

                if (i == period - 1)
                {
                    // Use Simple Moving Average (SMA) for the first EMA value
                    double sma = prices.Take(period).Average();
                    ema.Add(sma);
                    previousEMA = sma;
                }
                else
                {
                    // EMA Calculation
                    double currentEMA = (prices[i] - previousEMA.Value) * multiplier + previousEMA.Value;
                    ema.Add(currentEMA);
                    previousEMA = currentEMA;
                }
            }
            return ema;
        }
    }
}
