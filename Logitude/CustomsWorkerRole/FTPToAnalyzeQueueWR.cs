
using CustomsWorkerRole.L2U;

using Logitude.Server.Tools.Models;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
//using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage.Blob;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnifreightIIG.UServer;
using CustomsWorkerRole.Queue;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Simplog.Server.Infrastructure;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.Helpers;
using Logitude.Server.Tools;
using Newtonsoft.Json;
using System.Net.Http;
using Logitude.Customs.BL.Messaging.Maman;
using Microsoft.Practices.Unity;
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.FTP;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.Server.Tools.Counters;
using Simplog.Data.InfrastructureModel.Repositories;
using Unifreight.Data.AmitalModel;
using Logitude.Server.Tools.Utils;
using Logitude.Customs.BL.Messaging.CustomsAnalyzeQueue;

namespace CustomsWorkerRole
{
    /* 
     * ///couriernet_global _global _global
Insert into BATCHSERVICESDEFINITIONS (CODE,CLASSNAME) values ('FTPToAnalyzeQueueWR','FTPToAnalyzeQueueWR');
Insert into BATCHSERVICESDEFINITIONMODS (CODE,INACTIVE,NUMBEROFTHREADS) values ('FTPToAnalyzeQueueWR',0,1);


        
INSERT INTO "ANALYZEQUEUESTATUS" (CODE, NAME) VALUES ('D', 'Done')
INSERT INTO "ANALYZEQUEUESTATUS" (CODE, NAME) VALUES ('F', 'Fail')
INSERT INTO "ANALYZEQUEUESTATUS" (CODE, NAME) VALUES ('W', 'Waiting')

     */

    //public class SendWebAPI2MamanGWMessageECTHRDataWR
    public class FTPToAnalyzeQueueWR
        : CustomsWorkerEntryPoint
    {
        QueueDescription _QueueDescription;
        QueueClient _QueueClient;
        public override void Run()
        {

            while (true)
            {

                if (!General.IsUpdating())
                {
                    try
                    {

                        WorkOnce();
                        Thread.Sleep(TimeSpan.FromSeconds(_SeedTenant));
                    }
                    catch (Exception e)
                    {
                        ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "FTPToAnalyzeQueueWR : Run() Method", null);
                        Thread.Sleep(10000);
                    }
                }
                else
                {
                    Thread.Sleep(60000);
                }


            }

        }
        bool _OnStartDone = false;
        //private DbQueueService _IQueueService;
        private ICommonDataContext _ICommonDataContext;

        private CommunicationLogRepository _CommunicationLogRep;
        private int _Tenant;

        private CommunicationLog _WaitingCommLog;

        private DateTime _LastCreateFtpDefinition;
        private List<CustomsPartnerFtpPM> _FtpDefinitions;
        private int _SeedTenant = 1;

        //private QueueResponse _ReceivedBrokeredMessage;

        public override bool OnStart()
        {
            try
            {
                if (_OnStartDone) return true;
                _OnStartDone = true;



                var myClass = this.GetType().Name;

                CreateFtpDefinitionsEvery10Min();

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "amital send data worker role start", null, null);
            }

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.


