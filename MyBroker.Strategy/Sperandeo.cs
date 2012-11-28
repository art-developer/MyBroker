using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyBroker.Domain;
using MyBroker.Domain.Interfaces;

namespace MyBroker.Strategy
{
    public class Sperandeo:IStrategyProvider
    {

        protected const int CANDLES_INTERVAL_MINUTES = 5;
        protected const int CANDLES_RANGE = 25;
        
        protected IDictionary<string,IList<Candle>> _candles;
        
        #region IStrategyProvider Members

        public virtual string GetName()
        {
            return "2 вершины (Виктор Сперандео)";
        }

        public IStrategyProvider Init(IDictionary<string,IDictionary<DateTime, decimal>> historyData)
        {
            _candles=StrategyHelper.BuildCandles(historyData, CANDLES_INTERVAL_MINUTES);
            return this;
        }

        protected Candle GetMaximumForInterval(string rateName, int interval, Candle to, decimal fromValue, decimal toValue)
        {
            IList<Candle> candles = _candles[rateName].Where(item => item.OpenTime < to.OpenTime).OrderByDescending(item => item.OpenTime).Take(interval).ToList();
            if (candles.Count == 0)
                return null;
            decimal maxPrice = candles.Max(item => item.HighPrice);
            if (maxPrice < fromValue || maxPrice > toValue)
                return null;
            return candles.Where(item => item.HighPrice == maxPrice).FirstOrDefault();
        }

        protected Candle GetMaximumForInterval(string rateName, int interval, Candle to)
        {
            IList<Candle> candles = _candles[rateName].Where(item => item.OpenTime < to.OpenTime).OrderByDescending(item => item.OpenTime).Take(interval).ToList();
            if (candles.Count == 0)
                return null;
            decimal maxPrice = candles.Max(item => item.HighPrice);
            return candles.Where(item => item.HighPrice == maxPrice).FirstOrDefault();
        }

        protected Candle GetMinimumForInterval(string rateName, int interval,Candle to)
        {
            IList<Candle> candles = _candles[rateName].Where(item => item.OpenTime < to.OpenTime).OrderByDescending(item => item.OpenTime).Take(interval).ToList();
            if (candles.Count == 0)
                return null;
            decimal minPrice = candles.Min(item => item.LowPrice);
            return candles.Where(item => item.LowPrice == minPrice).FirstOrDefault();
        }

        protected Candle GetMinimumBetween(string rateName, Candle from, Candle to)
        {
            IList<Candle> candles = _candles[rateName].Where(item => item.OpenTime < to.OpenTime && item.OpenTime > from.OpenTime).ToList();
            if (candles.Count == 0)
                return null;
            decimal minPrice = candles.Min(item => item.LowPrice);
            Candle minimum = candles.Where(item => item.LowPrice == minPrice).FirstOrDefault();
            if (minimum.OpenTime.AddMinutes(CANDLES_INTERVAL_MINUTES) == to.OpenTime)
            {
                return GetMinimumBetween(rateName, from, minimum);
            }
            if (minimum.OpenTime.AddMinutes(-CANDLES_INTERVAL_MINUTES) == from.OpenTime)
            {
                return GetMinimumBetween(rateName, minimum, to);
            }

            return minimum;
        }

        protected Candle GetMaximumBetween(string rateName, Candle from, Candle to)
        {
            IList<Candle> candles = _candles[rateName].Where(item => item.OpenTime < to.OpenTime && item.OpenTime > from.OpenTime).ToList();
            if(candles.Count == 0)
                return null;
            decimal maxPrice = candles.Max(item => item.HighPrice);
            Candle maximum=candles.Where(item => item.HighPrice == maxPrice).FirstOrDefault();
            if(maximum.OpenTime.AddMinutes(CANDLES_INTERVAL_MINUTES)==to.OpenTime)
            {
                return GetMaximumBetween(rateName, from, maximum);
            }
            if (maximum.OpenTime.AddMinutes(-CANDLES_INTERVAL_MINUTES) == from.OpenTime)
            {
                return GetMaximumBetween(rateName, maximum, to);
            }

            return maximum;
        }

        protected Candle GetMaximumBreakout(string rateName, Candle from, decimal value)
        {
            IList<Candle> candles = _candles[rateName].Where(item => item.OpenTime > from.OpenTime).OrderBy(item=>item.OpenTime).ToList();
            foreach (Candle candle in candles)
            {
                if (candle.HighPrice > value)
                    return candle;
            }
            return null;
        }

        protected Candle GetMinimumBreakout(string rateName, Candle from, decimal value)
        {
            IList<Candle> candles = _candles[rateName].Where(item => item.OpenTime > from.OpenTime).OrderBy(item => item.OpenTime).ToList();
            foreach (Candle candle in candles)
            {
                if (candle.LowPrice < value)
                    return candle;
            }
            return null;
        }

