namespace ECommerceMovies.API.Services.Authentication
{
    public interface IAuthService
    {
        Task Login(string username, string password);
    }
}
