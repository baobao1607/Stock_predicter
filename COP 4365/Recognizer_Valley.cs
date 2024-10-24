using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COP_4365
{
    /// <summary>
    /// this class represents for the valley pattern recognizer. It is inherited from the recognizer abstract class
    /// </summary>
    internal class Recognizer_Valley : Recognizer
    {
        /// <summary>
        /// This method is the default constructor for the valley recognizer class
        /// It passes in string valley into the pattern name and 3 into the number of smartcandlestick objects used.
        /// </summary>
        public Recognizer_Valley() : base("Valley", 3) { }

        /// <summary>
        /// This method recognize if the candlestick has a patttern valley or not. It takes in a list of smart candlestick 
        /// and the index of smartcandlestick object that user want to explore on.
        /// </summary>
        /// <param name="lscs">A list of smartcandlestick object</param>
        /// <param name="index">The index of the smartcandlestick object that user want to explore</param>
        /// <returns>A boolean value whether it forms a valley pattern or not</returns>
        override public bool Recognize(List<SmartCandlestick> lscs, int index)
        {
            //assign the smartcandlestick object to scs- the variable of the type smartcandlestick
            SmartCandlestick scs = lscs[index];
            // try to get the value from the key value pair (key is the pattern) from the pattern dictionary of the object
            bool there = scs.dictionary_patterns.TryGetValue(patternName, out bool r);
            // if not success (meaning there is no such key -value pair)
            if (!there)
            {
                // check if the object is at the very top or at the end of the list
                if ((index - 1 < 0) || (index + 1 >= lscs.Count()))
                {
                    // if it is, then add the key value pair into its dictionary (key is the pattern name, value is false)
                    scs.dictionary_patterns[patternName] = false;
                    // return false
                    return false;
                }
                // otherwise, the previous and next smartcandlestick object can be assigned
                else
                {
                    //assign the previous and next smartcandlestick object into temporary variable of the type smartcandlesticks
                    SmartCandlestick scs_previous = lscs[index - 1];
                    SmartCandlestick scs_next = lscs[index + 1];
                    //indicate whether the 3 objects form valley pattern or not (boolean type)
                    r = (scs.low < scs_previous.low) && (scs.low < scs_next.low);
                    // add in the key value pair with key is the pattern name, value is boolean r.
                    scs.dictionary_patterns.Add(patternName, r);
                }
            }
            // return the boolean result
            return r;
        }

    }
}
