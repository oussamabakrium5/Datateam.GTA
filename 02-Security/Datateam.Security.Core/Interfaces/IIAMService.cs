using System.Security.Claims;

namespace Datateam.Security
{
    public interface IIAMService
    {
        string GenerateTokenString(LoginUser user);
        Task<bool> Login(LoginUser user);
        Task<bool> RegisterUser(RegisterUser user);
        /*Task<ClaimsPrincipal> GenerateCookie(LoginUser user);*/
    }
}
