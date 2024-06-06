using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Utils;
using Logitude.SystemLogs;
using Newtonsoft.Json.Linq;
using NPOI.Util;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Xml.Serialization;


namespace WebFreight.Web.Controller
{
    public class SendGridNotificationsController : ApiController
    {
        string values = "";
        int tenant = 0;
        string communicationLogId = "";

        public HttpResponseMessage ReceiveNotification([FromBody] JArray values)

        {
            try
            {
                //using (var reader = new StreamReader(str))
                //{
                //    values = reader.ReadToEnd();
                //}
                var jsonData = values;
            //    JArray jsonData = JArray.Parse(values);
                if (jsonData != null)
                {
                    List<ResponseItem> emailsList = ((JArray)jsonData).Select(x =>
                        new ResponseItem
                        {
                            Email = x["email"] != null ? (x["email"]).ToString() : null,
                            Event = x["event"] != null ? (x["event"]).ToString() : null,
                            Reason = x["reason"] != null ? (x["reason"]).ToString() : null,
                            Response = x["response"] != null ? (x["response"]).ToString() : null,
                            CommunicationLogId = x["CommunicationLogId"] != null ? (x["CommunicationLogId"]).ToString() : null,
                            Tenant = x["Tenant"] != null ? (int)x["Tenant"] : 0,
                            CommunicationLogCreateDate = x["CommunicationLogCreateDate"] != null ? (x["CommunicationLogCreateDate"]).ToString() : null,
                            DeploymentStage = x["DeploymentStage"] != null ? (x["DeploymentStage"]).ToString() : null

                        }).ToList();

                    tenant = emailsList.Select(a => a.Tenant).FirstOrDefault();
                    communicationLogId = emailsList.Where(a => a != null).Select(a => a.CommunicationLogId).FirstOrDefault();
                    string deploymentStage = emailsList.Where(a => a != null).Select(a => a.DeploymentStage).FirstOrDefault();
                    if (deploymentStage == LogitudeSettings.DeploymentStage)
                    {
                        InsertNewAnalyzeQueue(emailsList, tenant);
                    }

                    Logger.LogInfo("הגיע בהצלחה SendGrid {0}", emailsList[0].CommunicationLogId);
                }
            }

            catch (Exception errorInfo)
            {
                string errorMessage = errorInfo.Message;
                AzureLog.SaveLogsInStorage("SendGrid Page error  " + Environment.NewLine + errorMessage, "E", DateTime.Now, errorInfo.Message, errorInfo.StackTrace, 0, null, null, null);
                //throw;

                    Logger.LogInfo("נכשל");
            }

            return Request.CreateResponse();


        }

        private void InsertNewAnalyzeQueue(List<ResponseItem> emailsList, int tenant)
        {
            try
            {
                Type myType = emailsList.GetType();
                MemoryStream myMemoryStream = new MemoryStream();
                XmlSerializer ser = new XmlSerializer(myType);
                ser.Serialize(myMemoryStream, emailsList);
                myMemoryStream.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(myMemoryStream);
                string content = reader.ReadToEnd();
                byte[] bytearray = myMemoryStream.ToArray();
                AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();
                AnalyzeQueue analyzeQueue = new AnalyzeQueue()
                {
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(0),
                    From = "SendGrid",
                    Subject = "SendGrid",
                    Id = IdCounter.GetNumber("AnalyzeQueue", 0),
                    MessageBody = bytearray,
                    Status = "W",
                    Retries = 0,
                    ConnectedToEntity = false,
                    ConnectedToTenant = false,
                    FileSize = bytearray.Length,
                    Tenant = tenant,
                    CommunicationLogId = communicationLogId,

                };

                analyzeQueue.SearchFields = analyzeQueue.From + ',' + analyzeQueue.Status;
                analyzeQueueReposiory.Add(analyzeQueue);
                analyzeQueueReposiory.SubmitChanges();
            }
            catch (Exception errorInfo)
            {
                string errorMessage = errorInfo.Message + (errorInfo.InnerException != null ? (errorInfo.InnerException.InnerException != null ? errorInfo.InnerException.InnerException.Message : errorInfo.InnerException.Message) : "Rabaia");
                AzureLog.SaveLogsInStorage("Send Grid Page error  " + Environment.NewLine + errorMessage, "E", DateTime.Now, errorInfo.Message, errorInfo.StackTrace, 0, null, null, null);
                //throw;
            }
        }
    }

    public class ResponseItem
    {
        public string Email { get; set; }
        public string Event { get; set; }
        public string Reason { get; set; }
        public string Response { get; set; }
        public string CommunicationLogId { get; set; }
        public int Tenant { get; set; }
        public string CommunicationLogCreateDate { get; set; }
        public string DeploymentStage { get; set; }

    }
}
