using Logitude.DeploymentAgentService.Models;
using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Management;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Timers;

namespace Logitude.DeploymentAgentService
{
    public partial class LogitudeDeploymentAgentService : ServiceBase
    {
        protected long ServiceIntervalInSeconds = Convert.ToInt64(ConfigurationManager.AppSettings["ServiceIntervalInSeconds"]);
        protected string AgentServiceId = ConfigurationManager.AppSettings["AgentServiceId"];
        protected string InstanceName = ConfigurationManager.AppSettings["InstanceName"];

        protected Timer ServiceTimer = new Timer();

        protected Agent AgentInfo = null;
        protected string InstanceFolderPath = null;

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
            GetAgentInfo();
            GetInstanceFolderPath();
            StartDeploymentProcess();
        }

        protected void GetAgentInfo()
        {
            try
            {
                DeploymentApiHttpRequest<Agent> httpRequest = new DeploymentApiHttpRequest<Agent>("/Agents/" + AgentServiceId, HttpRequestType.NoBodyRequestType.Get);
                AgentInfo = httpRequest.GetResponse();
            }
            catch (Exception exception)
            {
                AgentInfo = null;
                WriteToLogsFile("Cannot Get Agent Service Information With Exception: " + exception.Message);
            }
        }

        protected void GetInstanceFolderPath()
        {
            try
            {
                string agentServiceType = AgentInfo?.ServiceType.Code.ToLower();

                if (agentServiceType == "web")
                {
                    InstanceFolderPath = GetWebApplicationFolderPath();
                }
                else if (agentServiceType == "wr")
                {
                    InstanceFolderPath = GetWorkerRoleFolderPath();
                }
            }
            catch (Exception exception)
            {
                InstanceFolderPath = null;
                WriteToLogsFile("Cannot Get Instance Folder Path With Exception: " + exception.Message);
            }

            if (String.IsNullOrEmpty(InstanceFolderPath))
            {
                WriteToLogsFile("Cannot Get Instance Folder Path");
            }
        }

        protected void StartDeploymentProcess()
        {
            if (AgentInfo != null && !String.IsNullOrEmpty(InstanceFolderPath))
            {
                if (!IsAgentCurrentVersionLatest())
                {
                    DisableAgentServiceTimer();

                    string agentServiceType = AgentInfo.ServiceType.Code.ToLower();

                    if (agentServiceType == "web")
                    {
                        DeployPackage();
                    }
                    else if (agentServiceType == "wr")
                    {
                        DeployPackageForWorkerRole();
                    }

                    EnableAgentServiceTimer();
                }
            }
        }

        protected bool IsAgentCurrentVersionLatest()
        {
            return (AgentInfo.CurrentVersion == AgentInfo.NewVersion);
        }

