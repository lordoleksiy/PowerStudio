using Azure.Core;
using PowerStudio.Interfaces;
using System;
using System.Threading.Tasks;

namespace PowerStudio.Services
{
    public class MsalTokenCredential : TokenCredential
    {
        private readonly IAuthenticationService _authService;
        public MsalTokenCredential(IAuthenticationService authService)
        {
            _authService = authService;
        }

        public override AccessToken GetToken(TokenRequestContext requestContext, System.Threading.CancellationToken cancellationToken)
        {
            var token = _authService.GetTokenAsync().Result;
            return new AccessToken(token, _authService.ExpiresOn);
        }

        public override async ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, System.Threading.CancellationToken cancellationToken)
        {
            var token = await _authService.GetTokenAsync();
            return await Task.FromResult(new AccessToken(token, _authService.ExpiresOn));
        }
    }
}
