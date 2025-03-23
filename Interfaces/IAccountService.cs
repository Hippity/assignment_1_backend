using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieBackend.Interaces
{
    public interface IAccountService
    {
        Task<object> GetUserInfoAsync(ClaimsPrincipal user);
        Task<string> GenerateJwtTokenAsync(ClaimsPrincipal user);
        Task<object> GetUserInfoFromExternalLoginAsync(ClaimsPrincipal externalUser);
    }
}