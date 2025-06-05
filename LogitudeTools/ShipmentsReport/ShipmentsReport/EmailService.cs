using NetCommonHelper.Logger;
using System.Net.Mail;

namespace ShipmentsReport
{
    public class EmailService
    {
        static readonly DevLog logger = DevLog.Instance;
        private readonly string smtp;
        private readonly string password;
        private readonly string user;
        private readonly string fromAddress;

        public EmailService(string smtp, string user, string password)
        {
            this.smtp = smtp;
            this.password = password;
            this.user = user;
            this.fromAddress = "un@amital.co.il";
        }

        public void Send(List<string> addressList, string subject, string body,Attachment attachment)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromAddress);
            addressList.ForEach(address => message.To.Add(new MailAddress(address)));
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;
            message.Attachments.Add(attachment);

            SmtpClient smtpClient = new SmtpClient(smtp);
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
                logger.WriteFatal(e, $"send email faild, smtp: {smtp}, from: {fromAddress}, address: {string.Join(", ", addressList)}, subject: {subject}, body: {body}, error message: {e.ToString()}");
                throw;
            }
        }
    }
}
