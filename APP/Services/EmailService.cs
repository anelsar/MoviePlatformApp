using FluentEmail.Core;
using FluentEmail.Smtp;
using System.Net.Mail;
using System.Threading.Tasks;

namespace APP.Services
{
    public class EmailService : IEmailService
    {
        
        public async Task SendEmail(string body, string subject, string destination)
        {
            var sender = new SmtpSender(() => new SmtpClient("localhost")
            {
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Port = 25
            });
            Email.DefaultSender = sender;

            var email = await Email
                .From("movieapp-noreply@test.com")
                .To(destination)
                .Subject(subject)
                .Body(body)
                .SendAsync();
        }
    }
}