using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.Messaging.Customs.PerformanceLogger;
using Logitude.Customs.BL.Messaging.Customs.SignQueueBL;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools;
using Logitude.Server.Tools.ExternalServices;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.Utils;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Microsoft.ServiceBus.Messaging;
//using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Transactions;

namespace CustomsWorkerRole
{
    /* 
     * ///_global _global
Insert into BATCHSERVICESDEFINITIONS (CODE,CLASSNAME) values ('CustomsHSMSignWR','CustomsHSMSignWR');
Insert into BATCHSERVICESDEFINITIONMODS (CODE,INACTIVE,NUMBEROFTHREADS) values ('CustomsHSMSignWR',0,1);

    insert into QUEUEDEFINITIONS (CODE, NAME,DUPLICATEMESSAGESAUTOREMOVE) 
select 'CustomsHSMSignWR','CustomsHSMSignWR',0
from dual
where not exists(select * 
                 from QUEUEDEFINITIONS 
                 where (CODE ='CustomsHSMSignWR'));
     */


    public class CustomsHSMSignWR
        : CustomsWorkerEntryPoint
    {
        const int MaxRetries = 5;

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
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                    }
                    catch (Exception e)
                    {
                        ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "CustomsHSMSignWR : Run() Method", null);
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
        private CustomDbQueueService _CustomDbQueueService;
        private int _Tenant;


