using CommunicationWorkerRole.Services;
using Logitude.BL.InfrastructureModel.DataContracts;
using Logitude.Server.Tools.FTP;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Transactions;

namespace Logitude.IntegrationTests.FTP
{
    [TestClass]
    public class FTPSchedulerTaskTest
    {

        //[TestMethod]
        //public void Upload_INTRA_FTPFiles_Test()
        //{


        //    SchedulerDetails schedulerDetails = new SchedulerDetails()
        //    {
        //        Tenant = 1,
        //        FTPDetails = new FTPSchedulerDetails()
        //        {
        //            Folder = "inbound",
        //            From = "My FTP Server",
        //            Host = "ftp.inttraworks.inttra.com",
        //            Password = "Fm640gDl",
        //            Subject = "test",
        //            UserName = "c0464340",
                    
        //        }
        //    };

        //    FTPService ftpService = new FTPService(schedulerDetails.FTPDetails.Host, schedulerDetails.FTPDetails.UserName, schedulerDetails.FTPDetails.Password);
        //    string folderPath = Environment.CurrentDirectory.Replace(@"bin\Debug", "FTPFiles");
        //    //string filePath = HttpContext.Current.Server.MapPath(".") + "\\bin\\" + "exceptionslogfile.txt";
        //    foreach (string file in Directory.EnumerateFiles(folderPath))
        //    {

        //        byte[] contents = File.ReadAllBytes(file);
        //        string p_message;
        //        string fileName = Path.GetFileName(file);
        //        ftpService.Upload(fileName, schedulerDetails.FTPDetails.Folder, contents, out p_message);
        //        //ftpService.Upload(fileName, settingsData.folder, filedata, out p_message);
        //    }


        //}

