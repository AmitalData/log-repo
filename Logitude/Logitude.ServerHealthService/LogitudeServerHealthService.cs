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
        protected DateTime? LastEmailAlert = null;

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
                DriveInfo[] serverDrives = DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Fixed).ToArray();

                foreach (DriveInfo drive in serverDrives)
                {
                    if (drive.IsReady)
                    {
                        bool isDriveSpaceLow = IsDriveSpaceLow(drive);
                        bool shouldSendEmails = ShouldSendEmails();

                        if (isDriveSpaceLow && shouldSendEmails)
                        {
                            string driveLabelWithName = drive.VolumeLabel + " (" + drive.Name.Replace(@"\", String.Empty) + ")";
                            string warningMessageSubject = "Low Drive Space In " + Environment.MachineName;
                            string warningMessageBody = BuildWarningMessageBody(driveLabelWithName, drive.TotalSize, drive.TotalFreeSpace);

                            SendEmails(warningMessageSubject, warningMessageBody);
                            LastEmailAlert = DateTime.Now;
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
        
        protected string BuildWarningMessageBody(string driveLabelWithName, long driveTotalSize, long driveTotalFreeSpace)
        {
            string warningMessage = "Low Free Space Was Detected In Drive " + driveLabelWithName + " That In Server " + Environment.MachineName + ".\n";
            warningMessage += "Total Space: " + Math.Round(ConvertBytesToGigabytes(driveTotalSize), 2).ToString() + " GB.\n";
            warningMessage += "Total Free Space: " + Math.Round(ConvertBytesToGigabytes(driveTotalFreeSpace), 2).ToString() + " GB.\n";
            warningMessage += "\n" + "From Logitude Server Health Service.";

            return warningMessage;
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

        protected bool ShouldSendEmails()
        {
            if(LastEmailAlert == null)
            {
                return true;
            }
            else
            {
                if((DateTime.Now - LastEmailAlert.Value).TotalSeconds >= Settings.GeneralSettings.EmailAlertTimer.IntervalInSeconds)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        protected bool IsDriveSpaceLow(DriveInfo drive)
        {
            bool isDriveSpaceLow = false;
            long driveTotalSize = drive.TotalSize;
            long driveTotalFreeSpace = drive.TotalFreeSpace;

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
                Drive driveToCheck = Settings.DrivesSettings.Drives.Where(d => drive.Name.ToLower().Contains(d.Name.ToLower())).FirstOrDefault();
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
                            if (driveTotalFreeSpace < ConvertGigabytesToBytes(driveToCheck.MinimumFreeSpaceGB))
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