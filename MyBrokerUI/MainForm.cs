using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MyBroker.Controller;
using MyBroker.Domain;
using System.Diagnostics;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using System.Threading;

namespace MyBrokerUI
{
    public partial class MainForm : Form
    {

        private const string MAIL_TO = "bushuev.eugene.subscribe@gmail.com";
        private const string MAIL_FROM = "bushuev.eugene@gmail.com";
        private const string MAIL_SMTP_HOST = "smtp.gmail.com";
        private const string MAIL_SMTP_USERNAME = "bushuev.eugene@gmail.com";
        private const string MAIL_SMTP_PASSWORD = "834159672";

        private Dictionary<string, decimal> _currentRates;
        private MailController _mailController = null;
        
        public MainForm()
        {
            InitializeComponent();
            InitChart();
            InitOptions();
            FillStrategyList();
            _mailController = new MailController(MAIL_FROM, MAIL_SMTP_HOST, MAIL_SMTP_USERNAME, MAIL_SMTP_PASSWORD, MAIL_TO);
            MainController.Instance.MessageEvent += OnMessage;
            MainController.Instance.NewRatesEvent += OnNewRates;
            MainController.Instance.StopEvent += OnStop;
            MainController.Instance.OpenOrderEvent += OnOpenOrder;
            MainController.Instance.CloseOrderEvent += OnCloseOrder;
        }

        private void FillStrategyList()
        {
            IList<KeyValuePair<string, string>> strategies = new List<KeyValuePair<string, string>>();
            strategies.Add(new KeyValuePair<string, string>(null, "Не совершать реальных сделок"));
            strategies.Add(new KeyValuePair<string, string>("2 вершины (Виктор Сперандео)", "2 вершины (Виктор Сперандео)"));
            cbActiveStrategy.DataSource = strategies;
            cbActiveStrategy.SelectedIndex = 0;
        }

        private void InitOptions()
        {
            clbRates.SetItemChecked(0, true);
        }

        private void InitChart()
        {
            chart1.Series[0].YValueType = ChartValueType.Double;
            chart1.Series[0].XValueType = ChartValueType.Time;    
            
            
            var chartArea = chart1.ChartAreas[0];
            chartArea.AxisY2.MajorGrid.LineColor = Color.Yellow;
            chartArea.AxisY2.LabelStyle.Format = "#.####";
            chartArea.AxisX.MajorGrid.LineColor = Color.Yellow;
            chartArea.AxisX.LabelStyle.Format = "HH:mm";
            chartArea.AxisX.Interval = 5;
            chartArea.AxisX.IntervalType = DateTimeIntervalType.Minutes;

            
            chartArea.AxisX.ScrollBar.IsPositionedInside = true;
            chartArea.AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            _currentRates = new Dictionary<string, decimal>();
            EnableSettings(false);
            DateTime start = new DateTime(dtpStart.Value.Year, dtpStart.Value.Month, dtpStart.Value.Day,0,0,0);
            DateTime end = new DateTime(dtpEnd.Value.Year, dtpEnd.Value.Month, dtpEnd.Value.Day, 0, 0, 0);
            string activeStrategy = cbActiveStrategy.SelectedValue==null?null:cbActiveStrategy.SelectedValue.ToString();
            MainController.Instance.Start(GetSelectedRates(), nudSumm.Value, Application.StartupPath, chbCurrentRates.Checked ? DateTime.Now.ToUniversalTime() : start, chbCurrentRates.Checked ? (DateTime?)null : end,activeStrategy);
        }

       
        private void btnStop_Click(object sender, EventArgs e)
        {
            MainController.Instance.Stop();
        }

        private IList<string> GetSelectedRates()
        {
            IList<string> res = new List<string>();
            foreach (object obj in clbRates.CheckedItems)
            { 
                res.Add(obj.ToString());
            }
            return res;
        }

        void OnStop()
        {
            EnableSettings(true);
        }

        private void EnableSettings(bool enable)
        {
            clbRates.Enabled = enable;
            nudSumm.Enabled = enable;
            btnStart.Enabled = enable;
            chbCurrentRates.Enabled = enable;
            dtpStart.Enabled = enable;
            dtpEnd.Enabled = enable;
            btnStop.Enabled = !enable;
            cbActiveStrategy.Enabled = enable;

        }
        private Order order = null;
        private void btnMakeOrder_Click(object sender, EventArgs e)
        {
            order = new OrderController("Стратегия по тренду (2 часа)", nudSumm.Value, new List<string>(){"EUR/USD"}).OpenOrder("Стратегия по тренду (2 часа)", 0, 1, 10, "EUR/USD",DateTime.Now,"");
        }

