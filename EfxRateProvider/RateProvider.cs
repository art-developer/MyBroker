using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using System.Data;
using System.Xml;
using System.Globalization;
using MyBroker.Domain;

namespace EfxRateProvider
{
    public class RateProvider
    {
        private const string CURRENT_RATES_URL = "http://api.efxnow.com/WebServices2.8/Service.asmx/GetRatesDataSet";
        private const string HISTORY_RATES_URL = "http://api.efxnow.com/WebServices2.8/Service.asmx/GetHistoricRatesDataSet";
        private const string KEY = "9087310700";

        private RateProvider()
        { 
        
        }

        private static RateProvider _instance=new RateProvider();
        
        public static RateProvider Instance
        {
            get 
            {
                return _instance;
            }
        }

        public IList<RateRecord> GetRates()
        {
            List<RateRecord> result = new List<RateRecord>();
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("Key", KEY);
            PostSubmitter post = new PostSubmitter(CURRENT_RATES_URL, nvc);
            post.Type = PostSubmitter.PostTypeEnum.Post;
            string resp = post.Post();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(resp.ToCharArray()));
            DataSet ds = new DataSet();
            ds.ReadXml(ms);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                RateRecord rt = new RateRecord();
                rt.Name = ds.Tables[0].Rows[i]["Quote"].ToString();
                string strRate = ds.Tables[0].Rows[i]["Display"].ToString().Substring(0, ds.Tables[0].Rows[i]["Display"].ToString().IndexOf('/'));
                rt.Value = decimal.Parse(strRate);
                object dt = ds.Tables[0].Rows[i]["UpdateTime"];
                DateTime updateTime = Convert.ToDateTime(dt).ToUniversalTime();
                rt.UpdateTime = updateTime;
                result.Add(rt);
            }
            return result;
        }


        /// <summary>
        /// ВОзвращает историю котировок за конкретный период
        /// </summary>
        /// <param name="rateName">Название валютной пары (например EUR/USD)</param>
        /// <param name="startDate">Дата начала периода</param>
        /// <param name="endDate">Дата окончания периода</param>
        /// <returns></returns>
        public IDictionary<DateTime,decimal> GetRatesHistory(string rateName,DateTime startDate,DateTime endDate)
        {
            Dictionary<DateTime, decimal> result = new Dictionary<DateTime, decimal>();
            try
            {
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("Key", KEY);
                nvc.Add("Quote", rateName);
                nvc.Add("StartDateTime", startDate.AddHours(-4).ToString("yyyy-MM-dd HH:mm:ss"));
                nvc.Add("EndDateTime", endDate.AddHours(-4).ToString("yyyy-MM-dd HH:mm:ss"));
                PostSubmitter post = new PostSubmitter(HISTORY_RATES_URL, nvc);
                post.Type = PostSubmitter.PostTypeEnum.Post;
                string resp = post.Post();
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(resp.ToCharArray()));
                DataSet ds = new DataSet();
                ds.ReadXml(ms);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
                    object dt = ds.Tables[0].Rows[i]["Time"];
                    DateTime updateTime = Convert.ToDateTime(dt).ToUniversalTime();
                    decimal price = decimal.Parse(ds.Tables[0].Rows[i]["Bid"].ToString());
                    if (!result.ContainsKey(updateTime))
                        result.Add(updateTime, price);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new Dictionary<DateTime, decimal>();
            }

        }
    }
}
