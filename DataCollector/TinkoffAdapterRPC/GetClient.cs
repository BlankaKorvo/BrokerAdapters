using DataCollector;
using Serilog;
using System.IO;
using Tinkoff.InvestApi;

public static class GetClient
{
    static InvestApiClient client;

    /// <summary>
    /// Получение клиента.
    /// </summary>
    public static InvestApiClient Grpc
    {
        get
        {
            if (client == null)
            {
                client = GetGrpcClient();
            }
            return client;
        }
    }
    /// <summary>
    /// Получение клиента
    /// </summary>
    /// <returns></returns>
    static InvestApiClient GetGrpcClient()
    {
        string token = File.ReadAllLines("toksann.dll")[0].Trim();
        InvestApiClient client = InvestApiClientFactory.Create(token);
        return client;
    }
}