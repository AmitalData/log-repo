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
        protected int PercentageOfTotalDriveSpace;
        protected string FromAddress;
        protected string ToAddresses;
        protected string SmtpClientHost;
        protected int SmtpClientPort;
        protected string SmtpClientUsername;
        protected string SmtpClientPassword;

        public LogitudeServerHealthService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            WriteToLogsFile("Service Is Started At " + GetCurrentDateTime(true));
            ReadConfigurations();
            ServiceTimer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            ServiceTimer.Interval = 3600000;//1 hour
            ServiceTimer.Enabled = true;
        }

        protected override void OnStop()
        {
            WriteToLogsFile("Service Is Stopped At " + GetCurrentDateTime(true));
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
                    double percentageOfTotal = ((double)PercentageOfTotalDriveSpace / 100) * driveTotalSize;

                    if (driveTotalFreeSpace < percentageOfTotal)
                    {
                        string driveName = drive.VolumeLabel + " (" + drive.Name.Replace(@"\", String.Empty) + ")";
                        string warningMessage = BuildWarningMessage(driveName, driveTotalSize, driveTotalFreeSpace);
                        string subject = "Logitude Server Health Warning";
                        SendEmails(subject, warningMessage);
                    }
                }
            }
        }

        protected void ReadConfigurations()
        {
            PercentageOfTotalDriveSpace = Convert.ToInt32(ConfigurationManager.AppSettings["PercentageOfTotalDriveSpace"]);
            FromAddress = ConfigurationManager.AppSettings["FromAddress"];
            ToAddresses = ConfigurationManager.AppSettings["ToAddresses"];
            SmtpClientHost = ConfigurationManager.AppSettings["SmtpClientHost"];
            SmtpClientPort = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpClientPort"]);
            SmtpClientUsername = ConfigurationManager.AppSettings["SmtpClientUsername"];
            SmtpClientPassword = ConfigurationManager.AppSettings["SmtpClientPassword"];
        }

        protected string BuildWarningMessage(string driveName, long driveTotalSize, long driveTotalFreeSpace)
        {
            string warningMessage = "The Drive " + driveName + " Have Free Space Less Than " + PercentageOfTotalDriveSpace.ToString() + "% Of It's Total Space.\n";
            warningMessage += "Total Space: " + ConvertBytesToGigabytes(driveTotalSize).ToString() + " GB.\n";
            warningMessage += "Total Free Space: " + ConvertBytesToGigabytes(driveTotalFreeSpace).ToString() + " GB.\n";
            warningMessage += "\n" + "From Logitude Server Health Service.";

            return warningMessage;
        }

        protected void SendEmails(string subject, string message)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient(SmtpClientHost);
                MailMessage mailMessage = new MailMessage();

                mailMessage.From = new MailAddress(FromAddress);
                foreach (var emailAddress in ToAddresses.Split(',').ToList())
                {
                    mailMessage.To.Add(emailAddress);
                }
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.Subject = subject;
                mailMessage.Body = message;
                smtpClient.Port = SmtpClientPort;
                smtpClient.Credentials = new NetworkCredential(SmtpClientUsername, SmtpClientPassword);
                smtpClient.Send(mailMessage);

                WriteToLogsFile("Warning Message Sent Successfully To " + ToAddresses + " At " + GetCurrentDateTime(true));
            }
            catch (Exception exception)
            {
                WriteToLogsFile("Error While Sending Warning Message At " + GetCurrentDateTime(true) + " With Exception: " + exception.Message);
            }
        }

        protected void WriteToLogsFile(string log)
        {
            try
            {
                string logsFileDirectory = "C:" + "\\Logitude.ServerHealthService.Logs";

                if (!Directory.Exists(logsFileDirectory))
                {
                    Directory.CreateDirectory(logsFileDirectory);
                }

                string logsFilePath = logsFileDirectory + "\\ServerHealthServiceLogs_" + GetCurrentDateTime(false).Replace('/', '_') + ".txt";

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
            catch (Exception)
            {
                //Error While Writing To Logs File
            }
        }

        protected string GetCurrentDateTime(bool withTime)
        {
            string dateTimeFormat = "dd/MM/yyyy";
            if (withTime)
            {
                dateTimeFormat += " hh:mm tt";
            }

            return DateTime.Now.Date.ToString(dateTimeFormat);
        }

        protected double ConvertBytesToGigabytes(long bytes)
        {
            double gigabytes = bytes * 9.31 * Math.Pow(10, -10);
            return gigabytes;
        }
    }
}