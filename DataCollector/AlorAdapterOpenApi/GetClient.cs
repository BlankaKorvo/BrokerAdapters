using DataCollector;
using RestSharp.Authenticators;
using RestSharp;
using Serilog;
using System.IO;
using System.Reflection;
using Tinkoff.InvestApi;
using System.IdentityModel.Tokens.Jwt;
using static Google.Rpc.Context.AttributeContext.Types;
using System.Text.Json;
using System;
using Google.Api;

namespace DataCollector.AlorAdapterOpenApi
{
    public static class AlorClient
    {
        static string RefreshToken = "0a5a4c06-d39e-4bf5-9648-1e44b830b66f";
        static RestClient client;
        public static RestClient GetOpenApiClient
        {
            get
            {
                if (client == null)
                {
                    client = CreateClient(RefreshToken);
                }
                return client;
            }
        }
        public static RestClient RefreshOpenApiClient
        {
            get
            {
                client = CreateClient(RefreshToken);
                return client;
            }
        }

        static RestClient CreateClient(string RefreshToken)
        {
            var AccessToken = CreateAccessToken(RefreshToken);
            var authenticator = new JwtAuthenticator(AccessToken);
            var options = new RestClientOptions("https://api.alor.ru")
            {
                Authenticator = authenticator
            };
            var restClient = new RestClient(options);
            return restClient;
        }
        static string CreateAccessToken(string RefreshToken)
        {
            var authenticator = new JwtAuthenticator(RefreshToken);
            var options = new RestClientOptions("https://oauth.alor.ru")
            {
                Authenticator = authenticator
            };          
            var authClient = new RestClient(options);
            var request = new RestRequest($"refresh?token={RefreshToken}");
            var response = authClient.ExecutePost(request);

            var accessTokenAdmin = JsonSerializer.Deserialize<AloreAccToken>(response?.Content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return accessTokenAdmin.AccessToken;
        }
    }
    internal class AloreAccToken
    {
        public string AccessToken { get; set; }
    }
}