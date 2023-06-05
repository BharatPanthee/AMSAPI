using AMSAPI.Models;

namespace AMSAPI.Services
{
    public interface IUserService
    {
        User? Authenticate(string username, string password, string? clientCode, IHostEnvironment hostEnvironment);
    }
}
