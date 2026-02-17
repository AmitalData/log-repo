using Logitude.Server.Tools.Counters;
using Logitude.SystemLogs;
using Newtonsoft.Json.Linq;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using WebFreight.Web.Helpers;

using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Globalization;
using WebFreight.Web.Helpers.APIHelpers;

namespace WebFreight.Web
{
    public partial class SendGridWebPage : System.Web.UI.Page
    {
        string values = "";
        int tenant = 0;
        string communicationLogId = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                using (var reader = new StreamReader(Request.InputStream))
                {
                    values = reader.ReadToEnd();
                }
                JArray jsonData = JArray.Parse(values);
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
                            CommunicationLogCreateDate = x["CommunicationLogCreateDate"] != null ? (x["CommunicationLogCreateDate"]).ToString() : null
                        }).ToList();

                    tenant = emailsList.Select(a => a.Tenant).FirstOrDefault();
                    communicationLogId = emailsList.Where(a => a != null).Select(a => a.CommunicationLogId).FirstOrDefault();
                    InsertNewAnalyzeQueue(emailsList, tenant);
                }
            }

            catch (Exception errorInfo)
            {
                string errorMessage = errorInfo.Message;
                AzureLog.SaveLogsInStorage("SendGrid Page error  " + Environment.NewLine + errorMessage, "E", DateTime.Now, errorInfo.Message, errorInfo.StackTrace, 0, null, null, null);
                //throw;
            }
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
    }
}