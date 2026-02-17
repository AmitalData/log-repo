using Logitude.TimeManagement.Data.Repositories;
using Newtonsoft.Json.Linq;
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
using System.Web;
using System.Xml.Serialization;

namespace WebFreight.Web.Helpers
{
    public class TFSAggregatorAnalyzer
    {
        private AnalyzeQueue myAnalyzeQueue;
        private AnalyzeQueueRepository analyzeQueueRepository;
        string WiId;
        CommunicationLogRepository myCommunicationLogRepository;
        TFSAggregatorParseWebhook TFSWebhook;
        int Tenant;

        public TFSAggregatorAnalyzer(AnalyzeQueue analyzeQueue, AnalyzeQueueRepository analyzeQueueRepository)
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
                string Stringdetails = Encoding.UTF8.GetString(myAnalyzeQueue.MessageBody);
                dynamic data = JObject.Parse(Stringdetails);
                if (data != null)
                {
                    if (data.resource != null)
                    {
                        WiId = data.resource.workItemId;
                    }
                }
            }

            catch (Exception ex)
            {
                myAnalyzeQueue.Status = "F";
                myAnalyzeQueue.ErrorMessage = "TFSAggregator Page Load failed: " + ex.Message;
                myAnalyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(myAnalyzeQueue.Tenant);
                analyzeQueueRepository.Update(myAnalyzeQueue);
                analyzeQueueRepository.SubmitChanges();
                return;
            }

            if (!string.IsNullOrEmpty(WiId))
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


                    TFSWebhook = new TFSAggregatorParseWebhook(WiId, myAnalyzeQueue.Id, myAnalyzeQueue.Tenant);
                
                myAnalyzeQueue.Subject = TFSWebhook.projectNo;
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

        private bool HasTMEmplyeeTimeAnalyzeQueueId()
        {
            TMEmployeeTimeRepository repository = new TMEmployeeTimeRepository(Tenant);
            bool check = repository.GetTMEmployeeTimeByAnalyzeQueueId(myAnalyzeQueue.Id, myAnalyzeQueue.Tenant);
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

            if (myAnalyzeQueue.Status == "F")
            {
                if (myAnalyzeQueue.ConnectedToTenant && myAnalyzeQueue.CommunicationLogId != null)
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
            }

            myAnalyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(myAnalyzeQueue.Tenant);
            analyzeQueueRepository.Update(myAnalyzeQueue);
            analyzeQueueRepository.SubmitChanges();
        }
    }
}