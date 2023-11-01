using Microsoft.OpenApi.Extensions;
using MimeKit;

namespace ECommerceMovies.API.Dto
{
    public class MessageDto
    {
        public List<MailboxAddress> To { get; set; } = new List<MailboxAddress>();
        public string Subject { get; set; }
        public string Body { get; set; }

        public MessageDto(IEnumerable<string> to, string subject, string body)
        {
            To = new List<MailboxAddress>();

            To.AddRange(to.Select(x => MailboxAddress.Parse(x)));
            //To.AddRange(to.Select(x => new MailboxAddress("Name", "name@email.com")));
            Subject = subject;
            Body = body;
        }

        public MessageDto(string to, string subject, string body)
        {
            To.Add( MailboxAddress.Parse(to));
            Subject = subject;
            Body = body;
        }

        public MessageDto(MailboxAddress to, string subject, string body)
        {
            To.Add(to);
            Subject = subject;
            Body = body;
        }
    }
}