            return base.OnStart();
        }

        private void CreateFtpDefinitionsEvery10Min()
        {
            if (DateTime.Now.Subtract(_LastCreateFtpDefinition) < TimeSpan.FromMinutes(15))
            {
                return;
            }
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {

                //ProccessReceivedMessage();


                _LastCreateFtpDefinition = DateTime.Now;
                _FtpDefinitions = new List<CustomsPartnerFtpPM>();
                var customsPartnerFtpDetails = new CustomsPartnerFtpDetails();
                var ftpIncustomsPartnerFtpDetails = customsPartnerFtpDetails.GetAllInterfaceDetails()
                    .Where(r => r.TypeCode == CustomsPartnerFtpDetails.TypeCode_In)
                    .Where(r => r.ViaMethod == "FTP")
                    .ToList();

                ftpIncustomsPartnerFtpDetails.ForEach(ftpIncustomsPartnerFtpDetail =>
                {

                    var myCustomsPartnerFtpQueryService = new CustomsPartnerFtpQueryService(_SeedTenant);
                    var pmCustomsPartnerFtp = myCustomsPartnerFtpQueryService.GetBy(_SeedTenant, ftpIncustomsPartnerFtpDetail.Code /*CustomsPartnerFtpDetails.InterfaceName_ECSPCL*/,
                        ftpIncustomsPartnerFtpDetail.Partner,
                        ftpIncustomsPartnerFtpDetail.TypeCode);



                    if (pmCustomsPartnerFtp != null)
                    {


                        FTPDetailRepository ftpDetailsRepository = new FTPDetailRepository(_SeedTenant);
                        FTPDetail ftpDetail = ftpDetailsRepository.GetSingleFTPDetail(pmCustomsPartnerFtp.FtpDetailsId, pmCustomsPartnerFtp.Tenant);
                        if (ftpDetail != null)
                        {

                            pmCustomsPartnerFtp.MyFtpDetail = ftpDetail;
                            _FtpDefinitions.Add(pmCustomsPartnerFtp);
                        }
                    }
                });
            }
        }

        public override void WorkOnce()
        {

            try
            {
                OnStart();

                WorkUntilQEmpty_Db();


            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole" + this.GetType().Name, " : Run() Method", null);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                _OnStartDone = false;
            }


        }

        private void WorkUntilQEmpty_Db()
        {
            CreateFtpDefinitionsEvery10Min();

            while (true)
            {

                foreach (CustomsPartnerFtpPM ftpDef in _FtpDefinitions)
                {
                    DownloadFTPFiles(ftpDef);
                }


                //Thread.Sleep(TimeSpan.FromSeconds(5));

                Thread.Sleep(TimeSpan.FromSeconds(15));//not using soo mach 
                break;


            }
        }

        private void DownloadFTPFiles(CustomsPartnerFtpPM customsPartnerFtpPM)
        {

            try
            {
                var customsPartnerFtpDetails = new CustomsPartnerFtpDetails();
                var defInterfaceDetails = customsPartnerFtpDetails.GetAllInterfaceDetails()
                    .Where(r => r.Code == customsPartnerFtpPM.InterfaceName).First();
                Debug.WriteLine($"DownloadFTPFiles({customsPartnerFtpPM.InterfaceName})");
                var ftpDetail = customsPartnerFtpPM.MyFtpDetail;

                Debug.WriteLine($"FTPService({ftpDetail.Host}, {ftpDetail.UserName}, {ftpDetail.Password})");
                FTPService ftpService = new FTPService(ftpDetail.Host, ftpDetail.UserName, ftpDetail.Password);
                Debug.WriteLine($"DirectoryListSimple({ftpDetail.Folder})");
                var directoryFiles = ftpService.DirectoryListSimple(ftpDetail.Folder).ToList();

                if (!string.IsNullOrWhiteSpace(customsPartnerFtpPM.FileExt))
                {
                    directoryFiles = directoryFiles.Where(f => (
                    Path.GetExtension(f)
                    .Equals(customsPartnerFtpPM.FileExt, StringComparison.CurrentCultureIgnoreCase)))
                    .ToList();
                }
                if (!string.IsNullOrWhiteSpace(customsPartnerFtpPM.FileExt))
                {
                    directoryFiles = directoryFiles.Where(f => (
                     Path.GetFileNameWithoutExtension(f)
                    .Equals(customsPartnerFtpPM.FileName, StringComparison.CurrentCultureIgnoreCase)))
                    .ToList();
                }
                directoryFiles = directoryFiles.Where(r => !String.IsNullOrWhiteSpace(r)).ToList();
                directoryFiles = directoryFiles.OrderBy(fileName => fileName).ToList();
                foreach (string fileName in directoryFiles)
                {

                    var fileWithFolder = ftpDetail.Folder + "/" + fileName;
                    Debug.WriteLine($"ftpService.Download({fileWithFolder})");
					string p_message = "";
					byte[] fileData = ftpService.Download(fileWithFolder,out p_message);



                    Debug.WriteLine($"SaveMessageToAnalyzeQueue");
                    var analyzeQueueUtil = new AnalyzeQueueUtil();

                    
                    analyzeQueueUtil.SaveMessageToAnalyzeQueue(fileName, fileData, customsPartnerFtpPM.Tenant, "",defInterfaceDetails,null);

                    Debug.WriteLine($"ftpService.Delete({fileName})");
                    ftpService.Delete(fileWithFolder);
                }

            }

            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "FTP To AnalyzeQueue WorkerRole", ex.Message, null);
            }
        }

    }



}
