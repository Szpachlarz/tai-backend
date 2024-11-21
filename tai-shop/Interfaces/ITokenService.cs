using tai_shop.Models;

namespace tai_shop.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
