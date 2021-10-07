using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TinkoffAdapter.Authority;
using Tinkoff.Trading.OpenApi.Models;
using Tinkoff.Trading.OpenApi.Network;
using TinkoffAdapter.DataHelper;
using Operation = MarketDataModules.Models.TradeTarget;
using Orderbook = MarketDataModules.Models.Orderbooks;
using CandleInterval = MarketDataModules.Models.Candles.CandleInterval;
using MarketDataModules;
using MarketDataModules.Models;

namespace TinkoffAdapter.TinkoffTrade
{
    public class TinkoffTrading //: TransactionModel
    {
        public TransactionModel transactionModel { get; set; }

        //public async Task BuyStoksByMarginAsync(TransactionModel transactionModel)
        //{
        //    Log.Information("Start BuyStoks: " + transactionModel.Figi);
        //    List<Order> orders = await RetryPolicy.Model.RetryToManyReq().ExecuteAsync(async () => await Auth.Context.OrdersAsync());
        //    foreach (Order order in orders)
        //    {
        //        if (order.Figi == transactionModel.Figi)
        //        {
        //            await RetryPolicy.Model.RetryToManyReq().ExecuteAsync(async () => await Auth.Context.CancelOrderAsync(order.OrderId));
        //            Log.Information("Delete order by figi: " + transactionModel.Figi + " RequestedLots " + order.RequestedLots + " ExecutedLots " + order.ExecutedLots + " Price " + order.Price + " Operation " + order.Operation + " Status " + order.Status + " Type " + order.Type);
        //        }
        //    }
        //    int lots = await CalculationLotsByMarginAsync(transactionModel);
        //    //transactionModel.Quantity = await CalculationLotsByMargin(transactionModel);
        //    if (lots == 0)
        //    {
        //        Log.Information("Not any lot in margin: " + transactionModel.Purchase);
        //        return; }

        //    await RetryPolicy.Model.RetryToManyReq().ExecuteAsync(async () => await Auth.Context.PlaceLimitOrderAsync(new LimitOrder(transactionModel.Figi, lots, OperationType.Buy, transactionModel.Price)));
        //    //Instrument instrument = await marketDataCollector.GetInstrumentByFigi(transactionModel.Figi, Provider.Tinkoff);
        //    using (StreamWriter sw = new StreamWriter("_operation", true, System.Text.Encoding.Default))
        //    {
        //        sw.WriteLine(DateTime.Now + @" Buy " + transactionModel.Figi + " " /*+ instrument.Ticker */+ " Quantity: " + lots + " price: " + transactionModel.Price + " mzda: " + (transactionModel.Price * 0.02m) / 100m);
        //        sw.WriteLine();
        //    }
        //    Log.Information("Create order for Buy " + lots + " lots " + "figi: " + transactionModel.Figi + "price: " + transactionModel.Price);
        //    Log.Information("Stop BuyStoks: " + transactionModel.Figi);
        //}

        public async Task TransactStoksAsyncs()
        {
            Log.Information("Start TransactStoksAsyncs: " + transactionModel.Figi);
            if (
                transactionModel.Quantity == 0
                ||
                transactionModel.Figi == null
                ||
                transactionModel.Price == 0
                )
            {
                Log.Information("The operation is not possible. Not enough data: " + "transactionModel.Quantity = " + transactionModel.Quantity + " transactionModel.Figi =" + transactionModel.Figi + " transactionModel.Price = " + transactionModel.Price);
                Log.Information("Stop TransactStoksAsyncs: " + transactionModel.Figi);
                return;
            }

            List<Order> orders = await RetryPolicy.PollyRetray.RetryToManyReq().ExecuteAsync(async () => await Auth.Context.OrdersAsync());
            foreach (Order order in orders)
            {
                if (order.Figi == transactionModel.Figi)
                {
                    await RetryPolicy.PollyRetray.RetryToManyReq().ExecuteAsync(async () => await Auth.Context.CancelOrderAsync(order.OrderId));
                    Log.Information("Delete order by figi: " + transactionModel.Figi + " RequestedLots " + order.RequestedLots + " ExecutedLots " + order.ExecutedLots + " Price " + order.Price + " Operation " + order.Operation + " Status " + order.Status + " Type " + order.Type);
                }
            }

            using (StreamWriter sw = new StreamWriter("_operation", true, System.Text.Encoding.Default))
            {
                sw.WriteLine(DateTime.Now + " " + GetOperationType().ToString() + " " + transactionModel.Figi + " " /*+ instrument.Ticker */+ " Quantity: " + transactionModel.Quantity + " price: " + transactionModel.Price + " mzda: " + (transactionModel.Price * 0.025m) / 100m);
                sw.WriteLine();
            }

            await RetryPolicy.PollyRetray.RetryToManyReq().ExecuteAsync(async () => await Auth.Context.PlaceLimitOrderAsync(new LimitOrder(transactionModel.Figi, transactionModel.Quantity, GetOperationType(), transactionModel.Price)));

            Log.Information("Create order for Buy " + transactionModel.Quantity + " lots " + "figi: " + transactionModel.Figi + "price: " + transactionModel.Price);
            Log.Information("Stop TransactStoksAsyncs: " + transactionModel.Figi);
        }

