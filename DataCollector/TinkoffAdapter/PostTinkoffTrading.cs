using MarketDataModules.Trading;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Tinkoff.Trading.OpenApi.Models;

namespace DataCollector.TinkoffAdapter
{
    public class PostTinkoffTrading //: TransactionModel
    {
        public TransactionModel TransactionModel { get; set; }

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
            Log.Information("Start TransactStoksAsyncs: " + TransactionModel.Figi);
            if (
                TransactionModel.Quantity == 0
                ||
                TransactionModel.Figi == null
                ||
                TransactionModel.Price == 0
                )
            {
                Log.Information("The operation is not possible. Not enough data: " + "transactionModel.Quantity = " + TransactionModel.Quantity + " transactionModel.Figi =" + TransactionModel.Figi + " transactionModel.Price = " + TransactionModel.Price);
                Log.Information("Stop TransactStoksAsyncs: " + TransactionModel.Figi);
                return;
            }

            List<Order> orders = await RetryPolicy.PollyRetrayPolitics.RetryToManyReq().ExecuteAsync(async () => await Authorisation.Context.OrdersAsync());
            foreach (Order order in orders)
            {
                if (order.Figi == TransactionModel.Figi)
                {
                    await RetryPolicy.PollyRetrayPolitics.RetryToManyReq().ExecuteAsync(async () => await Authorisation.Context.CancelOrderAsync(order.OrderId));
                    Log.Information("Delete order by figi: " + TransactionModel.Figi + " RequestedLots " + order.RequestedLots + " ExecutedLots " + order.ExecutedLots + " Price " + order.Price + " Operation " + order.Operation + " Status " + order.Status + " Type " + order.Type);
                }
            }

            using (StreamWriter sw = new("_operation", true, System.Text.Encoding.Default))
            {
                sw.WriteLine(DateTime.Now + " " + GetOperationType().ToString() + " " + TransactionModel.Figi + " " /*+ instrument.Ticker */+ " Quantity: " + TransactionModel.Quantity + " price: " + TransactionModel.Price + " mzda: " + (TransactionModel.Price * 0.025m) / 100m);
                sw.WriteLine();
            }

            await RetryPolicy.PollyRetrayPolitics.RetryToManyReq().ExecuteAsync(async () => await Authorisation.Context.PlaceLimitOrderAsync(new LimitOrder(TransactionModel.Figi, TransactionModel.Quantity, GetOperationType(), TransactionModel.Price)));

            Log.Information("Create order for Buy " + TransactionModel.Quantity + " lots " + "figi: " + TransactionModel.Figi + "price: " + TransactionModel.Price);
            Log.Information("Stop TransactStoksAsyncs: " + TransactionModel.Figi);
        }

