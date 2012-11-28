using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyBroker.Domain;
using MyBroker.Domain.Interfaces;

namespace MyBroker.Strategy
{
    public class Sperandeo2:Sperandeo,IStrategyProvider
    {

       
        #region IStrategyProvider Members

        public override string GetName()
        {
            return "2 вершины (Виктор Сперандео)2";
        }

       

        /*public override IStrategyDecision GetStrategyDecision(RateRecord rec)
        {
            StrategyHelper.CloseCandle(_candles,rec,CANDLES_INTERVAL_MINUTES);
            BaseStrategyDecision decision = new BaseStrategyDecision();

            Candle maximum = GetMaximumForInterval(rec.Name,CANDLES_RANGE,_candles[rec.Name].Last());
            Candle minimum = GetMinimumForInterval(rec.Name, CANDLES_RANGE, _candles[rec.Name].Last());
            
            decimal currentMax = GetCurrentMax(rec.Name);
            decimal currentMin = GetCurrentMin(rec.Name);

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
                            Candle takeProfit = minimum;
                            if (previousMaximum2 != null)
                            {
                                Candle previousMinimum2=GetMinimumBetween(rec.Name, previousMaximum2, previousMaximum);
                                if (previousMinimum2 != null)
                                    takeProfit = previousMinimum2;
                            }

                            if (currentMin < breakout.LowPrice)
                            {
                                if (rec.Value - takeProfit.LowPrice > 0.0010m
                                    &&
                                    breakout.HighPrice-rec.Value>0.0005m
                                    )
                                {
                                    decision.TakeProfit = takeProfit.LowPrice;
                                    decision.StopLoss = breakout.HighPrice;
                                    StringBuilder sb = new StringBuilder();
                                    sb.AppendFormat("Takeprofit:{0}{1}", decision.TakeProfit,Environment.NewLine);
                                    sb.AppendFormat("Stoploss:{0}{1}", decision.StopLoss, Environment.NewLine);
                                    sb.AppendFormat("Предпоследний колебательный минимум был:{0:yyy-MM-dd HH:mm}{1}", takeProfit.OpenTime, Environment.NewLine);
                                    decision.AdditionalInfo = sb.ToString();
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
                            
                            Candle takeProfit=maximum;
                            if (previousMinimum2 != null)
                            {
                                Candle previousMaximum2 = GetMaximumBetween(rec.Name, previousMinimum2, previousMinimum);
                                if (previousMaximum2 != null)
                                    takeProfit = previousMaximum2;
                            }

                            if (currentMax > breakout.HighPrice)
                            {
                                if (takeProfit.HighPrice - rec.Value > 0.0010m
                                    &&
                                        rec.Value - breakout.LowPrice > 0.0005m
                                    )
                                {
                                    decision.TakeProfit = takeProfit.HighPrice;
                                    decision.StopLoss = breakout.LowPrice;
                                    StringBuilder sb = new StringBuilder();
                                    sb.AppendFormat("Takeprofit:{0}{1}", decision.TakeProfit,Environment.NewLine);
                                    sb.AppendFormat("Stoploss:{0}{1}", decision.StopLoss, Environment.NewLine);
                                    sb.AppendFormat("Предпоследний колебательный максимум был:{0:yyy-MM-dd HH:mm}{1}", takeProfit.OpenTime, Environment.NewLine);
                                    decision.AdditionalInfo = sb.ToString();
                                }
                            }
                            
                        }
                    }
                }
            }

            return decision;
        }*/

        #endregion
    }
}
