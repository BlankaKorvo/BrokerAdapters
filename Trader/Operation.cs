using DataCollector;
using MarketDataModules;
using MarketDataModules.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using Tinkoff.Trading.OpenApi.Models;
using Tinkoff.Trading.OpenApi.Network;
using TinkoffAdapter;
using TinkoffAdapter.Authority;
using TinkoffAdapter.DataHelper;
using TinkoffAdapter.TinkoffTrade;
using CandleInterval = MarketDataModules.Models.Candles.CandleInterval;
namespace Analysis.Screeners.CandlesScreener
{
    public class Operation : TransactionModel //: GetStocksHistory
    {
        GetTinkoffData getTinkoffData = new GetTinkoffData();
        MarketDataCollector marketDataCollector = new MarketDataCollector();
        TinkoffTrading TinkoffTrading = new TinkoffTrading();

        //async public Task TransactionAsync(TransactionModel transactionModel)
        //{
            //Log.Information("Start Transaction method. Figi: " + transactionModel.Figi);
            //if (
            //    transactionModel == null
            //    ||
            //    transactionModel.Figi == null
            //    ||
            //    //transactionModel.Purchase == 0
            //    //||
            //    transactionModel.Price == 0
            //    ||
            //    transactionModel.Quantity == 0)
            //{
            //    Log.Information("Figi: " + transactionModel.Figi);
            //    //Log.Information("Margin: " + transactionModel.Purchase);
            //    Log.Information("Price: " + transactionModel.Price);
            //    Log.Information("Quantity: " + transactionModel.Quantity);
            //    //Log.Information("Operation: " + transactionModel.TradeTarget.ToString());
            //    Log.Warning("Transaction is not correct for implementation");
            //    Log.Information("Stop Transaction method. Figi: " + transactionModel.Figi);
            //    return;
            //}

            //    switch (transactionModel.Operation)
            //    {
            //        case Operation.toLong:
            //            Log.Information("Start Buy Stoks to Long. Figi: " + transactionModel.Figi);
            //            await TinkoffTrading.BuyStoksAsync(transactionModel);
            //            Log.Information("Stop Transaction method. Figi: " + transactionModel.Figi);
            //            return;

            //        case Operation.fromLong:
            //            Log.Information("Start Sell Stoks from Long. Figi: " + transactionModel.Figi);
            //            await SellStoksFromLongAsync(transactionModel);
            //            Log.Information("Stop Transaction method. Figi: " + transactionModel.Figi);
            //            return;

            //        case Operation.toShort:
            //            //not implemented
            //            Log.Warning("Sell to short is not implemented");
            //            return;

            //        case Operation.fromShort:
            //            Log.Warning("By from short is not implemented");
            //            return;
            //    }
        }

        //public async Task<TransactionModel> PurchaseDecisionAsync(CandlesList candleList, Orderbook orderbook)
        //{
        //    Log.Information("Start PurchaseDecision method. Figi: " + this.Figi);
        //    TransactionModel transactionModel = new TransactionModel();
        //    transactionModel.Figi = this.Figi;
        //    transactionModel.Purchase = this.Purchase;
        //    ////Получаем свечи
        //    //CandlesList candleList = await marketDataCollector.GetCandlesAsync(transactionModel.Figi, candleInterval, CandlesCount);

        //    ////Получаем стакан
        //    //Orderbook orderbook = await marketDataCollector.GetOrderbookAsync(transactionModel.Figi, Provider.Tinkoff);
        //    if (orderbook == null)
        //    {
        //        Log.Information("Orderbook " + transactionModel.Figi + " is null");
        //        transactionModel.Operation = Operation.notTrading;
        //        return transactionModel;
        //    }
        //    decimal ask = orderbook.Asks.FirstOrDefault().Price;
        //    decimal bid = orderbook.Bids.FirstOrDefault().Price;
        //    int quantityAsksFirst = orderbook.Asks.FirstOrDefault().Quantity;
        //    int quantityBidsFirst = orderbook.Bids.FirstOrDefault().Quantity;
        //    decimal deltaPrice = (ask + bid) / 2;

        //    Mishmash mishmash = new Mishmash() { candleList = candleList, deltaPrice = deltaPrice, orderbook = orderbook };