        private Tinkoff.Trading.OpenApi.Models.OperationType GetOperationType()
        {
            if (TransactionModel.TradeOperation == MarketDataModules.Trading.TradingOperationType.Buy)
                return Tinkoff.Trading.OpenApi.Models.OperationType.Buy;
            else if (TransactionModel.TradeOperation == MarketDataModules.Trading.TradingOperationType.Sell)
                return Tinkoff.Trading.OpenApi.Models.OperationType.Sell;
            else
                throw new NotImplementedException();
        }
        public async Task BuyStoksAsyncs()
        {
            Log.Information("Start BuyStoksAsyncs: " + TransactionModel.Figi);
            if (
                TransactionModel.Quantity == 0
                ||
                TransactionModel.Figi == null
                ||
                TransactionModel.Price == 0
                )
            {
                Log.Information("The operation is not possible. Not enough data: " + "transactionModel.Quantity = " + TransactionModel.Quantity + " transactionModel.Figi =" + TransactionModel.Figi + " transactionModel.Price = " + TransactionModel.Price);
                Log.Information("Stop BuyStoksAsyncs: " + TransactionModel.Figi);
                return;
            }

            List<Order> orders = await RetryPolicy.PollyRetrayPolitics.RetryToManyReq().ExecuteAsync(async () => await Authorisation.Context.OrdersAsync());
            foreach (Order order in orders)
            {
                if (order.Figi == TransactionModel.Figi)
                {
                    await RetryPolicy.PollyRetrayPolitics.RetryToManyReq().ExecuteAsync(async () => await Authorisation.Context.CancelOrderAsync(order.OrderId));
                    Log.Information("Delete order by figi: " + TransactionModel.Figi + " RequestedLots " + order.RequestedLots + " ExecutedLots " + order.ExecutedLots + " Price " + order.Price + " Operation " + order.Operation + " Status " + order.Status + " Type " + order.Type);
                }
            }

            await RetryPolicy.PollyRetrayPolitics.RetryToManyReq().ExecuteAsync(async () => await Authorisation.Context.PlaceLimitOrderAsync(new LimitOrder(TransactionModel.Figi, TransactionModel.Quantity, Tinkoff.Trading.OpenApi.Models.OperationType.Buy, TransactionModel.Price)));

            using (StreamWriter sw = new("_operation", true, System.Text.Encoding.Default))
            {
                sw.WriteLine(DateTime.Now + @" Buy " + TransactionModel.Figi + " " /*+ instrument.Ticker */+ " Quantity: " + TransactionModel.Quantity + " price: " + TransactionModel.Price + " mzda: " + (TransactionModel.Price * 0.025m) / 100m);
                sw.WriteLine();
            }
            Log.Information("Create order for Buy " + TransactionModel.Quantity + " lots " + "figi: " + TransactionModel.Figi + "price: " + TransactionModel.Price);
            Log.Information("Stop BuyStoksAsyncs: " + TransactionModel.Figi);
        }

        public async Task SellStoksAsyncs()
        {
            Log.Information("Start SellStoksAsyncs: " + TransactionModel.Figi);
            if (
                TransactionModel.Quantity == 0
                ||
                TransactionModel.Figi == null
                ||
                TransactionModel.Price == 0
                )
            {
                Log.Information("The operation is not possible. Not enough data: " + "transactionModel.Quantity = " + TransactionModel.Quantity + " transactionModel.Figi =" + TransactionModel.Figi + " transactionModel.Price = " + TransactionModel.Price);
                Log.Information("Stop SellStoksAsyncs: " + TransactionModel.Figi);
                return;
            }

            List<Order> orders = await RetryPolicy.PollyRetrayPolitics.RetryToManyReq().ExecuteAsync(async () => await Authorisation.Context.OrdersAsync());
            foreach (Order order in orders)
            {
                if (order.Figi == TransactionModel.Figi)
                {
                    await RetryPolicy.PollyRetrayPolitics.RetryToManyReq().ExecuteAsync(async () => await Authorisation.Context.CancelOrderAsync(order.OrderId));
                    Log.Information("Delete order by figi: " + TransactionModel.Figi + " RequestedLots " + order.RequestedLots + " ExecutedLots " + order.ExecutedLots + " Price " + order.Price + " Operation " + order.Operation + " Status " + order.Status + " Type " + order.Type);
                }
            }

            await RetryPolicy.PollyRetrayPolitics.RetryToManyReq().ExecuteAsync(async () => await Authorisation.Context.PlaceLimitOrderAsync(new LimitOrder(TransactionModel.Figi, TransactionModel.Quantity, Tinkoff.Trading.OpenApi.Models.OperationType.Sell, TransactionModel.Price)));

            using (StreamWriter sw = new("_operation", true, System.Text.Encoding.Default))
            {
                sw.WriteLine(DateTime.Now + @" Sell " + TransactionModel.Figi + " " /*+ instrument.Ticker */+ " Quantity: " + TransactionModel.Quantity + " price: " + TransactionModel.Price + " mzda: " + (TransactionModel.Price * 0.025m) / 100m);
                sw.WriteLine();
            }
            Log.Information("Create order for Sell " + TransactionModel.Quantity + " lots " + "figi: " + TransactionModel.Figi + "price: " + TransactionModel.Price);
            Log.Information("Stop SellStoksAsyncs: " + TransactionModel.Figi);
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
