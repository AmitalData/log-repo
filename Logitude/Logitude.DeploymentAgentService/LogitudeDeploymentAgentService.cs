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
            WriteToLogsFile("Deployment Agent Service Started");
            ServiceTimer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            ServiceTimer.Interval = ServiceIntervalInSeconds * 1000;
            ServiceTimer.Enabled = true;
        }

        protected override void OnStop()
        {
            WriteToLogsFile("Deployment Agent Service Stopped");
        }

        protected void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            Configurations configurations = ReadFTPConfigurationsFile();
            if(configurations != null)
            {
                if (IsFTPConfigurationsFileValid(configurations))
                {
                    if(!IsCurrentVersionLatest(configurations.CurrentPackage.Version))
                    {
                        StartDeploymentProcess(configurations);
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
                WriteToLogsFile("Cannot Read FTP Configurations File With Exception: " + exception.Message);
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

        protected bool IsCurrentVersionLatest(string packageVersion)
        {
            return (CurrentVersion?.ToLower() == packageVersion.ToLower());
        }

        protected void StartDeploymentProcess(Configurations configurations)
        {
            ServiceTimer.Enabled = false;
            string packageVersion = configurations.CurrentPackage.Version;
            string packageUrl = configurations.CurrentPackage.Url;

            WriteToLogsFile("Deployment Process For Version " + packageVersion + " Started");

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
                            bool renameIISFolderResult = RenameIISFolder(IISFolderName, IISFolderName + ".Old");
                            if (renameIISFolderResult)
                            {
                                bool renameIISTempFolderResult = RenameIISFolder(IISFolderName + ".Temp", IISFolderName);
                                if (renameIISTempFolderResult)
                                {
                                    UpdateCurrentVersion(packageVersion);
                                }
                            }
                        }
                    }
                }
            }

            ServiceTimer.Enabled = true;
        }

        protected bool CreateIISTempFolder()
        {
            WriteToLogsFile("Create IIS Temp Folder Started");
            
            bool result = false;

            try
            {
                string iisTempFolderUrl = @"C:\inetpub\wwwroot\" + IISFolderName + ".Temp";
                if (Directory.Exists(iisTempFolderUrl))
                {
                    DeleteDirectoryContents(iisTempFolderUrl);
                    DeleteDirectory(iisTempFolderUrl);
                }
                Directory.CreateDirectory(iisTempFolderUrl);
                
                result = true;
                WriteToLogsFile("IIS Temp Folder Created Successfully");
            }
            catch(Exception exception)
            {
                WriteToLogsFile("Cannot Create IIS Temp Folder With Exception: " + exception.Message);
            }

            return result;
        }

        protected bool DownloadPackageFromFTP(string packageUrl)
        {
            WriteToLogsFile("Download Package From FTP Started");

            bool isDownloadPackageFromFTPCompleted = false;
            bool result = false;

            try
            {
                WebClient webClient = new WebClient();
                string packageFileFtpUrl = FTPUrl + @"/" + packageUrl;
                webClient.Credentials = new NetworkCredential(FTPUsername, FTPPassword);

                webClient.DownloadFileCompleted += (sender, e) => {
                    isDownloadPackageFromFTPCompleted = true;
                };

                webClient.DownloadFileAsync(new Uri(packageFileFtpUrl), @"C:\inetpub\wwwroot\" + IISFolderName + @".Temp\" + Path.GetFileName(packageFileFtpUrl));
            }
            catch(Exception exception)
            {
                WriteToLogsFile("Cannot Download Package From FTP With Exception: " + exception.Message);
            }

            while (!isDownloadPackageFromFTPCompleted)
            {
                //Do something while package downloading
            }

            if (isDownloadPackageFromFTPCompleted)
            {
                result = true;
                WriteToLogsFile("Package Downloaded Successfully");
            }

            return result;
        }

        protected bool ExtractDownloadedPackage(string packageUrl)
        {
            WriteToLogsFile("Extract The Downloaded Package Started");

            bool result = false;

            try
            {
                string packageFileFtpUrl = FTPUrl + @"/" + packageUrl;
                ZipFile.ExtractToDirectory(@"C:\inetpub\wwwroot\" + IISFolderName + @".Temp\" + Path.GetFileName(packageFileFtpUrl), @"C:\inetpub\wwwroot\" + IISFolderName + ".Temp");

                result = true;
                WriteToLogsFile("The Downloaded Package Extracted Successfully");
            }
            catch(Exception exception)
            {
                WriteToLogsFile("Cannot Extract The Downloaded Package With Exception: " + exception.Message);
            }

            return result;
        }

        protected bool CopyConfigFilesToIISTempFolder()
        {
            WriteToLogsFile("Copy Config Files To IIS Temp Folder Started");

            bool result = false;

            try
            {
                string agentConfigFilesUrl = AppDomain.CurrentDomain.BaseDirectory + @"\ConfigFiles";
                string iisTempFolderPath = @"C:\inetpub\wwwroot\" + IISFolderName + ".Temp";
                Copy(agentConfigFilesUrl, iisTempFolderPath);

                result = true;
                WriteToLogsFile("Config Files Copied Successfully");
            }
            catch(Exception exception)
            {
                WriteToLogsFile("Cannot Copy Config Files To IIS Temp Folder With Exception: " + exception.Message);
            }

            return result;
        }

        protected bool RenameIISFolder(string sourceFolderName, string destinationFolderName)
        {
            WriteToLogsFile("Rename Folder " + sourceFolderName + " To " + destinationFolderName + " On IIS Started");

            bool result = false;

            string sourceUrl = @"C:\inetpub\wwwroot\" + sourceFolderName;
            string destinationUrl = @"C:\inetpub\wwwroot\" + destinationFolderName;

            try
            {
                if (destinationUrl.EndsWith(".Old") && Directory.Exists(destinationUrl))
                {
                    DeleteDirectoryContents(destinationUrl);
                    DeleteDirectory(destinationUrl);
                }
                Directory.Move(sourceUrl, destinationUrl);

                result = true;
                WriteToLogsFile("Folder " + sourceFolderName + " Renamed To " + destinationFolderName + " On IIS Successfully");
            }
            catch (Exception exception)
            {
                WriteToLogsFile("Cannot Rename Folder From " + sourceFolderName + " To " + destinationFolderName + " With Exception: " + exception.Message);
            }

            return result;
        }

        protected void UpdateCurrentVersion(string packageVersion)
        {
            CurrentVersion = packageVersion;
            WriteToLogsFile("Deployment Process For Version " + packageVersion + " Completed Successfully");
        }

        protected void Copy(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo sourceDirectoryinfo = new DirectoryInfo(sourceDirectory);
            DirectoryInfo targetDirectoryInfo = new DirectoryInfo(targetDirectory);
            CopyAll(sourceDirectoryinfo, targetDirectoryInfo);
        }

        protected void CopyAll(DirectoryInfo sourceDirectoryinfo, DirectoryInfo targetDirectoryInfo)
        {
            foreach (FileInfo fileInfo in sourceDirectoryinfo.GetFiles())
            {
                fileInfo.CopyTo(Path.Combine(targetDirectoryInfo.FullName, fileInfo.Name), true);
            }

            foreach (DirectoryInfo sourceSubDirectoryinfo in sourceDirectoryinfo.GetDirectories())
            {
                DirectoryInfo targetSubDirectoryinfo = targetDirectoryInfo.CreateSubdirectory(sourceSubDirectoryinfo.Name);
                CopyAll(sourceSubDirectoryinfo, targetSubDirectoryinfo);
            }
        }

        protected void DeleteDirectoryContents(string directoryUrl)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryUrl);

            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                fileInfo.Delete();
            }

            foreach (DirectoryInfo subDirectoryInfo in directoryInfo.GetDirectories())
            {
                DeleteDirectoryContents(subDirectoryInfo.FullName);
                subDirectoryInfo.Delete();
            }
        }

        protected void DeleteDirectory(string directoryUrl)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryUrl);
            directoryInfo.Delete();
        }

        protected void WriteToLogsFile(string log)
        {
            try
            {
                string logsFolderPath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
                if (!Directory.Exists(logsFolderPath))
                {
                    Directory.CreateDirectory(logsFolderPath);
                }
                string logsFilepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + GetCurrentDateTime(false).Replace('/', '_') + ".txt";
                if (!File.Exists(logsFilepath))
                {
                    using (StreamWriter streamWriter = File.CreateText(logsFilepath))
                    {
                        streamWriter.WriteLine(log.Replace("\n", " ").TrimEnd('.') + ". At " + GetCurrentDateTime(true));
                    }
                }
                else
                {
                    using (StreamWriter streamWriter = File.AppendText(logsFilepath))
                    {
                        streamWriter.WriteLine(log.Replace("\n", " ").TrimEnd('.') + ". At " + GetCurrentDateTime(true));
                    }
                }
            }
            catch (Exception)
            {
                //Error While Writing To Logs File
            }
        }

        protected string GetCurrentDateTime(bool isWithTime)
        {
            string dateTimeFormat = "dd/MM/yyyy";
            if (isWithTime)
            {
                dateTimeFormat += " hh:mm:ss tt";
            }

            return DateTime.Now.ToString(dateTimeFormat);
        }
    }
}