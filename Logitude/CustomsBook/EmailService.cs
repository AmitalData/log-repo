using NLog;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace CustomsBook
{
    public class EmailService
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly string smtp;
        private readonly string password;
        private readonly string user;
        private readonly string fromAddress;

        public EmailService(string smtp, string user, string password, string fromAddress = "un@amital.co.il")
        {
            this.smtp = smtp;
            this.password = password;
            this.user = user;
            this.fromAddress = fromAddress;
        }

        public void Send(List<string> addressList, string subject, string body)
        {
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress(fromAddress);
                addressList.ForEach(address => message.To.Add(new MailAddress(address)));
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = false;

                using (SmtpClient smtpClient = new SmtpClient(smtp))
                {
                    smtpClient.Port = 587;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new System.Net.NetworkCredential(user, password);
                    smtpClient.EnableSsl = true;

                    try
                    {
                        smtpClient.Send(message);
                    }
                    catch (Exception e)
                    {
                        logger.Error(e, $"Failed to send email. smtp: {smtp}, to: {string.Join(",", addressList)}");
                        throw;
                    }
                }
            }
        }
    }
}
