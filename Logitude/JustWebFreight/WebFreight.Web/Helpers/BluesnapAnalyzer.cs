using Logitude.TimeManagement.Data.Repositories;
using Newtonsoft.Json;
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
using System.Net;
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace WebFreight.Web.Helpers
{
    public class BluesnapAnalyzer
    {
        private AnalyzeQueue myAnalyzeQueue;
        private AnalyzeQueueRepository analyzeQueueRepository;
        CommunicationLogRepository myCommunicationLogRepository;
        int Tenant;
        private DateTime? transactionDate;
        private double? taxAmountUSD;
        private double? invoiceAmountUSD;
        private string contractId;
        private string analyzeQueueSubject;
        Dictionary<string, string> queryParameters;
        string stringdetails;

        public BluesnapAnalyzer(AnalyzeQueue analyzeQueue, AnalyzeQueueRepository analyzeQueueRepository)
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
                this.FillAnalyzeQueueQueryParameters();
                this.ProcessBluesnapTransaction();
                this.AnalyzeData();
            }
            catch (Exception ex)
            {
                myAnalyzeQueue.Status = "F";
                myAnalyzeQueue.ErrorMessage = "Bluesnap Page Load failed: " + ex.Message;
                myAnalyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(myAnalyzeQueue.Tenant);
                analyzeQueueRepository.Update(myAnalyzeQueue);
                analyzeQueueRepository.SubmitChanges();
                return;
            }
        }

        private void ProcessBluesnapTransaction()
        {
            if (queryParameters.Count > 0)
            {
                if (queryParameters.ContainsKey("accountId"))
                {
                    BluesnapExecutionService bluesnapExecutionService = new BluesnapExecutionService(0);
                    TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                    TenantManagement tenantManagement = tenantManagementRepository.GetSingleTenantManagementByBluesnapAccountId(queryParameters["accountId"]);
                    bluesnapExecutionService.Tenant = tenantManagement != null ? tenantManagement.Id : 0;
                    bool isSaveToBluesnapTransaction = IsSaveToBluesnapTransaction(queryParameters);
                    if (isSaveToBluesnapTransaction)
                    {
                        MapReuiredFields(queryParameters);
                        var args = new
                        {
                            transactionDate = transactionDate,
                            taxAmountUSD = taxAmountUSD,
                            invoiceAmountUSD = invoiceAmountUSD,
                            contractId = contractId,
                            stringdetails = stringdetails,
                            analyzeQueueSubject = analyzeQueueSubject,
                        };

                        bluesnapExecutionService.SaveBluesnapTransaction(args);
                    }
                }
            }
        }

        private void FillAnalyzeQueueQueryParameters()
        {
            var values = BluesnapHelper.DeserializeAnalyzeQueueMessageBody(myAnalyzeQueue.MessageBody);
            queryParameters = values.Item1;
            stringdetails = values.Item2;
        }

        private void MapReuiredFields(Dictionary<string, string> queryParameters)
        {
            if (queryParameters.ContainsKey("transactionDate"))
            {
                if (!string.IsNullOrEmpty(queryParameters["transactionDate"]))
                {
                    this.transactionDate = DateTime.Parse(queryParameters["transactionDate"]);
                }
            }

            if (queryParameters.ContainsKey("contractId"))
            {
                if (!string.IsNullOrEmpty(queryParameters["contractId"]))
                {
                    this.contractId = (queryParameters["contractId"]).ToString();
                }
            }

            if (queryParameters.ContainsKey("invoiceAmountUSD"))
            {
                if (!string.IsNullOrEmpty(queryParameters["invoiceAmountUSD"]))
                {
                    this.invoiceAmountUSD = Double.Parse(queryParameters["invoiceAmountUSD"]);
                }
            }

            if (queryParameters.ContainsKey("taxAmountUSD"))
            {
                if (!string.IsNullOrEmpty(queryParameters["taxAmountUSD"]))
                {
                    this.taxAmountUSD = Double.Parse(queryParameters["taxAmountUSD"]);
                }
            }

            this.analyzeQueueSubject = myAnalyzeQueue.Subject == "Bluesnap Payment - Amital" ? "Amital" : "Logitude";
        }

        private bool IsSaveToBluesnapTransaction(Dictionary<string, string> queryParameters)
        {
            bool isSaveToBluesnapTransaction = true;
            if (!queryParameters.ContainsKey("transactionDate"))
            {
                isSaveToBluesnapTransaction = false;
            }

            if (queryParameters.ContainsKey("transactionType"))
            {
                string transactionType = null;
                if (!string.IsNullOrEmpty(queryParameters["transactionType"]))
                {
                    transactionType = (queryParameters["transactionType"]).ToString();
                }
                if (transactionType.Equals("CONTRACT_CHANGE"))
                {
                    isSaveToBluesnapTransaction = false;
                }
            }

            return isSaveToBluesnapTransaction;
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
            }

            myAnalyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(myAnalyzeQueue.Tenant);
            analyzeQueueRepository.Update(myAnalyzeQueue);
            analyzeQueueRepository.SubmitChanges();
        }
    }
}