        private void btnCloseOrder_Click(object sender, EventArgs e)
        {
            new OrderController("Стратегия по тренду (2 часа)", nudSumm.Value, new List<string>(){"EUR/USD"}).CloseOrder(order, new RateRecord());
            
        }

        private void DisplayCandle(Candle candle)
        {
            if (candle == null)
                return;
            if (GetSelectedRates()[0] != candle.RateName)
                return;
            DataPoint point = new DataPoint(candle.OpenTime.ToOADate(), new double[] { (double)candle.HighPrice, (double)candle.LowPrice, (double)candle.OpenPrice, (double)candle.ClosePrice });

            point.Color = Color.Black;
            DataPoint existingPoint=chart1.Series[0].Points.Where(item => item.XValue == candle.OpenTime.ToOADate()).FirstOrDefault();
            if (existingPoint != null)
            {
                chart1.Series[0].Points.Remove(existingPoint);
            }
            
            chart1.Series[0].Points.Add(point);
            chart1.ChartAreas[0].AxisX.Maximum = candle.OpenTime.AddMinutes(10).ToOADate();
            chart1.ChartAreas[0].AxisX.Minimum = candle.OpenTime.AddHours(-3).ToOADate();
            chart1.ChartAreas[0].AxisY2.Maximum = (double)candle.HighPrice + 0.0050; //by the values found on the loop, not good i want it to be automated when u scroll
            chart1.ChartAreas[0].AxisY2.Minimum = (double)candle.LowPrice - 0.0050;
        }

        


        private void OnNewRates(RateRecord rec,Candle candle)
        {
            try
            {
                if (rec != null)
                {
                    if (_currentRates.Keys.Contains(rec.Name))
                        _currentRates[rec.Name] = rec.Value;
                    else
                        _currentRates.Add(rec.Name, rec.Value);
                    tbCurrentRates.Text = "";
                    foreach (string key in _currentRates.Keys)
                    {
                        tbCurrentRates.Text += string.Format("{0} - {3:dd-MM-yyyy HH-mm-ss} - {1}{2}", key, _currentRates[key], Environment.NewLine, rec.UpdateTime);
                    }
                }
                DisplayCandle(candle);
            }
            catch (Exception ex)
            {
                OnMessage(ex.Message);
            }
        }

        private void chbCurrentRates_CheckedChanged(object sender, EventArgs e)
        {
            dtpEnd.Enabled = !chbCurrentRates.Checked;
            dtpStart.Enabled = !chbCurrentRates.Checked;
        }

        private void OnMessage(string message)
        {
            string logMesssage = string.Format("{0:dd.MM.yyy HH:mm:ss} - {1}{2}", DateTime.Now, message, Environment.NewLine);
            tbLog.Text += logMesssage;
            Trace.Write(logMesssage);
        }

        private void OnOpenOrder(Order order)
        {
            try
            {
                OnMessage(order.GetShortOpenMessage());
                string fileName = Path.Combine(Application.StartupPath, order.OpenTime.ToString("dd-MM-yyyy-HH-mm") + ".jpg");
                chart1.SaveImage(fileName, ChartImageFormat.Jpeg);
                Dictionary<string, string> images = new Dictionary<string, string>();
                images.Add("chart", fileName);
                _mailController.Send(order.GetShortOpenMessage(), order.GetFullOpenMessage() + Environment.NewLine + "<img src=cid:chart>", images);
            }
            catch (Exception ex)
            {
                OnMessage(ex.Message);
            }
        }

        private void OnCloseOrder(Order order)
        {
            try
            {
                OnMessage(order.GetShortResultMessage());
                string fileName = Path.Combine(Application.StartupPath, order.OpenTime.ToString("dd-MM-yyyy-HH-mm") + "(" + (order.IsProfit ? "ПРИБЫЛЬ" : "УБЫТОК") + ")" + ".jpg");
                chart1.SaveImage(fileName, ChartImageFormat.Jpeg);
                Dictionary<string, string> images = new Dictionary<string, string>();
                images.Add("chart", fileName);
                _mailController.Send(order.GetShortResultMessage(), order.GetFullResultMessage() + Environment.NewLine + "<img src=cid:chart>", images);
            }
            catch (Exception ex)
            {
                OnMessage(ex.Message);
            }
        }

    }
}
