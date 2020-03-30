using Logitude.DeploymentAgentService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Logitude.DeploymentAgentService
{
    public partial class LogitudeDeploymentAgentService : ServiceBase
    {
        protected long ServiceIntervalInSeconds = Convert.ToInt64(ConfigurationManager.AppSettings["ServiceIntervalInSeconds"]);
        protected string FTPUrl = ConfigurationManager.AppSettings["FTPUrl"];
        protected string FTPUsername = ConfigurationManager.AppSettings["FTPUsername"];
        protected string FTPPassword = ConfigurationManager.AppSettings["FTPPassword"];
        protected string FTPConfigurationsFileUrl = ConfigurationManager.AppSettings["FTPConfigurationsFileUrl"];
        protected string IISFolderName = ConfigurationManager.AppSettings["IISFolderName"];

        protected Timer ServiceTimer = new Timer();

        protected string CurrentVersion = null;

        public LogitudeDeploymentAgentService()
        {
            InitializeComponent();
        }
        
        protected override void OnStart(string[] args)
        {
            WriteToLogsFile("Deployment Agent Service Is Started");
            ServiceTimer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            ServiceTimer.Interval = ServiceIntervalInSeconds * 1000;
            ServiceTimer.Enabled = true;
        }

        protected override void OnStop()
        {
            WriteToLogsFile("Deployment Agent Service Is Stopped");
        }

        protected void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            Configurations configurations = ReadFTPConfigurationsFile();
            if(configurations != null)
            {
                if (IsFTPConfigurationsFileValid(configurations))
                {
                    if(CurrentVersion?.ToLower() != configurations.CurrentPackage.Version.ToLower())
                    {
                        string packageVersion = configurations.CurrentPackage.Version;
                        string packageUrl = configurations.CurrentPackage.Url;

                        bool createIISTempFolderResult = CreateIISTempFolder();
                        if (createIISTempFolderResult)
                        {
                            bool downloadPackageFromFTPResult = DownloadPackageFromFTP(packageUrl);
                            if (downloadPackageFromFTPResult)
                            {
                                bool extractDownloadedPackageResult = ExtractDownloadedPackage(packageUrl);
                                if (extractDownloadedPackageResult)
                                {
                                    bool copyConfigFilesToIISTempFolderResult = CopyConfigFilesToIISTempFolder();
                                    if (copyConfigFilesToIISTempFolderResult)
                                    {
                                        UpdateCurrentVersion(packageVersion);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        protected Configurations ReadFTPConfigurationsFile()
        {
            Configurations configurations = null;

            try
            {
                WebClient webClient = new WebClient();
                string configurationsFileUrl = FTPUrl + @"/" + FTPConfigurationsFileUrl;
                webClient.Credentials = new NetworkCredential(FTPUsername, FTPPassword);

                string configurationsXmlString = webClient.DownloadString(configurationsFileUrl);
                configurations = configurationsXmlString.ParseXML<Configurations>();
            }
            catch (Exception exception)
            {
                WriteToLogsFile("Cannot Read Configurations File From FTP Server With Exception: " + exception.Message);
            }

            return configurations;
        }

        protected bool IsFTPConfigurationsFileValid(Configurations configurations)
        {
            bool isValid = true;

            if(configurations.CurrentPackage.Version == null)
            {
                isValid = false;
                WriteToLogsFile("Version Is Not Defined In FTP Configurations File");
            }
            if(configurations.CurrentPackage.Url == null)
            {
                isValid = false;
                WriteToLogsFile("Package Url Is Not Defined In FTP Configurations File");
            }

            return isValid;
        }

        protected bool CreateIISTempFolder()
        {
            bool result = false;

            try
            {
                string iisTempFolderUrl = @"C:\inetpub\wwwroot\" + IISFolderName + ".Temp";
                if (!Directory.Exists(iisTempFolderUrl))
                {
                    Directory.CreateDirectory(iisTempFolderUrl);
                }
                else
                {
                    Directory.Delete(iisTempFolderUrl, true);
                    Directory.CreateDirectory(iisTempFolderUrl);
                }

                result = true;
            }
            catch(Exception exception)
            {
                WriteToLogsFile("Cannot Create IIS Temp Folder With Exception: " + exception.Message);
            }

            return result;
        }

        protected bool DownloadPackageFromFTP(string packageUrl)
        {
            bool result = false;

            try
            {
                WebClient webClient = new WebClient();
                string packageFileFtpUrl = FTPUrl + @"/" + packageUrl;
                webClient.Credentials = new NetworkCredential(FTPUsername, FTPPassword);
                webClient.DownloadFile(packageFileFtpUrl, @"C:\inetpub\wwwroot\" + IISFolderName + @".Temp\" + Path.GetFileName(packageFileFtpUrl));

                result = true;
            }
            catch(Exception exception)
            {
                WriteToLogsFile("Cannot Download Package From FTP With Exception: " + exception.Message);
            }

            return result;
        }

        protected bool ExtractDownloadedPackage(string packageUrl)
        {
            bool result = false;

            try
            {
                string packageFileFtpUrl = FTPUrl + @"/" + packageUrl;
                ZipFile.ExtractToDirectory(@"C:\inetpub\wwwroot\" + IISFolderName + @".Temp\" + Path.GetFileName(packageFileFtpUrl), @"C:\inetpub\wwwroot\" + IISFolderName + ".Temp");

                result = true;
            }
            catch(Exception exception)
            {
                WriteToLogsFile("Cannot Extract The Downloaded Package With Exception: " + exception.Message);
            }

            return result;
        }

        protected bool CopyConfigFilesToIISTempFolder()
        {
            bool result = false;

            try
            {
                string agentConfigFilesUrl = AppDomain.CurrentDomain.BaseDirectory + @"\ConfigFiles";
                string iisTempFolderPath = @"C:\inetpub\wwwroot\" + IISFolderName + ".Temp";
                Copy(agentConfigFilesUrl, iisTempFolderPath);

                result = true;
            }
            catch(Exception exception)
            {
                WriteToLogsFile("Cannot Copy Agent Config Files To IIS Temp Folder With Exception: " + exception.Message);
            }

            return result;
        }

        protected void UpdateCurrentVersion(string packageVersion)
        {
            CurrentVersion = packageVersion;
        }










        protected void Copy(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);
            CopyAll(diSource, diTarget);
        }

        protected void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
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
    }
}