        private Tinkoff.Trading.OpenApi.Models.OperationType GetOperationType()
        {
            if (transactionModel.TradeOperation == MarketDataModules.Models.Trading.TradingOperationType.Buy)
                return Tinkoff.Trading.OpenApi.Models.OperationType.Buy;
            else if (transactionModel.TradeOperation == MarketDataModules.Models.Trading.TradingOperationType.Sell)
                return Tinkoff.Trading.OpenApi.Models.OperationType.Sell;
            else
                throw new NotImplementedException();
        }
        public async Task BuyStoksAsyncs()
        {
            Log.Information("Start BuyStoksAsyncs: " + transactionModel.Figi);
            if (
                transactionModel.Quantity == 0
                ||
                transactionModel.Figi == null
                ||
                transactionModel.Price == 0
                )
            {
                Log.Information("The operation is not possible. Not enough data: " + "transactionModel.Quantity = " + transactionModel.Quantity + " transactionModel.Figi =" + transactionModel.Figi + " transactionModel.Price = " + transactionModel.Price);
                Log.Information("Stop BuyStoksAsyncs: " + transactionModel.Figi);
                return;
            }

            List<Order> orders = await RetryPolicy.PollyRetray.RetryToManyReq().ExecuteAsync(async () => await Auth.Context.OrdersAsync());
            foreach (Order order in orders)
            {
                if (order.Figi == transactionModel.Figi)
                {
                    await RetryPolicy.PollyRetray.RetryToManyReq().ExecuteAsync(async () => await Auth.Context.CancelOrderAsync(order.OrderId));
                    Log.Information("Delete order by figi: " + transactionModel.Figi + " RequestedLots " + order.RequestedLots + " ExecutedLots " + order.ExecutedLots + " Price " + order.Price + " Operation " + order.Operation + " Status " + order.Status + " Type " + order.Type);
                }
            }

            await RetryPolicy.PollyRetray.RetryToManyReq().ExecuteAsync(async () => await Auth.Context.PlaceLimitOrderAsync(new LimitOrder(transactionModel.Figi, transactionModel.Quantity, Tinkoff.Trading.OpenApi.Models.OperationType.Buy, transactionModel.Price)));

            using (StreamWriter sw = new StreamWriter("_operation", true, System.Text.Encoding.Default))
            {
                sw.WriteLine(DateTime.Now + @" Buy " + transactionModel.Figi + " " /*+ instrument.Ticker */+ " Quantity: " + transactionModel.Quantity + " price: " + transactionModel.Price + " mzda: " + (transactionModel.Price * 0.025m) / 100m);
                sw.WriteLine();
            }
            Log.Information("Create order for Buy " + transactionModel.Quantity + " lots " + "figi: " + transactionModel.Figi + "price: " + transactionModel.Price);
            Log.Information("Stop BuyStoksAsyncs: " + transactionModel.Figi);
        }

        public async Task SellStoksAsyncs()
        {
            Log.Information("Start SellStoksAsyncs: " + transactionModel.Figi);
            if (
                transactionModel.Quantity == 0
                ||
                transactionModel.Figi == null
                ||
                transactionModel.Price == 0
                )
            {
                Log.Information("The operation is not possible. Not enough data: " + "transactionModel.Quantity = " + transactionModel.Quantity + " transactionModel.Figi =" + transactionModel.Figi + " transactionModel.Price = " + transactionModel.Price);
                Log.Information("Stop SellStoksAsyncs: " + transactionModel.Figi);
                return;
            }

            List<Order> orders = await RetryPolicy.PollyRetray.RetryToManyReq().ExecuteAsync(async () => await Auth.Context.OrdersAsync());
            foreach (Order order in orders)
            {
                if (order.Figi == transactionModel.Figi)
                {
                    await RetryPolicy.PollyRetray.RetryToManyReq().ExecuteAsync(async () => await Auth.Context.CancelOrderAsync(order.OrderId));
                    Log.Information("Delete order by figi: " + transactionModel.Figi + " RequestedLots " + order.RequestedLots + " ExecutedLots " + order.ExecutedLots + " Price " + order.Price + " Operation " + order.Operation + " Status " + order.Status + " Type " + order.Type);
                }
            }

            await RetryPolicy.PollyRetray.RetryToManyReq().ExecuteAsync(async () => await Auth.Context.PlaceLimitOrderAsync(new LimitOrder(transactionModel.Figi, transactionModel.Quantity, Tinkoff.Trading.OpenApi.Models.OperationType.Sell, transactionModel.Price)));

            using (StreamWriter sw = new StreamWriter("_operation", true, System.Text.Encoding.Default))
            {
                sw.WriteLine(DateTime.Now + @" Sell " + transactionModel.Figi + " " /*+ instrument.Ticker */+ " Quantity: " + transactionModel.Quantity + " price: " + transactionModel.Price + " mzda: " + (transactionModel.Price * 0.025m) / 100m);
                sw.WriteLine();
            }
            Log.Information("Create order for Sell " + transactionModel.Quantity + " lots " + "figi: " + transactionModel.Figi + "price: " + transactionModel.Price);
            Log.Information("Stop SellStoksAsyncs: " + transactionModel.Figi);
        }

