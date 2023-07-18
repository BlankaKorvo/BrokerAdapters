using DataCollector;
using Serilog;
using System.IO;
using System.Reflection;
using Tinkoff.InvestApi;

namespace DataCollector.TinkoffAdapterGrpc
{
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
            string pathT = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
            string token = File.ReadAllLines(Path.Combine(pathT, "toksann.dll"))[0].Trim();
            InvestApiClient client = InvestApiClientFactory.Create(token);
            return client;
        }
    }
}