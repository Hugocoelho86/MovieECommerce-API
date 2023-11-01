using ECommerceMovies.API.Dto;

namespace ECommerceMovies.API.Services.Email
{
    public interface IEmailService
    {
        void SendEmail (MessageDto message);
    }
}