        protected void DeployPackage()
        {
            int packageVersion = AgentInfo.NewVersion;
            string packageUrl = AgentInfo.NewVersionArtifact.FolderName + "/" + AgentInfo.NewVersionArtifact.FileName;

            WriteToLogsFile("Deployment Process For Version " + packageVersion.ToString() + " Started");

            bool createTempFolderResult = CreateTempFolder();
            if (createTempFolderResult)
            {
                bool downloadPackageFromFTPResult = DownloadPackageFromFTP(packageUrl);
                if (downloadPackageFromFTPResult)
                {
                    bool extractDownloadedPackageResult = ExtractDownloadedPackage(packageUrl);
                    if (extractDownloadedPackageResult)
                    {
                        bool copyConfigFilesToTempFolderResult = CopyConfigFilesToTempFolder();
                        if (copyConfigFilesToTempFolderResult)
                        {
                            bool renameOriginalFolderResult = RenameInstanceFolder(null, ".Old");
                            if (renameOriginalFolderResult)
                            {
                                bool renameTempFolderResult = RenameInstanceFolder(".Temp", null);
                                if (renameTempFolderResult)
                                {
                                    UpdateCurrentVersion();
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void DeployPackageForWorkerRole()
        {
            bool stopWorkerRoleServiceResult = StopWorkerRoleService();
            if (stopWorkerRoleServiceResult)
            {
                DeployPackage();
                StartWorkerRoleService();
            }
        }

        protected void DisableAgentServiceTimer()
        {
            ServiceTimer.Enabled = false;
        }

        protected void EnableAgentServiceTimer()
        {
            ServiceTimer.Enabled = true;
        }

        protected bool CreateTempFolder()
        {
            WriteToLogsFile("Create Temp Folder Started");
            
            bool result = false;

            try
            {
                string tempFolderUrl = InstanceFolderPath + ".Temp";
                if (Directory.Exists(tempFolderUrl))
                {
                    DeleteDirectoryContents(tempFolderUrl);
                    DeleteDirectory(tempFolderUrl);
                }
                Directory.CreateDirectory(tempFolderUrl);
                
                result = true;
                WriteToLogsFile("Temp Folder Created Successfully");
            }
            catch(Exception exception)
            {
                WriteToLogsFile("Cannot Create Temp Folder With Exception: " + exception.Message);
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
                string packageFileFtpUrl = AgentInfo.NewVersionArtifact.FtpUrl.TrimEnd('/') + "/" + packageUrl;
                webClient.Credentials = new NetworkCredential(AgentInfo.NewVersionArtifact.FtpUsername, AgentInfo.NewVersionArtifact.FtpPassword);

                webClient.DownloadFileCompleted += (sender, e) => {
                    isDownloadPackageFromFTPCompleted = true;
                };

                webClient.DownloadFileAsync(new Uri(packageFileFtpUrl), InstanceFolderPath + @".Temp\" + Path.GetFileName(packageFileFtpUrl));
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
                string packageFileFtpUrl = AgentInfo.NewVersionArtifact.FtpUrl.TrimEnd('/') + "/" + packageUrl;
                ZipFile.ExtractToDirectory(InstanceFolderPath + @".Temp\" + Path.GetFileName(packageFileFtpUrl), InstanceFolderPath + ".Temp");

                result = true;
                WriteToLogsFile("The Downloaded Package Extracted Successfully");
            }
            catch(Exception exception)
            {
                WriteToLogsFile("Cannot Extract The Downloaded Package With Exception: " + exception.Message);
            }

            return result;
        }

        protected bool CopyConfigFilesToTempFolder()
        {
            WriteToLogsFile("Copy Config Files To Temp Folder Started");

            bool result = false;

            try
            {
                string agentConfigFilesUrl = AppDomain.CurrentDomain.BaseDirectory + @"\ConfigFiles";
                string tempFolderPath = InstanceFolderPath + ".Temp";
                Copy(agentConfigFilesUrl, tempFolderPath);

                result = true;
                WriteToLogsFile("Config Files Copied Successfully");
            }
            catch(Exception exception)
            {
                WriteToLogsFile("Cannot Copy Config Files To Temp Folder With Exception: " + exception.Message);
            }

            return result;
        }
        
        protected bool RenameInstanceFolder(string sourcePattern, string destinationPattern)
        {
            string sourceUrl = InstanceFolderPath + sourcePattern;
            string destinationUrl = InstanceFolderPath + destinationPattern;

            WriteToLogsFile("Rename Folder " + sourceUrl + " To " + destinationUrl + " Started");

            bool result = false;

            try
            {
                if (destinationUrl.EndsWith(".Old") && Directory.Exists(destinationUrl))
                {
                    DeleteDirectoryContents(destinationUrl);
                    DeleteDirectory(destinationUrl);
                }
                Directory.Move(sourceUrl, destinationUrl);

                result = true;
                WriteToLogsFile("Folder " + sourceUrl + " Renamed To " + destinationUrl + " Successfully");
            }
            catch (Exception exception)
            {
                WriteToLogsFile("Cannot Rename Folder From " + sourceUrl + " To " + destinationUrl + " With Exception: " + exception.Message);
            }

            return result;
        }

        protected void UpdateCurrentVersion()
        {
            try
            {
                SaveAgent saveAgent = new SaveAgent()
                {
                    ServiceTypeCode = AgentInfo.ServiceType.Code,
                    CurrentVersion = AgentInfo.NewVersion,
                    NewVersion = AgentInfo.NewVersion,
                    NewVersionArtifactId = AgentInfo.NewVersionArtifact.Id
                };
                DeploymentApiHttpRequest<Agent> httpRequest = new DeploymentApiHttpRequest<Agent>("/Agents/" + AgentInfo.Id, saveAgent, HttpRequestType.BodyRequestType.Put);
                httpRequest.GetResponse();

                WriteToLogsFile("Deployment Process For Version " + AgentInfo.NewVersion + " Completed Successfully");
            }
            catch (Exception exception)
            {
                WriteToLogsFile("Cannot Get Agent Service Information With Exception: " + exception.Message);
            }
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

        protected string GetWebApplicationFolderPath()
        {
            ServerManager serverManager = new ServerManager();
            string webApplicationPath = serverManager.Sites[InstanceName].Applications["/"].VirtualDirectories["/"].PhysicalPath;
            return webApplicationPath;
        }

        protected string GetWorkerRoleFolderPath()
        {
            string servicePath = null;

            ManagementClass managementClass = new ManagementClass("Win32_Service");
            foreach (ManagementObject managementObject in managementClass.GetInstances())
            {
                string serviceName = managementObject.GetPropertyValue("Name").ToString();
                if (serviceName == InstanceName)
                {
                    servicePath = Path.GetDirectoryName(managementObject.GetPropertyValue("PathName").ToString().Trim('"'));
                    break;
                }
            }

            return servicePath;
        }

        protected bool StopWorkerRoleService()
        {
            WriteToLogsFile("Stop " + InstanceName + " Worker Role Started");

            bool result = false;

            try
            {
                ServiceController serviceController = new ServiceController(InstanceName);

                if (!((serviceController.Status.Equals(ServiceControllerStatus.Stopped)) || (serviceController.Status.Equals(ServiceControllerStatus.StopPending))))
                {
                    serviceController.Stop();
                }

                result = true;
                WriteToLogsFile(InstanceName + " Worker Role Stopped Successfully");
            }
            catch (Exception exception)
            {
                WriteToLogsFile("Cannot Stop " + InstanceName + " Worker Role With Exception: " + exception.Message);
            }

            return result;
        }

        protected bool StartWorkerRoleService()
        {
            WriteToLogsFile("Start " + InstanceName + " Worker Role Started");

            bool result = false;

            try
            {
                ServiceController serviceController = new ServiceController(InstanceName);

                if ((serviceController.Status.Equals(ServiceControllerStatus.Stopped)) || (serviceController.Status.Equals(ServiceControllerStatus.StopPending)))
                {
                    serviceController.Start();
                }

                result = true;
                WriteToLogsFile(InstanceName + " Worker Role Started Successfully");
            }
            catch (Exception exception)
            {
                WriteToLogsFile("Cannot Start " + InstanceName + " Worker Role With Exception: " + exception.Message);
            }

            return result;
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