        //    if (mishmash.Long() == true)
        //    {
        //        Log.Information("Go to Long: " + transactionModel.Figi);
        //        transactionModel.Quantity = quantityAsksFirst;
        //        transactionModel.Operation = Operation.toLong;
        //        transactionModel.Price = ask;
        //    }
        //    else if (mishmash.FromLong() == true)
        //    {
        //        Log.Information("Go from Long: " + transactionModel.Figi);
        //        transactionModel.Quantity = quantityBidsFirst;
        //        transactionModel.Operation = Operation.fromLong;
        //        transactionModel.Price = bid;
        //    }
        //    Log.Information("Stop PurchaseDecision for: " + transactionModel.Figi);
        //    return transactionModel;
        //}

        //public async Task<TransactionModelBase> PurchaseDecisionAsync(string figi)
        //{
        //    Log.Information("Start PurchaseDecision method. Figi: " + this.Figi);
        //    TransactionModelBase transactionModelBase = new TransactionModelBase();
        //    transactionModelBase.Figi = figi;
        //    //Получаем свечи
        //    CandlesList candleList = await marketDataCollector.GetCandlesAsync(figi, candleInterval, CandlesCount);

        //    //Получаем стакан
        //    Orderbook orderbook = await marketDataCollector.GetOrderbookAsync(figi, Provider.Tinkoff);
        //    if (orderbook == null)
        //    {
        //        Log.Information("Orderbook " + figi + " is null");
        //        transactionModelBase.Operation = Operation.notTrading;
        //        return transactionModelBase;
        //    }
        //    decimal ask = orderbook.Asks.FirstOrDefault().Price;
        //    decimal bid = orderbook.Bids.FirstOrDefault().Price;
        //    int quantityAsksFirst = orderbook.Asks.FirstOrDefault().Quantity;
        //    int quantityBidsFirst = orderbook.Bids.FirstOrDefault().Quantity;
        //    decimal deltaPrice = (ask + bid) / 2;

        //    Mishmash mishmash = new Mishmash() { candleList = candleList, deltaPrice = deltaPrice, orderbook = orderbook };

        //    StochFiveMinutes stochFiveMinutes = new StochFiveMinutes() { candleList = candleList, deltaPrice = deltaPrice };

        //    if (stochFiveMinutes.Long() == true)
        //    {
        //        Log.Information("Go to Long: " + transactionModelBase.Figi);
        //        transactionModelBase.Operation = Operation.toLong;
        //        transactionModelBase.Price = ask;
        //    }
        //    else if (mishmash.FromLong() == true)
        //    {
        //        Log.Information("Go from Long: " + transactionModelBase.Figi);
        //        transactionModelBase.Operation = Operation.fromLong;
        //        transactionModelBase.Price = bid;
        //    }
        //    Log.Information("Stop PurchaseDecision for: " + figi);
        //    return transactionModelBase;
        //}



        //public async Task Screener(CandleInterval candleInterval, int candleCount, decimal margin, int notTradeMinuts)
        //{
        //    Log.Information("Start Trade method:");
        //    Log.Information("candleInterval:" + candleInterval);
        //    Log.Information("candleCount:" + candleInterval);
        //    Log.Information("margin:" + margin);
        //    Log.Information("notTradeMinuts:" + notTradeMinuts);
        //    //List<CandlesList> candleLists = await SortUsdCandlesAsync(candleInterval, candleCount, margin, notTradeMinuts);
        //    List<CandlesList> candleLists = await AllUsdCandlesAsync(candleInterval, candleCount);
        //    Log.Information("Get Sort USD candles");
        //    Log.Information("Start of sorted candleLists");
        //    Log.Information("Count = " + candleLists.Count);
        //    string nameOfFile = "stoks " + DateTime.Now;
        //    using (StreamWriter sw = new StreamWriter(nameOfFile.Replace(":", "_").Replace(".", "_"), true, System.Text.Encoding.Default))
        //    {
        //        sw.WriteLine("Count = " + candleLists.Count);
        //        sw.WriteLine("Margin: " + margin);
        //        sw.WriteLine("NotTradeMinuts: " + notTradeMinuts);
        //        foreach (var item in candleLists)
        //        {
        //            sw.WriteLine(item.Figi);
        //            Log.Information(item.Figi);
        //        }
        //    }
        //    Log.Information("Stop of sorted candleLists");
        //    await CycleTrading(candleInterval, candleCount, margin, candleLists);
        //}

