using System;
using System.IO;
using System.ServiceProcess;
using System.Timers;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Net;
using Logitude.ServerHealthService.Models;

namespace Logitude.ServerHealthService
{
    public partial class LogitudeServerHealthService : ServiceBase
    {
        protected Timer ServiceTimer = new Timer();
        protected Settings Settings;
        protected DateTime? LastEmailsAlert = null;

        public LogitudeServerHealthService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            WriteToLogsFile("Service Is Started");
            ReadServiceSettings();
            ServiceTimer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            ServiceTimer.Interval = Settings.GeneralSettings.DrivesCheckTimer.IntervalInSeconds * 1000;
            ServiceTimer.Enabled = true;
        }

        protected override void OnStop()
        {
            WriteToLogsFile("Service Is Stopped");
        }

        protected void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            try
            {
                DriveInfo[] drivesInfo = GetServerDrives();

                foreach (DriveInfo driveInfo in drivesInfo)
                {
                    if (driveInfo.IsReady)
                    {
                        bool isDriveFreeSpaceLow = IsDriveFreeSpaceLow(driveInfo);
                        bool isTimeForNextEmailsAlert = IsTimeForNextEmailsAlert();

                        if (isDriveFreeSpaceLow && isTimeForNextEmailsAlert)
                        {
                            string warningMessageSubject = BuildWarningMessageSubject();
                            string warningMessageBody = BuildWarningMessageBody(driveInfo);

                            SendEmails(warningMessageSubject, warningMessageBody);
                            UpdateLastEmailsAlert();
                        }
                    }
                }
            }
            catch(Exception exception)
            {
                WriteToLogsFile("Error While Checking Drives Spaces With Exception: " + exception.Message);
            }
        }

        protected void ReadServiceSettings()
        {
            try
            {
                string serviceSettingsXmlFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\Settings.xml";
                string settingsXmlString = File.ReadAllText(serviceSettingsXmlFilePath);
                Settings = settingsXmlString.ParseXML<Settings>();
            }
            catch (Exception exception)
            {
                WriteToLogsFile("Error While Reading Service Settings With Exception: " + exception.Message);
                this.Stop();
            }
        }

        protected string BuildWarningMessageSubject()
        {
            string serverName = GetServerName();
            string subject = "Low Drive Free Space In " + serverName;
            return subject;
        }

        protected string BuildWarningMessageBody(DriveInfo driveInfo)
        {
            string serverName = GetServerName();
            string driveLabelWithName = driveInfo.VolumeLabel + " (" + driveInfo.Name.Replace(@"\", String.Empty) + ")";
            double driveTotalSizeInGB = Math.Round(ConvertBytesToGigabytes(driveInfo.TotalSize), 2);
            double driveTotalFreeSpaceInGB = Math.Round(ConvertBytesToGigabytes(driveInfo.TotalFreeSpace), 2);

            string warningMessage = "Low Free Space Was Detected In Drive " + driveLabelWithName + " That In Server " + serverName + ".\n";
            warningMessage += "Total Space: " + driveTotalSizeInGB.ToString() + " GB.\n";
            warningMessage += "Total Free Space: " + driveTotalFreeSpaceInGB.ToString() + " GB.\n";
            warningMessage += "\n" + "From Logitude Server Health Service.";
            return warningMessage;
        }

        protected DriveInfo[] GetServerDrives()
        {
            return DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Fixed).ToArray();
        }

        protected string GetServerName()
        {
            return Environment.MachineName;
        }

        protected void SendEmails(string subject, string body)
        {
            try
            {
                System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient(Settings.EmailSettings.SmtpClient.Host);
                MailMessage mailMessage = new MailMessage();

                mailMessage.From = new MailAddress(Settings.EmailSettings.From.Address);
                foreach (var emailAddress in Settings.EmailSettings.To.Addresses.Split(',').ToList())
                {
                    mailMessage.To.Add(emailAddress);
                }
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                smtpClient.Port = Settings.EmailSettings.SmtpClient.Port;
                smtpClient.Credentials = new NetworkCredential(Settings.EmailSettings.SmtpClient.Username, Settings.EmailSettings.SmtpClient.Password);
                smtpClient.Send(mailMessage);

                WriteToLogsFile("Warning Message Sent Successfully To " + Settings.EmailSettings.To.Addresses);
            }
            catch (Exception exception)
            {
                WriteToLogsFile("Error While Sending Warning Message With Exception: " + exception.Message);
            }
        }

        protected void WriteToLogsFile(string log)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + GetCurrentDateTime(false).Replace('/', '_') + ".txt";
                if (!File.Exists(filepath))
                {
                    using (StreamWriter streamWriter = File.CreateText(filepath))
                    {
                        streamWriter.WriteLine(log.TrimEnd('.') + ". At " + GetCurrentDateTime(true));
                    }
                }
                else
                {
                    using (StreamWriter streamWriter = File.AppendText(filepath))
                    {
                        streamWriter.WriteLine(log.TrimEnd('.') + ". At " + GetCurrentDateTime(true));
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

            return DateTime.Now.ToString(dateTimeFormat);
        }

        protected double ConvertBytesToGigabytes(double bytes)
        {
            double gigabytes = bytes * 9.31 * Math.Pow(10, -10);
            return gigabytes;
        }

        protected double ConvertGigabytesToBytes(double gigabytes)
        {
            double bytes = gigabytes / (9.31 * Math.Pow(10, -10));
            return bytes;
        }
        
        protected bool IsTimeForNextEmailsAlert()
        {
            if(LastEmailsAlert == null)
            {
                return true;
            }
            else
            {
                if((DateTime.Now - LastEmailsAlert.Value).TotalSeconds >= Settings.GeneralSettings.EmailAlertTimer.IntervalInSeconds)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        protected void UpdateLastEmailsAlert()
        {
            LastEmailsAlert = DateTime.Now;
        }

        protected bool IsDriveFreeSpaceLow(DriveInfo driveInfo)
        {
            bool isDriveSpaceLow = false;
            long driveTotalSize = driveInfo.TotalSize;
            long driveTotalFreeSpace = driveInfo.TotalFreeSpace;

            if (Settings.DrivesSettings.Drives.Count == 0)
            {
                double percentageOfTotal = ((double)10 / 100) * driveTotalSize;
                if (driveTotalFreeSpace < percentageOfTotal)
                {
                    isDriveSpaceLow = true;
                }
            }
            else
            {
                Drive driveToCheck = Settings.DrivesSettings.Drives.Where(d => driveInfo.Name.ToLower().Contains(d.Name.ToLower())).FirstOrDefault();
                if (driveToCheck != null)
                {
                    if (driveToCheck.MinimumFreeSpacePercent != 0 || driveToCheck.MinimumFreeSpaceGB != 0)
                    {
                        if (driveToCheck.MinimumFreeSpacePercent != 0)
                        {
                            double percentageOfTotal = (driveToCheck.MinimumFreeSpacePercent / 100) * driveTotalSize;
                            if (driveTotalFreeSpace < percentageOfTotal)
                            {
                                isDriveSpaceLow = true;
                            }
                        }

                        if (driveToCheck.MinimumFreeSpaceGB != 0)
                        {
                            double minimumFreeSpaceInBytes = ConvertGigabytesToBytes(driveToCheck.MinimumFreeSpaceGB);
                            if (driveTotalFreeSpace < minimumFreeSpaceInBytes)
                            {
                                isDriveSpaceLow = true;
                            }
                        }
                    }
                }
            }

            return isDriveSpaceLow;
        }
    }
}