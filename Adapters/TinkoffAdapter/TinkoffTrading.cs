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
using Operation = MarketDataModules.TradeOperation;
using Orderbook = MarketDataModules.Orderbook;
using CandleInterval = MarketDataModules.CandleInterval;
using MarketDataModules;

namespace TinkoffAdapter.TinkoffTrade
{
    public class TinkoffTrading //: TransactionModel
    {
        //public Context context { get; set; }
        //public string figi { get; set; }
        public CandleInterval candleInterval { get; set; }
        //public int countStoks { get; set; }
        public int CandlesCount { get; set; } = 120;
        //public decimal budget { get; set; }

        //время ожидания между следующим циклом
        int sleep { get; set; } = 0;

        //GetTinkoffData market = new GetTinkoffData();
        //MarketDataCollector marketDataCollector = new MarketDataCollector();

       

        public async Task BuyStoksAsync(TransactionModel transactionModel)
        {
            Log.Information("Start BuyStoks: " + transactionModel.Figi);
            List<Order> orders = await RetryPolicy.Model.RetryToManyReq().ExecuteAsync(async () => await Auth.Context.OrdersAsync());
            foreach (Order order in orders)
            {
                if (order.Figi == transactionModel.Figi)
                {
                    await RetryPolicy.Model.RetryToManyReq().ExecuteAsync(async () => await Auth.Context.CancelOrderAsync(order.OrderId));
                    Log.Information("Delete order by figi: " + transactionModel.Figi + " RequestedLots " + order.RequestedLots + " ExecutedLots " + order.ExecutedLots + " Price " + order.Price + " Operation " + order.Operation + " Status " + order.Status + " Type " + order.Type);
                }
            }
            int lots = await CalculationLotsByMarginAsync(transactionModel);
            //transactionModel.Quantity = await CalculationLotsByMargin(transactionModel);
            if (lots == 0)
            {
                Log.Information("Not any lot in margin: " + transactionModel.Purchase);
                return; }

            await RetryPolicy.Model.RetryToManyReq().ExecuteAsync(async () => await Auth.Context.PlaceLimitOrderAsync(new LimitOrder(transactionModel.Figi, lots, OperationType.Buy, transactionModel.Price)));
            //Instrument instrument = await marketDataCollector.GetInstrumentByFigi(transactionModel.Figi, Provider.Tinkoff);
            using (StreamWriter sw = new StreamWriter("_operation", true, System.Text.Encoding.Default))
            {
                sw.WriteLine(DateTime.Now + @" Buy " + transactionModel.Figi + " " /*+ instrument.Ticker */+ " Quantity: " + lots + " price: " + transactionModel.Price + " mzda: " + (transactionModel.Price * 0.02m) / 100m);
                sw.WriteLine();
            }
            Log.Information("Create order for Buy " + lots + " lots " + "figi: " + transactionModel.Figi + "price: " + transactionModel.Price);
            Log.Information("Stop BuyStoks: " + transactionModel.Figi);
        }

        public async Task SellStoksFromLongAsync(TransactionModel transactionModel)
        {
            Log.Information("Start SellStoksFromLong: " + transactionModel.Figi);
            int lots = await CalculationStocksFromLongAsync(transactionModel);
            if (lots == 0)
            { return; }
            await RetryPolicy.Model.RetryToManyReq().ExecuteAsync(async () => await Auth.Context.PlaceLimitOrderAsync(new LimitOrder(transactionModel.Figi, lots, OperationType.Sell, transactionModel.Price)));
            using (StreamWriter sw = new StreamWriter("_operation", true, System.Text.Encoding.Default))
            {
                sw.WriteLine(DateTime.Now + @" Sell " + transactionModel.Figi + " Quantity: " + lots + " price: " + transactionModel.Price + " mzda: " + (transactionModel.Price * 0.02m) / 100m);
                sw.WriteLine();
            }
            Log.Information("Create order for Sell " + lots + " stocks " + "figi: " + transactionModel.Figi + "price: " + transactionModel.Price);
            Log.Information("Stop SellStoksFromLong: " + transactionModel.Figi);
        }        
        private async Task<int> CalculationStocksBuyCount(string figi, int countLotsToBuy)
        {
            int lots = await CountLotsInPortfolioAsync(figi);
            Log.Information("Need to buy stocks: " + countLotsToBuy);

            int countLotsToBuyReal = countLotsToBuy - lots;
            Log.Information("Real need to buy: " + countLotsToBuyReal);

            return countLotsToBuyReal;
        }
        private async Task<int> CalculationLotsByMarginAsync(TransactionModel transactionModel)
        {
            Log.Information("Start CalculationLotsByMargin method. Figi: " + transactionModel.Figi);
            int lots = await CountLotsInPortfolioAsync(transactionModel.Figi);
            Log.Information("Lots " + transactionModel.Figi + " in portfolio: " + lots);
            decimal sumCostLotsInPorfolio = transactionModel.Price * Convert.ToDecimal(lots);
            decimal remainingCostLots = transactionModel.Purchase - sumCostLotsInPorfolio;
            if (remainingCostLots <= 0)
            {
                Log.Information("Stop CalculationLotsByMargin method. Figi: " + transactionModel.Figi + " Return: 0");
                return 0;
            }
            int countLotsToBuy = Convert.ToInt32(Math.Floor(remainingCostLots / transactionModel.Price));
            if (countLotsToBuy <= transactionModel.Quantity)
            {
                Log.Information("Need to buy: " + countLotsToBuy);
                Log.Information("Stop CalculationLotsByMargin method. Figi: " + transactionModel.Figi );
                return countLotsToBuy;
            }
            else
            {
                Log.Information("Need to buy: " + transactionModel.Quantity);
                Log.Information("Stop CalculationLotsByMargin method. Figi: " + transactionModel.Figi );
                return transactionModel.Quantity;
            }
        }

        private async Task<int> CalculationStocksFromLongAsync(TransactionModel transactionModel)
        {
            Log.Information("Start CalculationStocksFromLong method. Figi: " + transactionModel.Figi);
            int lots = await CountLotsInPortfolioAsync(transactionModel.Figi);
            Log.Information("Lots " + transactionModel.Figi + " in portfolio: " + lots);
            if (lots <= transactionModel.Quantity)
            {
                Log.Information("Need to sell: " + lots);
                Log.Information("Stop CalculationStocksFromLong method. Figi: " + transactionModel.Figi);
                return lots;
            }
            else
            {
                Log.Information("Need to buy: " + transactionModel.Quantity);
                Log.Information("Stop CalculationStocksFromLong method. Figi: " + transactionModel.Figi);
                return transactionModel.Quantity;
            }
        }
        private async Task<int> CountLotsInPortfolioAsync(string figi)
        {
            Log.Information("Start CountLotsInPortfolio method. Figi: " + figi);
            var portfolio = await RetryPolicy.Model.RetryToManyReq().ExecuteAsync(async () => await Auth.Context.PortfolioAsync());
            int lots = 0;
            foreach (var item in portfolio.Positions)
            {
                if (item.Figi == figi)
                {
                    lots = item.Lots;
                    Log.Information("Lots " + figi + " in portfolio: " + lots);
                    Log.Information("Stop CountLotsInPortfolio method. Figi: " + figi);
                    break;
                }
            };
            Log.Information("Stop CountLotsInPortfolio method. Figi: " + figi);
            return lots;
        }
    }
}
