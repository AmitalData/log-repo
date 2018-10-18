#if false


using Logitude.Customs.BL.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.ResponseServices;
using Logitude.Server.Tools.Counters;
using Logitude.SystemLogs;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

using Logitude.Customs.BL.EntityUpdateServices;
using System.Xml.Linq;
using Microsoft.Practices.Unity;

using Logitude.CustomsMessaging.MessageAnalyzer;
using System.Transactions;
using System.Diagnostics;
//using CustomsWorkerRole.Contracts;


namespace CustomsWorkerRole
{
    public class AnalyzeQueueMessagesWR : WorkerEntryPoint
    {
        static  AnalyzeQueueMessagesWR(){}

        public override void Run()
        {

            while (true)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        WorkOnce();
                        Thread.Sleep(10000);
                    }
                    catch (Exception e)
                    {
                        ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "AnalyzeCustomMessageWR : Run() Method", null);
                        Thread.Sleep(10000);
                    }
                }
                else
                {
                    Thread.Sleep(60000);
                }

            }

        }

        private void AnalyzeQueueCustomMessage(AnalyzeQueue analyzeQueue, AnalyzeQueueRepository analyzeQueueRepository)
        {

            //  TODO: -ask ihab(itzik) -what dont found default  User
          
            
            //if (messageAnalyzerService == null)
            //{
            //    throw new Exception("Message not mapped " + rootName); 
            //}

            

            //XmlNodeList bodyNodeList = xml.GetElementsByTagName("Body");
            //XmlNode bodyNode = bodyNodeList[0];
            //XmlNode messageTypeNode = bodyNode.ChildNodes[0];
            //string messageTitle = xml.DocumentElement.Name;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.Clear();

                    Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("Start AnalyzeQueueCustomMessage :" + analyzeQueue.Id);
                    string result = System.Text.Encoding.UTF8.GetString(analyzeQueue.MessageBody);
                    var myESBResponseParser = new UnifreightIIG.Common.MessageLib.General.ESBResponseParser(result);
                    var messErr = "";
                    if (!myESBResponseParser.Procces(out messErr))
                    {
                        throw new Exception("myESBResponseParser.Procces failed " + messErr);
                    }
                    var reqXml = myESBResponseParser.Body;




                    //var myXDocument = XDocument.Parse(reqXml);
                    //var rootName = myXDocument.Root.Name.LocalName;
                    //var messageAnalyzerService = _UnityContainer.Resolve<IMessageAnalyzerService>(rootName);
                    //messageAnalyzerService.Analyze(reqXml);

                    var myAnalyzeQueueMessage = new AnalyzeQueueMessage();
                    using (TransactionScope scopeAnalyzeQueueCustomMessage = new TransactionScope(TransactionScopeOption.RequiresNew))
                    {

                        myAnalyzeQueueMessage.AnalyzeQueueCustomMessage(reqXml);
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("AnalyzeQueueCustomMessage Done");
                        


                        var messageAnalyzerService = myAnalyzeQueueMessage.MessageAnalyzerService;

                        if (messageAnalyzerService.ResponseData != null && !String.IsNullOrWhiteSpace(messageAnalyzerService.ResponseData.ApplicationID)) // success to connect to entity !!
                        {

                            analyzeQueue.Tenant = messageAnalyzerService.ResolveTenant();
                            //analyzeQueue.EntityReference = shipment.ShipmentNumber;
                            //analyzeQueue.ObjectTableName = "Shipment";
                            analyzeQueue.EntityReference = messageAnalyzerService.ResponseData.ApplicationID;// physicalCheckMessage.NoticeToClient.checkId.ToString();
                            analyzeQueue.ObjectTableName = messageAnalyzerService.GetObjectTableName();
                            analyzeQueue.ConnectedToEntity = true;
                            analyzeQueue.ConnectedToTenant = true;

                            Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("Connecting To Entity  : " + analyzeQueue.ObjectTableName + analyzeQueue.EntityReference);
                            var curCommunicationsParams = new Logitude.Server.Tools.CommunicationsParams()
                            {
                                Tenant = messageAnalyzerService.ResolveTenant(),

                                LoggingObjectTableId = messageAnalyzerService.GetLoggingObjectTableId(),// objectTable.Id,
                                LoggingEntityId = messageAnalyzerService.GetLoggingEntityId(),//_PhysicalCheckPM.Id,
                                LoggingEntityReference = messageAnalyzerService.GetLoggingEntityReference(),//_PhysicalCheckPM.CheckId,

                                Subject = messageAnalyzerService.GetSubject(),// "FU Status",

                                //LoggingUserId = ResolveLoggingUserId(),//loggingUserId,
                                Logs = Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.ToString(7950),
                                ByteData = analyzeQueue.MessageBody,
                                Status = "D",
                                To = "Logitude",
                                CommunicationLogTypeCode = "T",
                                FolderName = "IIGDCA",
                                From = "IIGDCA",
                                InOut = "I",
                            };
                            analyzeQueue.CommunicationLogId = Logitude.Server.Tools.Communications.AddCommunicationLog(curCommunicationsParams);
                            Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("Communication opened : " + analyzeQueue.CommunicationLogId);

                        }

                        scopeAnalyzeQueueCustomMessage.Complete();
                    }
                    analyzeQueue.ErrorMessage = "Success::" + Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.ToString(7920);
                    analyzeQueue.Status = "D";
                    analyzeQueueRepository.Update(analyzeQueue);
                    analyzeQueueRepository.SubmitChanges();

                }
                catch (Exception ex)
                {
                    //Message=Network access for Distributed Transaction Manager (MSDTC) has been disabled. Please enable DTC for network access in the security configuration for MSDTC using the Component Services Administrative tool.
                    //http://stackoverflow.com/questions/10130767/the-transaction-manager-has-disabled-its-support-for-remote-network-transactions
                    

                    analyzeQueue.ErrorMessage = ex.Message + (ex.InnerException != null ? Environment.NewLine + "InnerException: " + ex.InnerException.Message : "") + (ex.StackTrace != null ? Environment.NewLine + "Stack Trace: " + ex.StackTrace : "");
                    analyzeQueue.ErrorMessage = analyzeQueue.ErrorMessage.Length > 7950 ? analyzeQueue.ErrorMessage.Substring(0, 7950) : analyzeQueue.ErrorMessage;
                    var logLength = 7950 - analyzeQueue.ErrorMessage.Length- 5;
                    if (logLength > 1)
                    {
                        analyzeQueue.ErrorMessage += Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.ToString(logLength);
                    }
                    if (ex.Message.StartsWith("--"))// -- means that this error is known, so status will be faild and no need for retries ! ( by jalal)
                    {
                        analyzeQueue.Status = "F";
                    }
                    else
                    {
                        analyzeQueue.Retries++;
                        if (analyzeQueue.Retries == 5)
                        {
                            analyzeQueue.Status = "F";
                        }
                    }

                    analyzeQueueRepository.Update(analyzeQueue);
                    analyzeQueueRepository.SubmitChanges();

                }
                scope.Complete();
            }
            

         
        }

        private string ResolveLoggingUserId(int Tenant)
        {
            string systemEmail = "system@tenant" + Tenant.ToString()  + ".com";
            //var contactQuery = new ContactQuery(tenant);
            var loggedContact = "";
                //contactQuery.GetContactByNameAndTenant(serviceContextUser, tenant, true);
            return loggedContact;

        }

       


        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;


            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
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
            AnalyzeQueueRepository analyzeQueueRepository = new AnalyzeQueueRepository();
            AnalyzeQueue analyzeQueue = analyzeQueueRepository.GetOpenAnalyzeQueue("Customs");
            if (analyzeQueue != null)
            {
                AnalyzeQueueCustomMessage(analyzeQueue, analyzeQueueRepository);
                //Thread.Sleep(10000);
            }
            else
            {
                //Thread.Sleep(10000);
            }
        }
    }
}
#endif