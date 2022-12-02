//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Grpc.Core;
//using Google.Protobuf.WellKnownTypes;
//using Tinkoff.InvestApi.V1;
//using Tinkoff.InvestApi;
//using System.IO;

//namespace DataCollector.TinkoffAdapterRPC
//{
//    internal class Class1
//    {
//        void Method()
//        {
//            string token = File.ReadAllLines("toksan.dll")[0].Trim();
//            InvestApiClient client = InvestApiClientFactory.Create($"<{token}>");
//            GetCandlesRequest getCandlesRequest = new GetCandlesRequest() { Figi = "", From = DateTime.Now.AddDays(-1).ToTimestamp(), Interval = CandleInterval._1Min, To = DateTime.Now.ToTimestamp() };
//            client.MarketData.GetCandlesAsync(getCandlesRequest);
//        }
//    }
//}
