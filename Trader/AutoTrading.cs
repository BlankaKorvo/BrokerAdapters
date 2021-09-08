using Analysis.TradeDecision;
using DataCollector;
using MarketDataModules;
using MarketDataModules.Models.Portfolio;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinkoffAdapter.TinkoffTrade;
using TradingAlgorithms.IndicatorSignals;

namespace Trader
{
    public class AutoTrading
    {
        public CandleInterval CandleInterval { get; set; }
        public int CandlesCount { get; set; }

        TinkoffTrading tinkoffTrading = new TinkoffTrading(); 
        MarketDataCollector marketDataCollector = new MarketDataCollector();
        public async Task AutoTradingInstruments(InstrumentList instrumentList, int countStocks)
        {
            while (true)
            {

                Portfolio portfolio = await marketDataCollector.GetPortfolioAsync();
                List<Portfolio.Position> positions = portfolio.Positions;

                foreach (var item in instrumentList.Instruments)
                {

                    var orderbook = await marketDataCollector.GetOrderbookAsync(item.Figi, Provider.Tinkoff, 20);

                    if (orderbook == null)
                    {
                        Log.Information("Orderbook null");
                        Log.Information("Stop AutoTradingInstruments: " + item.Figi);
                    }
                    else
                    {
                        var candleList = await marketDataCollector.GetCandlesAsync(item.Figi, CandleInterval, CandlesCount);

                        var bestAsk = orderbook.Asks.FirstOrDefault().Price;
                        var bestBid = orderbook.Bids.FirstOrDefault().Price;
                        int currentLots = CountStoksInPortfolioByFigi(portfolio, item.Figi);

                        TradeTarget tradeTarget = (new GmmaDecision() { candleList = candleList, orderbook = orderbook, bestAsk = bestAsk, bestBid = bestBid }).TradeVariant();

                        if
                            (
                            tradeTarget == TradeTarget.toLong
                            &&
                            countStocks > currentLots
                            )
                        {
                            await new TinkoffTrading() 
                            {transactionModel =  
                                new TransactionModel() 
                                { Figi = item.Figi, Price = bestAsk, TradeOperation = TradeOperation.Buy, Quantity = countStocks - currentLots }
                            }.TransactStoksAsyncs();
                          
                        }
                        else if
                            (
                            tradeTarget == TradeTarget.fromLong
                            &&
                            currentLots > 0
                            )
                        {
                            await new TinkoffTrading()
                            {
                                transactionModel =
                            new TransactionModel() { Figi = item.Figi, Price = bestBid, TradeOperation = TradeOperation.Sell, Quantity = currentLots }
                            }.TransactStoksAsyncs();
                        }
                        else if
                            (
                            tradeTarget == TradeTarget.toShort
                            &&
                            0 - countStocks < currentLots
                            )
                        {
                            await new TinkoffTrading()
                            {
                                transactionModel =
                            new TransactionModel() { Figi = item.Figi, Price = bestBid, TradeOperation = TradeOperation.Sell, Quantity = currentLots + countStocks }
                            }.TransactStoksAsyncs();
                        }
                        else if
                            (
                            tradeTarget == TradeTarget.fromShort
                            &&
                            currentLots < 0
                            )
                        {
                            await new TinkoffTrading()
                            {
                                transactionModel =
                            new TransactionModel() { Figi = item.Figi, Price = bestAsk, TradeOperation = TradeOperation.Buy, Quantity = 0 - currentLots }
                            }.TransactStoksAsyncs();
                        }

                    }
                }
            }
        }

        private int CountStoksInPortfolioByFigi (Portfolio portfolio, string figi)
        {
            var lots = from position in portfolio.Positions
                       where position.Figi == figi
                       select position.Lots;

            return lots.Sum();
        }
       
    }
    
}