        //private async Task CycleTrading(CandleInterval candleInterval, int candleCount, decimal margin, List<CandlesList> candleLists)
        //{
        //    while (true)
        //    {
        //        Log.Information("Start Screener Stoks");
        //        await Trading(candleInterval, candleCount, margin, candleLists);
        //    }
        //}
        //public async Task CycleTrading(CandleInterval candleInterval, int candleCount, decimal maxMoneyForTrade, List<string> figiList)
        //{
        //    while (true)
        //    {
        //        Log.Information("Start CycleTrading");
        //        await Trading(candleInterval, candleCount, maxMoneyForTrade, figiList);
        //    }
        //}

        //public async Task Trading(CandleInterval candleInterval, int candleCount, decimal budget, List<CandlesList> CandleLists)
        //{
        //    Log.Information("Start ScreenerStocks: ");
        //    Log.Information("Count instruments =  " + CandleLists.Count);
        //    Log.Information("candleCount =  " + candleCount);
        //    Log.Information("budget =  " + budget);
        //    foreach (var item in CandleLists)
        //    {
        //        if (ValidCandles(item, item.Candles.Last().Close) == false)
        //        {
        //            Log.Information(item.Figi + " not valid instrument");
        //            continue;
        //        }
        //        Log.Information("Start ScreenerStocks for: " + item.Figi);

        //        TinkoffTrading tinkoffTrading = new TinkoffTrading() { Figi = item.Figi, CandlesCount = candleCount, candleInterval = candleInterval, Purchase = budget };

        //        Log.Information("Get object TinkoffTrading with FIGI: " + item.Figi);

        //        TransactionModel transactionData = await tinkoffTrading.PurchaseDecisionAsync();

        //        Log.Information("Get TransactionModel: " + transactionData.Figi);

        //        if (transactionData.Operation == MarketDataModules.Operation.notTrading)
        //        { continue; }

        //        Log.Information("TransactionModel margin = " + transactionData.Purchase);
        //        Log.Information("TransactionModel operation = " + transactionData.Operation);
        //        Log.Information("TransactionModel price = " + transactionData.Price);
        //        Log.Information("TransactionModel quantity = " + transactionData.Quantity);

        //        //переписать логику нахрен....

        //        if (transactionData.Operation == MarketDataModules.Operation.toLong)
        //        {
        //            Log.Information("If transactionData.Operation = " + MarketDataModules.Operation.toLong.ToString());
        //            Log.Information("Start first transaction");
        //            await tinkoffTrading.TransactionAsync(transactionData);
        //            int i = 2;
        //            do
        //            {
        //                Log.Information("Start " + i + " transaction");
        //                transactionData = await tinkoffTrading.PurchaseDecisionAsync();
        //                await tinkoffTrading.TransactionAsync(transactionData);
        //                i++;
        //            }
        //            while (await getTinkoffData.PresentInPortfolioAsync(transactionData.Figi));
        //            Log.Information("Stop ScreenerStocks after trading");
        //        }
        //        else
        //        {
        //            Log.Information("Stop ScreenerStocks");
        //            continue;
        //        }
        //    }            
        //}

        //public async Task Trading(CandleInterval candleInterval, int candleCount, decimal maxMoneyForTrade, List<string> figis)
        //{
        //    Log.Information("Start Trading");
        //    foreach (string figi in figis)
        //    {
        //        CandlesList candlesList = await marketDataCollector.GetCandlesAsync(figi, candleInterval, candleCount);
        //        if (
        //            candlesList == null
        //            ||
        //            ValidCandles(candlesList, candlesList.Candles.Last().Close) == false
        //            )
        //        {
        //            Log.Information(figi + " not valid instrument");
        //            continue;
        //        }
        //        Log.Information("Start ScreenerStocks for: " + figi);
        //        TinkoffTrading tinkoffTrading = new TinkoffTrading() { Figi = figi, CandlesCount = candleCount, candleInterval = candleInterval, Purchase = maxMoneyForTrade };
        //        Log.Information("Get object TinkoffTrading with FIGI: " + figi);
        //        TransactionModel transactionData = await tinkoffTrading.PurchaseDecisionAsync();
        //        Log.Information("Get TransactionModel: " + transactionData.Figi);
        //        if (transactionData.Operation == MarketDataModules.Operation.notTrading)
        //        { continue; }
        //        Log.Information("TransactionModel margin = " + transactionData.Purchase);
        //        Log.Information("TransactionModel operation = " + transactionData.Operation);
        //        Log.Information("TransactionModel price = " + transactionData.Price);
        //        Log.Information("TransactionModel quantity = " + transactionData.Quantity);

