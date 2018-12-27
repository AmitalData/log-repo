


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

namespace CustomsWorkerRole
{
    /* 
     * ///couriernet_global _global _global
Insert into BATCHSERVICESDEFINITIONS (CODE,CLASSNAME) values ('CustomsAnalyzeQueueWR','CustomsAnalyzeQueueWR');
Insert into BATCHSERVICESDEFINITIONMODS (CODE,INACTIVE,NUMBEROFTHREADS) values ('CustomsAnalyzeQueueWR',0,1);
     */

    //public class SendWebAPI2MamanGWMessageECTHRDataWR
    public class CustomsAnalyzeQueueWR
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
                        Thread.Sleep(TimeSpan.FromSeconds(5));
                    }
                    catch (Exception e)
                    {
                        ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "CustomsAnalyzeQueueWR : Run() Method", null);
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
        private List<InterfaceDetails> _CustomsPartnerAnalyzeQueueService;
        private int _SeedTenant = 1;

        //private QueueResponse _ReceivedBrokeredMessage;

        public override bool OnStart()
        {
            try
            {
                if (_OnStartDone) return true;
                _OnStartDone = true;



                var myClass = this.GetType().Name;

                

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

            var customsPartnerFtpDetails = new CustomsPartnerFtpDetails();
            var _CustomsAnalyzeQueueServices = customsPartnerFtpDetails.GetAllInterfaceDetails()
            .Where(r => r.TypeCode == CustomsPartnerFtpDetails.TypeCode_In)
            .Where(r => r.AnalyzeQueueService != AnalyzeQueueServiceEnum.none)
            .ToList();

        while (true)
            {

                foreach (InterfaceDetails @interface in _CustomsAnalyzeQueueServices)
                {
                    try
                    {
                        AnalyzeQueueRepository analyzeQueueRepository = new AnalyzeQueueRepository();
                        var from = @interface.Partner + "," + @interface.Code;
                        AnalyzeQueue analyzeQueue = analyzeQueueRepository.GetOpenAnalyzeQueue(from);
                        LastActivity = DateTime.UtcNow;

                        if (analyzeQueue != null)
                        {
                            var serviceAnalyzer = customsPartnerFtpDetails.GetCustomAnalyzerQueueService(@interface);
                            //ArtemusAnalyzer analyzer = new Artemus(analyzeQueue, analyzeQueueRepository);
                            serviceAnalyzer.Run(analyzeQueue, analyzeQueueRepository);
                            
                            LogDoneItemInMemory();
                        }
                        else
                        {
                            Thread.Sleep(500);
                        }
                    }

                    catch (Exception e)
                    {
                        ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "ArtemusAnalyzerWorkerRole : Run() Method", null);
                        Thread.Sleep(5000);
                    }
                }


                //Thread.Sleep(TimeSpan.FromSeconds(5));

                Thread.Sleep(TimeSpan.FromSeconds(1));//not using soo mach 
                break;


            }
        }



        public string BuildCommunicationLog(byte[] bytearray, CustomsPartnerFtpPM customsPartnerFtpPM)///using  by SendWEBAPIMessage2MamanWRWR
        {


            ICommonDataContext commonContext = CommonDataContext.GetContext(customsPartnerFtpPM.Tenant);
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
            DocumentRepository documentRepository = new DocumentRepository(commonContext);








            ContactRepository contactRepository = new ContactRepository(customsPartnerFtpPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(AuthenticationUtil.ResolveLoggingUserId(customsPartnerFtpPM.Tenant), customsPartnerFtpPM.Tenant);
            string loggedContactId = "";
            if (loggedContact != null)
            {
                loggedContactId = loggedContact.Id;
            }

            //.PostIt("", "F_unitedf", "Unit2019", data);
            var settings = new Courier2MamanCommSettings()
            {
                MessageCode = customsPartnerFtpPM.InterfaceName,
                Tenant = customsPartnerFtpPM.Tenant,
                LoggedContactId = loggedContactId
            };
            var settingsData = Logitude.Server.Tools.Utils.ProxyUtil.JsonConvertSerialize(settings);

            Document document = new Document()
            {
                CreateDate = DateTime.Now,
                Extension = "TXT",
                FileSize = bytearray.Length,
                Tenant = Convert.ToInt32(customsPartnerFtpPM.Tenant),
                Id = IdCounter.GetNumber("Document", customsPartnerFtpPM.Tenant),
                HasFile = true,
                Folder = CustomsPartnerFtpDetails.PartnerCode_Mamam.ToLower(),
            };

            documentRepository.Add(document);
            documentRepository.SubmitChanges();

            var def = (new CustomsPartnerFtpDetails()).GetAllInterfaceDetails().First(r => r.Code == customsPartnerFtpPM.InterfaceName);
            CommunicationLog commLog = new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", customsPartnerFtpPM.Tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(customsPartnerFtpPM.Tenant),
                LastStatusDateUTC = DateTime.UtcNow,
                //To = ,
                From = CustomsPartnerFtpDetails.PartnerCode_Mamam + "," + CustomsPartnerFtpDetails.InterfaceName_ECSTS,
                InOut = "O",
                //EntityId = declarationId,
                //ObjectTableId = objectTableId,
                Subject = customsPartnerFtpPM.InterfaceName,
                Tenant = customsPartnerFtpPM.Tenant,
                CommunicationLogTypeCode = "T",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(customsPartnerFtpPM.Tenant),
                CommunicationStatusTypeCode = "W",
                DocumentId = document.Id,
                CreateDateUTC = DateTime.UtcNow,
                CreatedByUserId = loggedContactId,
                LogSettings = settingsData,
                QueueName = def.QueueName //SBQueueNames.SendWEBAPIMessage2MamanQ.ToString() ///using  by SendWEBAPIMessage2MamanWR
            };

            communicationLogRepository.Add(commLog);
            communicationLogRepository.SubmitChanges();

            Logitude.Server.Tools.BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = customsPartnerFtpPM.Tenant,
                FileSize = bytearray.Length,
            };

            Logitude.Server.Tools.StorageService.IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
            storageservice.Write(bytearray.ToArray(), fileInfo);

            SendCommunicationLogMessageToQueue(commLog.QueueName, commLog.Id, customsPartnerFtpPM.Tenant);

            return commLog.Id;

        }

        private void SendCommunicationLogMessageToQueue(string queueName, string communicationLogId, int tenant)
        {
            try
            {
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue(queueName, 0);
                queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", communicationLogId }, { "Tenant", tenant.ToString() } });

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Send FTP CommunicationLog Queue", null, null);
            }
        }
    }



}
