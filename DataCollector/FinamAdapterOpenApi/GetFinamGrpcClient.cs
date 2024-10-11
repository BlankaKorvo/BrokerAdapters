using FinamClient;
using RestSharp;
using Serilog;
using System.IO;
using System.Reflection;
using Tinkoff.InvestApi;

namespace DataCollector.FinamAdapterOpenApi
{
    public static class GetFinamGrpcClient
    {
        private static FinamClient.FinamApi client;


        //private static DBAuth.DataBase.ApplicationDbContext? dbClient;

        public static FinamClient.FinamApi Client
        {
            get
            {
                if (client == null)
                {
                    string pathT = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
                    string token = File.ReadAllLines(Path.Combine(pathT, "finsann.dll"))[0].Trim();
                    client = new FinamApi(token!);
                }
                return client;
            }
        }
    }
}