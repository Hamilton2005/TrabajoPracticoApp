using TrabajoPracticoApp.Models;

namespace TrabajoPracticoApp.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        string GeneratePasswordResetToken();

    }
}