        public virtual IStrategyDecision GetStrategyDecision(RateRecord rec)
        {
            StrategyHelper.CloseCandle(_candles,rec,CANDLES_INTERVAL_MINUTES);
            
            BaseStrategyDecision decision = new BaseStrategyDecision();
            Candle maximum = GetMaximumForInterval(rec.Name,CANDLES_RANGE,_candles[rec.Name].Last());
            Candle minimum = GetMinimumForInterval(rec.Name, CANDLES_RANGE, _candles[rec.Name].Last());
            
            if (maximum == null || minimum == null)
                return decision;

            if (maximum.OpenTime > minimum.OpenTime) //тренд вверх
            {
                Candle previousMaximum=GetMaximumBetween(rec.Name, minimum, maximum);
                if (previousMaximum != null)
                {
                    Candle previousMinimum = GetMinimumBetween(rec.Name, previousMaximum, maximum);
                    if (previousMinimum != null)
                    {
                        //пробойный бар
                        Candle breakout = GetMaximumBreakout(rec.Name, previousMinimum, previousMaximum.HighPrice);
                        if (breakout != null)
                        {
                            Candle previousMaximum2 = GetMaximumBetween(rec.Name,minimum, previousMaximum);
                            if (previousMaximum2 != null)
                            {
                                //наш takeProfit
                                Candle previousMinimum2 = GetMinimumBetween(rec.Name, previousMaximum2, previousMaximum);
                                if (previousMinimum2 != null && rec.Value < breakout.LowPrice)
                                {
                                    if (rec.Value - previousMinimum2.LowPrice > 0.0010m
                                        &&
                                        breakout.HighPrice-rec.Value>0.0005m
                                        )
                                    {
                                        decision.TakeProfit = previousMinimum2.LowPrice;
                                        decision.StopLoss = breakout.HighPrice;
                                        StringBuilder sb = new StringBuilder();
                                        sb.AppendFormat("Takeprofit:{0}{1}", decision.TakeProfit,Environment.NewLine);
                                        sb.AppendFormat("Stoploss:{0}{1}", decision.StopLoss, Environment.NewLine);
                                        sb.AppendFormat("Предпоследний колебательный минимум был:{0:yyy-MM-dd HH:mm}{1}", previousMinimum2.OpenTime, Environment.NewLine);
                                        sb.AppendFormat("Пробойный бар:{0:yyy-MM-dd HH:mm}{1}", breakout.OpenTime, Environment.NewLine);
                                        decision.AdditionalInfo = sb.ToString();
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (minimum.OpenTime > maximum.OpenTime) // тренд вниз
            {
                Candle previousMinimum = GetMinimumBetween(rec.Name,maximum, minimum);

                if (previousMinimum != null)
                {
                    Candle previousMaximum = GetMaximumBetween(rec.Name, previousMinimum, minimum);
                    if (previousMaximum != null)
                    {
                        //пробойный бар
                        Candle breakout = GetMinimumBreakout(rec.Name, previousMaximum, previousMinimum.LowPrice);
                        if (breakout != null)
                        {
                            Candle previousMinimum2 = GetMinimumBetween(rec.Name, maximum, previousMinimum);
                            if (previousMinimum2 != null)
                            {
                                //наш takeProfit
                                Candle previousMaximum2 = GetMaximumBetween(rec.Name, previousMinimum2, previousMinimum);
                                if (previousMaximum2 != null && rec.Value > breakout.HighPrice)
                                {
                                    if (previousMaximum2.HighPrice - rec.Value > 0.0010m
                                        &&
                                         rec.Value - breakout.LowPrice > 0.0005m
                                        )
                                    {
                                        decision.TakeProfit = previousMaximum2.HighPrice;
                                        decision.StopLoss = breakout.LowPrice;
                                        StringBuilder sb = new StringBuilder();
                                        sb.AppendFormat("Takeprofit:{0}{1}", decision.TakeProfit,Environment.NewLine);
                                        sb.AppendFormat("Stoploss:{0}{1}", decision.StopLoss, Environment.NewLine);
                                        sb.AppendFormat("Предпоследний колебательный максимум был:{0:yyy-MM-dd HH:mm}{1}", previousMaximum2.OpenTime, Environment.NewLine);
                                        sb.AppendFormat("Пробойный бар:{0:yyy-MM-dd HH:mm}{1}", breakout.OpenTime, Environment.NewLine);
                                        decision.AdditionalInfo = sb.ToString();
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return decision;
        }

        #endregion
    }
}
