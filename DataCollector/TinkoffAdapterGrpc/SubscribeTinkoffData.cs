using Google.Protobuf.Collections;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using Tinkoff.Trading.OpenApi.Models;

namespace DataCollector.TinkoffAdapterGrpc
{
    public static class SubscribeTinkoffData
    {

        public static async Task<AsyncDuplexStreamingCall<MarketDataRequest, MarketDataResponse>> Candles(RepeatedField<CandleInstrument> subInstruments)
        {
            InvestApiClient client = GetClient.Grpc;
            AsyncDuplexStreamingCall<MarketDataRequest, MarketDataResponse> streamMarketData = client.MarketDataStream.MarketDataStream();
            // Отправляем запрос в стрим
            await streamMarketData.RequestStream.WriteAsync(new MarketDataRequest
            {
                SubscribeCandlesRequest = new SubscribeCandlesRequest
                {
                    Instruments = { subInstruments },
                    SubscriptionAction = SubscriptionAction.Subscribe,
                    WaitingClose = false
                }
            });
            return streamMarketData;
        }
        public static async Task<AsyncServerStreamingCall<PortfolioStreamResponse>> Portfolio()
        {
            InvestApiClient client = GetClient.Grpc;
            var accounts = new RepeatedField<string> {"" };
            PortfolioStreamRequest request = new PortfolioStreamRequest() {};
            var position = new PositionsStreamRequest();
            var portfolio = new PortfolioStreamRequest(request) ;
            AsyncServerStreamingCall<PortfolioStreamResponse> portfolioStream = client.OperationsStream.PortfolioStream(portfolio);

            portfolioStream.ResponseStream.ReadAllAsync<PortfolioStreamResponse>();

            return portfolioStream;
        }
    }
}
