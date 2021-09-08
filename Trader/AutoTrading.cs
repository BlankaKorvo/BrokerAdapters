using DataCollector;
using MarketDataModules;
using MarketDataModules.Models.Portfolio;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trader
{
    public class AutoTrading
    {
        //InstrumentList instrumentList { get; set; }

        MarketDataCollector marketDataCollector = new MarketDataCollector();
        public async Task AutoTradingInstruments(InstrumentList instrumentList, int countStocks)
        {
            while (true)
            {
                foreach (var item in instrumentList.Instruments)
                {
                    Portfolio portfolio = await marketDataCollector.GetPortfolioAsync();

                    TradeTarget tradeTarget = await PurchaseDecision(item);

                }            
            }
        }
        private async Task<TradeTarget> PurchaseDecision(Instrument instrument)
        {
            return TradeTarget.notTrading;
            //Log.Information("Start TradeOperation: " + figi);
            //var orderbook = await marketDataCollector.GetOrderbookAsync(figi, Provider.Tinkoff, 20);

            //if (orderbook == null)
            //{
            //    Log.Information("Orderbook null");
            //    return;
            //}

            //var candleList = await marketDataCollector.GetCandlesAsync(figi, candleInterval, candlesCount);

            //var bestAsk = orderbook.Asks.FirstOrDefault().Price;
            //var bestBid = orderbook.Bids.FirstOrDefault().Price;

            //GmmaDecision gmmaDecision = new GmmaDecision() { candleList = candleList, orderbook = orderbook, bestAsk = bestAsk, bestBid = bestBid };

            ////var gmmaSignalResult = signal.GmmaSignal(candleList, bestAsk , bestBid);

            //if (gmmaDecision.TradeVariant() == TradeOperation.toLong
            //    &&
            //    (lastOperation == TradeOperation.fromLong || lastOperation == TradeOperation.fromShort)
            //    )
            //{
            //    lastOperation = TradeOperation.toLong;
            //    using (StreamWriter sw = new StreamWriter("_operation", true, System.Text.Encoding.Default))
            //    {
            //        sw.WriteLine(DateTime.Now + @" Long " + item + "price " + bestAsk);
            //        sw.WriteLine();
            //    }
            //    Log.Information("Stop trade: " + item + " TradeOperation.toLong");
            //}

            //if (gmmaDecision.TradeVariant() == TradeOperation.fromLong
            //    &&
            //    (lastOperation == TradeOperation.toLong)
            //    )
            //{
            //    lastOperation = TradeOperation.fromLong;
            //    using (StreamWriter sw = new StreamWriter("_operation", true, System.Text.Encoding.Default))
            //    {
            //        sw.WriteLine(DateTime.Now + @" FromLong " + item + "price " + bestBid);
            //        sw.WriteLine();
            //    }
            //    Log.Information("Stop trade: " + item + " TradeOperation.fromLong");
            //}

            //if (gmmaDecision.TradeVariant() == TradeOperation.toShort
            //    &&
            //    (lastOperation == TradeOperation.fromLong || lastOperation == TradeOperation.fromShort)
            //    )
            //{
            //    lastOperation = TradeOperation.toShort;
            //    using (StreamWriter sw = new StreamWriter("_operation", true, System.Text.Encoding.Default))
            //    {
            //        sw.WriteLine(DateTime.Now + @" ToShort " + item + "price " + bestBid);
            //        sw.WriteLine();
            //    }
            //    Log.Information("Stop trade: " + item + " TradeOperation.toShort");
            //}

            //if (gmmaDecision.TradeVariant() == TradeOperation.fromShort
            //    &&
            //    (lastOperation == TradeOperation.toShort)
            //    )
            //{
            //    lastOperation = TradeOperation.fromShort;
            //    using (StreamWriter sw = new StreamWriter("_operation", true, System.Text.Encoding.Default))
            //    {
            //        sw.WriteLine(DateTime.Now + @" FromShort " + item + "price " + bestAsk);
            //        sw.WriteLine();
            //    }
            //    Log.Information("Stop trade: " + item + " TradeOperation.fromShort");
            //}
        }
    }
}
