using Logitude.BL.InfrastructureModel.DataContracts;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.FTP;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CommunicationWorkerRole.Services
{
    public class SFTPSchedulerTaskService: FTPSchedulerTaskServiceBase
    {
     

        public void ReadSFTPFilesBySchedulerDetailsToAnalyzeQueue(SchedulerDetails schedulerDetails)
        {
            string p_status = "";
            string p_message = "";
            SFTPService sftpService = new SFTPService();

            sftpService.Logon(schedulerDetails.FTPDetails.Host, schedulerDetails.FTPDetails.UserName, schedulerDetails.FTPDetails.Password, "22", schedulerDetails.FTPDetails.Folder, out p_status, out p_message);

            if (p_status == "-1")
            {
                AddWarning("SFTP upload file failed: " + p_message);
                return;
            }


            List<string> directoryFiles = GetFilteredDirectoryFileNamesByFTPDetails(schedulerDetails, sftpService);

            this.DownloadedFilesCount = 0;
            this.FailedFilesCount = 0;

            foreach (string fileName in directoryFiles)
            {
                string extention = Path.GetExtension(fileName);
                if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(extention))
                {
                    string p_read_status = "";
                    byte[] fileData = DownloadFTPFile(schedulerDetails, sftpService, fileName, out p_read_status);
                    if (p_read_status != "-1")
                    {
                        AddToAnalyzeQueue(fileName, fileData, schedulerDetails);
                        DeleteFTPFile(schedulerDetails, sftpService, fileName);
                    }
                }
            }


            AddMessage(FTPLogBuilder.BuildLogLine(this.GetFilesDownloadingSummery()));

        }

        public byte[] DownloadFTPFile(SchedulerDetails schedulerDetails, SFTPService sftpService, string fileName, out string p_status)
        {
            string p_message;
          
             
            string filePath = GetFilePath(schedulerDetails, fileName, out p_message);
            byte[] fileData = sftpService.DownloadFile(fileName, out p_status, out p_message);
            if (p_status == "-1")
            {
                this.FailedFilesCount++;
            }
            else
            {
                this.DownloadedFilesCount++;
            }

            AddStatusMessage(p_message, p_status);

            return fileData;
        }

        public void DeleteFTPFile(SchedulerDetails schedulerDetails, SFTPService sftpService, string fileName)
        {
            string p_message;
            string p_status = "";
            string filePath = GetFilePath(schedulerDetails, fileName, out p_message);
            sftpService.DeleteFile(fileName, out p_status, out p_message);
            AddStatusMessage(p_message, p_status);
        }


       

        private string GetFilePath(SchedulerDetails schedulerDetails, string fileName, out string p_message)
        {
            p_message = "";
            string filePath = fileName;
            if (!string.IsNullOrEmpty(schedulerDetails.FTPDetails.Folder) && !fileName.Contains(schedulerDetails.FTPDetails.Folder))
            {
                filePath = schedulerDetails.FTPDetails.Folder + "/" + fileName;
            }

            return filePath;
        }

        public List<string> GetFilteredDirectoryFileNamesByFTPDetails(SchedulerDetails schedulerDetails, SFTPService sftpService)
        {
            List<string> directoryFiles = new List<string>();
            //SFTPService sftpService = new SFTPService();
            string p_message;
            string p_status = "";
            string pattern = "*";
            //if (!string.IsNullOrWhiteSpace(schedulerDetails.FTPDetails.Extension))
            //{
            //    pattern += "." + schedulerDetails.FTPDetails.Extension.TrimStart('.');
            //}

            //if (!string.IsNullOrWhiteSpace(schedulerDetails.FTPDetails.Prefix))
            //{
            //    pattern = schedulerDetails.FTPDetails.Prefix + pattern;
            //    //directoryFiles = directoryFiles.Where(f => (!string.IsNullOrEmpty(f) &&
            //    //f.StartsWith(schedulerDetails.FTPDetails.Prefix, StringComparison.CurrentCultureIgnoreCase)))
            //    //.ToList();
            //}

            //if (!string.IsNullOrWhiteSpace(schedulerDetails.FTPDetails.Suffix))
            //{

            //    pattern = pattern + schedulerDetails.FTPDetails.Suffix;
            //    //directoryFiles = directoryFiles.Where(f => (!string.IsNullOrEmpty(f) &&
            //    //f.Replace(Path.GetExtension(f), "")
            //    //.EndsWith(schedulerDetails.FTPDetails.Suffix, StringComparison.CurrentCultureIgnoreCase)))
            //    //.ToList();
            //}

            pattern = (!string.IsNullOrWhiteSpace(schedulerDetails.FTPDetails.Prefix) ? schedulerDetails.FTPDetails.Prefix : "") + "*" + (!string.IsNullOrWhiteSpace(schedulerDetails.FTPDetails.Suffix) ? schedulerDetails.FTPDetails.Suffix : "") +
                (!string.IsNullOrWhiteSpace(schedulerDetails.FTPDetails.Extension) ? "." + schedulerDetails.FTPDetails.Extension.TrimStart('.') : "");

                           directoryFiles = sftpService.DirList(pattern, true, false, out p_status, out p_message);
            AddStatusMessage(p_message, p_status);

            //.Where(f => !string.IsNullOrWhiteSpace(f) && !string.IsNullOrWhiteSpace(Path.GetExtension(f))).ToList();

            //if (!string.IsNullOrWhiteSpace(schedulerDetails.FTPDetails.Extension))
            //{
            //    directoryFiles = directoryFiles.Where(f => (!string.IsNullOrEmpty(f) &&
            //    Path.GetExtension(f).TrimStart('.')
            //    .Equals(schedulerDetails.FTPDetails.Extension, StringComparison.CurrentCultureIgnoreCase)))
            //    .ToList();
            //}

            //if (!string.IsNullOrWhiteSpace(schedulerDetails.FTPDetails.Prefix))
            //{
            //    directoryFiles = directoryFiles.Where(f => (!string.IsNullOrEmpty(f) &&
            //    f.StartsWith(schedulerDetails.FTPDetails.Prefix, StringComparison.CurrentCultureIgnoreCase)))
            //    .ToList();
            //}

            //if (!string.IsNullOrWhiteSpace(schedulerDetails.FTPDetails.Suffix))
            //{
            //    directoryFiles = directoryFiles.Where(f => (!string.IsNullOrEmpty(f) &&
            //    f.Replace(Path.GetExtension(f), "")
            //    .EndsWith(schedulerDetails.FTPDetails.Suffix, StringComparison.CurrentCultureIgnoreCase)))
            //    .ToList();
            //}

            return directoryFiles;
        }

        private void AddToAnalyzeQueue(string fileName, byte[] fileData, SchedulerDetails schedulerDetails)
        {
            if (schedulerDetails.FTPDetails.Subject == "fail test")
            {
                throw new Exception("failure testing!");
            }

            var createdate = TenantServerConfigration.GetCurrentDateTime(0);
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                fileName = fileName.Split('/')[fileName.Split('/').Length - 1].ToLower();
                AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();

                AnalyzeQueue analyzeQueue = new AnalyzeQueue()
                {
                    Subject = schedulerDetails.FTPDetails.Subject,
                    CreateDate = createdate,
                    From = schedulerDetails.FTPDetails.From,
                    Id = IdCounter.GetNumber("AnalyzeQueue", 0),
                    MessageBody = fileData,
                    Status = "W",
                    Retries = 0,
                    ConnectedToEntity = false,
                    ConnectedToTenant = true,
                    Tenant = schedulerDetails.Tenant,
                    FileSize = fileData != null ? fileData.Length : 0,
                    FileName = fileName,
                };

                analyzeQueue.SearchFields = analyzeQueue.From + ',' + analyzeQueue.Status;
                analyzeQueueReposiory.Add(analyzeQueue);
                analyzeQueueReposiory.SubmitChanges();

                scope.Complete();
            }
        }
    }
}

