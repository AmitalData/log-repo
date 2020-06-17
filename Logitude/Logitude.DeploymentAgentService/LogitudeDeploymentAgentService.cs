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
        protected string AgentServiceId = ConfigurationManager.AppSettings["AgentServiceId"];
        protected string InstanceName = ConfigurationManager.AppSettings["InstanceName"];
        protected long ServiceIntervalInSeconds = Convert.ToInt64(ConfigurationManager.AppSettings["ServiceIntervalInSeconds"]);
        protected bool RunDBMigrations = (ConfigurationManager.AppSettings["RunDBMigrations"] == "true");

        protected string InProgressDeploymentStatusCode = "I";
        protected string CompletedDeploymentStatusCode = "C";
        protected string ErrorDeploymentStatusCode = "E";

        protected Timer ServiceTimer = new Timer();

        protected Agent AgentInfo = null;
        protected string InstanceFolderPath = null;

        public LogitudeDeploymentAgentService()
        {
            InitializeComponent();
        }
        
        protected override void OnStart(string[] args)
        {
            AddAgentLog("Deployment Agent Service Started");
            ServiceTimer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            ServiceTimer.Interval = ServiceIntervalInSeconds * 1000;
            ServiceTimer.Enabled = true;
        }

        protected override void OnStop()
        {
            AddAgentLog("Deployment Agent Service Stopped");
        }

        protected void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            DisableAgentServiceTimer();
            GetAgentInfo();
            StartDeploymentProcess();
            EnableAgentServiceTimer();
        }

        protected void GetAgentInfo()
        {
            try
            {
                DeploymentApiHttpRequest httpRequest = new DeploymentApiHttpRequest("/Agents/GetAgentDetails/" + AgentServiceId, HttpRequestType.NoBodyRequestType.Get);
                AgentInfo = httpRequest.GetResponse<Agent>();
            }
            catch (Exception exception)
            {
                AgentInfo = null;
                AddAgentLog("Cannot Get Agent Service Information With Exception: " + exception.Message, true);
            }
        }

        protected void StartDeploymentProcess()
        {
            if (AgentInfo != null)
            {
                if (!IsAgentCurrentVersionLatest())
                {
                    bool deploySuccess = false;
                    string agentServiceType = AgentInfo.ServiceType.Code.ToLower();

                    UpdateDeploymentStatus(InProgressDeploymentStatusCode);

                    if (agentServiceType == "web")
                    {
                        deploySuccess = DeployPackage();
                    }
                    else if (agentServiceType == "wr")
                    {
                        deploySuccess = DeployPackageForWorkerRole();
                    }

                    if (deploySuccess)
                    {
                        if (RunDBMigrations)
                        {
                            bool runDBMigrationsTool = RunDBMigrationsTool();
                            if (runDBMigrationsTool)
                            {
                                UpdateCurrentVersion();
                                AddAgentLog("Deployment Process For Version " + AgentInfo.NewVersion + " Was Completed");
                                UpdateDeploymentStatus(CompletedDeploymentStatusCode);
                            }
                        }
                        else
                        {
                            UpdateCurrentVersion();
                            AddAgentLog("Deployment Process For Version " + AgentInfo.NewVersion + " Was Completed");
                            UpdateDeploymentStatus(CompletedDeploymentStatusCode);
                        }
                    }
                    else
                    {
                        UpdateDeploymentStatus(ErrorDeploymentStatusCode);
                    }
                }
            }
        }

        protected bool IsAgentCurrentVersionLatest()
        {
            return (AgentInfo.CurrentVersion == AgentInfo.NewVersion) || AgentInfo.LastReleaseId == null;
        }

        protected bool DeployPackage()
        {
            bool deploySuccess = false;
            int packageVersion = AgentInfo.NewVersion;
            string packageUrl = AgentInfo.Artifact.FolderName + "/" + AgentInfo.Artifact.FileName;

            AddAgentLog("Deployment Process For Version " + packageVersion + " Started");

            bool getInstanceFolderPathResult = GetInstanceFolderPath();
            if (getInstanceFolderPathResult)
            {
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
                                        deploySuccess = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return deploySuccess;
        }

        protected bool DeployPackageForWorkerRole()
        {
            bool deploySuccess = false;
            bool stopWorkerRoleServiceResult = StopWorkerRoleService();
            if (stopWorkerRoleServiceResult)
            {
                deploySuccess = DeployPackage();
                StartWorkerRoleService();
            }

            return deploySuccess;
        }

        protected void DisableAgentServiceTimer()
        {
            ServiceTimer.Enabled = false;
        }

        protected void EnableAgentServiceTimer()
        {
            ServiceTimer.Enabled = true;
        }

        protected bool GetInstanceFolderPath()
        {
            AddAgentLog("Get Instance Folder Path Started");

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

                if (String.IsNullOrEmpty(InstanceFolderPath))
                {
                    AddAgentLog("Cannot Get Instance Folder Path", true);
                }
            }
            catch (Exception exception)
            {
                InstanceFolderPath = null;
                AddAgentLog("Cannot Get Instance Folder Path With Exception: " + exception.Message, true);
            }

            return !String.IsNullOrEmpty(InstanceFolderPath);
        }

        protected bool CreateTempFolder()
        {
            AddAgentLog("Create Temp Folder Started");
            
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
                AddAgentLog("Temp Folder Was Created");
            }
            catch(Exception exception)
            {
                AddAgentLog("Cannot Create Temp Folder With Exception: " + exception.Message, true);
            }

            return result;
        }

        protected bool DownloadPackageFromFTP(string packageUrl)
        {
            AddAgentLog("Download Package From FTP Started");

            bool isDownloadPackageFromFTPCompleted = false;
            bool result = false;

            try
            {
                WebClient webClient = new WebClient();
                string packageFileFtpUrl = AgentInfo.Artifact.FtpUrl.TrimEnd('/') + "/" + packageUrl;
                webClient.Credentials = new NetworkCredential(AgentInfo.Artifact.FtpUsername, AgentInfo.Artifact.FtpPassword);

                webClient.DownloadFileCompleted += (sender, e) => {
                    isDownloadPackageFromFTPCompleted = true;
                };

                webClient.DownloadFileAsync(new Uri(packageFileFtpUrl), InstanceFolderPath + @".Temp\" + Path.GetFileName(packageFileFtpUrl));
            }
            catch(Exception exception)
            {
                AddAgentLog("Cannot Download Package From FTP With Exception: " + exception.Message, true);
            }

            while (!isDownloadPackageFromFTPCompleted)
            {
                //Do something while package downloading
            }

            if (isDownloadPackageFromFTPCompleted)
            {
                result = true;
                AddAgentLog("Package Was Downloaded From FTP");
            }

            return result;
        }

        protected bool ExtractDownloadedPackage(string packageUrl)
        {
            AddAgentLog("Extract The Downloaded Package Started");

            bool result = false;

            try
            {
                string packageFileFtpUrl = AgentInfo.Artifact.FtpUrl.TrimEnd('/') + "/" + packageUrl;
                ZipFile.ExtractToDirectory(InstanceFolderPath + @".Temp\" + Path.GetFileName(packageFileFtpUrl), InstanceFolderPath + ".Temp");

                result = true;
                AddAgentLog("The Downloaded Package Was Extracted");
            }
            catch(Exception exception)
            {
                AddAgentLog("Cannot Extract The Downloaded Package With Exception: " + exception.Message, true);
            }

            return result;
        }

        protected bool CopyConfigFilesToTempFolder()
        {
            AddAgentLog("Copy Config Files To Temp Folder Started");

            bool result = false;

            try
            {
                string webConfigFilesUrl = AppDomain.CurrentDomain.BaseDirectory + @"\ConfigFiles\Web";
                string dbMigrationsConfigFilesUrl = AppDomain.CurrentDomain.BaseDirectory + @"\ConfigFiles\DBMigrations";
                string tempFolderPath = InstanceFolderPath + ".Temp";
                Copy(webConfigFilesUrl, tempFolderPath);
                Copy(dbMigrationsConfigFilesUrl, (tempFolderPath + @"\Logitude.DBMigrations"));

                result = true;
                AddAgentLog("Config Files Was Copied To Temp Folder");
            }
            catch(Exception exception)
            {
                AddAgentLog("Cannot Copy Config Files To Temp Folder With Exception: " + exception.Message, true);
            }

            return result;
        }
        
        protected bool RenameInstanceFolder(string sourcePattern, string destinationPattern)
        {
            string sourceUrl = InstanceFolderPath + sourcePattern;
            string destinationUrl = InstanceFolderPath + destinationPattern;

            AddAgentLog("Rename Folder " + sourceUrl + " To " + destinationUrl + " Started");

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
                AddAgentLog("Folder " + sourceUrl + " Was Renamed To " + destinationUrl);
            }
            catch (Exception exception)
            {
                AddAgentLog("Cannot Rename Folder " + sourceUrl + " To " + destinationUrl + " With Exception: " + exception.Message, true);
            }

            return result;
        }

        protected void UpdateCurrentVersion()
        {
            try
            {
                UpdateAgentCurrentVersion updateAgentCurrentVersion = new UpdateAgentCurrentVersion()
                {
                    CurrentVersion = AgentInfo.NewVersion
                };
                DeploymentApiHttpRequest httpRequest = new DeploymentApiHttpRequest("/Agents/UpdateCurrentVersion/" + AgentServiceId, updateAgentCurrentVersion, HttpRequestType.BodyRequestType.Put);
                httpRequest.GetResponse();

                AddAgentLog("Agent Current Version Was Updated To " + AgentInfo.NewVersion);
            }
            catch (Exception exception)
            {
                AddAgentLog("Cannot Update Agent Current Version With Exception: " + exception.Message, true);
            }
        }

        protected void UpdateDeploymentStatus(string deploymentStatusCode)
        {
            try
            {
                UpdateAgentDeploymentStatus updateAgentDeploymentStatus = new UpdateAgentDeploymentStatus()
                {
                    DeploymentStatusCode = deploymentStatusCode
                };
                DeploymentApiHttpRequest httpRequest = new DeploymentApiHttpRequest("/Agents/UpdateDeploymentStatus/" + AgentServiceId, updateAgentDeploymentStatus, HttpRequestType.BodyRequestType.Put);
                Agent updatedAgent = httpRequest.GetResponse<Agent>();

                AddAgentLog("Agent Deployment Status Was Updated To " + updatedAgent.DeploymentStatus.Name);
            }
            catch (Exception exception)
            {
                AddAgentLog("Cannot Update Agent Deployment Status With Exception: " + exception.Message, true);
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
            AddAgentLog("Stop " + InstanceName + " Worker Role Started");

            bool result = false;

            try
            {
                ServiceController serviceController = new ServiceController(InstanceName);

                if (!((serviceController.Status.Equals(ServiceControllerStatus.Stopped)) || (serviceController.Status.Equals(ServiceControllerStatus.StopPending))))
                {
                    serviceController.Stop();
                }

                result = true;
                AddAgentLog(InstanceName + " Worker Role Was Stopped");
            }
            catch (Exception exception)
            {
                AddAgentLog("Cannot Stop " + InstanceName + " Worker Role With Exception: " + exception.Message, true);
            }

            return result;
        }

        protected bool StartWorkerRoleService()
        {
            AddAgentLog("Start " + InstanceName + " Worker Role Started");

            bool result = false;

            try
            {
                ServiceController serviceController = new ServiceController(InstanceName);

                if ((serviceController.Status.Equals(ServiceControllerStatus.Stopped)) || (serviceController.Status.Equals(ServiceControllerStatus.StopPending)))
                {
                    serviceController.Start();
                }

                result = true;
                AddAgentLog(InstanceName + " Worker Role Was Started");
            }
            catch (Exception exception)
            {
                AddAgentLog("Cannot Start " + InstanceName + " Worker Role With Exception: " + exception.Message, true);
            }

            return result;
        }

        protected bool RunDBMigrationsTool()
        {
            AddAgentLog("Running Database Migrations Tool Started");

            bool result = false;

            string dbMigrationsToolDirectoryPath = InstanceFolderPath + @"\Logitude.DBMigrations";
            string filePath = dbMigrationsToolDirectoryPath + @"\Logitude.DBMigrations.exe";
            string arguments = string.Format(@"{0} {1} {2} {3} {4}", "-root", ("\"" + dbMigrationsToolDirectoryPath + "\""), "-ignoresettingscheck", "-deployment", "-exe");
            string workingDirectory = dbMigrationsToolDirectoryPath;

            ProcessHelper processHelper = new ProcessHelper(filePath, arguments, workingDirectory, null);
            ProcessRunResult processRunResult = processHelper.RunProcess();
            if (processRunResult.ProcessResult != null)
            {
                AddAgentLog("Database Migrations Tool Finished " + (processRunResult.ProcessResult.ExitCode == 0 ? "Successfully" : "With Errors"));

                if (!String.IsNullOrEmpty(processRunResult.ProcessResult.OutputDataReceived))
                {
                    AddAgentLog("Database Migrations Tool Returned Output Data:\n" + processRunResult.ProcessResult.OutputDataReceived);
                }

                if (!String.IsNullOrEmpty(processRunResult.ProcessResult.ErrorDataReceived))
                {
                    AddAgentLog("Database Migrations Tool Returned Output Error:\n" + processRunResult.ProcessResult.ErrorDataReceived);
                }

                result = true;
            }
            else
            {
                AddAgentLog("Exception While Running Database Migrations Tool: " + processRunResult.ExceptionMessage, true);
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
                        streamWriter.WriteLine(log.TrimEnd('.') + ". At " + GetCurrentDateTime(true));
                    }
                }
                else
                {
                    using (StreamWriter streamWriter = File.AppendText(logsFilepath))
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

        protected void AddAgentLog(string logMessage, bool isException = false)
        {
            if(AgentInfo != null)
            {
                try
                {
                    SaveAgentLog saveAgentLog = new SaveAgentLog()
                    {
                        AgentId = AgentInfo.Id,
                        LogMessage = logMessage,
                        IsException = isException,
                        LogDatetime = DateTime.Now,
                        ReleaseId = AgentInfo.LastReleaseId
                    };
                    DeploymentApiHttpRequest httpRequest = new DeploymentApiHttpRequest("/AgentLogs", saveAgentLog, HttpRequestType.BodyRequestType.Post);
                    httpRequest.GetResponse();
                }
                catch (Exception exception)
                {
                    WriteToLogsFile(exception.ToString());
                }
            }
            else
            {
                WriteToLogsFile(logMessage);
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