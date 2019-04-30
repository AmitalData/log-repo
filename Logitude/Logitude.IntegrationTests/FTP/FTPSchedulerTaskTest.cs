using CommunicationWorkerRole.Services;
using Logitude.BL.InfrastructureModel.DataContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System.Collections.Generic;
using System.Transactions;

namespace Logitude.IntegrationTests.FTP
{
    [TestClass]
    public class FTPSchedulerTaskTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            //	< SchedulerDetails xmlns: i = "http://www.w3.org/2001/XMLSchema-instance" >
            //< FTPDetails >
            //< Extension />
            //< Folder > DocumentsBackup </ Folder >
            //< From > Islam </ From >
            //< Host > 192.168.1.26 </ Host >
            //< Password > !I123456 </ Password >
            //< Prefix />
            //< Subject > test </ Subject >
            //< Suffix />
            //< UserName > islam </ UserName >
            //</ FTPDetails >
            //</ SchedulerDetails >

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
                    Prefix = "is",
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
    }
}
