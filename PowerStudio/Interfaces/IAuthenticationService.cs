using System;
using System.Threading.Tasks;

namespace PowerStudio.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> GetTokenAsync();
        Task LogoutUserAsync();
        DateTime ExpiresOn { get; }
        bool IsAuthenticated { get; }
    }
}
