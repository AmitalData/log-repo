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
    public partial class TFSWebPage : System.Web.UI.Page
    {
        int Tenant = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                myMEssage.Visible = false;
                string values = "";
                string token = HttpContext.Current.Request.Headers["Token"];
                if (string.IsNullOrEmpty(token))
                {
                    throw new Exception("Invalid Token");
                }
                string PrimaryhashedKey = PasswordGenerator.GetOldHashedPassword(token);
                ApiCredintialsRepository apiCredintialsRepository = new ApiCredintialsRepository();
                ApiCredintials apiCredintial = apiCredintialsRepository.GetApiCredintials_TFS(PrimaryhashedKey);
                if (apiCredintial == null)
                {
                    throw new Exception("Invalid Token1");
                }

                Tenant = apiCredintial != null ? apiCredintial.Tenant : 0;

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
                if (errorInfo.Message == "Invalid Token")
                {
                    myMEssage.Visible = true;
                }
                if (errorInfo.Message == "Invalid Token1")
                {
                    //throw errorInfo;
                }
                string errorMessage = errorInfo.Message;
                AzureLog.SaveLogsInStorage("TFS Page error  " + Environment.NewLine + errorMessage, "E", DateTime.Now, errorInfo.Message, errorInfo.StackTrace, 0, null, null, null);
            }
        }

        private void InsertNewAnalyzeQueue(string xmlfileText)
        {

            byte[] messageBytes = Encoding.ASCII.GetBytes(xmlfileText);

            AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();
            AnalyzeQueue analyzeQueue = new AnalyzeQueue()
            {
                CreateDate = TenantServerConfigration.GetCurrentDateTime(0),
                From = "TFS",
                Id = IdCounter.GetNumber("AnalyzeQueue", 0),
                MessageBody = messageBytes,
                Status = "W",
                Retries = 0,
                ConnectedToEntity = false,
                ConnectedToTenant = true,
                FileSize = xmlfileText.Length,
                Tenant = Tenant,
              
            };

            analyzeQueue.SearchFields = analyzeQueue.From + ',' + analyzeQueue.Status;
            analyzeQueueReposiory.Add(analyzeQueue);
            analyzeQueueReposiory.SubmitChanges();
        }
     
    }
}
