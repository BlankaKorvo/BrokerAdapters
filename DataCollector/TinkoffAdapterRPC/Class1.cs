using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using Tinkoff.InvestApi.V1;
using Tinkoff.InvestApi;

namespace DataCollector.TinkoffAdapterRPC
{
    internal class Class1
    {
        void Method()
        {
            CallInvoker callInvoker = null;
            var client = new InvestApiClient(callInvoker);
            var request = new GetLastPricesRequest
            {
                Figi = { "BBG0013HGFT4" }
            };
            var response = client.MarketData.GetLastPrices(request);
            foreach (var lastPrice in response.LastPrices)
            {
                Console.WriteLine(lastPrice.Price); // 61.62
            }
        }
    }
}
