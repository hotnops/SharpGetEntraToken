using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Broker;
using System.Security.Authentication;

namespace AcquireTokenSilentExample
{
    public class StaticClientWithProxyFactory : IMsalHttpClientFactory
    {
        private static readonly HttpClient s_httpClient;

        static HttpClientHandler handler;

        static StaticClientWithProxyFactory()
        {
            handler = new HttpClientHandler();
            handler.SslProtocols = SslProtocols.Tls12;
            s_httpClient = new HttpClient(handler);
        }

        public HttpClient GetHttpClient()
        {
            return s_httpClient;
        }
    }

    class Program
    {

        static async Task Main(string[] args)
        {
            Console.WriteLine("[*] Getting token");
            IMsalHttpClientFactory httpClientFactory = new StaticClientWithProxyFactory();
            // Replace these values with your app's values from the Azure portal
            // If no args are provided
            if (args.Length < 3)
            {
                Console.WriteLine("[*] Not enough args");
                return;
            }

            string clientId = args[0];
            string tenantId = args[1];
            string[] scopes = new string[] { args[2] };

            // Authority URL for Microsoft identity platform (Entra ID)
            string authority = $"https://login.microsoftonline.com/{tenantId}";

            // Create a PublicClientApplication instance
            var app = PublicClientApplicationBuilder.Create(clientId)
                .WithAuthority(authority)
                .WithHttpClientFactory(httpClientFactory)
                .WithBroker(new BrokerOptions(BrokerOptions.OperatingSystems.Windows))
                .Build();

            if (app == null)
            {
                Console.WriteLine("[*] Failed to initialize app");
                return;
            }

            // Attempt to acquire token silently
            IEnumerable<IAccount> accounts = await app.GetAccountsAsync();
            if (accounts == null)
            {
                Console.WriteLine("[*] Cannot obtain accounts enumerable");
                return;
            }

            IAccount accountToLogin = accounts.FirstOrDefault();
            if (accountToLogin == null)
            {
                accountToLogin = PublicClientApplication.OperatingSystemAccount;
            }
            try
            {
                var result = await app.AcquireTokenSilent(scopes, accountToLogin).ExecuteAsync();
                Console.WriteLine(result.AccessToken);
            }
            catch (MsalUiRequiredException)
            {
                Console.WriteLine("[!] MsalUiRequiredException. Interactive login required");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[!] " + ex.Message);
                Console.WriteLine(ex.ToString());
                return;
            }
        }
    }
}
