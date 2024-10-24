using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COP_4365
{
    /// <summary>
    /// this is an abstract class named Recognizer that recognize the patterns in the candlestick
    /// </summary>
    abstract class Recognizer
    {
        // declare a string named patternName that stores the pattern that the candlestick has
        public string patternName;
        // declare a variable pattern length for the recognized pattern.
        public int patternlength;
        /// <summary>
        /// this is an abstract method called recognize to recognizing a specific candlestick pattern
        /// </summary>
        /// <param name="lscs">A list of smartcandlestick</param>
        /// <param name="index">The index of the smartcandlestick object that user want to explore</param>
        /// <returns>A boolean value whether it forms a Bearish_Engulfing pattern or not</returns>
        public abstract bool Recognize(List<SmartCandlestick> lscs, int index);

        /// <summary>
        /// This method recognizes all the patterns that each smartcandlestick has in the list of smartcandlestick
        /// </summary>
        /// <param name="lscs">A list of smartcandlestick that user wants to recognize on</param>
        public void RecognizeAll(List<SmartCandlestick> lscs)
        {
            //iterate through the list of smartcandlestick
            for (int i = 0; i < lscs.Count; i++)
            {
                //call the recognize method on the current index of the list of smartcandlestick
                Recognize(lscs, i);
            }
        }
        /// <summary>
        /// this method is a default constructor for the Recognizer class
        /// </summary>
        /// <param name="patternName">The name of the recognized pattern</param>
        /// <param name="patternlength">The length of the recognized pattern</param>
        public Recognizer(string patternName, int patternlength)
        {
            //set the parameters to the properties of the recognize class
            this.patternName = patternName;
            this.patternlength = patternlength;
        }
    }



}
