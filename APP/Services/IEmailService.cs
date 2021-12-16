using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Services
{
    public interface IEmailService : IMessageService
    {
        Task SendEmail(string body, string subject, string destination);
    }
}
