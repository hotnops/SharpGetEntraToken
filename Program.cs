using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Broker;

namespace AcquireTokenSilentExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Replace these values with your app's values from the Azure portal
            // If no args are provided
            if (args.Length < 3)
            {
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
                .WithBroker(new BrokerOptions(BrokerOptions.OperatingSystems.Windows))
                .Build();

            // Attempt to acquire token silently
            IEnumerable<IAccount> accounts = await app.GetAccountsAsync();
            if (accounts == null)
            {
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
                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}
