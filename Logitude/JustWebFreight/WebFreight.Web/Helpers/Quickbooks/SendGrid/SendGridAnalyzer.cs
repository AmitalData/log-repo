using Logitude.Server.Tools.CToolWorkflows;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace WebFreight.Web.Helpers.SendGrid
{
    public class SendGridAnalyzer
    {
        private AnalyzeQueue myAnalyzeQueue;
        private AnalyzeQueueRepository analyzeQueueRepository;
        CommunicationLogRepository myCommunicationLogRepository;
        int Tenant;

        public SendGridAnalyzer(AnalyzeQueue analyzeQueue, AnalyzeQueueRepository analyzeQueueRepository)
        {
            if (analyzeQueue != null)
            {
                this.Tenant = analyzeQueue.Tenant;
                this.myAnalyzeQueue = analyzeQueue;
                this.analyzeQueueRepository = analyzeQueueRepository;
                this.myCommunicationLogRepository = new CommunicationLogRepository(this.Tenant);
            }
        }
        public void Run()
        {
            if (myAnalyzeQueue != null)
            {
                this.Deserialize();
            }
        }
        private void Deserialize()
        {
            try
            {
                this.AnalyzeData();
            }

            catch (Exception ex)
            {
                myAnalyzeQueue.Status = "F";
                myAnalyzeQueue.ErrorMessage = "SendGrid Analyzer failed: " + ex.Message;
                myAnalyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(myAnalyzeQueue.Tenant);
                analyzeQueueRepository.Update(myAnalyzeQueue);
                analyzeQueueRepository.SubmitChanges();
                return;
            }
        }

        private void AnalyzeData()
        {
            try
            {
                this.ConnectAnalyzeQueue();
            }
            catch (Exception ex)
            {
                this.OnCatchAnalyzingError(ex);
            }
        }

        private void ConnectAnalyzeQueue()
        {
            try
            {
                if (!myAnalyzeQueue.ConnectedToTenant)
                {
                    myAnalyzeQueue.ConnectedToTenant = true;
                    analyzeQueueRepository.Update(myAnalyzeQueue);
                    analyzeQueueRepository.SubmitChanges();
                }

                if (!myAnalyzeQueue.ConnectedToEntity)
                {
                    myAnalyzeQueue.ConnectedToEntity = true;
                    analyzeQueueRepository.Update(myAnalyzeQueue);
                    analyzeQueueRepository.SubmitChanges();
                }

                // work 
                byte[] messageBytes = myAnalyzeQueue.MessageBody;
                int length = messageBytes.Length;
                XmlDocument xmlDocument = new XmlDocument();
                MemoryStream myMemoryStream = new MemoryStream(messageBytes);
                xmlDocument.Load(myMemoryStream);
                myMemoryStream.Position = 0;
                ICommonDataContext context = CommonDataContext.GetContext(Tenant);
                CommunicationLogRepository commLogrepository = new CommunicationLogRepository(context);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ResponseItem>));
                List<ResponseItem> emailsList = (List<ResponseItem>)xmlSerializer.Deserialize(myMemoryStream);

                if (emailsList != null)
                {
                    if (emailsList.Count == 0)
                    {
                        myAnalyzeQueue.Status = "D";
                        myAnalyzeQueue.ErrorMessage = null;
                        analyzeQueueRepository.Update(myAnalyzeQueue);
                        analyzeQueueRepository.SubmitChanges();
                    }
                    foreach (var item in emailsList)
                    {
                        if (item.CommunicationLogCreateDate != null)
                        {
                            DateTime? createDate = DateTime.ParseExact(item.CommunicationLogCreateDate, "yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                            string communicationLogId = item.CommunicationLogId;
                            if (!string.IsNullOrEmpty(communicationLogId))
                            {
                                var currentLog = commLogrepository.GetSingleCommunicationLog(communicationLogId, Tenant, createDate);
                                var NotifyEmailDelivery = "";

                                if (currentLog != null)
                                {

                                    if (item.Event == "dropped" || item.Event == "bounce")
                                    {
                                        if (string.IsNullOrEmpty(currentLog.EmailDeliveryError))
                                        {
                                            currentLog.EmailDeliveryError = "";
                                        }
                                        currentLog.EmailDeliveryError += item.Email + ": " + item.Reason + "\n";
                                        NotifyEmailDelivery += item.Email + ": " + item.Reason + "\n";
                                    }
                                    else if (item.Event == "deferred")
                                    {
                                        if (string.IsNullOrEmpty(currentLog.EmailDeliveryError))
                                        {
                                            var reason = "Deferred email";
                                            var error = item.Email + ": " + reason + "\n";
                                            currentLog.EmailDeliveryError = error;
                                        }
                                        else
                                        {
                                            List<string> list = currentLog.EmailDeliveryError.Split('\n').ToList();
                                            string email = list.Where(a => a.Contains(item.Email)).FirstOrDefault();
                                            var reason = "Deferred email";
                                            var error = item.Email + ": " + reason + "\n";
                                            if (!string.IsNullOrEmpty(email))
                                            {
                                                currentLog.EmailDeliveryError.Replace(email, error);
                                            }
                                            else
                                            {
                                                currentLog.EmailDeliveryError += error;
                                            }
                                        }
                                    }
                                    else if (item.Event == "delivered")
                                    {
                                        if (!string.IsNullOrEmpty(currentLog.EmailDeliveryError) && currentLog.EmailDeliveryError.Contains(item.Email))
                                        {
                                            List<string> list = currentLog.EmailDeliveryError.Split('\n').ToList();
                                            string email = list.Where(a => a.Contains(item.Email)).FirstOrDefault();
                                            currentLog.EmailDeliveryError.Replace(email, "");
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(NotifyEmailDelivery) && !(!string.IsNullOrEmpty(currentLog.Subject) && currentLog.Subject.Contains("Email Delivery Failure")))
                                    {
                                        this.RunNotifyUser(NotifyEmailDelivery, currentLog.CreatedByUserId, currentLog.Subject, currentLog.Tenant, currentLog.Id);
                                    }

                                    if (currentLog.CommunicationStatusTypeCode != "D")
                                    {
                                        currentLog.CommunicationStatusTypeCode = "D";
                                        currentLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(currentLog.Tenant);
                                        currentLog.DoneDateUTC = DateTime.UtcNow;
                                        currentLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(currentLog.Tenant);
                                        currentLog.LastStatusDateUTC = DateTime.UtcNow;
                                    }

                                    commLogrepository.Update(currentLog);
                                    commLogrepository.SubmitChanges();

                                    if (FeatureToggleHelper.HasFeatureToggle("CTL", currentLog.Tenant))
                                    {
                                        EntityChangesMessageProducer.ProduceSendEmailMessage(currentLog);
                                    }
                                }
                            }
                        }

                        myAnalyzeQueue.Status = "D";
                        myAnalyzeQueue.ErrorMessage = null;
                        analyzeQueueRepository.Update(myAnalyzeQueue);
                        analyzeQueueRepository.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                this.OnCatchAnalyzingError(ex);
            }
        }

        private void RunNotifyUser(string emailDeliveryError, string createdByUserId, string subject,int tenant, string currentLogId)
        {
            try
            {
                IQueueService queueservice = QueueServiceManager.GetQueueService("sendgridnotifyqueue", Tenant);
                Dictionary<string, string> param = new Dictionary<string, string>() { { "Tenant", tenant.ToString() }, { "EmailDeliveryError", emailDeliveryError }, { "CreatedByUserId", createdByUserId }, { "EmailSubject", subject }, { "CurrentLogId", currentLogId } };
                queueservice.Send(param, tenant);
            }
            catch (Exception ex)
            {

            }
        }
        private void OnCatchAnalyzingError(Exception ex)
        {
            myAnalyzeQueue.ErrorMessage = ex.Message + (ex.InnerException != null ? Environment.NewLine + "InnerException: " + ex.InnerException.Message : "");
            myAnalyzeQueue.StackTrace = (ex.StackTrace != null ? Environment.NewLine + "Stack Trace: " + ex.StackTrace : "");
            myAnalyzeQueue.ErrorMessage = myAnalyzeQueue.ErrorMessage.Length > 7950 ? myAnalyzeQueue.ErrorMessage.Substring(0, 7950) : myAnalyzeQueue.ErrorMessage;
            myAnalyzeQueue.StackTrace = myAnalyzeQueue.StackTrace.Length > 7950 ? myAnalyzeQueue.StackTrace.Substring(0, 7950) : myAnalyzeQueue.StackTrace;

            if (ex.Message.StartsWith("--"))
            {
                myAnalyzeQueue.Status = "F";
            }

            else
            {
                myAnalyzeQueue.Retries++;

                if (myAnalyzeQueue.Retries >= 5)
                {
                    myAnalyzeQueue.Status = "F";

                    if (myAnalyzeQueue.ConnectedToTenant)
                    {
                        CommunicationLog commLog = myCommunicationLogRepository.GetSingleCommunicationLog(myAnalyzeQueue.CommunicationLogId, Tenant);
                        if (commLog != null)
                        {
                            commLog.CommunicationStatusTypeCode = "F";
                            commLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                            commLog.LastStatusDateUTC = DateTime.UtcNow;
                            commLog.ExceptionMessage = myAnalyzeQueue.ErrorMessage;

                            if (myAnalyzeQueue.StackTrace != null)
                            {
                                commLog.ExceptionMessage = commLog.ExceptionMessage + Environment.NewLine + "Stack Trace: " + myAnalyzeQueue.StackTrace;
                            }

                            myCommunicationLogRepository.Update(commLog);
                            myCommunicationLogRepository.SubmitChanges();

                            if (FeatureToggleHelper.HasFeatureToggle("CTL", commLog.Tenant))
                            {
                                EntityChangesMessageProducer.ProduceSendEmailMessage(commLog);
                            }
                        }
                    }
                }
            }
            myAnalyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(myAnalyzeQueue.Tenant);
            analyzeQueueRepository.Update(myAnalyzeQueue);
            analyzeQueueRepository.SubmitChanges();
        }
    }
}