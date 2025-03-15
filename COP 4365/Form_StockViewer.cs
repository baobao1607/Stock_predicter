using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.VisualStyles;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace COP_4365
{
    /// <summary>
    /// this class is named as "Form_StockViewer" that operate on the foundaton of the class "Form" provided by the Microsoft Visual Studio Framework
    /// </summary>
    public partial class Form_StockViewer : Form
    {
        // declare a variable called "list_of_candlesticks". This variable is set initially to null and has the form as a list holding the elements of the type "candlestick"
        List<Candlestick> list_of_candlesticks = null;
        // declare a varibale called "boundCandlestick". This variable is set intially to null and has the form of a bindinglist holding the elements of the type "candlestick"
        BindingList<Candlestick> boundCandlestick = null;
        //declare a dictionary to hold the recognizer;
        Dictionary<string, Recognizer> dict_recognizer = null;
        //declare a list contains smartcandlestick object
        List<SmartCandlestick> list_of_smartCandlesticks = null;
        // declare a list of filtered smartcandlestick This list holds the object after being filtered through the start and end date
        List<SmartCandlestick> filtered_smart_candlestick = null;
        //declare the string of directory
        String directory = @"C:\Users\baolam\Desktop\SPRING 2024\COP_4365_project_3\Stock_Data";
        private List<int?> list_candlestick = new List<int?> { null, null };
        List<WaveData> stock_data;
        List<double> closing_price;
        /// <summary>
        /// this method is the constructor for the form that takes the input of the pathname and the datatime value of the datatime stock picker
        /// </summary>
        /// <param name="pathname">the pathname of the choosing file</param>
        /// <param name="start">the start date</param>
        /// <param name="end">the end date</param>
        public Form_StockViewer(string pathname, DateTime start, DateTime end)
        {
            // intialize neccessary components for the form to start
            InitializeComponent();
            // initialize the list of recognizer by calling intialize_list_of_recognize
            initialize_dictionary_of_recognizer();
            // assign the start time to the input datetime start parameters
            date_time_picker_from.Value = start;
            // assign the end time to the input datatime end parameters
            date_time_picker_to.Value = end;
            // assign the list of candlesticks to the return of the go read file method that takes the pathname input parameters
            list_of_candlesticks = goReadFile(pathname);
            //fill in all the_patterns in the candlestick object and assign the list of smartcandlestick to the result
            recognizer_patterns();
            // filter the candlesticks using the filtercandlestick method
            filterCandleSticks();
            // normalize the chart based on the bindinglist of candlesticks
            normalize();
            //display the candlestick
            display_CandleStick();
            //initialize the stock data set using the initialize_stock_data method
            stock_data = initialize_stock_data();
        }

        /// <summary>
        /// This method is named as "Form_Stock_Viewer" which is a constructor that initialize 2 variables neccessary for the form to operate and
        /// also any componenets that already embedded in the network for form type.
        /// </summary>
        public Form_StockViewer()
        {
            // this code initialize all the componenets and properties needed for the form to work. These componenets and properties are set up on the designer file
            InitializeComponent();
            //intialize a list of recognizer by calling the method intialize_list_of_recognizer
            initialize_dictionary_of_recognizer();
            // Intialize the variable "list_of_candlestick" stored in Form_Stock viewer class with a capacity of 1024 items max.
            // This variable is set up at a list with each element holding the elemetns of the object "Candlesticks"
            list_of_candlesticks = new List<Candlestick>(1024);
            // intialize the variable "boundCandlestick" stored in the Form_Stock_viewer class with a dynamic capacity. This Binding list hold the elements
            // of the object "Candlesticks" which can be found in the file Candlestick.cs
            boundCandlestick = new BindingList<Candlestick>();
            //initialize a list of filtered smartCandlestick
            filtered_smart_candlestick = new List<SmartCandlestick>();
            stock_data = initialize_stock_data();
        }

        public List<WaveData> initialize_stock_data()
        {
            List<WaveData> results = new List<WaveData>();
            Analyze analyze = new Analyze();
            string filePath = "C:\\Users\\baolam\\Desktop\\SPRING 2024\\COP_4365_project_3\\Stock_Data\\WaveDataResults.csv";
            if (File.Exists(filePath))
            {
                results =  LoadFromFile(filePath);
            } else
            {
                results = analyze.Analyze_File(directory);
            }
            return results;
        }


        public List<WaveData> LoadFromFile(string filePath)
        {
            List<WaveData> result = new List<WaveData>();
            using (var reader = new StreamReader(filePath))
            {
                // Skip the header
                reader.ReadLine();

                // Read each line in the .csv file
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    WaveData waveData = new WaveData
                    {
                        Type = int.Parse(values[0]),
                        FibonacciHits = int.Parse(values[1]),
                        Distance = int.Parse(values[2]),
                        VolumeChange = float.Parse(values[3]),
                        Label = int.Parse(values[4]), // If Label is boolean
                        FirstRSI = float.Parse(values[5]),
                        LastRSI = float.Parse(values[6]),
                        RSI_different = float.Parse(values[7])
                    };

                    result.Add(waveData);
                }
            }
            return result;
        }

        /// <summary>
        /// This event-handler function describe what sequence of codes will be executed when button loader is click. 
        /// </summary>
        /// <param name="sender">Refes the object that throw the event which is button_loader</param>
        /// <param name="e">Refers to the event that the object intiated, in this case, the event is Click</param>
        private void button_loader_Click(object sender, EventArgs e)
        {
            //this code sets the text appear on the form to be "button_loader was clicked".
            this.Text = "button_loader was clicked";
            // The method display the file dialog to user. 
            openFileDialog_stockPicker.ShowDialog();
        }
        /// <summary>
        /// This event handler function called the sequence of codes to be executed when the user open multple files shown in the filedialog. 
        /// </summary>
        /// <param name="sender">The object that initiates the events, in this case, the openFileDialog_stock_picker object</param>
        /// <param name="e"> Refers the event that the object raised, in this case, when a file is opened</param>
        private void openFileDialog_stockPicker_FileOk(object sender, CancelEventArgs e)
        {
            //every time a new stock is picked, clear the current text in the combo box
            comboBox_candlestick_patterns.ResetText();
            // clear the previous annotations on the chart;
            chart_candlestick_OHLCV.Annotations.Clear();
            // assign the start_date to the date time picker value
            DateTime start_date = date_time_picker_from.Value;
            // assign the end_date to the date time picker value
            DateTime end_date = date_time_picker_to.Value;
            // count the number of files that user has chosen and assign that to numberofffiles variable
            int numberoffiles = openFileDialog_stockPicker.FileNames.Count();
            // run a loop starts at 0 and end at the number of files user chose
            for (int i = 0; i < numberoffiles; i++)
            {
                // getting the pathname of the chosen file
                string pathname = openFileDialog_stockPicker.FileNames[i];
                // getting the name of the file without the extension
                string ticker = Path.GetFileNameWithoutExtension(pathname);
                // creating an instance of the form-stockviewer object
                Form_StockViewer form_StockViewer;
                // if it is the first file that the user chose
                if (i == 0)
                {
                    //assign the current form to the form_stockviewer object
                    form_StockViewer = this;
                    // This method calls another method that read the data in the file and transform it into the list of candlestick
                    ReadCandleStickFromFile();
                    //this method fill in all the patterns for the candlestick objects and return a list of smartcandlestick
                    recognizer_patterns();
                    // this method calls another method that filtered the list of candlesticks and return a binding list of candlestick
                    filterCandleSticks();
                    // this method calls another method that change the properties of the chart area based on the stock data
                    normalize();
                    // This overload method bind the data in the binding list to the data grid view and the chart
                    display_CandleStick();
                    // Assign the text of this form to "parent + the filename"
                    form_StockViewer.Text = "Parent: " + ticker;
                }
                // if it is not the first file that the user chose
                else
                {
                    //construct the form-stock-viewer calling the constructor method
                    form_StockViewer = new Form_StockViewer(pathname, start_date, end_date);
                    //assign the text of this form to "Childer + the filename"
                    form_StockViewer.Text = "Child: " + ticker;
                    //show this form to the screen
                    form_StockViewer.Show();
                }
            }
        }
        /// <summary>
        /// This method assigns the list_of_candlesticks to the list that the GoReadFile method returns when getting the filename as input parameters
        /// </summary>
        private void ReadCandleStickFromFile()
        {
            // assign the list_of_candlesticks to the return value of the "goReadFile" method. Passed in the Filename of the file that the user opened as input parameter
            list_of_candlesticks = goReadFile(openFileDialog_stockPicker.FileName);
        }

        /// <summary>
        /// this even takes the filename as the input parameters, open the file, read the stock data, transform it into candlestick object and adding it into a list
        /// it returns a list stored the candlestick objects
        /// </summary>
        /// <param name="filename">The string that indicates the name of the file opened</param>
        /// <returns>A list stores the candlestick objects get from the data in the opened file</returns>
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
                else
                { this.Text = "Bad File" + filename; }
            }// return the list_of_candlesticks that hold all the candlesticks object
            return list_of_candlesticks;
        }
        /// <summary>
        /// This method takes two dates and filter the list_of_candlesticks such that all the Candlestick object's date lie between the two input dates
        /// </summary>
        /// <param name="unfiltered_list">The list of candlestick object that has not been filtered yet</param>
        /// <param name="startDate">The start date obtained from the date_time_picker</param>
        /// <param name="endDate">The end date obtained from the date_time_picker</param>
        /// <returns>the list of candlestick object that has been filtered</returns>
        private List<Candlestick> filterCandleSticks(List<Candlestick> unfiltered_list, DateTime startDate, DateTime endDate)
        {
            // declare a new list of candlestick object that has the same capacity as the unfiltered list
            List<Candlestick> filteredlist = new List<Candlestick>(unfiltered_list.Count());
            // run a for loop through each Candlestick object cs in the unfiltered list
            foreach (Candlestick cs in unfiltered_list)
            {
                // if the date of this Candlestick object is less than the start date then continue to the next code
                if (cs.date < startDate)
                    continue;
                // if the date of this Candlestick object is greater than the end date then break the iteration
                if (cs.date > endDate)
                    break;
                // if the date of this candlestick stays between the two date (start date and end date), add it into the filtered list
                filteredlist.Add(cs);
            }
            // return the filter list 
            return filteredlist;
        }

        /// <summary>
        /// this method is the overloading for the filtercandlestick method above, it assign a filter list as the output for the filtercandlesitck non-overload method
        /// it passes in the list_of_candlesticks, date value from date_time_picker as the input parameters
        /// </summary>
        private void filterCandleSticks()
        {
            // assign the list of candle stick called "filteredlist" as the output of the filterCandleStick non overloading method
            // this non overloading method will takes the list_of_candlesticks and date value from the date_time_picker as input
            List<Candlestick> filteredlist = filterCandleSticks(list_of_candlesticks, date_time_picker_from.Value, date_time_picker_to.Value);
            // the filtered list is then converted into the boundCandlestick
            boundCandlestick = new BindingList<Candlestick>(filteredlist);
            // filter the list of smartcandlestick by calling the method filtered_list_of_smartcandlestick and assign it to the filtered_smart_candlestick
            filtered_smart_candlestick = filtered_list_of_smart_candlestick(list_of_smartCandlesticks, date_time_picker_from.Value, date_time_picker_to.Value);

            closing_price = Calculate_Closing_Prices(filtered_smart_candlestick);
            // Access the static method directly using the class name
            var (macdLine, signalLine, histogram) = MCAD.CalculateMACD(closing_price);

            Display_MACD(macdLine, signalLine, histogram);
        }



        private void Display_MACD(List<double> macdLine, List<double> signalLine, List<double> histogram)
        {
            for (int i = chart_candlestick_OHLCV.Series.Count - 1; i >= 0; i--)
            {
                if (chart_candlestick_OHLCV.Series[i].ChartArea == "ChartArea_MACD")
                {
                    chart_candlestick_OHLCV.Series.RemoveAt(i);
                }
            }
            // Add a ChartArea
            chart_candlestick_OHLCV.ChartAreas[1].AxisX.Title = "Date";
            chart_candlestick_OHLCV.ChartAreas[1].AxisY.Title = "Value";
            chart_candlestick_OHLCV.ChartAreas[1].AxisX.MajorGrid.Enabled = false;
            chart_candlestick_OHLCV.ChartAreas[1].AxisX.LabelStyle.Format = "MM/dd/yyyy"; // Format dates on X-axis
            chart_candlestick_OHLCV.ChartAreas[1].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            // Add MACD Line series
            Series macdSeries = new Series("MACD Line")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 2,
                Color = Color.Blue,
                ChartArea = "ChartArea_MACD",
                IsXValueIndexed = true
            };
            for (int i = 0; i < macdLine.Count; i++)
            {
                macdSeries.Points.AddXY(boundCandlestick[i].date, macdLine[i]);
            }

            // Add Signal Line series
            Series signalSeries = new Series("Signal Line")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 2,
                Color = Color.Red,
                ChartArea = "ChartArea_MACD",
                IsXValueIndexed = true
            };
            for (int i = 0; i < signalLine.Count; i++)
            {
                signalSeries.Points.AddXY(boundCandlestick[i].date, signalLine[i]);
            }

            // Add Histogram series
            Series histogramSeries = new Series("Histogram")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.Green,
                ChartArea = "ChartArea_MACD",
                IsXValueIndexed = true
            };
            for (int i = 0; i < histogram.Count; i++)
            {
                histogramSeries.Points.AddXY(boundCandlestick[i].date, histogram[i]);
            }

            

            // Add series to the chart
            chart_candlestick_OHLCV.Series.Add(macdSeries);
            chart_candlestick_OHLCV.Series.Add(signalSeries);
            chart_candlestick_OHLCV.Series.Add(histogramSeries);
        }



        public List<double> Calculate_Closing_Prices(List<SmartCandlestick> source)
        {
            List<double> result = new List<double>();


            // Define the start and end SmartCandlesticks
            SmartCandlestick start_scs = source[0]; // First element in the source
            int starting_index = -1;

            // Find the index of start_scs in the main candlestick list
            for (int i = 0; i < list_of_smartCandlesticks.Count; i++)
            {
                if (list_of_smartCandlesticks[i].date == start_scs.date)
                {
                    starting_index = i;
                    break;
                }
            }

            // Calculate the starting index for historical data (26 periods before)
            int need_index = Math.Max(0, starting_index - 35);

            // Add historical data (if available)
            for (int i = need_index; i < starting_index; i++)
            {
                result.Add((double)list_of_smartCandlesticks[i].close);
            }

            // Add the closing prices from the source list
            foreach (var candlestick in source)
            {
                result.Add((double)candlestick.close);
            }

            return result;
        }


        /// <summary>
        /// this display function takes in a binding list of the candlestick object, bind the binding list to the data grid view and the chart 
        /// </summary>
        /// <param name="boundcandlestick_source">The binding list of candlestick object</param>
        /// <returns>The binding list of candlestick object</returns>
        private BindingList<Candlestick> display_CandleStick(BindingList<Candlestick> boundcandlestick_source)
        {
            // set the data souce for chart_candlestick_OHLCV 
            // the chart datasource will be the data in the binding list boundcandlestick_source
            chart_candlestick_OHLCV.DataSource = boundcandlestick_source;
            // get information from data source and display the data as charts
            chart_candlestick_OHLCV.DataBind();
            //return the input binding list
            return boundcandlestick_source;
        }
        /// <summary>
        /// this method is an overload for display_Candlestick method
        /// it takes the boundCandlestick binding list as input and return the same binding list
        /// </summary>
        private void display_CandleStick()
        {
            // assign boundCandlestick binding list as the return output for display_candlestick method
            // the input parameters is the same binding list "boundCandlestick"
            boundCandlestick = display_CandleStick(boundCandlestick);
        }
        /// <summary>
        /// // this method will change the properties of the list based on the input binding list source
        /// </summary>
        /// <param name="boundcandlestick_source">The binding list of object Candlestick</param>
        /// <returns>The same input binding list of object Candlestick</returns>
        private BindingList<Candlestick> normalize(BindingList<Candlestick> boundcandlestick_source)
        {
            //declare a variable min price and set it to greatest maximum decimal value
            decimal Min_price = decimal.MaxValue;
            // declare a variable max price and set it equal to 0
            decimal Max_price = 0;
            // create a loop run through every object candlestick in the binding list
            foreach (Candlestick cs in boundcandlestick_source)
            {
                // if the high value of a candlestick is greater than max_price, then assign max_price to the high value
                if (cs.high >= Max_price)
                {
                    Max_price = cs.high;
                } // if the low value of a candlestick is lower than the min_price, then assign min_price to the min value
                if (cs.low <= Min_price)
                {
                    Min_price = cs.low;
                }
            }
            // the code below shrink the chart based on the data
            //change the minimum value of axis y based on the obtained min price above
            //this value is calculate as 98% of the min_price
            chart_candlestick_OHLCV.ChartAreas["ChartArea_OHLC"].AxisY.Minimum = (double)(Min_price - (Min_price * 0.02m));
            //change the maximum value of axis y based on the obtained min price above
            //this value is calculate as 102% of the max price
            chart_candlestick_OHLCV.ChartAreas["ChartArea_OHLC"].AxisY.Maximum = (double)(Max_price + (Max_price * 0.02m));
            // return the input binding list
            return boundcandlestick_source;
        }
        /// <summary>
        /// this method is an overload method for normalize function.
        /// it takes in boundcandlestick as input and return the same binding list
        /// </summary>
        private void normalize()
        {
            // assign the binding list boundCandlestick as the output of the normalize function
            // this normalize method takes in the same binding list as input
            // it does not change value of the binding list
            boundCandlestick = normalize(boundCandlestick);
        }


        /// <summary>
        /// this event handler function activates the code when the update button is clicked
        /// </summary>
        /// <param name="sender">the update button</param>
        /// <param name="e">the event when the update button is clicked</param>
        private void button_update_date_Click(object sender, EventArgs e)
        {
            //reset the combo_box evertime user hit the update button
            comboBox_candlestick_patterns.ResetText();
            // called the filtercandlestick method to re filter the candlesticks
            filterCandleSticks();
            // normalize the chart based on the new filtered binding list
            normalize();
            // display the binding list on data grid view and chart
            display_CandleStick();
            // clear the previous annotations
            chart_candlestick_OHLCV.Annotations.Clear();
        }
        /// <summary>
        /// this method delete the previous item in the combo box items
        /// </summary>
        private void delete_combo_box_items()
        {
            // calling the method to delete the item previously stored in the combo box. 
            comboBox_candlestick_patterns.Items.Clear();
            // make the text of the combo box to null.
            comboBox_candlestick_patterns.ResetText();
        }
        /// <summary>
        /// this method adding the annotations to the chart based on the selected patterns on the combo box
        /// </summary>
        /// <param name="selected_patterns">the input of this method - the selected patterns in the combo box</param>
        private void updateAnnotate(string selected_patterns)
        {
            //clear any previous annotation on the chart
            chart_candlestick_OHLCV.Annotations.Clear();
            // count the number of data points on the first series of the chart
            int numberofpoints = chart_candlestick_OHLCV.Series["Series_OHLC"].Points.Count;
            //running through each point of data on the chart
            for (int i = 0; i < numberofpoints; i++)
            {
                //assign smartcandlestick object to the index of the filtered_smart_candlestick
                SmartCandlestick csc = filtered_smart_candlestick[i];
                //if the key value pair return true, add the arrow annotation to the object
                if (csc.dictionary_patterns[selected_patterns] == true)
                {
                    // get the pattern length of the recognizer 
                    int pattern_length = dict_recognizer[selected_patterns].patternlength;
                    // if the patttern length is 1
                    if (pattern_length == 1)
                    {
                        //add arrow annotation to that smartcandlestick object
                        add_arrow_annotation(i, csc);
                    }
                    // if the pattern length is 2
                    else if (pattern_length == 2)
                    {
                        //add arrow annotation to that smartcandlestick object
                        add_arrow_annotation(i, csc);
                        //add another arrow annotation to the smartcandlestick stands before the current one
                        //first check if this smartcandlestick object is out of bound or not
                        if (i - 1 >= 0)
                        {
                            add_arrow_annotation(i - 1, filtered_smart_candlestick[i - 1]);
                        }
                    }
                    // if the pattern length is 3
                    else
                    {
                        //add arrow annotation to that smartcandlestick object
                        add_arrow_annotation(i, csc);
                    }
                }
            }
        }
        /// <summary>
        /// this method handle the event when user select a different item in the combo box
        /// </summary>
        /// <param name="sender">the index of the item</param>
        /// <param name="e">when the index change</param>
        private void comboBox_candlestick_patterns_SelectedIndexChanged(object sender, EventArgs e)
        {
            // assign the current selecting item of the combo box to the selected pattern string
            string select_patterns = comboBox_candlestick_patterns.SelectedItem.ToString();
            //call the update annotate on this string
            updateAnnotate(select_patterns);
        }

        /// <summary>
        /// this method checks all patterns that each object or combo of object has in the list of candlestick and append them to the list of smartcandlestick.
        /// </summary>
        private void recognizer_patterns()
        {
            list_of_smartCandlesticks = new List<SmartCandlestick>();
            // run through each candlestick object in the list_of_candlestick
            foreach (Candlestick cs in list_of_candlesticks)
            {
                //convert it into a smartcandlestick object
                SmartCandlestick scs = new SmartCandlestick(cs);
                //add it into the list_of_smartcandlestick
                list_of_smartCandlesticks.Add(scs);
            }
            // for each pattern recognizer in the dictionary of recognizer values
            foreach (Recognizer r in dict_recognizer.Values)
            {
                // call recognize all to run the list_of_smartcandlestick through that recognizer. 
                r.RecognizeAll(list_of_smartCandlesticks);
            }
        }
        private List<SmartCandlestick> filtered_list_of_smart_candlestick(List<SmartCandlestick> source, DateTime startDate, DateTime endDate)
        {
            // declare a new list of candlestick object that has the same capacity as the unfiltered list
            List<SmartCandlestick> filteredlist = new List<SmartCandlestick>(source.Count());
            // run a for loop through each Candlestick object cs in the unfiltered list
            foreach (SmartCandlestick cs in source)
            {
                // if the date of this Candlestick object is less than the start date then continue to the next code
                if (cs.date < startDate)
                    continue;
                // if the date of this Candlestick object is greater than the end date then break the iteration
                if (cs.date > endDate)
                    break;
                // if the date of this candlestick stays between the two date (start date and end date), add it into the filtered list
                filteredlist.Add(cs);
            }
            // return the filter list 
            return filteredlist;
        }

        /// <summary>
        /// this method intialize the dictionary contains the recognizer 
        /// </summary>
        private void initialize_dictionary_of_recognizer()
        {
            // intialize the dictionary of recognizer of the form
            dict_recognizer = new Dictionary<string, Recognizer>();
            // Add a key-value pair: key is the pattern name of Recognizer_Bullish, value is a new Recognizer_Bullish object
            dict_recognizer.Add(new Recognizer_Bullish().patternName, new Recognizer_Bullish());
            // Add a key-value pair: key is the pattern name of Recognizer_Bearish, value is a new Recognizer_Bearish object
            dict_recognizer.Add(new Recognizer_Bearish().patternName, new Recognizer_Bearish());
            // Add a key-value pair: key is the pattern name of Recognizer_Doji, value is a new Recognizer_Doji object
            dict_recognizer.Add(new Recognizer_Doji().patternName, new Recognizer_Doji());
            // Add a key-value pair: key is the pattern name of Recognizer_DragonFly_Doji, value is a new Recognizer_DragonFly_Doji object
            dict_recognizer.Add(new Recognizer_DragonFly_Doji().patternName, new Recognizer_DragonFly_Doji());
            // Add a key-value pair: key is the pattern name of Recognizer_GraveStone_Doji, value is a new Recognizer_GraveStone_Doji object
            dict_recognizer.Add(new Recognizer_GraveStone_Doji().patternName, new Recognizer_GraveStone_Doji());
            // Add a key-value pair: key is the pattern name of Recognizer_Hammer, value is a new Recognizer_Hammer object
            dict_recognizer.Add(new Recognizer_Hammer().patternName, new Recognizer_Hammer());
            // Add a key-value pair: key is the pattern name of Recognizer_Marobozu, value is a new Recognizer_Marobozu object
            dict_recognizer.Add(new Recognizer_Marobozu().patternName, new Recognizer_Marobozu());
            // Add a key-value pair: key is the pattern name of Recognizer_Neutral, value is a new Recognizer_Neutral object
            dict_recognizer.Add(new Recognizer_Neutral().patternName, new Recognizer_Neutral());
            // Add a key-value pair: key is the pattern name of Recognizer_Bullish_Engulfing, value is a new Recognizer_Bullish_Engulfing object
            dict_recognizer.Add(new Recognizer_Bullish_Engulfing().patternName, new Recognizer_Bullish_Engulfing());
            // Add a key-value pair: key is the pattern name of Recognizer_Bearish_Engulfing, value is a new Recognizer_Bearish_Engulfing object
            dict_recognizer.Add(new Recognizer_Bearish_Engulfing().patternName, new Recognizer_Bearish_Engulfing());
            // Add a key-value pair: key is the pattern name of Recognizer_Bearish_Harami, value is a new Recognizer_Bearish_Harami object
            dict_recognizer.Add(new Recognizer_Bearish_Harami().patternName, new Recognizer_Bearish_Harami());
            // Add a key-value pair: key is the pattern name of Recognizer_Bullish_Harami, value is a new Recognizer_Bullish_Harami object
            dict_recognizer.Add(new Recognizer_Bullish_Harami().patternName, new Recognizer_Bullish_Harami());
            // Add a key-value pair: key is the pattern name of Recognizer_Peak, value is a new Recognizer_Peak object
            dict_recognizer.Add(new Recognizer_Peak().patternName, new Recognizer_Peak());
            // Add a key-value pair: key is the pattern name of Recognizer_Valley, value is a new Recognizer_Valley object
            dict_recognizer.Add(new Recognizer_Valley().patternName, new Recognizer_Valley());
            // iterate throught each key of the dictionary to add the pattern name of the recognizer to the combo box
            foreach (string patternName in dict_recognizer.Keys)
            {
                // add the pattern name of each recognizer to the combo box items
                comboBox_candlestick_patterns.Items.Add(patternName);
            }
        }

        /// <summary>
        /// this method add an arrow annotaion pointing to the smartcandlestick at index i on the graph
        /// </summary>
        /// <param name="i">index of the data points on the graph</param>
        /// <param name="csc">smartcandlestick object where user wants to point the arrow annotation to</param>
        private void add_arrow_annotation(int i, SmartCandlestick csc)
        {
            //create a new object of arrow annotation
            ArrowAnnotation arrowAnnotation = new ArrowAnnotation();
            // anchor this arrow annotaion to the current data point
            arrowAnnotation.AnchorDataPoint = chart_candlestick_OHLCV.Series["Series_OHLC"].Points[i];
            // set the size of the annotation
            arrowAnnotation.ArrowSize = 1;
            //set the fill color of the annotation
            arrowAnnotation.BackColor = Color.Blue;
            //set the width of the annotation
            arrowAnnotation.Width = 0;
            //calculate the distance between the candlestick and the maximum Y axis
            double above_cs_height = chart_candlestick_OHLCV.ChartAreas[0].AxisY.Maximum - (double)(csc.topPrice);
            // calculate the distance between the candlestick and the minimum Y axis
            double below_cs_height = (double)(csc.bottomPrice) - chart_candlestick_OHLCV.ChartAreas[0].AxisY.Minimum;
            // if the candlestick lean more to the bottom of the chart
            if (above_cs_height > below_cs_height)
            {
                // set the position of the annotation above the candlestick
                arrowAnnotation.Y = (chart_candlestick_OHLCV.ChartAreas[0].AxisY.Maximum + (double)(csc.topPrice)) * 0.5;
                // set the height to negative so it point down
                arrowAnnotation.Height = -12;
                // otherwise, if the candlestick lean more the top of the chart
            }
            else
            {
                // set the position of the annotation below the candlestick
                arrowAnnotation.Y = (chart_candlestick_OHLCV.ChartAreas[0].AxisY.Minimum + (double)(csc.bottomPrice)) * 0.5;
                // set the height to postivie so it point up
                arrowAnnotation.Height = 12;
            }
            // add the annotation to the collection of the annotations
            chart_candlestick_OHLCV.Annotations.Add(arrowAnnotation);
        }

        private void button_Train_Click(object sender, EventArgs e)
        {
            Training trainer = new Training(stock_data);
            List<WaveData> newWave = get_Wave();
            if (newWave.Count == 0)
            {
                return;
            } 
            trainer.Train(newWave);
            list_candlestick = new List<int?> { null, null };
            chart_candlestick_OHLCV.Annotations.Clear();
        }

        private List<WaveData> get_Wave()
        {
            List<WaveData> newWave = new List<WaveData>();
            int type = -1;
            bool result = false ;
            if (list_candlestick[0].HasValue && list_candlestick[1].HasValue)
            {
                int first_index = list_candlestick[0].Value;
                int second_index = list_candlestick[1].Value;
                Recognizer_Peak recognizer = new Recognizer_Peak();
                Recognizer_Valley recognizer_valley = new Recognizer_Valley();
                Analyze analyze = new Analyze();
                List<double> closing_price = analyze.get_closing_price(list_of_smartCandlesticks);
                var (macdLine, signalLine, histogram) = MCAD.CalculateMACD_DataSet(closing_price);
                if (recognizer.Recognize(list_of_smartCandlesticks, first_index))
                {
                    type = 1;
                    result = analyze.IsValidWave(first_index, second_index, list_of_smartCandlesticks, type);
                }
                else if (recognizer_valley.Recognize(list_of_smartCandlesticks, first_index))
                {
                    type = 0;
                    result = analyze.IsValidWave(first_index, second_index, list_of_smartCandlesticks, type);
                }
                else 
                {
                    result = false;
                }
                if (!result)
                {
                    MessageBox.Show("This is an invalid wave");
                    list_candlestick = new List<int?> { null, null };
                    chart_candlestick_OHLCV.Annotations.Clear();
                }
                else
                {

                    int index = second_index - first_index + 1;
                    int fibonacci_hits = analyze.get_fibonnaci_hit(first_index, second_index, list_of_smartCandlesticks, type);
                    float volume_change = analyze.Calculate_Volume(first_index, second_index, list_of_smartCandlesticks);
                    decimal first_rsi = analyze.Calculate_RSI(list_of_smartCandlesticks, first_index);
                    decimal second_rsi = analyze.Calculate_RSI(list_of_smartCandlesticks, second_index);
                    decimal rsi_different = second_rsi - first_rsi;
                    int crossover = analyze.calculate_crossover(second_index, macdLine, signalLine);

                    int bullish_divergence = analyze.calculate_bullish(list_of_smartCandlesticks, macdLine, second_index);
                    int bearish_divergence = analyze.calculate_bearish(list_of_smartCandlesticks, macdLine, second_index);

                    int zero_line_position = analyze.calculate_zero_line_position(macdLine[second_index]);
                    int MACD_distance = analyze.CalculateMACDCrossoverDistance(macdLine, signalLine, second_index);


                    float MACD_distance_zero = analyze.CalculateMACDDistanceFromZero(macdLine[second_index]);
                    WaveData wave = new WaveData
                    {
                        Type = type,                      
                        Distance = index,                  
                        FibonacciHits = fibonacci_hits,  
                        VolumeChange = volume_change,
                        FirstRSI = (float)first_rsi,
                        LastRSI = (float)second_rsi,
                        RSI_different = (float)rsi_different,
                        MACDLine = (float)(macdLine[second_index]),
                        SignalLine = (float)(signalLine[second_index]),
                        MACDHistogram = (float)(histogram[second_index]),
                        MACDSignalCrossover = crossover,
                        BearishDivergence = bearish_divergence,
                        BullishDivergence = bullish_divergence,
                        MACDZeroLinePosition = zero_line_position,
                        MACDCrossoverDistance = MACD_distance,
                        MACDDistanceFromZero = MACD_distance_zero
                    };
                    newWave.Add(wave);
                }
            } 
            else
            {
                MessageBox.Show("Please select a wave");
            }
            return newWave;
        }

        private void chart_candlestick_OHLCV_MouseClick(object sender, MouseEventArgs e)
        {
            HitTestResult hitResult = chart_candlestick_OHLCV.HitTest(e.X, e.Y);
            if (hitResult.ChartElementType == ChartElementType.DataPoint)
            {
                HandleCandlestickClick(hitResult);
            }
            else
            {
                MessageBox.Show("You clicked on a non-candlestick element.");
            }
        }

        private void HandleCandlestickClick(HitTestResult hitResult)
        {
            // Find the index of the clicked candlestick
            int index = -1;
            int data_point = -1;
            for (int i = 0; i < list_of_smartCandlesticks.Count; i++)
            {
                if (list_of_smartCandlesticks[i].date == DateTime.FromOADate(hitResult.Series.Points[hitResult.PointIndex].XValue))
                {
                    index = i;
                    break;
                }
            }
            for (int i = 0; i <= filtered_smart_candlestick.Count; i++)
            {
                if (filtered_smart_candlestick[i].date == DateTime.FromOADate(hitResult.Series.Points[hitResult.PointIndex].XValue))
                {
                    data_point = i;
                    break;
                }
            }
            // If the index is valid, update the list
            if (index != -1)
            {
                if (list_candlestick[0] == null)
                {
                    list_candlestick[0] = index;
                    MessageBox.Show($"First candlestick clicked");
                    add_arrow_annotation(data_point, filtered_smart_candlestick[data_point]);
                }
                else if (list_candlestick[1] == null)
                {
                    list_candlestick[1] = index;
                    MessageBox.Show($"Second candlestick clicked");
                    add_arrow_annotation(data_point, filtered_smart_candlestick[data_point]);
                }
            }
        }

    }
}

