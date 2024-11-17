using WaterReminder.Auth.Models;

namespace WaterReminder.Auth.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
