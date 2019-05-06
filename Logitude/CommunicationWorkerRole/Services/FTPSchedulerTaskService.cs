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
using System.Transactions;

namespace CommunicationWorkerRole.Services
{
    public class FTPSchedulerTaskService
    {
        ////private SchedulerDetails schedulerDetails { get; set; }
        ////private FTPService ftpService { get; set; }
        //public FTPSchedulerTaskService()
        //{
        //    //schedulerDetails = theSchedulerDetails;
           
        //}

        public void ReadFTPFilesBySchedulerDetailsToAnalyzeQueue(SchedulerDetails schedulerDetails)
        {
            FTPService ftpService = new FTPService(schedulerDetails.FTPDetails.Host, schedulerDetails.FTPDetails.UserName, schedulerDetails.FTPDetails.Password);

            List<string> directoryFiles = GetFilteredDirectoryFileNamesByFTPDetails(schedulerDetails);

            foreach (string fileName in directoryFiles)
            {
                string extention = Path.GetExtension(fileName);
                if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(extention))
                {
                    byte[] fileData = DownloadFTPFile(schedulerDetails, ftpService, fileName);
                    AddToAnalyzeQueue(fileName, fileData, schedulerDetails);
                    DeleteFTPFile(schedulerDetails, ftpService, fileName);
                }
            }
        }

        public byte[] DownloadFTPFile(SchedulerDetails schedulerDetails, FTPService ftpService, string fileName)
        {
            string p_message;
            string filePath = GetFilePath(schedulerDetails, fileName, out p_message);
            byte[] fileData = ftpService.Download(filePath, out p_message);
            return fileData;
        }

        private void DeleteFTPFile(SchedulerDetails schedulerDetails, FTPService ftpService, string fileName)
        {
            string p_message;
            string filePath = GetFilePath(schedulerDetails, fileName, out p_message);
            ftpService.Delete(filePath);
        }


        //private void DownloadFileToAnalyzeQueueAndDelete(SchedulerDetails schedulerDetails, FTPService ftpService, string fileName)
        //{
        //    string extention = Path.GetExtension(fileName);
        //    if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(extention))
        //    {
        //        string p_message;
        //        string filePath = GetFilePath(schedulerDetails, fileName, out p_message);
        //        byte[] fileData = ftpService.Download(filePath, out p_message);
        //        AddToAnalyzeQueue(fileName, fileData, schedulerDetails);
        //        ftpService.Delete(filePath);
        //    }

        //    return fileData;
        //}

        private static string GetFilePath(SchedulerDetails schedulerDetails, string fileName, out string p_message)
        {
            p_message = "";
            string filePath = fileName;
            if (!string.IsNullOrEmpty(schedulerDetails.FTPDetails.Folder) && !fileName.Contains(schedulerDetails.FTPDetails.Folder))
            {
                filePath = schedulerDetails.FTPDetails.Folder + "/" + fileName;
            }

            return filePath;
        }

        public List<string> GetFilteredDirectoryFileNamesByFTPDetails(SchedulerDetails schedulerDetails)
        {
            FTPService ftpService = new FTPService(schedulerDetails.FTPDetails.Host, schedulerDetails.FTPDetails.UserName, schedulerDetails.FTPDetails.Password);

            List<string> directoryFiles = ftpService.DirectoryListSimple(schedulerDetails.FTPDetails.Folder).Where(f => !string.IsNullOrWhiteSpace(f) && !string.IsNullOrWhiteSpace(Path.GetExtension(f))).ToList();

            if (!string.IsNullOrWhiteSpace(schedulerDetails.FTPDetails.Extension))
            {
                directoryFiles = directoryFiles.Where(f => (!string.IsNullOrEmpty(f) &&
                Path.GetExtension(f).TrimStart('.')
                .Equals(schedulerDetails.FTPDetails.Extension, StringComparison.CurrentCultureIgnoreCase)))
                .ToList();
            }

            if (!string.IsNullOrWhiteSpace(schedulerDetails.FTPDetails.Prefix))
            {
                directoryFiles = directoryFiles.Where(f => (!string.IsNullOrEmpty(f) &&
                f.StartsWith(schedulerDetails.FTPDetails.Prefix, StringComparison.CurrentCultureIgnoreCase)))
                .ToList();
            }

            if (!string.IsNullOrWhiteSpace(schedulerDetails.FTPDetails.Suffix))
            {
                directoryFiles = directoryFiles.Where(f => (!string.IsNullOrEmpty(f) &&
                f.Replace(Path.GetExtension(f), "")
                .EndsWith(schedulerDetails.FTPDetails.Suffix, StringComparison.CurrentCultureIgnoreCase)))
                .ToList();
            }

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
                    FileSize = fileData.Length,
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
