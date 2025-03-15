using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COP_4365
{   /// <summary>
    /// this class represents for the candlestick object. A candlestick object has 6 properties and a constructor to intialize an instance of the Candlestick object
    /// </summary>
    public class Candlestick
    {   
        //declare the first attribute of the candlestick object
        //this variable has datatime type and name of "date"
        //the "get, set" provides methods for user to get the value and modify the value of the attribute 
        public DateTime date { get; set; }
        //declare the second attribute of the candlestick object
        //this variable has decimal type and name of "open"
        //the "get, set" provides methods for user to get the value and modify the value of the attribute 
        public decimal open {  get; set; }
        //declare the third attribute of the candlestick object
        //this variable has decimal type and name of "close"
        //the "get, set" provides methods for user to get the value and modify the value of the attribute 
        public decimal close { get; set; }
        //declare the fourth attribute of the candlestick object
        //this variable has decimal type and name of "low"
        //the "get, set" provides methods for user to get the value and modify the value of the attribute 
        public decimal low {  get; set; }
        //declare the fifth attribute of the candlestick object
        //this variable has decimal type and name of "high"
        //the "get, set" provides methods for user to get the value and modify the value of the attribute 
        public decimal high { get; set; }
        //declare the last attribute of the candlestick object
        //this variable has ulong type and name of "volume"
        //the ulong type is used to represent large integers
        //the "get, set" provides methods for user to get the value and modify the value of the attribute 
        public ulong volume { get; set; }

/// <summary>
/// this method is a constructor to create an instance of the candlestick object
/// the input of this method is a string of data,in this case, a line from the stock data
/// the output is a candlestick object
/// </summary>
/// <param name="rowOfData">A line from the stock data file</param>
    public Candlestick(string rowOfData)
        {
            //declare a array of seperators that contains symbols at which the string is splited
            char[] seperators = new char[] { ',', ' ','"'};
            //assign an array of type string and split the input line into smaller elements based on the seperators string
            // the removeEmptyEntries method ensures that the substring does not have any empty elements 
            string[] subs = rowOfData.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
            // assign the first element of the sub_string which represent the date to the dateString
            string dateString = subs[0];
            //Parse the string "Datestring" into datetime type and assign it into the date attributes of the candlestick
            date = DateTime.Parse(dateString);
            //declare a variable temp that has type "decimal"
            decimal temp;
            // try to parse the value of subs[1] to temp variable
            // this method will return a boolean type (true of false)
            // assign this boolean value to bool variable success
            bool success = decimal.TryParse(subs[1], out temp);
            // this sucess returns 1 (if the condition holds)
            if (success) 
            { 
                // assign temp to open
                open = temp;
                // round the value of open to two decimal points
                open = decimal.Round(open, 2);
            };
            // try to parse the value of subs[2] to temp
            // assign boolean variable success to the method tryparse
            success = decimal.TryParse(subs[2], out temp);
            // if sucess returns 1 (the condition holds)
            if (success)
            {
                //assign temp to high
                high = temp;
                //round the value of high two 2 decimal point
                high = decimal.Round(high, 2);
            };
            //try to parse the value of subs[3] to temp
            //assign boolean success variable to the tryparse method
            success = decimal.TryParse(subs[3], out temp);
            // if sucess returns 1  (condition holds)
            if (success)
            {   
                //assign temp to low
                low = temp;
                // round the value of low to 2 decimal points
                low = decimal.Round(low, 2);
            };
            //try to parse the value of subs[4] to temp
            //assign boolean success variable to the tryparse method
            success = decimal.TryParse(subs[4], out temp);
            //if the condition holds
            if (success)
            {
                //assign temp to close
                close = temp;
                //round the value of close to 2 decimal poins
                close = decimal.Round(close, 2);
            };
            // declare the variable tempVolume of type ulong
            ulong tempVolume;
            //tryparsing the value of subs[6] to tempVolume
            //assign the boolean success to the tryparse method
            success = ulong.TryParse(subs[5], out tempVolume);
            // if the condition holds, assign tempvolume to volume
            if (success) volume = tempVolume;
        }
/// <summary>
/// default constructor for Candlestick object that set the properties to their default value
/// </summary>
    public Candlestick() { }
    }
}
