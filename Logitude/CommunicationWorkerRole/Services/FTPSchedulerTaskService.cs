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
        private SchedulerDetails schedulerDetails { get; set; }
        private FTPService ftpService { get; set; }
        public FTPSchedulerTaskService(SchedulerDetails theSchedulerDetails)
        {
            schedulerDetails = theSchedulerDetails;
            ftpService = new FTPService(schedulerDetails.FTPDetails.Host, schedulerDetails.FTPDetails.UserName, schedulerDetails.FTPDetails.Password);
        }

        public void ReadFTPFilesBySchedulerDetailsToAnalyzeQueue()
        {

            List<string> directoryFiles = FilterDirectoryFilesByFTPDetails(schedulerDetails);

            foreach (string fileName in directoryFiles)
            {
                DownloadFileToAnalyzeQueueAndDelete(schedulerDetails, ftpService, fileName);
            }
        }

        private void DownloadFileToAnalyzeQueueAndDelete(SchedulerDetails schedulerDetails, FTPService ftpService, string fileName)
        {
            string extention = Path.GetExtension(fileName);
            if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(extention))
            {
                string p_message = "";
                var filePath = fileName;
                if (!string.IsNullOrEmpty(schedulerDetails.FTPDetails.Folder) && !fileName.Contains(schedulerDetails.FTPDetails.Folder))
                {
                    filePath = schedulerDetails.FTPDetails.Folder + "/" + fileName;
                }

                byte[] fileData = ftpService.Download(filePath, out p_message);

                AddToAnalyzeQueu(fileName, fileData, schedulerDetails, schedulerDetails.Tenant);


                ftpService.Delete(filePath);
            }
        }

        public List<string> FilterDirectoryFilesByFTPDetails(SchedulerDetails schedulerDetails)
        {
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

        private void AddToAnalyzeQueu(string fileName, byte[] fileData, SchedulerDetails schedulerDetails, int tenant)
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
                    Tenant = tenant,
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
