using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COP_4365
{
    /// <summary>
    /// This class represented the smartcandlestick object inherited from the Candlestick object
    /// </summary>
    internal class SmartCandlestick : Candlestick
    {
        //declare the first attribute of the Smartcandlestick object
        //this variable has decimal type and name of "Range"
        //the "get, set" provides methods for user to get the value and modify the value of the attribute 
        public decimal Range { get; set; }
        //declare the first attribute of the Smartcandlestick object
        //this variable has decimal type and name of "topPrice"
        //the "get, set" provides methods for user to get the value and modify the value of the attribute 
        public decimal topPrice { get; set; }
        //declare the first attribute of the Smartcandlestick object
        //this variable has decimal type and name of "bottomPrice"
        //the "get, set" provides methods for user to get the value and modify the value of the attribute 
        public decimal bottomPrice { get; set; }
        //declare the first attribute of the Smartcandlestick object
        //this variable has decimal type and name of "bodyRange"
        //the "get, set" provides methods for user to get the value and modify the value of the attribute 
        public decimal bodyRange { get; set; }
        //declare the first attribute of the Smartcandlestick object
        //this variable has decimal type and name of "upperTail"
        //the "get, set" provides methods for user to get the value and modify the value of the attribute 
        public decimal upperTail { get; set; }
        //declare the first attribute of the Smartcandlestick object
        //this variable has decimal type and name of "lowerTail"
        //the "get, set" provides methods for user to get the value and modify the value of the attribute 
        public decimal lowerTail { get; set; }
        // declare a dictionary storing the patterns of each smart candlesticj object. The key of the dictionary is the patterns:
        // Marobozu, Bullish, Bearish,... and the value is boolean(true or false)
        public Dictionary<string, bool> dictionary_patterns { get; set; }
        /// <summary>
        /// This class is the constructor for the smartcandlestick object that takes the input of a row of data
        /// The ouput is a smartcandlestick object
        /// </summary>
        /// <param name="rowOfdata"></param>
        public SmartCandlestick(string rowOfdata) : base(rowOfdata)
        {
            // call the computeExtraProperties method to calculate extra properties of the smart candlestick object
            ComputeExtraProperties();
            // the patterns properties of smartcandlestick (the dictionary) is constructed using the computepatterns method
            dictionary_patterns = computepatterns();
        }
        /// <summary>
        /// this method is called computeExtraProperites which will calculate the extra properties of the smart candlestick object
        /// </summary>
        private void ComputeExtraProperties()
        {
            // assign the range equals high - low
            Range = high - low;
            // assign the top price equals the higher value between open and close
            topPrice = Math.Max(open, close);
            // assign the bottomPrice equals the lesser value between open and close
            bottomPrice = Math.Min(close, open);
            // the body range is calculated by getting the topPrice - bottomPrice
            bodyRange = topPrice - bottomPrice;
            // the upperTail is calculated by getting the high - topPrice
            upperTail = high - topPrice;
            // the lowerTail is calculated by getting the high - topPrice
            lowerTail = bottomPrice - close;
        }
        /// <summary>
        /// this constructor of the smartcandlestick takes in the parameter of the candlestick object and return a smartcandlestick object
        /// </summary>
        /// <param name="cs"></param>
        public SmartCandlestick(Candlestick cs)
        {
            // the date is assigned to the candlestick date
            date = cs.date;
            // the open is assigned to the candlestick open
            open = cs.open;
            // the high is assigned to the candlestick high
            high = cs.high;
            // the low is assigned to the candlestick low
            low = cs.low;
            // the volume is assigned to the candlestick volume
            volume = cs.volume;
            // the close is assigned to the candlestick close
            close = cs.close;
            // call the compute extra properites to calculate the properties of the smart candlestick object
            ComputeExtraProperties();
            // compute the patterns properties (building the dictionary)
            dictionary_patterns = computepatterns();
        }
        /// <summary>
        /// this method compute the patterns dictionary of the smart candlestick object
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, bool> computepatterns()
        {
            Dictionary<string, bool> dictionary_patterns = new Dictionary<string, bool>();
            // adding the key value pair of patterns-boolean with key is "Marobozu" and value based on whether the range - bodyRange < 0.02
            dictionary_patterns.Add("Marobozu", Range - bodyRange < 0.02m);
            // adding the key value pair of patterns-boolean with key is "Bullish" and value based on whether close > open
            dictionary_patterns.Add("Bullish", close > open);
            // adding the key value pair of patterns-boolean with key is "Bullish" and value based on whether close < open
            dictionary_patterns.Add("Bearish", close < open);
            // adding the key value pair of patterns-boolean with key is "Neutral" and value based on whether bodyRange < 1
            dictionary_patterns.Add("Neutral", Math.Abs(close - open) < 0.5m);
            // adding the key value pair of patterns-boolean with key is "Hammer" and value based on the conditions
            dictionary_patterns.Add("Hammer", upperTail < 0.03m * Range && bodyRange >= 0.2m * Range && bodyRange <= 0.3m * Range);
            // adding the key value pair of patterns-boolean with key is "Doji" and value based on the conditions
            dictionary_patterns.Add("Doji", bodyRange < open * 0.05m);
            // adding the key value pair of patterns-boolean with key is "DragonFly_Doji" and value based on the conditions
            dictionary_patterns.Add("DragonFly_Doji", bodyRange < 0.1m * Range && upperTail < 0.1m * Range && (lowerTail > 2 * bodyRange));
            // adding the key value pair of patterns-boolean with key is "GraveStone_Doji" and value based on the conditions
            dictionary_patterns.Add("GraveStone_Doji", (bodyRange < 0.1m * Range) && (lowerTail < 0.1m * Range) && (upperTail > 2 * bodyRange));
            // return the patterns dictionary
            return dictionary_patterns;
        }
    }
}
