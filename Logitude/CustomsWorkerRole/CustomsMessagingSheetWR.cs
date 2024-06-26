
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CustomsWorkerRole.Queue;
using UnifreightIIG.Common.Utils;
using Logitude.Server.Tools;
using Logitude.CustomsMessaging.MessagingServices;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Customs.BL.Messaging.Customs;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using Logitude.Server.Tools.QueueService;

namespace CustomsWorkerRole
{
    public class CustomsMessagingSheetWR : CustomsCommandBase
    {
        public CustomsMessagingSheetWR()
            : base(SBQueueNames.CustomsMessagingSheetBQ.ToString())
        {

        }
       
        protected override bool ProcessMessage_Db(CustomDBQueueMessage msgResponse)
        {
            ProcessMessage_DbOverride(msgResponse);
            return true;
        }
        private void ProcessMessage_DbOverride(CustomDBQueueMessage message)
        {

            try
            {

                var analyzeClass = message.GetProperty<string>(QueueExt.QueuePropertyNames.InterfaceTypeCode, "");//, "Logitude.CustomsMessaging.MessagingServices.DF_MSG10000_ImportDeclarationMessagingService");
                if (String.IsNullOrWhiteSpace(analyzeClass))
                {
                    message.SafeComplete();
                    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method :analyzeClass ==null", null);
                    //message.DeadLetter();
                    return;//
                }


                if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>(analyzeClass))
                {
                    message.SafeComplete();
                    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage():!ContainerAccessor.Container.IsRegistered :analyzeClass=" + analyzeClass, null);
                    //message.DeadLetter();
                    return;
                }

                var tenant = message.GetProperty<int>(QueueExt.QueuePropertyNames.Tenant, -1);
                if (tenant == -1)
                {
                    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method :tenant==-1", null);
                    return;
                }
                //{"InterfaceTypeCode":"8250","Tenant":"1","CorrelationId":"4292c133-b256-45b2-9c89-b439b53c2ebb"}
                string CorrelationId = message.GetProperty<string>(QueueExt.QueuePropertyNames.CorrelationId, "");
                if (String.IsNullOrWhiteSpace(CorrelationId))
                {
                    message.SafeComplete();
                    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method :CorrelationId ==null", null);
                    //message.DeadLetter();
                    return;//
                }
                var anaO = ContainerAccessor.Container.Resolve<IMessagingServiceInterfaceType>(analyzeClass);
                anaO.MyOverrideControllerModel = new OverrideControllerModel()
                {
                    IsCustomsMessagingSheetWR = true
                };
                var resDat = anaO.SendSheet(tenant, CorrelationId);

                message.SafeComplete();

            }
            catch (CustomsRequestsSheetDomainModelServiceException customsRequestsSheetServiceException)
            {
                ExceptionHandler.HandleException(customsRequestsSheetServiceException, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method/CustomsRequestsSheetServiceException ", null);
                if (customsRequestsSheetServiceException.What2Do == CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue)
                {
                    message.SafeComplete();
                }
                else
                {
                    message.SetProperty<DateTime>(QueueExt.QueuePropertyNames.LastExecAt, DateTime.UtcNow);
                    message.SafeAbandon();
                }

            }
            catch (Exception ex)
            {

                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method", null);
                if (message.Properties.Keys.Contains("CommunicationLogId"))
                {

                    message.SafeComplete();
                    if (false)
                    {
                        string communicationLogId = message.Properties["CommunicationLogId"].ToString();
                        if (communicationLogId != null)
                        {
                            message.SafeAbandon();
                        }
                        else
                        {
                            message.SafeComplete();
                        }
                    }

                }
                else
                {
                    message.SafeComplete();
                }
                //throw;
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method", null);
            }
        }
        private void ProcessMessage(BrokeredMessage message)
        {

            try
            {

                var analyzeClass = message.GetProperty<string>(QueueExt.QueuePropertyNames.InterfaceTypeCode, "");//, "Logitude.CustomsMessaging.MessagingServices.DF_MSG10000_ImportDeclarationMessagingService");
                if (String.IsNullOrWhiteSpace(analyzeClass))
                {
                    message.SafeComplete();
                    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method :analyzeClass ==null", null);
                    //message.DeadLetter();
                    return;//
                }

                
                if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>(analyzeClass))
                {
                    message.SafeComplete();
                    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage():!ContainerAccessor.Container.IsRegistered :analyzeClass=" + analyzeClass, null);
                    //message.DeadLetter();
                    return;
                }
                
                var tenant = message.GetProperty<int>(QueueExt.QueuePropertyNames.Tenant, -1);
                if (tenant == -1)
                {
                    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method :tenant==-1", null);
                    return;
                }

                var anaO = ContainerAccessor.Container.Resolve<IMessagingServiceInterfaceType>(analyzeClass);

                var resDat = anaO.SendSheet(tenant, message.CorrelationId);

                message.SafeComplete();

            }
            catch (CustomsRequestsSheetDomainModelServiceException customsRequestsSheetServiceException)
            {
                ExceptionHandler.HandleException(customsRequestsSheetServiceException, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method/CustomsRequestsSheetServiceException ", null);
                if (customsRequestsSheetServiceException.What2Do == CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue)
                {
                    message.SafeComplete();
                }
                else
                {
                    message.SetProperty<DateTime>(QueueExt.QueuePropertyNames.LastExecAt, DateTime.UtcNow);
                    message.SafeAbandon();
                }

            }
            catch (Exception ex)
            {

                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method", null);
                if (message.Properties.Keys.Contains("CommunicationLogId"))
                {

                    message.SafeComplete();
                    if (false)
                    {
                        string communicationLogId = message.Properties["CommunicationLogId"].ToString();
                        if (communicationLogId != null)
                        {
                            message.SafeAbandon();
                        }
                        else
                        {
                            message.SafeComplete();
                        }
                    }

                }
                else
                {
                    message.SafeComplete();
                }
                //throw;
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method", null);
            }
        }

    }
#if false
    

