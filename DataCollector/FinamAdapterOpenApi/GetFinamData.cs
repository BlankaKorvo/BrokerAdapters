using DataCollector.RetryPolicy;
using DataCollector.TinkoffAdapterGrpc;
using Google.Protobuf.WellKnownTypes;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using static Google.Rpc.Context.AttributeContext.Types;
using RestSharp;
using System.Text.Json;
using FinamClient;
using Finam.TradeApi.Proto.V1;
using System.Collections.Concurrent;

namespace DataCollector.FinamAdapterOpenApi
{
    public static class GetFinamData
    {
        public static GetPortfolioResult GetPortfolio(string Id = "524380R6K8P")
        {
            try
            {
                var portfolio = PollyRetrayPolitics.RetryAll().Execute(() => GetFinamGrpcClient.Client.GetPortfolioAsync(Id).GetAwaiter().GetResult());
                return portfolio;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                Log.Debug($"GetPortfolio {$"{Id}"} retern null");
                return null;
            }
        } 
    }
}