        private CustomDBQueueMessage _CustomDBQueueMessage;
        private string myClass;
        public CustomsHSMSignWR()
        {

        }
        public override bool OnStart()
        {
            try
            {
                if (_OnStartDone) return true;
                _OnStartDone = true;
                DoneItemsInRange = new Dictionary<DateTime, int>();


                this.myClass = this.GetType().Name;
                int tenantConfig = SettingUtil.GetTenantDBFromConfig();
                var customsEnvironmentSettingQueryService = new CustomsEnvironmentSettingQueryService(tenantConfig);
                var customsEnvironmentSettingPM = customsEnvironmentSettingQueryService.GetEnvironmentSettingPM(tenantConfig) ?? new CustomsEnvironmentSettingPM();

                {
                    base.WorkerQueueType = WorkerQueueType.DB;
					_CustomDbQueueService = new CustomDbQueueService(myClass, tenantConfig, queueDefinitionCode: this.BatchServiceCode);
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

                    //case WorkerQueueType.RabbitMQ:
                    //    WorkUntilPrcossesStop_RabbitMQ();
                    //    break;
                    case WorkerQueueType.DB:
                    default:
                        {
                            WorkUntilQEmpty_Db();
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



        private void WorkUntilQEmpty_Db()
        {
            while (!WorkerRoleServiceLocator.PleaseShutDown)
            {

                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {


                        using (TransactionScope scopeRecive = TransactionFactory.GetNewReadCommittedTransaction())
                        {
                            Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("HSM:using (TransactionScope scopeRecive = TransactionFactory.GetNewReadCommittedTransactio");

                            _CustomDBQueueMessage = _CustomDbQueueService.Receive(CustomsWorkerRole.Utils.GenUtil.GetQueueTimeOutInMin() * 60);
                            scopeRecive.Complete();
                        }

                        if (_CustomDBQueueMessage == null || String.IsNullOrWhiteSpace(_CustomDBQueueMessage.MessageId))
                        {
                            Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("if (_CustomDBQueueMessage == null || String.IsNullOrWhiteSpace(_CustomDBQueueMessage.MessageId");

                            QueueThreadStateService.Upsert(QueueThreadStateService.GetWRKey(this.GetType().Name), "Sleep...");
                            scope.Complete();
                            Thread.Sleep(TimeSpan.FromSeconds(CustomsWorkerRole.Utils.GenUtil.IfNoQueue_ServerWaitTimeInSec()));
                            break;
                        }

                        if (_CustomDBQueueMessage.Retries>10)
                        {
                            Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("if (_CustomDBQueueMessage.Retries>10)");

                            _CustomDBQueueMessage.SafeComplete();
                            scope.Complete();
                            continue;
                        }
                        int.TryParse(_CustomDBQueueMessage.Properties["Tenant"].ToString(), out _Tenant);

                        string customsRequestsSheetId = null;
                        string interfaceTypeCode = null;
                        string signByPersonalId = null;
                        //string signQueueByCompanyOrPersonal = null;

                        customsRequestsSheetId = _CustomDBQueueMessage.Properties["CorrelationId"].ToString();
                        interfaceTypeCode = _CustomDBQueueMessage.Properties["InterfaceTypeCode"].ToString();
                        signByPersonalId = _CustomDBQueueMessage.Properties["SignByPersonalId"].ToString();
                        ///signQueueByCompanyOrPersonal = _CustomDBQueueMessage.Properties["SignQueueByCompanyOrPersonal"].ToString();

                        ProccessHSMSign(
                            customsRequestsSheetId,
                            interfaceTypeCode
                            //signByPersonalId,
                            //signQueueByCompanyOrPersonal
                            );

                        scope.Complete();
                    }
                }
                finally
                {
                    PerformanceM.SleepMSAfterEachQueuePeek();
                }
            }
        }




        public void ProccessHSMSign(
            string customsRequestsSheetId ,
            string interfaceTypeCode 
            ///string signByPersonalId ,
            //string signQueueByCompanyOrPersonal 
            )
        {

            LogMessagingUtil.Instance.Clear();



            RequestParamsBase dRequestParamsBase = null;



            try
            {
                //{ "SignByPersonalId":"032443830","InterfaceTypeCode":"2751","Tenant":"6","CorrelationId":"2c799c10-d9b2-4da5-b872-9cfdb91cf727"}



                var anaO = ContainerAccessor.Container.Resolve<IMessagingServiceInterfaceType>(interfaceTypeCode);
                CustomsSettingQueryService settingService = new CustomsSettingQueryService(_Tenant);
                var setting = settingService.GetSettingByTenantN(_Tenant);
                
                var (bytesToSign, requestParamsBase) = anaO.PasiveSignGetBytesToSign(_Tenant, customsRequestsSheetId);
                dRequestParamsBase = requestParamsBase;

                SignQueueByType signQueueByType = SignQueueByType.None;
                string companypersonal = "";
                Enum.TryParse<SignQueueByType>(requestParamsBase.SignQueueByCompanyOrPersonal, out signQueueByType);
                switch (signQueueByType)
                {

                    case SignQueueByType.SignQueueByPersonId:
                        companypersonal = "P";
                        break;
                    case SignQueueByType.SignQueueByCustomsAgentId:
                    default:
                        companypersonal = "C";
                        break;
                }

                var hSMSignFileService = new HSMSignFileService();

                LogMessagingUtil.Instance.AppendLine(
                    $"hSMSignFile({customsRequestsSheetId}, {requestParamsBase.SignByPersonalId}, {companypersonal})");
                byte[] signBytes = hSMSignFileService
                    .SignCustomsRequest(
                    _Tenant, customsRequestsSheetId,
                    requestParamsBase.SignByPersonalId, companypersonal,
                    setting.CustomsAgentId,bytesToSign, requestParamsBase.HsmStationContext);

                MessagingServiceFactoryHelper.InitContainer();
                var mySendSheetSignModel = new SendSheetSignModel();
                mySendSheetSignModel.CustomsRequestsSheetId = customsRequestsSheetId;
                mySendSheetSignModel.Tenant = _Tenant;
                mySendSheetSignModel.CustomRequestSignedByteArryPasiveSign = signBytes;
                mySendSheetSignModel.CurrentSignCertificateName = "CurrentSignCertificate";
                if (_CustomDBQueueMessage!=null)
                {
                    mySendSheetSignModel.ExportTaskQueueId = _CustomDBQueueMessage.MessageId;
                }
                


                MessagingServiceFactoryHelper.InitContainer();
                anaO = ContainerAccessor.Container.Resolve<IMessagingServiceInterfaceType>(interfaceTypeCode);
                //anaO.CompleteResponseSignBytes(tenant, CustomsRequestsSheetId, mySignBytes);

                var responseData = anaO.SendSheet(_Tenant, customsRequestsSheetId, mySendSheetSignModel);//complete to queue
                var responseDataBase = (responseData as Logitude.CustomsMessaging.Common.ResponseData.ResponseDataBase);
                if (responseDataBase?.HasException == true)
                {
                    Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("(responseDataBase?.HasException == true");

                    //stringBuilder.AppendLine($"SendSheet:HandleException:{responseDataBase.UserMessage}");
                    ExceptionHandler.HandleException(new Exception(message: responseDataBase.UserMessage), DateTime.Now, _Tenant, "", "ProccessHSMSign-SignTaskAsDone", "", null);
                }
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrWhiteSpace(customsRequestsSheetId))
                {
                    CRSSetException(customsRequestsSheetId, ex);

                }

                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex, $"hSMSignFile({_Tenant},{customsRequestsSheetId}, {dRequestParamsBase?. SignByPersonalId}, {dRequestParamsBase?.SignByPersonalId})");
                ExceptionHandler.HandleException(ex, DateTime.Now, _Tenant, "", "ProccessHSMSign-MarkExportSignTaskAsDone", "", null);


            }
        }

        private void CRSSetException(string customsRequestsSheetId, Exception ex)
        {
            try
            {

            
            CustomsRequestsSheetDomainModelUtil.SetExceptionMessage(_Tenant, customsRequestsSheetId,
            $"RetryNumber:{_CustomDBQueueMessage.MyQueueResponse.RetryNumber}-{ex.ToString()}");
                if (_CustomDBQueueMessage.MyQueueResponse.RetryNumber >= MaxRetries)
                {
                    var myContext = CustomContext.GetContext(_Tenant);
                    var customsRequestsSheetUpdateService = new CustomsRequestsSheetUpdateService(_Tenant);
                    var list = new List<CustomsRequestsSheetList>() { new CustomsRequestsSheetList() { Id = customsRequestsSheetId } };


                    customsRequestsSheetUpdateService.CancelRequests(list, _Tenant, myContext);

                    _CustomDbQueueService.Complete();

                }
            }
            catch (Exception)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex, $"hSMSignFile-SetExceptionMessage({_Tenant},{customsRequestsSheetId})");
                ExceptionHandler.HandleException(ex, DateTime.Now, _Tenant, "", "ProccessHSMSign-SetExceptionMessage", "", null);

            }
        }

        public void DebugStep(string customsRequestsSheetId, string interfaceTypeCode, int tenant, 
            string signByPersonalId,
            string signQueueByCompanyOrPersonal)
        {
            bool todo = DateTime.Now > new DateTime(2023, 1, 1);
            if (todo)
            {
                throw new Exception("todo - fix it -take time ?!?!");
            }
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                _Tenant = tenant;
                ProccessHSMSign(customsRequestsSheetId, interfaceTypeCode);
                scope.Complete();
            }
        }
    }



}
