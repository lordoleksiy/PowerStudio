using Microsoft.Identity.Client;
using PowerStudio.Interfaces;
using PowerStudio.Models.Settings;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PowerStudio.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private static IPublicClientApplication _publicClientApp;
        public DateTime ExpiresOn { get; private set; }
        public bool IsAuthenticated { get; private set; } = false;
        private readonly string _scopes;
        public AuthenticationService(AzureAdSettings settings) 
        {
            _scopes = settings.Scopes;
            _publicClientApp = PublicClientApplicationBuilder.Create(settings.ClientId)
                .WithTenantId(settings.TenantId)
                .WithRedirectUri(settings.RedirectUrl)
                .Build();
        }
        public async Task<string> GetTokenAsync()
        {
            var accounts = await _publicClientApp.GetAccountsAsync(_scopes);

            AuthenticationResult result = null;
            if (accounts.Any())
            {
                result = await _publicClientApp.AcquireTokenSilent([_scopes], accounts.FirstOrDefault())
                    .ExecuteAsync();
            }
            else
            {
                result = await _publicClientApp.AcquireTokenInteractive([_scopes])
                    .ExecuteAsync();
            }
            ExpiresOn = result.ExpiresOn.UtcDateTime;
            IsAuthenticated = true;
            return result.AccessToken;
        }

        public async Task LogoutUserAsync()
        {
            var accounts = await _publicClientApp.GetAccountsAsync();
            if (accounts.Any())
            {
                await _publicClientApp.RemoveAsync(accounts.FirstOrDefault());
            }
            IsAuthenticated = false;
        }
    }
}
