using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace StudentCourseRegistrationAssignment.BLL.EmailServices
{
    public class EmailService : IEmailService
    {
        public async Task SendAsync(string to, string subject, string body)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("moustafa.anafea@gmail.com", "mhpz htqt xhya tuow"),
                EnableSsl = true
            };

            var message = new MailMessage("moustafa.anafea@gmail.com", to, subject, body)
            {
                IsBodyHtml = true
            };

            await client.SendMailAsync(message);
        }
    }
}
