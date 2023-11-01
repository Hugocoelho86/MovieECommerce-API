using ECommerceMovies.API.Dto;

namespace ECommerceMovies.API.Services.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync (MessageDto message);
    }
}
