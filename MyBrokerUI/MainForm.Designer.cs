namespace MyBrokerUI
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.btnMakeTestOrder = new System.Windows.Forms.Button();
            this.nudSumm = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.chbCurrentRates = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbCurrentRates = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.clbRates = new System.Windows.Forms.CheckedListBox();
            this.btnCloseOrder = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label7 = new System.Windows.Forms.Label();
            this.cbActiveStrategy = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudSumm)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnMakeTestOrder
            // 
            this.btnMakeTestOrder.Location = new System.Drawing.Point(335, 239);
            this.btnMakeTestOrder.Name = "btnMakeTestOrder";
            this.btnMakeTestOrder.Size = new System.Drawing.Size(123, 23);
            this.btnMakeTestOrder.TabIndex = 2;
            this.btnMakeTestOrder.Text = "Открыть сделку";
            this.btnMakeTestOrder.UseVisualStyleBackColor = true;
            this.btnMakeTestOrder.Click += new System.EventHandler(this.btnMakeOrder_Click);
            // 
            // nudSumm
            // 
            this.nudSumm.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudSumm.Location = new System.Drawing.Point(260, 16);
            this.nudSumm.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudSumm.Name = "nudSumm";
            this.nudSumm.Size = new System.Drawing.Size(104, 20);
            this.nudSumm.TabIndex = 6;
            this.nudSumm.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(174, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Сумма сделки";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbActiveStrategy);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.dtpEnd);
            this.groupBox2.Controls.Add(this.dtpStart);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.chbCurrentRates);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.tbCurrentRates);
            this.groupBox2.Controls.Add(this.nudSumm);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.clbRates);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(447, 221);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Настройки";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(199, 152);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(10, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "-";
            // 
            // dtpEnd
            // 
            this.dtpEnd.Enabled = false;
            this.dtpEnd.Location = new System.Drawing.Point(206, 190);
            this.dtpEnd.Margin = new System.Windows.Forms.Padding(2);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(151, 20);
            this.dtpEnd.TabIndex = 12;
            // 
            // dtpStart
            // 
            this.dtpStart.Enabled = false;
            this.dtpStart.Location = new System.Drawing.Point(58, 190);
            this.dtpStart.Margin = new System.Windows.Forms.Padding(2);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(129, 20);
            this.dtpStart.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 192);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Период";
            // 
            // chbCurrentRates
            // 
            this.chbCurrentRates.AutoSize = true;
            this.chbCurrentRates.Checked = true;
            this.chbCurrentRates.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbCurrentRates.Location = new System.Drawing.Point(9, 167);
            this.chbCurrentRates.Margin = new System.Windows.Forms.Padding(2);
            this.chbCurrentRates.Name = "chbCurrentRates";
            this.chbCurrentRates.Size = new System.Drawing.Size(127, 17);
            this.chbCurrentRates.TabIndex = 9;
            this.chbCurrentRates.Text = "Текущие котировки";
            this.chbCurrentRates.UseVisualStyleBackColor = true;
            this.chbCurrentRates.CheckedChanged += new System.EventHandler(this.chbCurrentRates_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(174, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Котировки";
            // 
            // tbCurrentRates
            // 
            this.tbCurrentRates.Enabled = false;
            this.tbCurrentRates.Location = new System.Drawing.Point(174, 61);
            this.tbCurrentRates.Multiline = true;
            this.tbCurrentRates.Name = "tbCurrentRates";
            this.tbCurrentRates.Size = new System.Drawing.Size(253, 69);
            this.tbCurrentRates.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Валютные пары";
            // 
            // clbRates
            // 
            this.clbRates.FormattingEnabled = true;
            this.clbRates.Items.AddRange(new object[] {
            "EUR/USD",
            "GBP/USD",
            "AUD/USD",
            "NZD/USD",
            "USD/CAD"});
            this.clbRates.Location = new System.Drawing.Point(9, 36);
            this.clbRates.Name = "clbRates";
            this.clbRates.Size = new System.Drawing.Size(159, 94);
            this.clbRates.TabIndex = 0;
            // 
            // btnCloseOrder
            // 
            this.btnCloseOrder.Location = new System.Drawing.Point(207, 239);
            this.btnCloseOrder.Name = "btnCloseOrder";
            this.btnCloseOrder.Size = new System.Drawing.Size(123, 23);
            this.btnCloseOrder.TabIndex = 9;
            this.btnCloseOrder.Text = "Закрыть сделку";
            this.btnCloseOrder.UseVisualStyleBackColor = true;
            this.btnCloseOrder.Click += new System.EventHandler(this.btnCloseOrder_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 239);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "Старт";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(96, 239);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 6;
            this.btnStop.Text = "Стоп";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // tbLog
            // 
            this.tbLog.Location = new System.Drawing.Point(11, 297);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLog.Size = new System.Drawing.Size(447, 219);
            this.tbLog.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 271);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Журнал";
            // 
            // chart1
            // 
            chartArea3.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea3);
            this.chart1.Location = new System.Drawing.Point(476, 12);
            this.chart1.Name = "chart1";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Candlestick;
            series3.Name = "Series1";
            series3.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            series3.YValuesPerPoint = 4;
            this.chart1.Series.Add(series3);
            this.chart1.Size = new System.Drawing.Size(597, 442);
            this.chart1.TabIndex = 10;
            this.chart1.Text = "chart1";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 143);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(109, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Активная стратегия";
            // 
            // cbActiveStrategy
            // 
            this.cbActiveStrategy.DisplayMember = "Value";
            this.cbActiveStrategy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbActiveStrategy.FormattingEnabled = true;
            this.cbActiveStrategy.Location = new System.Drawing.Point(177, 139);
            this.cbActiveStrategy.Name = "cbActiveStrategy";
            this.cbActiveStrategy.Size = new System.Drawing.Size(250, 21);
            this.cbActiveStrategy.TabIndex = 15;
            this.cbActiveStrategy.ValueMember = "Key";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1085, 528);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.btnCloseOrder);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnMakeTestOrder);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "My Broker";
            ((System.ComponentModel.ISupportInitialize)(this.nudSumm)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnMakeTestOrder;
        private System.Windows.Forms.NumericUpDown nudSumm;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckedListBox clbRates;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnCloseOrder;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbCurrentRates;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chbCurrentRates;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.ComboBox cbActiveStrategy;
        private System.Windows.Forms.Label label7;
    }
}