        //        //переписать логику нахрен....

        //        if (transactionData.Operation == MarketDataModules.Operation.toLong)
        //        {
        //            Log.Information("If transactionData.Operation = " + MarketDataModules.Operation.toLong.ToString());
        //            Log.Information("Start first transaction");
        //            await tinkoffTrading.TransactionAsync(transactionData);
        //            int i = 2;
        //            do
        //            {
        //                Log.Information("Start " + i + " transaction");
        //                transactionData = await tinkoffTrading.PurchaseDecisionAsync();
        //                await tinkoffTrading.TransactionAsync(transactionData);
        //                i++;
        //            }
        //            while (await getTinkoffData.PresentInPortfolioAsync(transactionData.Figi));
        //            Log.Information("Stop ScreenerStocks after trading");
        //        }
        //        else
        //        {
        //            Log.Information("Stop ScreenerStocks");
        //            continue;
        //        }
        //    }
        //}
        //public async Task<List<TransactionModelBase>> GetAllTransactionModels(CandleInterval candleInterval, int candleCount, decimal maxMoneyForTrade, List<Instrument> instrumentList)
        //{
        //    Log.Information("Start GetTransactionModels");
        //    List<TransactionModelBase> TransactionModelBaseList = new List<TransactionModelBase>();
        //    foreach (var item in instrumentList)
        //    {
        //        CandlesList candlesList = await marketDataCollector.GetCandlesAsync(item.Figi, candleInterval, candleCount);
        //        if (
        //            candlesList == null
        //            ||
        //            ValidCandles(candlesList, candlesList.Candles.Last().Close) == false
        //            )
        //        {
        //            Log.Information(item.Figi + " not valid instrument");
        //            continue;
        //        }

        //        TinkoffTrading tinkoffTrading = new TinkoffTrading() { Figi = item.Figi, CandlesCount = candleCount, candleInterval = candleInterval, Purchase = maxMoneyForTrade };

        //        TransactionModelBase transactionModelBase = await tinkoffTrading.PurchaseDecisionAsync(item.Figi);
        //        Log.Information("Get TransactionModelBase: " + transactionModelBase.Figi);
        //        Log.Information("TransactionModel operation = " + transactionModelBase.Operation);
        //        Log.Information("TransactionModel price = " + transactionModelBase.Price);
        //        TransactionModelBaseList.Add(transactionModelBase);
        //    }
        //    Log.Information("Stop GetTransactionModels");
        //    return TransactionModelBaseList;
        //}

        //async Task<List<CandlesList>> SortUsdCandlesAsync(CandleInterval candleInterval, int candleCount, decimal margin, int notTradeMinuts)
        //{
        //    Log.Information("Start SortUsdCandles. Param: ");
        //    Log.Information("candleInterval: " + candleInterval);
        //    Log.Information("candleCount: " + candleCount);
        //    Log.Information("margin: " + margin);
        //    Log.Information("notTradeMinuts: " + notTradeMinuts);
        //    List<CandlesList> allUsdCandleLists = await AllUsdCandlesAsync(candleInterval, candleCount);
        //    Log.Information("Get All USD candlesLists. Count: " + allUsdCandleLists.Count);
        //    List<CandlesList> validCandleLists = AllValidCandles(allUsdCandleLists, margin, notTradeMinuts);
        //    Log.Information("Return Valid candlesLists. Count: " + validCandleLists.Count);
        //    Log.Information("Stop SortUsdCandles");
        //    return validCandleLists;
        //}
    
    }
