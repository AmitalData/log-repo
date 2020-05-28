using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.SystemLogs;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Helpers;

namespace WebFreight.Web
{
    public partial class BluesnapPaymentLogitude : System.Web.UI.Page
    {
        int Tenant = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {              
                string values = "";             
                using (var reader = new StreamReader(Request.InputStream))
                {
                    values = reader.ReadToEnd();
                }

                if (!string.IsNullOrEmpty(values))
                {
                    this.InsertNewAnalyzeQueue(values);
                }
            }
            catch (Exception errorInfo)
           {
                string errorMessage = errorInfo.Message;
                AzureLog.SaveLogsInStorage("Bluesnap payment logitude error  " + Environment.NewLine + errorMessage, "E", DateTime.Now, errorInfo.Message, errorInfo.StackTrace, 0, null, null, null);
            }
        }


        private void InsertNewAnalyzeQueue(string xmlfileText)
        {

            byte[] messageBytes = Encoding.ASCII.GetBytes(xmlfileText);

            AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();
            AnalyzeQueue analyzeQueue = new AnalyzeQueue()
            {
                CreateDate = TenantServerConfigration.GetCurrentDateTime(0),
                From = "Bluesnap",
                Id = IdCounter.GetNumber("AnalyzeQueue", 0),
                MessageBody = messageBytes,
                Status = "W",
                Retries = 0,
                ConnectedToEntity = false,
                ConnectedToTenant = true,
                FileSize = xmlfileText.Length,
                Tenant = Tenant,
                Subject= "Bluesnap Payment - Logitude"

            };

            analyzeQueue.SearchFields = analyzeQueue.From + ',' + analyzeQueue.Status;
            analyzeQueueReposiory.Add(analyzeQueue);
            analyzeQueueReposiory.SubmitChanges();
        }
     
    }
}
