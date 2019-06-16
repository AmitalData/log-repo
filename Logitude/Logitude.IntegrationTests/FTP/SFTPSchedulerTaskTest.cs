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
    public class SFTPSchedulerTaskTest
    {

        [TestMethod]
        public void Upload_Download_Delete_FTPFiles_Pdf_Extension_Test()
        {


            SchedulerDetails schedulerDetails = new SchedulerDetails()
            {
                Tenant = 1,
                FTPDetails = new FTPSchedulerDetails()
                {
                    Folder = "DocumentsBackup",
                    From = "My FTP Server",
                    Host = "192.168.1.26",
                    Password = "password",
                    Prefix = "",
                    Subject = "test",
                    Suffix = "",
                    UserName = "tester",
                    Extension = "pdf",
                }
            };

            //private static string filePath = HttpContext.Current.Server.MapPath(".") + "\\bin\\" + "exceptionslogfile.txt";
            UploadTestFiles(schedulerDetails);

            //ftpService.Upload()



            SFTPService sftpService = new SFTPService();
            string p_status = "";
            string p_message = "";
            sftpService.Logon(schedulerDetails.FTPDetails.Host, schedulerDetails.FTPDetails.UserName, schedulerDetails.FTPDetails.Password, "22", schedulerDetails.FTPDetails.Folder, out p_status, out p_message);

            SFTPSchedulerTaskService fTPSchedulerTaskService = new SFTPSchedulerTaskService();
            List<string> directoryFiles = fTPSchedulerTaskService.GetFilteredDirectoryFileNamesByFTPDetails(schedulerDetails, sftpService);

            var mExist = directoryFiles.Exists(f => (!string.IsNullOrEmpty(f) &&
                Path.GetExtension(f).TrimStart('.').ToLower() != schedulerDetails.FTPDetails.Extension.Trim('.')));


            foreach (string fileName in directoryFiles)
            {

                byte[] fileData = sftpService.DownloadFile(fileName, out p_status, out p_message);
                string extention = Path.GetExtension(fileName);
                if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(extention))
                {
                    //byte[] fileData = DownloadFTPFile(schedulerDetails, sftpService, fileName);
                    //AddToAnalyzeQueue(fileName, fileData, schedulerDetails);
                    //DeleteFTPFile(schedulerDetails, sftpService, fileName);
                }
                string folderPath = Environment.CurrentDirectory.Replace(@"bin\Debug", "DownloadedFiles");
                folderPath += "//" + fileName;
                FileStream fileStream = new FileStream(folderPath, FileMode.Create);
                fileStream.Write(fileData, 0, fileData.Length);


                fTPSchedulerTaskService.DeleteFTPFile(schedulerDetails, sftpService, fileName);


            }


            Assert.IsFalse(mExist, "Expected pdf files but get different extensions");



            //fTPSchedulerTaskService.ReadFTPFilesBySchedulerDetailsToAnalyzeQueue(schedulerDetails);

            //using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            //{
            //    AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();
            //    analyzeQueueReposiory.GetAnalyzeQueues(schedulerDetails.Tenant).wh
            //}
        }

        [TestMethod]
        public void DownloadAndDeleteFTPFiles_Prefix_Test()
        {

            SchedulerDetails schedulerDetails = new SchedulerDetails()
            {
                Tenant = 1,
                FTPDetails = new FTPSchedulerDetails()
                {
                    Folder = "DocumentsBackup",
                    From = "My FTP Server",
                    Host = "192.168.1.26",
                    Password = "password",
                    Prefix = "az",
                    Subject = "test",
                    Suffix = "",
                    UserName = "tester",
                }
            };

            UploadTestFiles(schedulerDetails);

            SFTPService sftpService = new SFTPService();
            string p_status = "";
            string p_message = "";
            sftpService.Logon(schedulerDetails.FTPDetails.Host, schedulerDetails.FTPDetails.UserName, schedulerDetails.FTPDetails.Password, "22", schedulerDetails.FTPDetails.Folder, out p_status, out p_message);

            SFTPSchedulerTaskService fTPSchedulerTaskService = new SFTPSchedulerTaskService();
            List<string> directoryFiles = fTPSchedulerTaskService.GetFilteredDirectoryFileNamesByFTPDetails(schedulerDetails, sftpService);
            List<string> validFiles = new List<string>();
            if (!string.IsNullOrWhiteSpace(schedulerDetails.FTPDetails.Prefix))
            {
                validFiles = directoryFiles.FindAll(f => (!string.IsNullOrEmpty(f) &&
                f.StartsWith(schedulerDetails.FTPDetails.Prefix, StringComparison.CurrentCultureIgnoreCase)));

            }

           
          
            
            foreach (string fileName in directoryFiles)
            {

                byte[] fileData = sftpService.DownloadFile(fileName, out p_status, out p_message);
                string extention = Path.GetExtension(fileName);
                if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(extention))
                {
                    //byte[] fileData = DownloadFTPFile(schedulerDetails, sftpService, fileName);
                    //AddToAnalyzeQueue(fileName, fileData, schedulerDetails);
                    //DeleteFTPFile(schedulerDetails, sftpService, fileName);
                }
                string folderPath = Environment.CurrentDirectory.Replace(@"bin\Debug", "DownloadedFiles");
                folderPath += "//" + fileName;
                FileStream fileStream = new FileStream(folderPath, FileMode.Create);
                fileStream.Write(fileData, 0, fileData.Length);


                fTPSchedulerTaskService.DeleteFTPFile(schedulerDetails, sftpService, fileName);


            }

            Assert.AreEqual(directoryFiles.Count, validFiles.Count, "Expected files with [test] prefix but get different extensions");
           
        }

        private void UploadTestFiles(SchedulerDetails schedulerDetails)
        {
            string p_status = "";
            string p_message = "";

            SFTPService sftpService = new SFTPService();
            sftpService.Logon(schedulerDetails.FTPDetails.Host, schedulerDetails.FTPDetails.UserName, schedulerDetails.FTPDetails.Password, "22", schedulerDetails.FTPDetails.Folder, out p_status, out p_message);

            string folderPath = Environment.CurrentDirectory.Replace(@"bin\Debug", "FTPFiles");
            //string filePath = HttpContext.Current.Server.MapPath(".") + "\\bin\\" + "exceptionslogfile.txt";
            foreach (string file in Directory.EnumerateFiles(folderPath))
            {

                byte[] contents = File.ReadAllBytes(file);
                
                string fileName = Path.GetFileName(file);
                sftpService.Upload(fileName, contents, true, false, out p_status, out p_message);
            }
        }
    }
}
