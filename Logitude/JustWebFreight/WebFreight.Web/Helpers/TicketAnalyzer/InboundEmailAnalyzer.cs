using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web;
using System.Xml.Serialization;

namespace WebFreight.Web.Helpers.TicketAnalyzer
{
    public partial class InboundEmailAnalyzer
    {
        private AnalyzeQueue myAnalyzeQueue;
        private AnalyzeQueueRepository analyzeQueueRepository;

        InboundParseWebhook inboundWebhook;
        EmailUpload EmailDetails;
        CommunicationLogRepository myCommunicationLogRepository;

        int Tenant;
        public InboundEmailAnalyzer(AnalyzeQueue analyzeQueue, AnalyzeQueueRepository analyzeQueueRepository)
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
                MemoryStream memorystream = new MemoryStream(myAnalyzeQueue.MessageBody);
                XmlSerializer serializer = new XmlSerializer(typeof(EmailUpload));
                EmailDetails = (EmailUpload)serializer.Deserialize(memorystream);
            }

            catch (Exception ex)
            {
                myAnalyzeQueue.Status = "F";
                myAnalyzeQueue.ErrorMessage = "Inbound Email Page Load failed: " + ex.Message;
                myAnalyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(myAnalyzeQueue.Tenant);
                analyzeQueueRepository.Update(myAnalyzeQueue);
                analyzeQueueRepository.SubmitChanges();
                return;
            }

            if (EmailDetails != null)
            {
                this.AnalyzeData(myAnalyzeQueue.From);
            }
        }

        private void AnalyzeData(string from)
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

                bool check = HasInbounEmailAnalyzeQueueId();
                if (!check)
                {
                    inboundWebhook = new InboundParseWebhook(EmailDetails, myAnalyzeQueue.Id);
                }

                myAnalyzeQueue.Status = "D";
                myAnalyzeQueue.ErrorMessage = null;
                analyzeQueueRepository.Update(myAnalyzeQueue);
                analyzeQueueRepository.SubmitChanges();
            }

            catch (Exception ex)
            {
                this.OnCatchAnalyzingError(ex);
            }
        }

        private bool HasInbounEmailAnalyzeQueueId()
        {
            InboundEmailRepository repository = new InboundEmailRepository(Tenant);
            bool check = repository.GetInboundEmailByAnalyzeQueueId(myAnalyzeQueue.Id,myAnalyzeQueue.Tenant);
            return check;
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

                }
            }

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
                }
            }
            myAnalyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(myAnalyzeQueue.Tenant);
            analyzeQueueRepository.Update(myAnalyzeQueue);
            analyzeQueueRepository.SubmitChanges();
        }
    }

    public class EmailUpload
    {
        public string Sender { get; set; }

        public string RecipientEmail { get; set; }

        public string Subject { get; set; }

        public string StrippedBodyPlain { get; set; }

        public string FullBodyPlain { get; set; }

        public string To { get; set; }

        public string CCs { get; set; }

        public string StrippedText { get; set; }

        public string StrippedSignature { get; set; }

        public string BodyHtml { get; set; }

        public string StrippedHtml { get; set; }

        public int? AttachmentCount { get; set; }

        public long TimeStampSeconds { get; set; }

        public DateTime TimeStamp { get; set; }

       //public List<HttpPostedFile> AttachmentsFiles { get; set; }
        public List<FileAttachment> AttachmentsFiles { get; set; }
    }

    public class FileAttachment
    {
        public int ContentLength { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public byte[] InputStream { get; set; }
    }
}