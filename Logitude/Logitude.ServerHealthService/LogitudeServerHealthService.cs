using System;
using System.IO;
using System.ServiceProcess;
using System.Timers;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Net;
using System.Configuration;

namespace Logitude.ServerHealthService
{
    public partial class LogitudeServerHealthService : ServiceBase
    {
        protected Timer ServiceTimer = new Timer();
        protected int PercentageOfTotalSize = Convert.ToInt32(ConfigurationManager.AppSettings["PercentageOfTotalSize"]);
        protected string EmailAddresses = ConfigurationManager.AppSettings["EmailAddresses"];

        public LogitudeServerHealthService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            WriteToLogsFile("Service Is Started At " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"));
            ServiceTimer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            ServiceTimer.Interval = 3600000;//1 hour
            ServiceTimer.Enabled = true;
        }

        protected override void OnStop()
        {
            WriteToLogsFile("Service Is Stopped At " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"));
        }

        protected void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            DriveInfo[] serverDrives = DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Fixed).ToArray();

            foreach (DriveInfo drive in serverDrives)
            {
                if (drive.IsReady == true)
                {
                    long driveTotalSize = drive.TotalSize;
                    long driveTotalFreeSpace = drive.TotalFreeSpace;
                    double percentageOfTotal = ((double)PercentageOfTotalSize / 100) * driveTotalSize;

                    if (driveTotalFreeSpace < percentageOfTotal)
                    {
                        string driveName = drive.VolumeLabel + " (" + drive.Name.Replace(@"\", String.Empty) + ")";
                        string warningMessage = BuildWarningMessage(driveName, driveTotalSize, driveTotalFreeSpace);
                        SendEmails(warningMessage);
                    }
                }
            }
        }

        protected string BuildWarningMessage(string driveName, long driveTotalSize, long driveTotalFreeSpace)
        {
            string warningMessage = "The Drive " + driveName + " Have Free Space Less Than " + PercentageOfTotalSize.ToString() + "% Of It's Total Space.\n";
            warningMessage += "Total Space: " + driveTotalSize.ToString() + " Bytes.\n";
            warningMessage += "Total Free Space: " + driveTotalFreeSpace.ToString() + " Bytes.\n";
            warningMessage += "\n" + "From Logitude Server Health Service.";

            return warningMessage;
        }

        protected void SendEmails(string message)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net");

                mailMessage.From = new MailAddress("fanar@logitudeworld.com");
                if (EmailAddresses.Contains(","))
                {
                    foreach (var emailAddress in EmailAddresses.Split(',').ToList())
                    {
                        mailMessage.To.Add(emailAddress);
                    }
                }
                else
                {
                    mailMessage.To.Add(EmailAddresses);
                }
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.Subject = "Logitude Server Health Warning";
                mailMessage.Body = message;
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential("LogitudeworldTestAccount", "!T1234567");
                smtpClient.Send(mailMessage);

                WriteToLogsFile("Warning Message Sent Successfully To " + EmailAddresses + " At " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"));
            }
            catch (Exception exception)
            {
                WriteToLogsFile("Error While Sending Warning Message At " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + " With Exception: " + exception.Message);
            }
        }

        protected void WriteToLogsFile(string log)
        {
            string logsFileDirectory = "C:" + "\\Logitude.ServerHealthService.Logs";

            if (!Directory.Exists(logsFileDirectory))
            {
                Directory.CreateDirectory(logsFileDirectory);
            }

            string logsFilePath = logsFileDirectory + "\\ServerHealthServiceLogs_" + DateTime.Now.Date.ToString("dd/MM/yyyy").Replace('/', '_') + ".txt";

            if (!File.Exists(logsFilePath))
            {
                using (StreamWriter streamWriter = File.CreateText(logsFilePath))
                {
                    streamWriter.WriteLine(log);
                }
            }
            else
            {
                using (StreamWriter streamWriter = File.AppendText(logsFilePath))
                {
                    streamWriter.WriteLine(log);
                }
            }
        }
    }
}