namespace COP_4365
{
    partial class Form_StockViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.button_loader = new System.Windows.Forms.Button();
            this.openFileDialog_stockPicker = new System.Windows.Forms.OpenFileDialog();
            this.candlestickBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.chart_candlestick_OHLCV = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.date_time_picker_from = new System.Windows.Forms.DateTimePicker();
            this.date_time_picker_to = new System.Windows.Forms.DateTimePicker();
            this.Label_from = new System.Windows.Forms.Label();
            this.label_end_at = new System.Windows.Forms.Label();
            this.button_update_date = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBox_candlestick_patterns = new System.Windows.Forms.ComboBox();
            this.button_Predict = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.candlestickBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_candlestick_OHLCV)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_loader
            // 
            this.button_loader.Location = new System.Drawing.Point(407, 16);
            this.button_loader.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_loader.Name = "button_loader";
            this.button_loader.Size = new System.Drawing.Size(116, 50);
            this.button_loader.TabIndex = 1;
            this.button_loader.Text = "Pick a Stock";
            this.button_loader.UseVisualStyleBackColor = true;
            this.button_loader.Click += new System.EventHandler(this.button_loader_Click);
            // 
            // openFileDialog_stockPicker
            // 
            this.openFileDialog_stockPicker.Filter = "All Files|*.csv|Monthly|*Month.csv|Weekly|*Week.csv|Daily|*Day.csv|AAPL|AAPL-*.cs" +
    "v|GOOG|GOOG-*.csv|IBM|IBM-*.csv|MSFT|MSFT-*csv";
            this.openFileDialog_stockPicker.FilterIndex = 2;
            this.openFileDialog_stockPicker.InitialDirectory = "C:\\Users\\baolam\\Desktop\\SPRING 2024\\COP_4365_project_3\\Stock_Data";
            this.openFileDialog_stockPicker.Multiselect = true;
            this.openFileDialog_stockPicker.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog_stockPicker_FileOk);
            // 
            // candlestickBindingSource
            // 
            this.candlestickBindingSource.CurrentChanged += new System.EventHandler(this.button_loader_Click);
            // 
            // chart_candlestick_OHLCV
            // 
            chartArea5.Name = "ChartArea_OHLC";
            chartArea6.AlignWithChartArea = "ChartArea_OHLC";
            chartArea6.Name = "ChartArea_volume";
            this.chart_candlestick_OHLCV.ChartAreas.Add(chartArea5);
            this.chart_candlestick_OHLCV.ChartAreas.Add(chartArea6);
            legend3.Enabled = false;
            legend3.Name = "Legend1";
            this.chart_candlestick_OHLCV.Legends.Add(legend3);
            this.chart_candlestick_OHLCV.Location = new System.Drawing.Point(11, 10);
            this.chart_candlestick_OHLCV.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chart_candlestick_OHLCV.Name = "chart_candlestick_OHLCV";
            series5.ChartArea = "ChartArea_OHLC";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Candlestick;
            series5.CustomProperties = "PriceDownColor=Red, PriceUpColor=Lime";
            series5.IsXValueIndexed = true;
            series5.Legend = "Legend1";
            series5.Name = "Series_OHLC";
            series5.XValueMember = "date";
            series5.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            series5.YValueMembers = "high,low,open,close";
            series5.YValuesPerPoint = 4;
            series6.ChartArea = "ChartArea_volume";
            series6.IsXValueIndexed = true;
            series6.Legend = "Legend1";
            series6.Name = "Series_volume";
            series6.XValueMember = "date";
            series6.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            series6.YValueMembers = "volume";
            this.chart_candlestick_OHLCV.Series.Add(series5);
            this.chart_candlestick_OHLCV.Series.Add(series6);
            this.chart_candlestick_OHLCV.Size = new System.Drawing.Size(1016, 373);
            this.chart_candlestick_OHLCV.TabIndex = 3;
            // 
            // date_time_picker_from
            // 
            this.date_time_picker_from.CustomFormat = "MM-dd-yyyy";
            this.date_time_picker_from.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.date_time_picker_from.Location = new System.Drawing.Point(108, 16);
            this.date_time_picker_from.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.date_time_picker_from.Name = "date_time_picker_from";
            this.date_time_picker_from.Size = new System.Drawing.Size(128, 22);
            this.date_time_picker_from.TabIndex = 4;
            this.date_time_picker_from.Value = new System.DateTime(2022, 1, 1, 0, 0, 0, 0);
            // 
            // date_time_picker_to
            // 
            this.date_time_picker_to.CustomFormat = "MM-dd-yyyy";
            this.date_time_picker_to.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.date_time_picker_to.Location = new System.Drawing.Point(600, 16);
            this.date_time_picker_to.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.date_time_picker_to.Name = "date_time_picker_to";
            this.date_time_picker_to.Size = new System.Drawing.Size(128, 22);
            this.date_time_picker_to.TabIndex = 6;
            // 
            // Label_from
            // 
            this.Label_from.AutoSize = true;
            this.Label_from.Location = new System.Drawing.Point(19, 16);
            this.Label_from.Name = "Label_from";
            this.Label_from.Size = new System.Drawing.Size(68, 16);
            this.Label_from.TabIndex = 7;
            this.Label_from.Text = "Start From\r\n";
            // 
            // label_end_at
            // 
            this.label_end_at.AutoSize = true;
            this.label_end_at.Location = new System.Drawing.Point(543, 16);
            this.label_end_at.Name = "label_end_at";
            this.label_end_at.Size = new System.Drawing.Size(46, 16);
            this.label_end_at.TabIndex = 8;
            this.label_end_at.Text = "End At";
            // 
            // button_update_date
            // 
            this.button_update_date.Location = new System.Drawing.Point(272, 16);
            this.button_update_date.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_update_date.Name = "button_update_date";
            this.button_update_date.Size = new System.Drawing.Size(121, 50);
            this.button_update_date.TabIndex = 13;
            this.button_update_date.Text = "Update";
            this.button_update_date.UseVisualStyleBackColor = true;
            this.button_update_date.Click += new System.EventHandler(this.button_update_date_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.date_time_picker_to);
            this.panel1.Controls.Add(this.button_update_date);
            this.panel1.Controls.Add(this.button_loader);
            this.panel1.Controls.Add(this.date_time_picker_from);
            this.panel1.Controls.Add(this.label_end_at);
            this.panel1.Controls.Add(this.Label_from);
            this.panel1.Location = new System.Drawing.Point(14, 387);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(793, 80);
            this.panel1.TabIndex = 14;
            // 
            // comboBox_candlestick_patterns
            // 
            this.comboBox_candlestick_patterns.FormattingEnabled = true;
            this.comboBox_candlestick_patterns.Location = new System.Drawing.Point(1034, 10);
            this.comboBox_candlestick_patterns.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBox_candlestick_patterns.Name = "comboBox_candlestick_patterns";
            this.comboBox_candlestick_patterns.Size = new System.Drawing.Size(108, 24);
            this.comboBox_candlestick_patterns.TabIndex = 14;
            this.comboBox_candlestick_patterns.SelectedIndexChanged += new System.EventHandler(this.comboBox_candlestick_patterns_SelectedIndexChanged);
            // 
            // button_Predict
            // 
            this.button_Predict.Location = new System.Drawing.Point(1047, 280);
            this.button_Predict.Name = "button_Predict";
            this.button_Predict.Size = new System.Drawing.Size(172, 46);
            this.button_Predict.TabIndex = 15;
            this.button_Predict.Text = "Predict";
            this.button_Predict.UseVisualStyleBackColor = true;
            this.button_Predict.Click += new System.EventHandler(this.button_Predict_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(1047, 348);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(172, 24);
            this.comboBox1.TabIndex = 16;
            // 
            // Form_StockViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1267, 602);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button_Predict);
            this.Controls.Add(this.comboBox_candlestick_patterns);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.chart_candlestick_OHLCV);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form_StockViewer";
            this.Text = "Please Pick A Stock";
            ((System.ComponentModel.ISupportInitialize)(this.candlestickBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_candlestick_OHLCV)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_loader;
        private System.Windows.Forms.OpenFileDialog openFileDialog_stockPicker;
        private System.Windows.Forms.BindingSource candlestickBindingSource;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_candlestick_OHLCV;
        private System.Windows.Forms.DateTimePicker date_time_picker_from;
        private System.Windows.Forms.DateTimePicker date_time_picker_to;
        private System.Windows.Forms.Label Label_from;
        private System.Windows.Forms.Label label_end_at;
        private System.Windows.Forms.Button button_update_date;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox comboBox_candlestick_patterns;
        private System.Windows.Forms.Button button_Predict;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}