        //public async Task SellStoksFromLongAsync(TransactionModel transactionModel)
        //{
        //    Log.Information("Start SellStoksFromLong: " + transactionModel.Figi);
        //    int lots = await CalculationStocksFromLongAsync(transactionModel);
        //    if (lots == 0)
        //    { return; }
        //    await RetryPolicy.Model.RetryToManyReq().ExecuteAsync(async () => await Auth.Context.PlaceLimitOrderAsync(new LimitOrder(transactionModel.Figi, lots, OperationType.Sell, transactionModel.Price)));
        //    using (StreamWriter sw = new StreamWriter("_operation", true, System.Text.Encoding.Default))
        //    {
        //        sw.WriteLine(DateTime.Now + @" Sell " + transactionModel.Figi + " Quantity: " + lots + " price: " + transactionModel.Price + " mzda: " + (transactionModel.Price * 0.02m) / 100m);
        //        sw.WriteLine();
        //    }
        //    Log.Information("Create order for Sell " + lots + " stocks " + "figi: " + transactionModel.Figi + "price: " + transactionModel.Price);
        //    Log.Information("Stop SellStoksFromLong: " + transactionModel.Figi);
        //}        
        //private async Task<int> CalculationStocksBuyCount(string figi, int countLotsToBuy)
        //{
        //    int lots = await CountLotsInPortfolioAsync(figi);
        //    Log.Information("Need to buy stocks: " + countLotsToBuy);

        //    int countLotsToBuyReal = countLotsToBuy - lots;
        //    Log.Information("Real need to buy: " + countLotsToBuyReal);

        //    return countLotsToBuyReal;
        //}
        //private async Task<int> CalculationLotsByMarginAsync(TransactionModel transactionModel)
        //{
        //    Log.Information("Start CalculationLotsByMargin method. Figi: " + transactionModel.Figi);
        //    int lots = await CountLotsInPortfolioAsync(transactionModel.Figi);
        //    Log.Information("Lots " + transactionModel.Figi + " in portfolio: " + lots);
        //    decimal sumCostLotsInPorfolio = transactionModel.Price * Convert.ToDecimal(lots);
        //    decimal remainingCostLots = transactionModel.Purchase - sumCostLotsInPorfolio;
        //    if (remainingCostLots <= 0)
        //    {
        //        Log.Information("Stop CalculationLotsByMargin method. Figi: " + transactionModel.Figi + " Return: 0");
        //        return 0;
        //    }
        //    int countLotsToBuy = Convert.ToInt32(Math.Floor(remainingCostLots / transactionModel.Price));
        //    if (countLotsToBuy <= transactionModel.Quantity)
        //    {
        //        Log.Information("Need to buy: " + countLotsToBuy);
        //        Log.Information("Stop CalculationLotsByMargin method. Figi: " + transactionModel.Figi );
        //        return countLotsToBuy;
        //    }
        //    else
        //    {
        //        Log.Information("Need to buy: " + transactionModel.Quantity);
        //        Log.Information("Stop CalculationLotsByMargin method. Figi: " + transactionModel.Figi );
        //        return transactionModel.Quantity;
        //    }
        //}

        //private async Task<int> CalculationStocksFromLongAsync(TransactionModel transactionModel)
        //{
        //    Log.Information("Start CalculationStocksFromLong method. Figi: " + transactionModel.Figi);
        //    int lots = await CountLotsInPortfolioAsync(transactionModel.Figi);
        //    Log.Information("Lots " + transactionModel.Figi + " in portfolio: " + lots);
        //    if (lots <= transactionModel.Quantity)
        //    {
        //        Log.Information("Need to sell: " + lots);
        //        Log.Information("Stop CalculationStocksFromLong method. Figi: " + transactionModel.Figi);
        //        return lots;
        //    }
        //    else
        //    {
        //        Log.Information("Need to buy: " + transactionModel.Quantity);
        //        Log.Information("Stop CalculationStocksFromLong method. Figi: " + transactionModel.Figi);
        //        return transactionModel.Quantity;
        //    }
        //}
        //private async Task<int> CountLotsInPortfolioAsync(string figi)
        //{
        //    Log.Information("Start CountLotsInPortfolio method. Figi: " + figi);
        //    var portfolio = await RetryPolicy.Model.RetryToManyReq().ExecuteAsync(async () => await Auth.Context.PortfolioAsync());
        //    int lots = 0;
        //    foreach (var item in portfolio.Positions)
        //    {
        //        if (item.Figi == figi)
        //        {
        //            lots = item.Lots;
        //            Log.Information("Lots " + figi + " in portfolio: " + lots);
        //            Log.Information("Stop CountLotsInPortfolio method. Figi: " + figi);
        //            break;
        //        }
        //    };
        //    Log.Information("Stop CountLotsInPortfolio method. Figi: " + figi);
        //    return lots;
        //}
    }
}
