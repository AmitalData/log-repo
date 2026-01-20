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
using Logitude.Customs.BL.Messaging.Customs.PerformanceLogger;
using Logitude.Server.Tools;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;

namespace CustomsWorkerRole
{
    public class SendDataToExternalServicesWR : CustomsWorkerEntryPoint
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
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                    }
                    catch (Exception e)
                    {
                        ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "SendDataToAmitalWR : Run() Method", null);
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
        private string myClass;
        private CustomDbQueueService _CustomDbQueueService;
        public override bool OnStart()
        {
            try
            {
                if (_OnStartDone) return true;
                _OnStartDone = true;
                DoneItemsInRange = new Dictionary<DateTime, int>();
                string emailQueueName = ThreadedRoleEntryPoint.GetQueueByEnviroment(SBQueueNames.SendDataToExternalServicesBQ.ToString()); //Amitalqueue
                                                                                                                                           //if (StorageAcountDetails.NameSpaceManager.QueueExists(emailQueueName))
                                                                                                                                           //{
                                                                                                                                           //    StorageAcountDetails.NameSpaceManager.DeleteQueue(emailQueueName);
                                                                                                                                           //}


                myClass = this.GetType().Name;
                if(this.BatchServiceCode == "CustomsCommandAnalyzeResponseWR_Group1")
                {
                    var dd = "";
                }

                _CustomDbQueueService = new CustomDbQueueService(SBQueueNames.SendDataToExternalServicesBQ.ToString(), SettingUtil.GetTenantDBFromConfig(), queueDefinitionCode: this.BatchServiceCode );

                int tenantConfig = SettingUtil.GetTenantDBFromConfig();
                var customsEnvironmentSettingQueryService = new CustomsEnvironmentSettingQueryService(tenantConfig);
                var customsEnvironmentSettingPM = customsEnvironmentSettingQueryService.GetEnvironmentSettingPM(tenantConfig) ?? new CustomsEnvironmentSettingPM();

                if (CustomDbQueueService.SupportedRabbitMQList.Contains(SBQueueNames.SendDataToExternalServicesBQ.ToString()) && CustomDbQueueService.IsFeatureOnRABBITMQ_Communication() && customsEnvironmentSettingPM.UseRabbitMQ)
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

        public bool OnStart_old()
        {
            try
            {
                if (_OnStartDone) return true;
                _OnStartDone = true;
                DoneItemsInRange = new Dictionary<DateTime, int>();
                string emailQueueName = ThreadedRoleEntryPoint.GetQueueByEnviroment(SBQueueNames.SendDataToExternalServicesBQ.ToString()); //Amitalqueue
                //if (StorageAcountDetails.NameSpaceManager.QueueExists(emailQueueName))
                //{
                //    StorageAcountDetails.NameSpaceManager.DeleteQueue(emailQueueName);
                //}
                if (LogitudeSettings.QueueServiceMode != "db")
                {
                    var testDelete = false;
                    if (testDelete)
                    {
                        StorageAcountDetails.NameSpaceManager.DeleteQueue(emailQueueName);
                    }

                    if (!StorageAcountDetails.NameSpaceManager.QueueExists(emailQueueName))
                    {
                        _QueueDescription = new QueueDescription(emailQueueName);
                        _QueueDescription.MaxSizeInMegabytes = 5120;
                        _QueueDescription.LockDuration = TimeSpan.FromMinutes(5);//due debug + raise after 5 min !!
                        // queueDescription.DefaultMessageTimeToLive = new TimeSpan(3, 1, 0);
                        _QueueDescription.MaxDeliveryCount = 100;
                        StorageAcountDetails.NameSpaceManager.CreateQueue(_QueueDescription);
                    }

                    _QueueClient = StorageAcountDetails.CreateServiceBusQueueClient(emailQueueName);
                    if (false)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            var m = new BrokeredMessage();
                            m.Properties["itzikTest"] = i;
                            m.Properties["CommunicationLogId"] = "1-5512";
                            m.Properties["Tenant"] = 1;
                            _QueueClient.Send(m);
                        }
                    }
                    //RoleEnvironment.Changing += RoleEnvironmentChanging;
                }
                else
                {
                    var myClass = this.GetType().Name;
                    _CustomDbQueueService = new CustomDbQueueService(SBQueueNames.SendDataToExternalServicesBQ.ToString(), SettingUtil.GetTenantDBFromConfig(), queueDefinitionCode: this.BatchServiceCode);
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


        //private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        //{

        //    // If a configuration setting is changing
        //    if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
        //    {

        //        // Set e.Cancel to true to restart this role instance
        //        e.Cancel = true;
        //    }
        //}
#if false
        

        ICommonDataContext context;
        private void SendCommunicationLog(string communicationLogId, int tenant, AmitalStandardCommunicationModel myAmitalCommunicationModel)
        {
            context = CommonDataContext.GetContext(tenant);
            CommunicationLogRepository communicationLogRep = new CommunicationLogRepository(context);
            CommunicationLog cl = communicationLogRep.GetSingleCommunicationLog(communicationLogId, tenant);
            var ExceptionMessage="";
            
            if (cl.Retries == null)
            {
                cl.Retries = 0;
            }
            try
            {
                if (cl.Retries < 5)
                {
                    SendWaitingCommunicationLog(cl, myAmitalCommunicationModel);
                    //cl.ExceptionMessage 
                    //cl.ExternalDocument  
                    //cl.Logs  
                    cl.CorrelationID  
                    cl.CommunicationStatusTypeCode = "D";
                }

                else
                {
                    cl.CommunicationStatusTypeCode = "F";
                }

                if (context != null)
                {
                    CommunicationLogRepository commLogrepository = new CommunicationLogRepository(context);
                    commLogrepository.Update(cl);
                    commLogrepository.SubmitChanges();
                }


            }

            catch (Exception exc)
            {
                ExceptionHandler.HandleException(exc, DateTime.Now, tenant, "", "WorkerRole", "");

                cl.Retries++;
                if (context != null)
                {
                    CommunicationLogRepository commLogrepository = new CommunicationLogRepository(context);
                    commLogrepository.Update(cl);
                    commLogrepository.SubmitChanges();
                }
                throw;

            }
        }

        public void SendWaitingCommunicationLog(CommunicationLog waitingCommLog, AmitalStandardCommunicationModel myAmitalCommunicationModel)
        {

           
            CommunicationAttachmentRepository communicationAttachmentRep = new CommunicationAttachmentRepository(waitingCommLog.Tenant);

            List<CommunicationAttachment> attachmentsList = communicationAttachmentRep.GetCommunicationAttachmentsForCommLog(waitingCommLog.Id, waitingCommLog.Tenant).ToList();//(from attach in context.CommunicationAttachments
           
            string xmlfile = "";
            if (waitingCommLog != null)
            {
                string filename;
                if (waitingCommLog.Document != null)
                {
                    filename = waitingCommLog.DocumentId + "." + waitingCommLog.Document.Extension;

        Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
                    {
                        FileName = waitingCommLog.Document.Id,
                        FolderName = waitingCommLog.Document.Folder,
                        Extension = waitingCommLog.Document.Extension,
                        Tenant = waitingCommLog.Document.Tenant,
                        FileSize = waitingCommLog.Document.FileSize,
                    };
                    
                      Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                     byte[] bytedata = storageservice.Read(fileInfo);
                     xmlfile = encoding.GetString(bytedata);

                  

                    //CloudBlobContainer blobContainer = StorageAcountDetails.GetCurrentContainer(waitingCommLog.Tenant);
                    //var blobfile = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(filename, waitingCommLog.Document.Folder));

                   
                    //using (MemoryStream memstream = new MemoryStream())
                    //{
                    //    blobfile.DownloadToStream(memstream);
                    //    Encoding encoding = Encoding.UTF8;
                    //    xmlfile = encoding.GetString(memstream.ToArray());

                    //}

                    SendMessageToUServer(xmlfile, myAmitalCommunicationModel);
                   
                }
            }
        }

        private void SendMessageToUServer(string xmlfile,  AmitalStandardCommunicationModel myAmitalCommunicationModel)
        {
            //implement the code to send the xml file to amital;
        
            string P_MOREPARAMS = "";
            string P_XML_DATA = "";
            string P_MESSAGE = "";
            if (String.IsNullOrWhiteSpace(xmlfile))
            {
                throw new ArgumentNullException("SendFileToAmitalService():xmlfile is null");
            }
            var myParams = new Hashtable();
            myParams.Add("componentname", "GWSFLOGITUDE");
            myParams.Add("Operation", "AnalyzeStandard");
            if (myAmitalCommunicationModel.UnifaceMethodType ==   AmitalStandardCommunicationModel.OperationMethod.DataAccess )
            {
                myParams["Operation"]="DataAccess";
                myParams["GWSFLOGITUDE:componentname"] = myAmitalCommunicationModel.UnifaceComponentName;
                myParams["GWSFLOGITUDE:operation"] = myAmitalCommunicationModel.UnifaceOperation ;
            }
            



            myParams["GWSFLOGITUDE:Xml"] = xmlfile;
            //myParams.Add("GWSFBLO:UnifreightUserID", p_user);
            var myUServerDNS = UnifreightIIGCommonUtil.GetTenantSetting().UServerDNS;
            var myUServerPort = UnifreightIIGCommonUtil.GetTenantSetting().UServerPort;
            var myUServerUtil = new UServerUtil(myUServerDNS, myUServerPort); ;
            myUServerUtil.DoIt(myParams, ref P_MOREPARAMS, out P_XML_DATA, out P_MESSAGE);
            if (String.IsNullOrWhiteSpace(P_XML_DATA))
            {
                throw new Exception("UServer did not return response : " + P_MESSAGE);
            }
           NetCommonHelper.Logger.DevLog.Instance.WriteDebug(myUServerUtil.UnifreightTester); 
            // var UNIQUE_ENVIRONMENT_ID = UnifaceAssociativeListUtil.GetValue(P_XML_DATA, "UNIQUE_ENVIRONMENT_ID");
            

        }
#endif


        public override void WorkOnce()
        {
            
            try
            {
                OnStart();
                //if (LogitudeSettings.QueueServiceMode != "db")
                //{
                //    WorkUntilQEmpty();
                //}
                //else
                //{
                //    WorkUntilQEmpty_Db();
                //}
                switch (base.WorkerQueueType)
                {

                    case WorkerQueueType.RabbitMQ:
                        WorkUntilPrcossesStop_RabbitMQ();
                        break;
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
        private void WorkUntilPrcossesStop_RabbitMQ()
        {
            var rabbitMQConsumerService = new RabbitMQConsumerService(myClass, SBQueueNames.SendDataToExternalServicesBQ.ToString());
            rabbitMQConsumerService.WorkUntilPrcossesStop_RabbitMQ(
                (CustomDBQueueMessage customDBQueueMessage) =>
                {
                    ///var receivedMessage= customDBQueueMessage.MyQueueResponse;




                    LogMessagingUtil.Instance
                        .AppendLine("SendDataToExternalServicesWR:ProccessReceivedMessage()")
                        .AppendLine("QUEUEMessageId:" + customDBQueueMessage.MessageId)
                        .AppendLine("RetryNumber:" + customDBQueueMessage.Retries);
                        
                        


                    LastActivity = DateTime.UtcNow;

                    var mySender = new SendDbQueueMessage2Unifreight(customDBQueueMessage, fromRabitHandler: true);
                    mySender.ProccessReceivedMessage();


                    LogDoneItemInMemory();
                    

                    return true;
                },
                base.LogDoneItemInMemory
                );
        }
        private void WorkUntilQEmpty_Db()
        {
            while (!WorkerRoleServiceLocator.PleaseShutDown)
            {
                
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {

                        CustomDBQueueMessage receivedMessage = null;

                        using (TransactionScope scopeRecive = TransactionFactory.GetNewReadCommittedTransaction())
                        {
                            receivedMessage = _CustomDbQueueService.Receive(CustomsWorkerRole.Utils.GenUtil.GetQueueTimeOutInMin() * 60);
                            scopeRecive.Complete();
                        }

                        if (receivedMessage == null || String.IsNullOrWhiteSpace(receivedMessage.MessageId))
                        {
                            //Thread.Sleep(TimeSpan.FromSeconds(5));
                            QueueThreadStateService.Upsert(QueueThreadStateService.GetWRKey(this.GetType().Name), "Sleep...");
                            //Thread.Sleep(TimeSpan.FromSeconds(15));//not using soo mach 
                            Thread.Sleep(TimeSpan.FromSeconds(CustomsWorkerRole.Utils.GenUtil.IfNoQueue_ServerWaitTimeInSec()));
                            break;
                        }

                        LastActivity = DateTime.UtcNow;
                        ProcessMessage_Db(receivedMessage);
                        scope.Complete();
                        LogDoneItemInMemory();
                    }
                }
                finally
                {
                    PerformanceM.SleepMSAfterEachQueuePeek();
                }
            }        
        }

       
        void WorkUntilQEmpty()
        {
            List<long> deferredSequenceNumbers = new List<long>();
            
                for (int filtterPriority = 2; filtterPriority < 3; filtterPriority++)
                {
                    while (true)
                    {
                       
                        BrokeredMessage receivedMessage = _QueueClient.Receive(TimeSpan.FromSeconds(5));

                        if (receivedMessage == null)
                        {
                            break;
                        }

                        // Low-priority messages will be dealt with later: 
                        //if (receivedMessage.Properties["Priority"] == "Low")
                        //{
                        //    receivedMessage.Defer();
                        //    Console.WriteLine("Deferred message with id {0}.", receivedMessage.MessageId);
                        //    // Deferred messages can only be retrieved by message receipt. Here, keeping track of the 
                        //    // message receipt for a later retrieval: 
                        //    deferredSequenceNumbers.Add(receivedMessage.SequenceNumber);
                        //    continue;
                        //}

                        var lastExecAt = receivedMessage.GetProperty<DateTime>(QueueExt.QueuePropertyNames.LastExecAt, DateTime.MinValue);

                        if (
                            DateTime.UtcNow.Subtract(lastExecAt) < TimeSpan.FromMinutes(2) ||
                            (receivedMessage.GetProperty<int>(QueueExt.QueuePropertyNames.Priority, 1) > filtterPriority)
                            )
                        {
                            receivedMessage.SafeAbandon();
                            continue;
                        }
                        

                        ProcessMessage(receivedMessage);
                    }        
                }
            
            // Process the low-priority messages: 
            foreach (long sequenceNumber in deferredSequenceNumbers)
            {
                ProcessMessage(_QueueClient.Receive(sequenceNumber));
            } 
        }

        private void ProcessMessage(BrokeredMessage message)
        {
            
            try
            {
                var mySender = new SendMessage2Unifreight(message);
                //var success = mySender.Parse();//crash if failed
                mySender.ProccessReceivedMessage();
                
                

            }
            catch (Exception ex)
            {
                
                if (message.Properties.Keys.Contains("CommunicationLogId"))
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
                else
                {
                    message.SafeComplete();
                }
                //throw;
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", "SendDataToExternalServicesWR : ProcessMessage() Method", null);
            }
        }
        private void ProcessMessage_Db(CustomDBQueueMessage message)
        {
            try
            {

                var mySender = new SendDbQueueMessage2Unifreight(message, fromRabitHandler: false);
                //var success = mySender.Parse();//crash if failed
                mySender.ProccessReceivedMessage();



            }
            catch (Exception ex)
            {

                if (message.Properties.Keys.Contains("CommunicationLogId"))
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
                else
                {
                    message.SafeComplete();
                }
                //throw;
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", "SendDataToExternalServicesWR : ProcessMessage() Method", null);
            }
        }



        
    }
}
