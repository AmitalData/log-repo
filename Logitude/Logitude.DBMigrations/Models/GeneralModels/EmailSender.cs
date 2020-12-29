using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DBMigrations.Models
{
    public class EmailSender
    {
        protected string Subject;
        protected string MessageBody;

        public EmailSender(string subject, string messageBody)
        {
            Subject = subject;
            MessageBody = messageBody;
        }

        public void Send()
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient(ToolConfigurations.SmtpClientHost);
                MailMessage mailMessage = new MailMessage();

                mailMessage.From = new MailAddress(ToolConfigurations.FromEmailAddress);
                foreach (var emailAddress in ToolConfigurations.ToEmailAddresses.Split(',').ToList())
                {
                    mailMessage.To.Add(emailAddress);
                }
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.Subject = Subject;
                mailMessage.Body = MessageBody + "\n\nSent From DBMigrations Tool Which Running On " + Environment.MachineName + "\nSent At " + DateTime.Now.ToString();
                smtpClient.Port = ToolConfigurations.SmtpClientPort;
                smtpClient.Credentials = new NetworkCredential(ToolConfigurations.SmtpClientUsername, ToolConfigurations.SmtpClientPassword);
                smtpClient.Send(mailMessage);

                Console.WriteLine("Email Message Sent Successfully To " + ToolConfigurations.ToEmailAddresses);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Cannot Send Email Message With Error: " + exception.Message);
            }
        }
    }
}