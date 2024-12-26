using BerberWeb.UI.Models;

namespace BerberWeb.UI.Services
{
    public interface IUserService
    {
        Task<string> LoginAsync(UserLoginViewModel userLoginViewModel);

    }
}
