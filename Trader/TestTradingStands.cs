//using Analysis.TradeDecision;
//using MarketDataModules.Models;
//using MarketDataModules.Models.Candles;
//using MarketDataModules.Models.Operation;
//using MarketDataModules.Models.Orderbooks;
//using MarketDataModules.Models.Portfolio;
//using Serilog;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Trader
//{
//    public class TestTradingStands
//    {
//        TradeOperation tradeOperation = null;
//        Portfolio.Position portfolioPosition = null;
//        TradeTarget lastTradeTarget = TradeTarget.fromLong;
//        private void TestTrading(Orderbook orderbook, CandlesList candleList, ref TradeOperation tradeOperation, ref Portfolio.Position portfolioPosition, ref TradeTarget lastTradeTarget)
//        {
//            //foreach (var item in instrumentList)
//            //{
//            Log.Information("Start trade: " + candleList.Figi);
//            //var orderbook = marketDataCollector.GetOrderbookAsync(item.Figi, Provider.Tinkoff, 50).GetAwaiter().GetResult();

//            //if (orderbook == null)
//            //{
//            //    Log.Information("Orderbook null");
//            //    continue;
//            //}

//            var bestAsk = orderbook.Asks.FirstOrDefault().Price;
//            var bestBid = orderbook.Bids.FirstOrDefault().Price;

//            //var candleList = marketDataCollector.GetCandlesAsync(item.Figi, candleInterval, candlesCount).GetAwaiter().GetResult();

//            //Portfolio portfolio = marketDataCollector.GetPortfolioAsync().GetAwaiter().GetResult();
//            //List<TradeOperation> tradeOperations = marketDataCollector.GetOperationsAsync(item.figi, DateTime.Now, DateTime.Now.AddDays(-100)).GetAwaiter().GetResult();
//            //List<TradeOperation> tradeOperations = new List<TradeOperation>();
//            //tradeOperations.Add(tradeOperation);

//            //Portfolio.Position position = null;
//            //foreach (Portfolio.Position itemP in portfolio.Positions)
//            //{
//            //    if (itemP.Figi == item.figi)
//            //    {
//            //        position = itemP;
//            //    }
//            //}
//            List<TradeOperation> tradeOperationResult = new List<TradeOperation>();
//            tradeOperationResult.Add(tradeOperation);

//            //MoneyAmount averagePositionPrice = item.MoneyAmountT;
//            //List<TradeOperation> tradeOperationResult = new List<TradeOperation> { tradeOperation };
//            //portfolioPosition = new Portfolio.Position(portfolioPosition.Name, portfolioPosition.Figi, portfolioPosition.Ticker, portfolioPosition.Isin, portfolioPosition.InstrumentType, portfolioPosition.Balance, portfolioPosition.Blocked, portfolioPosition.ExpectedYield, portfolioPosition.Lots, averagePositionPrice, portfolioPosition.AveragePositionPriceNoNkd);
//            //GmmaDecisionOneMinutes gmmaDecision = new GmmaDecisionOneMinutes() { candleList = candleList, orderbook = orderbook, bestAsk = bestAsk, bestBid = bestBid };
//            GmmaDecision gmmaDecisionOneMinutes = new GmmaDecision() { candleListMin = candleList, orderbook = orderbook, bestAsk = bestAsk, bestBid = bestBid, portfolioPosition = portfolioPosition, tradeOperations = tradeOperationResult };
//            TradeTarget tradeVariant = gmmaDecisionOneMinutes.TradeVariant();

//            //var gmmaSignalResult = signal.GmmaSignal(candleList, bestAsk , bestBid);

//            if (tradeVariant == TradeTarget.toLong
//                &&
//                portfolioPosition == null
//                )
//            {
//                int countBalance = 1;
//                portfolioPosition = new Portfolio.Position(default, candleList.Figi, default, default, default, countBalance, default, new MoneyAmount(Currency.Usd, bestAsk), countBalance, new MoneyAmount(Currency.Usd, bestAsk), default);
//                tradeOperation = new TradeOperation(default, default, default, default, default, default, bestAsk, default, default, candleList.Figi, default, default, DateTime.Now.ToUniversalTime(), default);

//                using (StreamWriter sw = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_operation " + candleList.Figi), true, System.Text.Encoding.Default))
//                {
//                    sw.WriteLine(DateTime.Now + @" Long " + candleList.Figi + "price " + bestAsk);
//                    sw.WriteLine();
//                }

//                Log.Information("Stop trade: " + candleList.Figi + " TradeOperation.toLong");
//            }

//            if (tradeVariant == TradeTarget.fromLong
//                &&
//                portfolioPosition?.Balance > 0)

//            {
//                portfolioPosition = null;
//                tradeOperation = new TradeOperation(default, default, default, default, default, default, bestBid, default, default, candleList.Figi, default, default, DateTime.Now.ToUniversalTime(), default);
//                using (StreamWriter sw = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_operation " + candleList.Figi), true, System.Text.Encoding.Default))
//                {
//                    sw.WriteLine(DateTime.Now + @" FromLong " + candleList.Figi + "price " + bestBid);
//                    sw.WriteLine();
//                }
//                Log.Information("Stop trade: " + candleList.Figi + " TradeOperation.fromLong");
//            }

//            if (tradeVariant == TradeTarget.toShort
//                &&
//                portfolioPosition == null
//                )
//            {

//                int countBalance = -1;
//                portfolioPosition = new Portfolio.Position(default, candleList.Figi, default, default, default, countBalance, default, new MoneyAmount(Currency.Usd, bestBid), countBalance, new MoneyAmount(Currency.Usd, bestBid), default);
//                tradeOperation = new TradeOperation(default, default, default, default, default, default, bestBid, default, default, candleList.Figi, default, default, DateTime.Now.ToUniversalTime(), default);
//                using (StreamWriter sw = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_operation " + candleList.Figi), true, System.Text.Encoding.Default))
//                {
//                    sw.WriteLine(DateTime.Now + @" ToShort " + candleList.Figi + "price " + bestBid);
//                    sw.WriteLine();
//                }
//                Log.Information("Stop trade: " + candleList.Figi + " TradeOperation.toShort");
//            }

//            if (tradeVariant == TradeTarget.fromShort
//                &&
//                portfolioPosition?.Balance < 0
//                )
//            {
//                portfolioPosition = null;
//                tradeOperation = new TradeOperation(default, default, default, default, default, default, bestAsk, default, default, candleList.Figi, default, default, DateTime.Now.ToUniversalTime(), default);


//                using (StreamWriter sw = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_operation " + candleList.Figi), true, System.Text.Encoding.Default))
//                {
//                    sw.WriteLine(DateTime.Now + @" FromShort " + candleList.Figi + "price " + bestAsk);
//                    sw.WriteLine();
//                }
//                Log.Information("Stop trade: " + candleList.Figi + " TradeOperation.fromShort");
//            }
            
//        }
    
//    }
//}
