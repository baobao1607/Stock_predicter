using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace COP_4365
{
    public class WaveData
    {
        public int Type { get; set; }

        public int FibonacciHits { get; set; }

        public int Distance { get; set; }

        public float VolumeChange { get; set; }

        public int Label { get; set; }

        public float FirstRSI { get; set; }

        public float LastRSI { get; set; }

        public float RSI_different { get; set; }
        public float MACDLine { get; set; }
        public float SignalLine { get; set; }
        public float MACDHistogram { get; set; }
        public int MACDSignalCrossover { get; set; } 
        public int BullishDivergence { get; set; } 
        public int BearishDivergence { get; set; } 
        public int MACDZeroLinePosition { get; set; } 
        public int MACDCrossoverDistance { get; set; } 
        public float MACDDistanceFromZero {get; set;}
    }
}
