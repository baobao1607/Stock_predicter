using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COP_4365
{
    /// <summary>
    /// this class represents for the Bullish_Harami pattern recognizer. It is inherited from the recognizer abstract class
    /// </summary>
    internal class Recognizer_Bullish_Harami : Recognizer
    {
        /// <summary>
        /// This method is the default constructor for the Bullish_Harami recognizer class
        /// It passes in string Bullish_Harami into the pattern name and 2 into the number of smartcandlestick objects used.
        /// </summary
        public Recognizer_Bullish_Harami() : base("Bullish_Harami", 2) { }
        /// <summary>
        /// This method recognize if the candlestick has a patttern Bullish_Harami or not. It takes in a list of smart candlestick 
        /// and the index of smartcandlestick object that user want to explore on.
        /// </summary>
        /// <param name="lscs">A list of smartcandlestick object</param>
        /// <param name="index">The index of the smartcandlestick object that user want to explore</param>
        /// <returns>A boolean value whether it forms a Bullish_Harami pattern or not</returns>
        override public bool Recognize(List<SmartCandlestick> lscs, int index)
        {
            //assign the smartcandlestick object to scs- the variable of the type smartcandlestick
            SmartCandlestick scs = lscs[index];
            // try to get the value from the key value pair (key is the pattern) from the pattern dictionary of the object
            bool there = scs.dictionary_patterns.TryGetValue(patternName, out bool r);
            // if not success (meaning there is no such key -value pair)
            if (!there)
            {
                // check if the previous index is out of the list range or not
                if (index - 1 < 0)
                {
                    // if it is out of range, add in dictionary pattern the key value pair (key is pattern name, value is false)
                    scs.dictionary_patterns[patternName] = false;
                    //return the boolean value whether it forms a Bullish_Harami pattern or not
                    return false;
                }
                // if it is not, check if it forms a Bullish_Harami or not
                else
                {
                    //assign the previous smartcandlestick object to variable scs_previous
                    SmartCandlestick scs_previous = lscs[index - 1];
                    // check whether it is a Bullish_Harami candlestick or not (boolean type)
                    r = (scs_previous.close < scs_previous.open) && (scs.close > scs.open) && (scs.open > scs_previous.close) && (scs.close < scs_previous.open);
                    // add in the dictionary with the pattern name as key and value is the boolean value
                    scs.dictionary_patterns[patternName] = r;
                }
            }
            //return the boolean value whether it forms a Bullish_Harami pattern or not
            return r;
        }
    }
}
