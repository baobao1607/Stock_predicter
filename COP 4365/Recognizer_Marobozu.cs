﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COP_4365
{
    /// <summary>
    /// this class represents for the marobozu pattern recognizer. It is inherited from the recognizer abstract class
    /// </summary>

    internal class Recognizer_Marobozu : Recognizer
    {
        /// <summary>
        /// This method recognize if the candlestick has a patttern marobozu or not. It takes in a list of smart candlestick 
        /// and the index of smartcandlestick object that user want to explore on.
        /// </summary>
        /// <param name="lscs">A list of smartcandlestick object</param>
        /// <param name="index">The index of the smartcandlestick object that user want to explore</param>
        /// <returns>A boolean value whether it forms a marobozu pattern or not</returns>
        override public bool Recognize(List<SmartCandlestick> lscs, int index)
        {
            //assign the smartcandlestick object to scs- the variable of the type smartcandlestick
            SmartCandlestick scs = lscs[index];
            // try to get the value from the key value pair (key is the pattern) from the pattern dictionary of the object
            bool there = scs.dictionary_patterns.TryGetValue(patternName, out bool result);
            // if not success (meaning there is no such key -value pair)
            if (!there)
            {
                // check whether it is a marobozu candlestick or not (boolean type)
                result = (scs.Range - scs.bodyRange) < 0.02m;
                // add in the dictionary with the pattern name as key and value is the boolean value
                scs.dictionary_patterns.Add("Marobozu", result);
            }
            //return the boolean value whether it forms a marobozu pattern or not
            return result;
        }
        /// <summary>
        /// This method is the default constructor for the marobozu recognizer class
        /// It passes in string marobozu into the pattern name and 1 into the number of smartcandlestick objects used.
        /// </summary>
        public Recognizer_Marobozu() : base("Marobozu", 1) { }
    }
}
