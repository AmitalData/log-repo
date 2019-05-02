using CommunicationWorkerRole.Services;
using Logitude.BL.InfrastructureModel.DataContracts;
using Logitude.Server.Tools.FTP;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace Logitude.IntegrationTests.FTP
{
    [TestClass]
    public class FTPSchedulerTaskTest
    {
        [TestMethod]
        public void Upload_DownloadFTPFiles_Prefix_Test()
        {


            SchedulerDetails schedulerDetails = new SchedulerDetails()
            {
                Tenant = 1,
                FTPDetails = new FTPSchedulerDetails()
                {
                    Folder = "DocumentsBackup",
                    From = "My FTP Server",
                    Host = "192.168.1.26",
                    Password = "!I123456",
                    Prefix = "test",
                    Subject = "test",
                    Suffix = "",
                    UserName = "Islam",
                }
            };

            //private static string filePath = HttpContext.Current.Server.MapPath(".") + "\\bin\\" + "exceptionslogfile.txt";
            FTPService ftpService = new FTPService(schedulerDetails.FTPDetails.Host, schedulerDetails.FTPDetails.UserName, schedulerDetails.FTPDetails.Password);
            string startupPath = Environment.CurrentDirectory.Replace(@"bin\Debug", "FTPFiles");
            //string filePath = HttpContext.Current.Server.MapPath(".") + "\\bin\\" + "exceptionslogfile.txt";
            //foreach (string file in Directory.EnumerateFiles(folderPath, "*.xml"))
            //{
            //    string contents = File.ReadAllText(file);
            //}

            //ftpService.Upload()


            FTPSchedulerTaskService fTPSchedulerTaskService = new FTPSchedulerTaskService();
            List<string> fileNames = fTPSchedulerTaskService.GetFilteredDirectoryFileNamesByFTPDetails(schedulerDetails);

            fTPSchedulerTaskService.ReadFTPFilesBySchedulerDetailsToAnalyzeQueue(schedulerDetails);

            //using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            //{
            //    AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();
            //    analyzeQueueReposiory.GetAnalyzeQueues(schedulerDetails.Tenant).wh
            //}
        }

        [TestMethod]
        public void DownloadFTPFiles_Prefix_Test()
        {

            SchedulerDetails schedulerDetails = new SchedulerDetails()
            {
                Tenant = 1,
                FTPDetails = new FTPSchedulerDetails()
                {
                    Folder = "DocumentsBackup",
                    From = "My FTP Server",
                    Host = "192.168.1.26",
                    Password = "!I123456",
                    Prefix = "test",
                    Subject = "test",
                    Suffix = "",
                    UserName = "Islam",
                }
            };
            FTPSchedulerTaskService fTPSchedulerTaskService = new FTPSchedulerTaskService();
            List<string> fileNames = fTPSchedulerTaskService.GetFilteredDirectoryFileNamesByFTPDetails(schedulerDetails);

            fTPSchedulerTaskService.ReadFTPFilesBySchedulerDetailsToAnalyzeQueue(schedulerDetails);

            //using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            //{
            //    AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();
            //    analyzeQueueReposiory.GetAnalyzeQueues(schedulerDetails.Tenant).wh
            //}
        }

        [TestMethod]
        public void DownloadFTPFiles_Extension_Pdf()
        {


            SchedulerDetails schedulerDetails = new SchedulerDetails()
            {
                Tenant = 1,
                FTPDetails = new FTPSchedulerDetails()
                {
                    Extension = "pdf",
                    Folder = "DocumentsBackup",
                    From = "My FTP Server",
                    Host = "192.168.1.26",
                    Password = "!I123456",
                    Subject = "test",
                    Suffix = "",
                    UserName = "Islam",
                }
            };
            FTPSchedulerTaskService fTPSchedulerTaskService = new FTPSchedulerTaskService();
            List<string> fileNames = fTPSchedulerTaskService.GetFilteredDirectoryFileNamesByFTPDetails(schedulerDetails);

            fTPSchedulerTaskService.ReadFTPFilesBySchedulerDetailsToAnalyzeQueue(schedulerDetails);

            //using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            //{
            //    AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();
            //    analyzeQueueReposiory.GetAnalyzeQueues(schedulerDetails.Tenant).wh
            //}
        }


        [TestMethod]
        public void DownloadFTPFiles_Extension_Xml_Prefix_SAT_Suffix_online()
        {


            SchedulerDetails schedulerDetails = new SchedulerDetails()
            {
                Tenant = 1,
                FTPDetails = new FTPSchedulerDetails()
                {
                    Extension = "xml",
                    Folder = "DocumentsBackup",
                    From = "My FTP Server",
                    Host = "192.168.1.26",
                    Password = "!I123456",
                    Subject = "test",
                    Prefix = "sat",
                    Suffix = "online",
                    UserName = "Islam",
                }
            };
            FTPSchedulerTaskService fTPSchedulerTaskService = new FTPSchedulerTaskService();
            List<string> fileNames = fTPSchedulerTaskService.GetFilteredDirectoryFileNamesByFTPDetails(schedulerDetails);

            fTPSchedulerTaskService.ReadFTPFilesBySchedulerDetailsToAnalyzeQueue(schedulerDetails);

            //using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            //{
            //    AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();
            //    analyzeQueueReposiory.GetAnalyzeQueues(schedulerDetails.Tenant).wh
            //}
        }

        [TestMethod]
        public void DownloadFTPFiles_Failure_Test()
        {
        }
    }
}
