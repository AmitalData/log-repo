using Logitude.Server.Tools;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.SQL;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using WebFreight.Web.Security;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ExternalTasksQueueWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ExternalTasksQueueWcfService.svc or ExternalTasksQueueWcfService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ExternalTasksQueueWcfService : IExternalTasksQueueWcfService
    {

        public string GetTaskFromQueue(int tenant, int priority)
        {
            Envelope envelope = new Envelope();
            CommunicationLog commLog = null;
            QueueResponse queueResponse = null;
			string result = null;
			//BrokeredMessage message = null;
			try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);

                if (CacheManager.CacheWrapper == null)
                {
                    CacheManager.CacheWrapper = new MockCacheWrapper();
                }

                

                string queueName = "externaltasksqueue" + tenant + priority;
                DbQueueService queueservice = new DbQueueService(queueName, tenant);//QueueServiceManager.GetQueueService(queueName, 0);
                queueResponse = queueservice.Receive(new TimeSpan(0, 0, 20));

                // QueueClient client = Communications.GetQueueClient("externaltasksqueue" + tenant + priority);

                //message = client.Receive(new TimeSpan(0, 0, 20));
                if (queueResponse.MessageId != null)
                {
                    //if (message.Properties["CommunicationLogId"] != null)
                    //{
                    string communicationLogId = queueResponse.MessageValues["CommunicationLogId"].ToString();
                    int.TryParse(queueResponse.MessageValues["Tenant"].ToString(), out tenant);

                    //   string communicationLogId = message.Properties["CommunicationLogId"].ToString();
                    //int.TryParse(message.Properties["Tenant"].ToString(), out tenant);
                    ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
                    DocumentRepository documentRepository = new DocumentRepository(commoncontext);
                    CommunicationLogRepository communicationLogRep = new CommunicationLogRepository(commoncontext);
                    commLog = communicationLogRep.GetSingleCommunicationLog(communicationLogId, tenant);
                    //if (commLog == null)
                    //{
                    //    if (response.RetryNumber <= 5)
                    //    {
                    //        int count = 0;
                    //        while (count < 3)
                    //        {
                    //            Thread.Sleep(50);
                    //            commLog = communicationLogRep.GetSingleCommunicationLog(communicationLogId, tenant);
                    //            if (commLog != null)
                    //            {
                    //                break;
                    //            }

                    //            count++;
                    //        }

                    //        if (commLog == null)
                    //        {
                    //            //AzureLog.SaveLogsInStorage("Message abandoned from activation queue (entityPM.CustomerStatusCode != WAC)", "L", DateTime.Now, "", "", 0, loggedContact.Id, loggedContact.EnglishName, HttpContext.Current.Request.UserHostAddress);
                    //            queueservice.CompleteAsFailed();
                    //            return null;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        //AzureLog.SaveLogsInStorage("Message removed from activation queue (entityPM.CustomerStatusCode != WAC)", "L", DateTime.Now, "", "", 0, loggedContact.Id, loggedContact.EnglishName, HttpContext.Current.Request.UserHostAddress);
                    //        queueservice.Complete();
                    //        return null;
                    //    }
                    //}
                    //else
                    //{
                    // Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, message.LockToken.ToString(), "P", "Message retrieved from queue " + DateTime.Now.ToString(), null);
                    if (commLog.CommunicationStatusTypeCode == "D")
                    {
                        Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, queueResponse.MessageId, "D", "Message removed from queue (Communication log status = Done) " + DateTime.Now.ToString(), null);
                        queueservice.Complete();
                    }
                    else
                    {
                        Document document = documentRepository.GetSingleDocument(tenant, commLog.DocumentId);
                        Uploader uploader = new Uploader();

                        byte[] filedata = uploader.DownloadFile(document.Id, document.Extension, document.Folder, tenant);
                        if (filedata != null)
                        {
                            XmlDocument doc = new XmlDocument();
                            MemoryStream ms = new MemoryStream(filedata);
                            doc.Load(ms);
                            //result = doc.InnerXml;

                            List<QueueTask> taskslist = LogitudeXmlSerializer.DeserializeObject<List<QueueTask>>(doc.InnerXml);
                            envelope.CommunicationLogId = communicationLogId;

                            envelope.Tasks = taskslist;


                        }
                        else
                        {
                            envelope.HasError = true;
                            envelope.ErrorMessage = "File Not found";
                            Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, queueResponse.MessageId, "F", "Exception occured while proccessing the queue message " + DateTime.Now.ToString(), envelope.ErrorMessage);
                            queueservice.CompleteAsFailed();

                        }
                        //envelope.Result = message.LockToken.ToString() + "," + communicationLogId;


                        result = LogitudeXmlSerializer.SerializeObjectToXmlString(envelope);

                    }

                    commLog.MessageLockId = queueResponse.MessageId;
                    communicationLogRep.Update(commLog);
                    communicationLogRep.SubmitChanges();
                    //}


                    // }
                    // else
                    // {
                    //    message.Complete();
                    // }

                    // + "," + message.SequenceNumber;
                }



                return result;

            }
            catch (Exception ex)
            {

                envelope.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                envelope.HasError = true;
                envelope.ErrorMessage = ex.Message;
                envelope.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    envelope.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                if (queueResponse != null && commLog != null)
                {
                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, queueResponse.MessageId, commLog.CommunicationStatusTypeCode, "Exception occured while proccessing the queue message " + DateTime.Now.ToString(), envelope.ErrorMessage);

                }

				result = LogitudeXmlSerializer.SerializeObjectToXmlString(envelope);
				return result;

            }
        }


        public Response MarkTaskAsDone(string communicationLogId, int tenant, int priority)
        {
            Response response = new Response();
            CommunicationLog commLog = null;
            try
            {

                SecurityUtility.AuthenticationOnTenant(tenant);

                if (CacheManager.CacheWrapper == null)
                {
                    CacheManager.CacheWrapper = new MockCacheWrapper();
                }

                //QueueClient client = Communications.GetQueueClient("externaltasksqueue" + tenant + priority);
                string queueName = "externaltasksqueue" + tenant + priority;
                DbQueueService queueservice = new DbQueueService(queueName, tenant);

                if (!string.IsNullOrEmpty(communicationLogId))
                {
                    ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
                    CommunicationLogRepository communicationLogRep = new CommunicationLogRepository(commoncontext);
                    commLog = communicationLogRep.GetSingleCommunicationLog(communicationLogId, tenant);
                    //commLog.CommunicationStatusTypeCode = "D";

                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, commLog.MessageLockId, "D", "Start of mark as done " + DateTime.Now.ToString(), null);

                    //Guid lockToken = new Guid(commLog.MessageLockId);
                    //client.Complete(lockToken);
                    queueservice.Complete(commLog.MessageLockId);
                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, commLog.MessageLockId, "D", "End of mark as done " + DateTime.Now.ToString(), null);

                    //commoncontext.SaveChanges();
                }
                else
                {
                    response.HasError = false;
                    response.ErrorMessage = "Invalid communicationLogId";

                }


            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                if (commLog != null)
                {
                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, commLog.MessageLockId, commLog.CommunicationStatusTypeCode, "Exception occured while marking the queue message as done " + DateTime.Now.ToString(), response.ErrorMessage);

                }


                return response;
            }

            return response;
        }



    }
}
