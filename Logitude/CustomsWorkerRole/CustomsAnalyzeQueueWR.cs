


using CustomsWorkerRole.L2U;

using Logitude.Server.Tools.Models;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
//using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage.Blob;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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
update  BATCHSERVICESDEFINITIONMODS  set  NUMBEROFTHREADS =5 where CODE='CustomsAnalyzeQueueWR';
Insert into BATCHSERVICESDEFINITIONS (CODE,CLASSNAME) values ('SendWEBAPIMessage2MamanQ','SendWEBAPIMessage2MamanQ');
Insert into BATCHSERVICESDEFINITIONMODS (CODE,INACTIVE,NUMBEROFTHREADS) values ('SendWEBAPIMessage2MamanQ',0,3);
update  BATCHSERVICESDEFINITIONMODS  set  NUMBEROFTHREADS =3 where CODE='SendWEBAPIMessage2MamanQ';
        
     */

    //public class SendWebAPI2MamanGWMessageECTHRDataWR
    public class CustomsAnalyzeQueueWR
        : CustomsWorkerEntryPoint
    {
        QueueDescription _QueueDescription;
        QueueClient _QueueClient;
        public override void Run()
        {

            while (!WorkerRoleServiceLocator.PleaseShutDown)
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
        

        

        private DateTime _LastCreateFtpDefinition;
        private List<InterfaceDetails> _CustomsPartnerAnalyzeQueueService;
        private int _SeedTenant = 1;
        private DbQueueService _IQueueService;
        private QueueResponse _ReceivedBrokeredMessage;

        //private QueueResponse _ReceivedBrokeredMessage;

        public override bool OnStart()
        {
            try
            {
                if (_OnStartDone) return true;
                _OnStartDone = true;
                DoneItemsInRange = new Dictionary<DateTime, int>();



                var myClass = this.GetType().Name;
                int tenantConfig = SettingUtil.GetTenantDBFromConfig();
                var customsEnvironmentSettingQueryService = new CustomsEnvironmentSettingQueryService(tenantConfig);
                var customsEnvironmentSettingPM = customsEnvironmentSettingQueryService.GetEnvironmentSettingPM(tenantConfig) ?? new CustomsEnvironmentSettingPM();

                if (CustomDbQueueService.SupportedRabbitMQList.Contains(SBQueueNames.AnalyzeQueueMQ.ToString()) && CustomDbQueueService.IsFeatureOnRABBITMQ_Communication() && customsEnvironmentSettingPM.UseRabbitMQ)
                {
                    base.WorkerQueueType = WorkerQueueType.RabbitMQ;
                }
                else
                {
                    base.WorkerQueueType = WorkerQueueType.DB;
                }



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

                switch (base.WorkerQueueType)
                {

                    case WorkerQueueType.RabbitMQ:
                        WorkUntilPrcossesStop_RabbitMQ();
                        break;
                    case WorkerQueueType.DB:
                    default:
                        {
                            bool useMessageQueue = true;
                            if (useMessageQueue)
                            {
                                WorkUntil_MessageQueue_Empty_Db();
                            }
                            else
                            {
                                WorkUntil_AnalyzeQueue_Empty_Db_NOTINUSE();
                            }

                        }
                        break;
                }

                
                


            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole" + this.GetType().Name, " : Run() Method", null);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                _OnStartDone = false;
            }


        }

        private void WorkUntilPrcossesStop_RabbitMQ()
        {
            var myClass = this.GetType().Name;
            var rabbitMQConsumerService = new RabbitMQConsumerService(myClass, SBQueueNames.AnalyzeQueueMQ.ToString());
            rabbitMQConsumerService.WorkUntilPrcossesStop_RabbitMQ(
                (CustomDBQueueMessage customDBQueueMessage) =>
                {
                    _ReceivedBrokeredMessage = customDBQueueMessage.MyQueueResponse;
                    string analyzeQueueID = _ReceivedBrokeredMessage.MessageValues["AnalyzeQueueID"].ToString();
                    string InterfaceCode = _ReceivedBrokeredMessage.MessageValues["InterfaceCode"].ToString();
                    string InterfacePartner = _ReceivedBrokeredMessage.MessageValues["InterfacePartner"].ToString();

                    int Tenant;
                    int.TryParse(_ReceivedBrokeredMessage.MessageValues["Tenant"].ToString(), out Tenant);


                    CheckParamsAndExec(analyzeQueueID, InterfaceCode, InterfacePartner, Tenant);

                    return true;
                },
                base.LogDoneItemInMemory
                );
        }
        private void WorkUntil_MessageQueue_Empty_Db()
        {
            while (true)
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {

					_IQueueService = new DbQueueService();
                    _IQueueService.InitializeQueue(SBQueueNames.AnalyzeQueueMQ.ToString(), SettingUtil.GetTenantDBFromConfig());


                    using (TransactionScope scopeRecive = TransactionFactory.GetNewReadCommittedTransaction())
                    {
                        _ReceivedBrokeredMessage = _IQueueService.Receive(CustomsWorkerRole.Utils.GenUtil.GetQueueTimeOutInMin() * 60);
                        scopeRecive.Complete();
                    }
                    if (_ReceivedBrokeredMessage == null || String.IsNullOrWhiteSpace(_ReceivedBrokeredMessage.MessageId))
                    {
                        //Thread.Sleep(TimeSpan.FromSeconds(5));
                        //Thread.Sleep(TimeSpan.FromSeconds(15));//not using soo mach 
                        Thread.Sleep(TimeSpan.FromSeconds(CustomsWorkerRole.Utils.GenUtil.IfNoQueue_ServerWaitTimeInSec()));
                        break;
                    }

                    LastActivity = DateTime.UtcNow;
                    ProccessReceivedMessage();
                    scope.Complete();
                    LogDoneItemInMemory();
                }
            }
        }

        private void ProccessReceivedMessage()
        {
            LogMessagingUtil.Instance.Clear();

            string analyzeQueueID = _ReceivedBrokeredMessage.MessageValues["AnalyzeQueueID"].ToString();
            string InterfaceCode = _ReceivedBrokeredMessage.MessageValues["InterfaceCode"].ToString();
            string InterfacePartner = _ReceivedBrokeredMessage.MessageValues["InterfacePartner"].ToString();

            int Tenant;
            int.TryParse(_ReceivedBrokeredMessage.MessageValues["Tenant"].ToString(), out Tenant);

            LogMessagingUtil.Instance
                .AppendLine("ProccessReceivedMessage()")
                .AppendLine("QUEUEMessageId:" + _ReceivedBrokeredMessage.MessageId)
                .AppendLine("RetryNumber:" + _ReceivedBrokeredMessage.RetryNumber)
                .AppendLine("analyzeQueueID:" + analyzeQueueID)
                .AppendLine("InterfaceCode:" + InterfaceCode)
                .AppendLine(",Tenant" + Tenant);

            if ( _ReceivedBrokeredMessage.RetryNumber < 1)
            {

                CheckParamsAndExec(analyzeQueueID, InterfaceCode, InterfacePartner, Tenant);
                _IQueueService.Complete();

                this.LogDoneItemInMemory();
            }
            else
            {
                LogMessagingUtil.Instance.AppendLine("RetryNumber >= 1>>> Failed ");
                
                _IQueueService.Complete();
            }
            

        }

        private void WorkUntil_AnalyzeQueue_Empty_Db_NOTINUSE()
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

                    {
                        try
                        {
                            AnalyzeQueueRepository analyzeQueueRepository = new AnalyzeQueueRepository();
                            var from = @interface.Partner + "," + @interface.Code;

                            LastActivity = DateTime.UtcNow;
                            while (true)
                            {

                                AnalyzeQueue analyzeQueue = analyzeQueueRepository.GetOpenAnalyzeQueue(from);
                                if (analyzeQueue == null)
                                {
                                    break;
                                }
                                LastActivity = DateTime.UtcNow;
                                Exec(customsPartnerFtpDetails, @interface, analyzeQueueRepository, analyzeQueue,1);
                                LogDoneItemInMemory();
                            }


                            Thread.Sleep(500);


                        }
                        catch (Exception e)
                        {
                            ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "CustomsAnalyzeQueueWR : Run() Method", null);
                            Thread.Sleep(5000);
                        }
                    }
                }


                //Thread.Sleep(TimeSpan.FromSeconds(5));

                Thread.Sleep(TimeSpan.FromSeconds(1));//not using soo mach 
                break;


            }
        }
        public void CheckParamsAndExec(string analyzeQueueID, string InterfaceCode, string InterfacePartner, int tenant)
        {
            string analyzeQueueFrom = InterfacePartner + "," + InterfaceCode;
            CheckParamsAndExec(analyzeQueueID, analyzeQueueFrom, tenant);

        }
        public void CheckParamsAndExec(string analyzeQueueID, string analyzeQueueFrom, int tenant)
        {

            analyzeQueueFrom = analyzeQueueFrom.Trim();
            if (string.IsNullOrEmpty(analyzeQueueFrom))
            {
                var e=/*throw*/ new Exception("analyzeQueueFrom is must");
                ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole" + this.GetType().Name, " : Run() Method", null);
                return; 

            }
            var parts = analyzeQueueFrom.Split(',').ToList();
            if (parts.Count != 2)
            {
                var e =/*throw*/ new Exception("analyzeQueueFrom.Split('-').ToList() != 2");
                ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole" + this.GetType().Name, " : Run() Method", null);
                return;
            }

            AnalyzeQueueRepository analyzeQueueRepository = new AnalyzeQueueRepository();
            var customsPartnerFtpDetails = new CustomsPartnerFtpDetails();
            var _CustomsAnalyzeQueueServices = customsPartnerFtpDetails.GetAllInterfaceDetails();
            InterfaceDetails @interface = customsPartnerFtpDetails.GetAllInterfaceDetails()
            .Where(r => r.Code == parts[1] && r.Partner == parts[0]).FirstOrDefault();
            if (@interface == null)
            {
                var e =/*throw*/ new Exception($"analyzeQueueFrom {analyzeQueueFrom} is not in customsPartnerFtpDetails.GetAllInterfaceDetails()");
                ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole" + this.GetType().Name, " : Run() Method", null);
                return;

            }


            AnalyzeQueue analyzeQueue = analyzeQueueRepository.GetSingleAnalyzeQueue(analyzeQueueID);
            if (analyzeQueue == null)
            {
                var e =/*throw*/ new Exception($"analyzeQueueID {analyzeQueueID} not in DB");
                ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole" + this.GetType().Name, " : Run() Method", null);
                return;

            }
            Exec(customsPartnerFtpDetails, @interface, analyzeQueueRepository, analyzeQueue, tenant);

        }

        private void Exec(CustomsPartnerFtpDetails customsPartnerFtpDetails, InterfaceDetails @interface, AnalyzeQueueRepository analyzeQueueRepository, AnalyzeQueue analyzeQueue, int tenant)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                var serviceAnalyzer = customsPartnerFtpDetails.GetCustomAnalyzerQueueService(@interface);
                //ArtemusAnalyzer analyzer = new Artemus(analyzeQueue, analyzeQueueRepository);
                serviceAnalyzer.Run(analyzeQueue, analyzeQueueRepository, tenant);
                scope.Complete();
            }
        }
    }

}