        [TestMethod]
        public void Upload_Download_FTPFiles_Pdf_Extension_Test()
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
                    //Prefix = "test",
                    Subject = "test",
                    Suffix = "",
                    UserName = "Islam",
                    Extension = "pdf",
                }
            };

            //private static string filePath = HttpContext.Current.Server.MapPath(".") + "\\bin\\" + "exceptionslogfile.txt";
            UploadTestFiles(schedulerDetails);

            //ftpService.Upload()


            FTPSchedulerTaskService fTPSchedulerTaskService = new FTPSchedulerTaskService();
            List<string> fileNames = fTPSchedulerTaskService.GetFilteredDirectoryFileNamesByFTPDetails(schedulerDetails);

            var mExist = fileNames.Exists(f => (!string.IsNullOrEmpty(f) &&
                Path.GetExtension(f).TrimStart('.').ToLower() != schedulerDetails.FTPDetails.Extension.Trim('.')));


            Assert.IsFalse(mExist, "Expected pdf files but get different extensions");

            //fTPSchedulerTaskService.ReadFTPFilesBySchedulerDetailsToAnalyzeQueue(schedulerDetails);

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

            UploadTestFiles(schedulerDetails);

            FTPSchedulerTaskService fTPSchedulerTaskService = new FTPSchedulerTaskService();
            List<string> fileNames = fTPSchedulerTaskService.GetFilteredDirectoryFileNamesByFTPDetails(schedulerDetails);
            List<string> validFiles = new List<string>();
            if (!string.IsNullOrWhiteSpace(schedulerDetails.FTPDetails.Prefix))
            {
                validFiles = fileNames.FindAll(f => (!string.IsNullOrEmpty(f) &&
                f.StartsWith(schedulerDetails.FTPDetails.Prefix, StringComparison.CurrentCultureIgnoreCase)));
                
            }

            Assert.AreEqual(fileNames.Count,validFiles.Count, "Expected files with [test] prefix but get different extensions");
            //fTPSchedulerTaskService.ReadFTPFilesBySchedulerDetailsToAnalyzeQueue(schedulerDetails);

            //using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            //{
            //    AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();
            //    analyzeQueueReposiory.GetAnalyzeQueues(schedulerDetails.Tenant).wh
            //}
        }

        [TestMethod]
        public void DownloadFTPFiles_Prefix_1_Suffix_2134_Extension_XML_Test()
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
                    UserName = "Islam",
                    Subject = "test",
                    Suffix = "2134",
                    Prefix = "1",
                    Extension = "xml"
                    
                }
            };

            UploadTestFiles(schedulerDetails);

            FTPSchedulerTaskService fTPSchedulerTaskService = new FTPSchedulerTaskService();
            List<string> fileNames = fTPSchedulerTaskService.GetFilteredDirectoryFileNamesByFTPDetails(schedulerDetails);
            List<string> validFiles = new List<string>();
            if (!string.IsNullOrWhiteSpace(schedulerDetails.FTPDetails.Prefix))
            {
                validFiles = fileNames.FindAll(f => (!string.IsNullOrEmpty(f) &&
                f.StartsWith(schedulerDetails.FTPDetails.Prefix, StringComparison.CurrentCultureIgnoreCase)));

            }

            Assert.AreEqual(fileNames.Count, validFiles.Count, "Expected files with ([1] prefix, suffix (2134) , extension (xml)) but get different extensions");
            //fTPSchedulerTaskService.ReadFTPFilesBySchedulerDetailsToAnalyzeQueue(schedulerDetails);

            //using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            //{
            //    AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();
            //    analyzeQueueReposiory.GetAnalyzeQueues(schedulerDetails.Tenant).wh
            //}
        }


        //[TestMethod]
        //public void DownloadFTPFiles_Extension_Pdf()
        //{


        //    SchedulerDetails schedulerDetails = new SchedulerDetails()
        //    {
        //        Tenant = 1,
        //        FTPDetails = new FTPSchedulerDetails()
        //        {
        //            Extension = "pdf",
        //            Folder = "DocumentsBackup",
        //            From = "My FTP Server",
        //            Host = "192.168.1.26",
        //            Password = "!I123456",
        //            Subject = "test",
        //            Suffix = "",
        //            UserName = "Islam",
        //        }
        //    };
        //    FTPSchedulerTaskService fTPSchedulerTaskService = new FTPSchedulerTaskService();
        //    List<string> fileNames = fTPSchedulerTaskService.GetFilteredDirectoryFileNamesByFTPDetails(schedulerDetails);

        //    fTPSchedulerTaskService.ReadFTPFilesBySchedulerDetailsToAnalyzeQueue(schedulerDetails);

        //    //using (TransactionScope scope = TransactionFactory.GetNewTransaction())
        //    //{
        //    //    AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();
        //    //    analyzeQueueReposiory.GetAnalyzeQueues(schedulerDetails.Tenant).wh
        //    //}
        //}


        //[TestMethod]
        //public void DownloadFTPFiles_Extension_Xml_Prefix_SAT_Suffix_online()
        //{


        //    SchedulerDetails schedulerDetails = new SchedulerDetails()
        //    {
        //        Tenant = 1,
        //        FTPDetails = new FTPSchedulerDetails()
        //        {
        //            Extension = "xml",
        //            Folder = "DocumentsBackup",
        //            From = "My FTP Server",
        //            Host = "192.168.1.26",
        //            Password = "!I123456",
        //            Subject = "test",
        //            Prefix = "sat",
        //            Suffix = "online",
        //            UserName = "Islam",
        //        }
        //    };
        //    FTPSchedulerTaskService fTPSchedulerTaskService = new FTPSchedulerTaskService();
        //    List<string> fileNames = fTPSchedulerTaskService.GetFilteredDirectoryFileNamesByFTPDetails(schedulerDetails);

        //    fTPSchedulerTaskService.ReadFTPFilesBySchedulerDetailsToAnalyzeQueue(schedulerDetails);

        //    //using (TransactionScope scope = TransactionFactory.GetNewTransaction())
        //    //{
        //    //    AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();
        //    //    analyzeQueueReposiory.GetAnalyzeQueues(schedulerDetails.Tenant).wh
        //    //}
        //}

        //[TestMethod]
        //public void DownloadFTPFiles_Failure_Test()
        //{
        //    SchedulerDetails schedulerDetails = new SchedulerDetails()
        //    {
        //        Tenant = 1,
        //        FTPDetails = new FTPSchedulerDetails()
        //        {
        //            Folder = "DocumentsBackup",
        //            From = "My FTP Server",
        //            Host = "192.168.1.26",
        //            Password = "!I123456",
        //            //Prefix = "test",
        //            Subject = "fail test",
        //            Suffix = "",
        //            UserName = "Islam",
        //            Extension = "xml",
        //        }
        //    };

        //    FTPSchedulerTaskService fTPSchedulerTaskService = new FTPSchedulerTaskService();
        //    List<string> fileNames = fTPSchedulerTaskService.GetFilteredDirectoryFileNamesByFTPDetails(schedulerDetails);

        //    fTPSchedulerTaskService.ReadFTPFilesBySchedulerDetailsToAnalyzeQueue(schedulerDetails);
        //}


        private void UploadTestFiles(SchedulerDetails schedulerDetails)
        {
            FTPService ftpService = new FTPService(schedulerDetails.FTPDetails.Host, schedulerDetails.FTPDetails.UserName, schedulerDetails.FTPDetails.Password);
            string folderPath = Environment.CurrentDirectory.Replace(@"bin\Debug", "FTPFiles");
            //string filePath = HttpContext.Current.Server.MapPath(".") + "\\bin\\" + "exceptionslogfile.txt";
            foreach (string file in Directory.EnumerateFiles(folderPath))
            {

                byte[] contents = File.ReadAllBytes(file);
                string p_message;
                string fileName = Path.GetFileName(file);
                ftpService.Upload(fileName, schedulerDetails.FTPDetails.Folder, contents, out p_message);
            }
        }
    }
}
