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
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace WebFreight.Web.Helpers
{
    public class TFSAnalyzer
    {
        private AnalyzeQueue myAnalyzeQueue;
        private AnalyzeQueueRepository analyzeQueueRepository;
        TFSResponse response;
        CommunicationLogRepository myCommunicationLogRepository;
        TFSParseWebhook TFSWebhook;
        string WiId;
        int Tenant;

        public TFSAnalyzer(AnalyzeQueue analyzeQueue, AnalyzeQueueRepository analyzeQueueRepository)
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
                if (Stringdetails.Contains("<TFSResponse"))
                {
                    MemoryStream memorystream = new MemoryStream(myAnalyzeQueue.MessageBody);
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(TFSResponse));
                    response = (TFSResponse)serializer.Deserialize(memorystream);
                }

                else
                {
                    dynamic data = JObject.Parse(Stringdetails);
                    if (data != null)
                    {
                        if (data.resource != null)
                        {
                            response = new TFSResponse();

                            if (data.resource != null)
                            {
                                this.WiId = data.resource.workItemId;
                                response.WorkItemId = data.resource.workItemId;
                                response.UniqueName = data.resource.revisedBy != null ? data.resource.revisedBy.uniqueName : "";
                                response.ChangedDate = data.resource.revision != null ? data.resource.revision.fields["System.ChangedDate"] : "";
                                response.CreatedBy = data.resource.revision != null ? data.resource.revision.fields["System.CreatedBy"] : "";
                                response.ChangedBy = data.resource.revision != null ? data.resource.revision.fields["System.ChangedBy"] : "";
                                response.AssignedTo = data.resource.revision != null ? data.resource.revision.fields["System.AssignedTo"] : "";
                                response.TaskState = data.resource.revision != null ? data.resource.revision.fields["System.State"] : "";
                                response.WorkItemType= data.resource.revision != null ? data.resource.revision.fields["System.WorkItemType"] : "";
                                response.Area = data.resource.revision != null ? data.resource.revision.fields["System.AreaPath"] : "";

                                if (response.CreatedBy != null && response.CreatedBy.Contains('<'))
                                {
                                    response.CreatedBy = response.CreatedBy.Split('<')[1].Split('>')[0];
                                }

                                if (response.ChangedBy != null && response.ChangedBy.Contains('<'))
                                {
                                    response.ChangedBy = response.ChangedBy.Split('<')[1].Split('>')[0];
                                }

                                if (response.AssignedTo != null && response.AssignedTo.Contains('<'))
                                {
                                    response.AssignedTo = response.AssignedTo.Split('<')[1].Split('>')[0];
                                }

                                response.Description = data.resource.revision != null ? data.resource.revision.fields["System.Description"] : "";
                                var remainingWorkOld = 0.0;
                                var remainingWorkNew = 0.0;
                                if (data.resource.fields != null)
                                {
                                    response.IterationPath = data.resource.revision != null ? data.resource.revision.fields["System.IterationPath"] : "";

                                    var remainingWork = data.resource.fields["Microsoft.VSTS.Scheduling.RemainingWork"];
                                    if (remainingWork != null)
                                    {
                                        if (remainingWork["oldValue"] != null)
                                        {
                                            remainingWorkOld = remainingWork["oldValue"];
                                        }
                                        if (remainingWork["newValue"] != null)
                                        {
                                            remainingWorkNew = remainingWork["newValue"];
                                        }


                                        if (remainingWorkOld != remainingWorkNew)
                                        {
                                            response.RemainingWork = remainingWorkNew;
                                        }
                                    }
                                }
                                if (data.resource.revision.relations != null)
                                {
                                    response.Relations = JsonConvert.DeserializeObject<RelationClass[]>(data.resource.revision.relations.ToString());
                                }
                            }
                        }
                    }
                }

                if(response != null)
                {
                    this.AnalyzeData(myAnalyzeQueue.From);
                }
            }

            catch (Exception ex)
            {
                myAnalyzeQueue.Status = "F";
                myAnalyzeQueue.ErrorMessage = "TFS Page Load failed: " + ex.Message;
                myAnalyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(myAnalyzeQueue.Tenant);
                analyzeQueueRepository.Update(myAnalyzeQueue);
                analyzeQueueRepository.SubmitChanges();
                return;
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

                bool check = HasTMEmplyeeTimeAnalyzeQueueId();
                if (!check)
                {
                    TFSWebhook = new TFSParseWebhook(response, myAnalyzeQueue.Id, myAnalyzeQueue.Tenant);
                }

                myAnalyzeQueue.AWBNumber = this.WiId;
                myAnalyzeQueue.Log = this.WiId != null ? ("Task " + this.WiId + " Updated") : "";
                myAnalyzeQueue.Subject = TFSWebhook?.projectNo;
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


    public class TFSResponse
    {
        // Resource
        public string WorkItemId { get; set; }
        public string WorkItemType { get; set; }
        public string UniqueName { get; set; }
        public string ProjectNumber { get; set; }

        //revision / fields
        public DateTime ChangedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ChangedBy { get; set; }
        public string AssignedTo { get; set; }
        public string Description { get; set; }
        public double? RemainingWork { get; set; }
        public string IterationPath { get; set; }
        public string TaskState { get; set; }
        public string Area { get; set; }
        public RelationClass[] Relations { get; set; }
    }

    public class RelationClass
    {
        public string Url { get; set; }
        public string Rel { get; set; } //Link Type
    }
}