    ///SBQueueNames.CustomsMessagingSheetBQ
    //_CustomsRequestsSheetService.InBatchModeToCreateQ() && !requestParams.SuppressSplitWR
    public class CustomsMessagingSheetWR : CustomsWorkerEntryPoint
    {
        bool _OnStartDone = false;
        QueueDescription _QueueDescription;
        QueueClient _QueueClient;
        private QueueClient _DeadletterQueueClient;

        public override void Run()
        {

            while (true)
            {

                if (General.IsUpdating())
                {
                    Thread.Sleep(60000);
                    continue;
                }
                try
                {
                    WorkOnce();
                }
                catch (Exception e)
                {
                    ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR : Run() Method", null);
                    Thread.Sleep(10000);
                }

            }

        }

        public override bool OnStart()
        {
            try
            {
                if (_OnStartDone) return true;
                _OnStartDone = true;

                MessagingServiceFactoryHelper.InitContainer();
                if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>("2715"))
                {
                    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage():!ContainerAccessor.Container.IsRegistered :analyzeClass=2715", null);
                    //message.DeadLetter();
                    ///return;
                }


                





                string emailQueueName = ThreadedRoleEntryPoint.GetQueueByEnviroment(Logitude.Server.Tools.Helpers.SBQueueNames.CustomsMessagingSheetBQ.ToString()); //Amitalqueue

                if (!StorageAcountDetails.NameSpaceManager.QueueExists(emailQueueName))
                {
                    _QueueDescription = new QueueDescription(emailQueueName);
                    _QueueDescription.MaxSizeInMegabytes = 5120;
                    _QueueDescription.LockDuration = TimeSpan.FromMinutes(5);//due debug + raise after 5 min !!
                    _QueueDescription.MaxDeliveryCount = 100;

                    StorageAcountDetails.NameSpaceManager.CreateQueue(_QueueDescription);
                }
                string deadLetterQueuePath = QueueClient.FormatDeadLetterPath(Logitude.Server.Tools.Helpers.SBQueueNames.CustomsMessagingSheetBQ.ToString());

                _DeadletterQueueClient = StorageAcountDetails.CreateServiceBusQueueClient(deadLetterQueuePath);

                 
                _QueueClient = StorageAcountDetails.CreateServiceBusQueueClient(emailQueueName);
                
                if (this.DebugMode)
                {
                    //TestCache();
                    //TestCL_MSG101_GetCustomerByEntityCustomerIdentificationMassagingService();
                    //Test3053();
                    //TestTableDCA();
                    ///TestCustomsRequestsSheetService10000();
                    //TestCustomsRequestsSheetService2715();
                    //SendDebugTest2715();
                    //SendDebugTest1000();
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
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }

        private void TestCache()
        {
            var qs = new Logitude.Customs.BL.EntityQueryServices.InterfaceManagementQueryService(1);
            bool getFromCache = true;
            var res = qs.GetSingle("key does not exist in DB !!!!", false, getFromCache);

        }

        private void TestCL_MSG101_GetCustomerByEntityCustomerIdentificationMassagingService()
        {

            try
            {


                var messageService = new CL_MSG101_GetCustomerByEntityCustomerIdentificationMassagingService();
                var reqParams = new Logitude.CustomsMessaging.Common.RequestParams.ClientSearchRequestParams()
                   {

                       Tenant = 1,
                       RequestName = "NewPaymentRequest ",
                       InterfaceTypeCode = messageService.MainInterfaceCode,
                       RequestVIA = Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.DCABatch,
                       PassportNumber = "40845497",
                       ExternalId = "40845497"///"510120041"
                   };
                
                var resData = messageService.SendSheet(reqParams);


                messageService = new CL_MSG101_GetCustomerByEntityCustomerIdentificationMassagingService();
                var resDataNew = messageService.SendSheet(1, resData.CustomsRequestsSheetId);
                resDataNew = messageService.SendSheet(1, resData.CustomsRequestsSheetId);
            }
            catch (Exception)
            {

                ///throw;
            }
        }
        public static string CreateBatchFile<T1>(T1 request)
        {

            MemoryStream stream = new MemoryStream();
            XmlTypeAttribute xmltype = GetXmltype<T1>();
            XmlSerializer ser = new XmlSerializer(typeof(T1), xmltype.Namespace);


            ser.Serialize(stream, request);
            stream.Seek(0, SeekOrigin.Begin);

            XmlDocument doc = new XmlDocument();
            doc.Load(stream);
            string v_filename = Path.Combine(@"c:\", request.GetType().Name);



            doc.Save(v_filename);
            return doc.OuterXml;
        }
        private static XmlTypeAttribute GetXmltype<TBusinessObject>()
        {
            object[] objArr = typeof(TBusinessObject).GetCustomAttributes(typeof(XmlTypeAttribute), false);
            return objArr[0] as XmlTypeAttribute;
        }
        private MemoryStream Ser<T>(T tObj)
        {
            
            //using (
            var stream = new MemoryStream();
            //)
            {
                object[] objArr = typeof(T).GetCustomAttributes(typeof(XmlTypeAttribute), false);
                var xmltype = objArr[0] as XmlTypeAttribute;
                var att = new XmlRootAttribute { Namespace = xmltype.Namespace };

                
                // serialize object to byte array
                XmlSerializer ser = new XmlSerializer(typeof(T), att);
                ser.Serialize(stream, tObj);
                stream.Position = 0;
                //stream.Seek(0, SeekOrigin.Begin);
                
                //System.Xml.Linq.XDocument xDoc = System.Xml.Linq.XDocument.Parse(p_xml);
                //xDoc.Validate(v_schemas, (o, e) =>
                //{
                //    throw new Exception("Validation against schema failed: Validation error was thrown." + Environment.NewLine + e.Message, e.Exception);
                //});

                return stream;
                
                //reader.Close();
            }
        }

        private void Test3053()
        {

            try
            {


                var messageService = new TSH_NG_3053_MSG8_AgentPaymentRequestMessageService();
                var reqParams = new Logitude.CustomsMessaging.Common.RequestParams.NewPaymentRequestParams()
                   {

                       Tenant = 1,
                       RequestName = "NewPaymentRequest ",
                       InterfaceTypeCode = "9000",
                       RequestVIA = Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.DCABatch,
                       PaymentNumber = "450186971",//"1",
                       ExternalId = "1-15"///"510120041"
                   };
                var resData = messageService.SendSheet(reqParams);


                messageService = new TSH_NG_3053_MSG8_AgentPaymentRequestMessageService();
                var resDataNew = messageService.SendSheet(1, resData.CustomsRequestsSheetId);
                resDataNew = messageService.SendSheet(1, resData.CustomsRequestsSheetId);
            }
            catch (Exception)
            {

                ///throw;
            }
        }

        private void TestTableDCA()
        {
            try
            {


                var messageService = new SYSTBL_NG_9000_MSG_SystemTableRequestMessageService();
                var resData = messageService.SendSheet(new Logitude.CustomsMessaging.Common.RequestParams.SystemTableRequestParams()
                {
                    TableId = "6",
                    Tenant = 1,
                    InterfaceTypeCode = "9000",
                    RequestVIA = Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.DCABatch

                });
                messageService = new SYSTBL_NG_9000_MSG_SystemTableRequestMessageService();
                var resDataNew = messageService.SendSheet(1, resData.CustomsRequestsSheetId);
                resDataNew = messageService.SendSheet(1, resData.CustomsRequestsSheetId);
            }
            catch (Exception)
            {

                ///throw;
            }
        }

        private void TestCustomsRequestsSheetService2715()
        {
#if false
            try
            {


                var req = new Logitude.CustomsMessaging.Common.RequestParams.GenericRequestParams()
                {
                    AppicationId = "1-8523",
                    Tenant = 1,
                    LoggingObjectTableId = "1-4985",
                    LoggingEnabled = true,
                    LoggingEntityId = "1-121",
                    LoggingUserId = ""
                };

                req.LoggingEnabled = true;
                req.LoggingEntityId = "1-8523";
                req.LoggingEntityReference = "14010213755911";
                req.LoggingObjectTableId = "1-4985";
                req.LoggingUserId = "1-5341";
                req.RequestName = "CustomsDocument Request";
                req.ResponseName = "CustomsDocument Response";
                req.Tenant = 1;
                req.AppicationId = "1-8523";
                req.InterfaceTypeCode = "1000";
                req.RequestVIA = Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.DCABatch;
                //if (false)//db check 
                //{
                //    var customsRequestsSheetService = CustomsRequestsSheetService
                //        .CreateNew<Logitude.CustomsMessaging.Common.RequestParams.GenericRequestParams>(req);
                //    var reqSheet = customsRequestsSheetService.GetRequestSheet();
                //   NetCommonHelper.Logger.DevLog.Instance.WriteDebug("reqSheet.Id" + reqSheet.Id);
                //    var retrieved = CustomsRequestsSheetService
                //        .Seed<Logitude.CustomsMessaging.Common.RequestParams.GenericRequestParams>(
                //        //"1-8"
                //        reqSheet.Id, 1);
                //    //var retrievedRequestParams = retrieved.RequestParams;
                //    retrieved.StartStep(CustomsRequestStepEnum.CustomRequest);
                //    retrieved.EndStep(new System.IO.MemoryStream());    
                //}
                
                var messService = new Logitude.CustomsMessaging.MessagingServices.D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityMessagingService();
                var responseData = messService.SendSheet(req);
               NetCommonHelper.Logger.DevLog.Instance.WriteDebug("reqSheet.Id" + responseData.CustomsRequestsSheetId);
                var messService1 = new Logitude.CustomsMessaging.MessagingServices.D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityMessagingService();
                var newTaa = messService1.SendSheet(req.Tenant, responseData.CustomsRequestsSheetId);

                var messService4 = new Logitude.CustomsMessaging.MessagingServices.DCAInCH_NG_190_MSG1_NoticeToClientMessagingService();
                var messService411 = messService4.SendSheet(1, "1-25");
            }
            catch (Exception)
            {

                throw;
            }
#endif

        }
        private void TestCustomsRequestsSheetService10000()
        {
            try
            {


                var req = new Logitude.CustomsMessaging.Common.RequestParams.GenericRequestParams()
                {
                    AppicationId = "1-8523",
                    Tenant = 1,
                    LoggingObjectTableId = "1-4985",
                    LoggingEnabled = true,
                    LoggingEntityId = "1-121",
                    LoggingUserId = ""
                };

                req.LoggingEnabled = true;
                req.LoggingEntityId = "1-117";
                req.LoggingEntityReference = "14040213755762";
                req.LoggingObjectTableId = "1-4985";
                req.LoggingUserId = "1-5341";
                req.RequestName = "CustomsDocument Request";
                req.ResponseName = "CustomsDocument Response";
                req.Tenant = 1;
                req.AppicationId = "1-117";
                req.InterfaceTypeCode = "10000";
                req.RequestVIA = Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.DCABatch;
                //if (false)//db check 
                //{
                //    var customsRequestsSheetService = CustomsRequestsSheetService
                //        .CreateNew<Logitude.CustomsMessaging.Common.RequestParams.GenericRequestParams>(req);
                //    var reqSheet = customsRequestsSheetService.GetRequestSheet();
                //   NetCommonHelper.Logger.DevLog.Instance.WriteDebug("reqSheet.Id" + reqSheet.Id);
                //    var retrieved = CustomsRequestsSheetService
                //        .Seed<Logitude.CustomsMessaging.Common.RequestParams.GenericRequestParams>(
                //        //"1-8"
                //        reqSheet.Id, 1);
                //    //var retrievedRequestParams = retrieved.RequestParams;
                //    retrieved.StartStep(CustomsRequestStepEnum.CustomRequest);
                //    retrieved.EndStep(new System.IO.MemoryStream());    
                //}

                var messService = new Logitude.CustomsMessaging.MessagingServices.DF_MSG10000_ImportDeclarationMessagingService();
                var responseData = messService.SendSheet(req);
               NetCommonHelper.Logger.DevLog.Instance.WriteDebug("reqSheet.Id" + responseData.CustomsRequestsSheetId);
                var messService1 = new Logitude.CustomsMessaging.MessagingServices.DF_MSG10000_ImportDeclarationMessagingService();
                var newTaa = messService1.SendSheet(req.Tenant, responseData.CustomsRequestsSheetId);

                //var messService4 = new Logitude.CustomsMessaging.MessagingServices.DCAInCH_NG_190_MSG1_NoticeToClientMessagingService();
                //var messService411 = messService4.SendSheet(1, "1-25");
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {

            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {

                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }



        public override void WorkOnce()
        {
            
            try
            {
                OnStart();
                WorkUntilQEmpty();
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole" + this.GetType().Name, " : Run() Method", null);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                _OnStartDone = false;
            }
        }

        void WorkUntilQEmpty()
        {
            List<long> deferredSequenceNumbers = new List<long>();
            bool proccesDone = false;
            for (int filtterPriority = 2; filtterPriority < 3; filtterPriority++)
            {
                while (true)
                {
                    BrokeredMessage receivedMessage = _QueueClient.Receive(TimeSpan.FromSeconds(1));

                    if (receivedMessage == null)
                    {
                        break;
                    }
                    if (this.DebugMode)
                    {
                        if (!receivedMessage.GetProperty<bool>(QueueExt.QueuePropertyNames.DebugMode, false))
                        {
                            ///receivedMessage.SafeAbandon();
                            continue;
                        }
                    }
                    //if (receivedMessage.DeliveryCount > 6) // default max DeliveryCount ==10
                    //{
                    //    receivedMessage.DeadLetter();
                    //    receivedMessage.Dispose();
                    //}


                    var lastExecAt = receivedMessage.GetProperty<DateTime>(QueueExt.QueuePropertyNames.LastExecAt, DateTime.MinValue);


                    if (receivedMessage.GetProperty<int>(QueueExt.QueuePropertyNames.Priority, 1) > filtterPriority
                        )
                    {
                        receivedMessage.SafeAbandon();
                        continue;
                    }
                    if (DateTime.UtcNow.Subtract(lastExecAt) < TimeSpan.FromMinutes(2))
                    {
                        // lock message for 5 min 
                        continue;
                    }

                    proccesDone = true;
                    ProcessMessage(receivedMessage);
                }
            }

            // Process the low-priority messages: 
            foreach (long sequenceNumber in deferredSequenceNumbers)
            {
                ProcessMessage(_QueueClient.Receive(sequenceNumber));
            }


            if (proccesDone) return;
            return;
            while (true)
            {
                BrokeredMessage msg = _DeadletterQueueClient.Receive();

                if (msg == null)
                {
                    break;
                }

                //Console.WriteLine("Deadlettered message.");
                //Console.WriteLine("MessageId:                  {0}", msg.MessageId);
                //Console.WriteLine("DeliveryCount:              {0}", msg.DeliveryCount);
                //Console.WriteLine("EnqueuedTimeUtc:            {0}", msg.EnqueuedTimeUtc);
                //Console.WriteLine("Size:                       {0} bytes", msg.Size);
                //Console.WriteLine("DeadLetterReason:           {0}",
                //    msg.Properties["DeadLetterReason"]);
                //Console.WriteLine("DeadLetterErrorDescription: {0}",
                //    msg.Properties["DeadLetterErrorDescription"]);
                //Console.WriteLine();
                msg.Complete();
            }


        }

        private void ProcessMessage(BrokeredMessage message)
        {

            try
            {

                var analyzeClass = message.GetProperty<string>(QueueExt.QueuePropertyNames.InterfaceTypeCode, "");//, "Logitude.CustomsMessaging.MessagingServices.DF_MSG10000_ImportDeclarationMessagingService");
                if (String.IsNullOrWhiteSpace(analyzeClass))
                {
                    message.SafeComplete();
                    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method :analyzeClass ==null", null);
                    //message.DeadLetter();
                    return;//
                }

                //var reqXml = message.GetBody<string>();

                //if (String.IsNullOrWhiteSpace(reqXml))
                //{
                //    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method:reqXml=null");
                //    //message.DeadLetter();
                //    return;
                //}
                if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>(analyzeClass))
                {
                    message.SafeComplete();
                    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage():!ContainerAccessor.Container.IsRegistered :analyzeClass=" + analyzeClass, null);
                    //message.DeadLetter();
                    return;
                }
                //var id = message.CorrelationId ?? message.SequenceNumber.ToString() ; //.GetHashCode().ToString();
                //var messagingBatchService = MessagingBatchService.TryGet(id);
                //ProcessState processState = ProcessState.Start;
                var tenant = message.GetProperty<int>(QueueExt.QueuePropertyNames.Tenant, -1);
                if (tenant == -1)
                {
                    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method :tenant==-1", null);
                    return ;
                }
                
                var anaO = ContainerAccessor.Container.Resolve<IMessagingServiceInterfaceType>(analyzeClass);

                //anaO.SendBatchStateMachine(reqXml, messagingBatchService);

                var resDat = anaO.SendSheet(tenant, message.CorrelationId);

                message.SafeComplete();

            }
             catch (CustomsRequestsSheetDomainModelServiceException customsRequestsSheetServiceException)
            {
                ExceptionHandler.HandleException(customsRequestsSheetServiceException, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method/CustomsRequestsSheetServiceException ",null);
                if (customsRequestsSheetServiceException.What2Do == CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue)
                {
                    message.SafeComplete();
                }
                else
                {
                    message.SetProperty<DateTime>(QueueExt.QueuePropertyNames.LastExecAt, DateTime.UtcNow);
                    message.SafeAbandon();
                }

            }
            catch (Exception ex)
            {

                 ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method", null);
                if (message.Properties.Keys.Contains("CommunicationLogId"))
                {

                    message.SafeComplete();
                    if (false)
                    {
                        string communicationLogId = message.Properties["CommunicationLogId"].ToString();
                        if (communicationLogId != null)
                        {
                            message.SafeAbandon();
                        }
                        else
                        {
                            message.SafeComplete();
                        }    
                    }  
                    
                }
                else
                {
                    message.SafeComplete();
                }
                //throw;
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method", null);
            }
        }

    }
#endif
}
