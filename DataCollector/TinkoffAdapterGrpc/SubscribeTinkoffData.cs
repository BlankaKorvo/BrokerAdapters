using Google.Protobuf.Collections;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;

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
    }
}
