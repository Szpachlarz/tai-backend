using tai_shop.Models;

namespace tai_shop.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
