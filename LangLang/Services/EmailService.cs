using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Services
{
    public static class EmailService
    {
        private const string SenderPassword = "ykmc owwc bbrz uhbq";
        private const string SenderEmail = "langlang3b@gmail.com";
        private const string ReceiverEmail = "krivokapic.sv41.2022@uns.ac.rs";

        public static void SendMessage(string subject, string body)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(SenderEmail);
            message.Subject = subject;
            message.To.Add(new MailAddress(ReceiverEmail));
            message.Body = body;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(SenderEmail,SenderPassword),
                EnableSsl = true,
            };

            smtpClient.Send(message);
        }
    }
}
