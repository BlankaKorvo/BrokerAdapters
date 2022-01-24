//using Serilog;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Text;
//using System.Threading.Tasks;
//using Tinkoff.Trading.OpenApi.Models;

//namespace DataCollector.TinkoffAdapter
//{
//    class PostLimitOrders
//    {
//        public static async Task BuyStoksAsyncs(string figi, int quantity, decimal price)
//        {
//            Log.Information("Start BuyStoksAsyncs: " + figi);
//            if (
//                quantity == 0
//                ||
//                figi == null
//                ||
//                price == 0
//                )
//            {
//                Log.Information("The operation is not possible. Not enough data: " + "transactionModel.Quantity = " + quantity + " transactionModel.Figi =" + figi + " transactionModel.Price = " + price);
//                Log.Information("Stop BuyStoksAsyncs: " + figi);
//                throw new Exception("BuyStoksAsyncs incomplite data");
//            }

//            List<Order> orders = await RetryPolicy.PollyRetrayPolitics.RetryToManyReq().ExecuteAsync(async () => await Authorisation.Context.OrdersAsync());
//            foreach (Order order in orders)
//            {
//                if (order.Figi == figi)
//                {
//                    await RetryPolicy.PollyRetrayPolitics.RetryToManyReq().ExecuteAsync(async () => await Authorisation.Context.CancelOrderAsync(order.OrderId));
//                    Log.Information("Delete order by figi: " + figi + " RequestedLots " + order.RequestedLots + " ExecutedLots " + order.ExecutedLots + " Price " + order.Price + " Operation " + order.Operation + " Status " + order.Status + " Type " + order.Type);
//                }
//            }

//            await RetryPolicy.PollyRetrayPolitics.RetryToManyReq().ExecuteAsync(async () => await Authorisation.Context.PlaceLimitOrderAsync(new LimitOrder(figi, quantity, OperationType.Buy, price)));

//            using (StreamWriter sw = new("_operation", true, Encoding.Default))
//            {
//                sw.WriteLine(DateTime.Now + @" Buy " + figi + " " + " Quantity: " + quantity + " price: " + price + " mzda: " + (price * 0.025m) / 100m);
//                sw.WriteLine();
//            }
//            Log.Information("Create order for Buy " + quantity + " lots " + "figi: " + figi + "price: " + price);
//            Log.Information("Stop BuyStoksAsyncs: " + figi);
//        }

//        public static async Task SellStoksAsyncs(string figi, int quantity, decimal price)
//        {
//            Log.Information("Start SellStoksAsyncs: " + figi);
//            if (
//                quantity == 0
//                ||
//                figi == null
//                ||
//                price == 0
//                )
//            {
//                Log.Information("The operation is not possible. Not enough data: " + "transactionModel.Quantity = " + quantity + " transactionModel.Figi =" + figi + " transactionModel.Price = " + price);
//                Log.Information("Stop SellStoksAsyncs: " + figi);
//                throw new Exception("SellStoksAsyncs incomplite data");
//            }

//            List<Order> orders = await RetryPolicy.PollyRetrayPolitics.RetryToManyReq().ExecuteAsync(async () => await Authorisation.Context.OrdersAsync());
//            foreach (Order order in orders)
//            {
//                if (order.Figi == figi)
//                {
//                    await RetryPolicy.PollyRetrayPolitics.RetryToManyReq().ExecuteAsync(async () => await Authorisation.Context.CancelOrderAsync(order.OrderId));
//                    Log.Information("Delete order by figi: " + figi + " RequestedLots " + order.RequestedLots + " ExecutedLots " + order.ExecutedLots + " Price " + order.Price + " Operation " + order.Operation + " Status " + order.Status + " Type " + order.Type);
//                }
//            }

//            await RetryPolicy.PollyRetrayPolitics.RetryToManyReq().ExecuteAsync(async () => await Authorisation.Context.PlaceLimitOrderAsync(new LimitOrder(figi, quantity, Tinkoff.Trading.OpenApi.Models.OperationType.Sell, price)));

//            using (StreamWriter sw = new("_operation", true, System.Text.Encoding.Default))
//            {
//                sw.WriteLine(DateTime.Now + @" Sell " + figi + " " /*+ instrument.Ticker */+ " Quantity: " +quantity + " price: " + price + " mzda: " + (price * 0.025m) / 100m);
//                sw.WriteLine();
//            }
//            Log.Information("Create order for Sell " + quantity + " lots " + "figi: " + figi + "price: " + price);
//            Log.Information("Stop SellStoksAsyncs: " + figi);
//        }
//    }
//}
