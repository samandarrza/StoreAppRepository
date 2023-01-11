using Store.Core.Entities;

namespace Store.Api.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(AppUser user, IList<string> roles, IConfiguration confg);
    }
}
