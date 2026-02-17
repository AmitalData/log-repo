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

                TFSResponse response = new TFSResponse();
                dynamic data = JObject.Parse(values);

                if (data.resource != null)
                {
                    response.WorkItemId = data.resource.workItemId;
                    response.UniqueName = data.resource.revisedBy != null ? data.resource.revisedBy.uniqueName : "";
                    response.ChangedDate = data.resource.revision != null ? data.resource.revision.fields["System.ChangedDate"] : "";
                    response.CreatedBy = data.resource.revision != null ? data.resource.revision.fields["System.CreatedBy"] : "";
                    response.ChangedBy = data.resource.revision != null ? data.resource.revision.fields["System.ChangedBy"] : "";
                    response.AssignedTo = data.resource.revision != null ? data.resource.revision.fields["System.AssignedTo"] : "";
                    response.TaskState = data.resource.revision != null ? data.resource.revision.fields["System.State"] : "";

                    if (response.CreatedBy.Contains('<'))
                    {
                        response.CreatedBy = response.CreatedBy.Split('<')[1].Split('>')[0];
                    }

                    if (response.ChangedBy.Contains('<'))
                    {
                        response.ChangedBy = response.ChangedBy.Split('<')[1].Split('>')[0];
                    }

                    if (response.AssignedTo.Contains('<'))
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

                    response.Relations = JsonConvert.DeserializeObject<RelationClass[]>(data.resource.revision.relations.ToString());
                }

                // Tenant 
                this.InsertNewAnalyzeQueue(response);
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
                //throw;
            }
        }

        private void InsertNewAnalyzeQueue(TFSResponse arg)
        {
            Type myType = arg.GetType();
            MemoryStream myMemoryStream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(myType);
            ser.Serialize(myMemoryStream, arg);
            myMemoryStream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(myMemoryStream);
            string content = reader.ReadToEnd();
            byte[] bytearray = myMemoryStream.ToArray();

            AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();
            AnalyzeQueue analyzeQueue = new AnalyzeQueue()
            {
                CreateDate = TenantServerConfigration.GetCurrentDateTime(0),
                From = "TFS",
                Id = IdCounter.GetNumber("AnalyzeQueue", 0),
                MessageBody = bytearray,
                Status = "W",
                Retries = 0,
                ConnectedToEntity = false,
                ConnectedToTenant = false,
                FileSize = bytearray.Length,
                Tenant = Tenant,
                //Subject = "Work Item Updated",
                AWBNumber = arg.WorkItemId,
                Log = arg.WorkItemId!=null? ("Task " + arg.WorkItemId+" Updated"):"",
            };

            analyzeQueue.SearchFields = analyzeQueue.From + ',' + analyzeQueue.Status;
            analyzeQueueReposiory.Add(analyzeQueue);
            analyzeQueueReposiory.SubmitChanges();
        }
        public string GetWorkItemById(int wi)
        {
            // Create a connection to the account
            string accountUri = "https://logitudeteam.visualstudio.com";
            var personalAccessToken = "wm4hokk3xqz3apceylabsky7lc6s7wwb326cdyeebowab3aey57a";
            int workItemId = wi;
            // new VssOAuthAccessTokenCredential(personalAccessToken)
            VssConnection connection = new VssConnection(new Uri(string.Format(accountUri)), new VssBasicCredential("maram@logitudeworld.com", personalAccessToken));
            // Get an instance of the work item tracking client
            WorkItemTrackingHttpClient witClient = connection.GetClient<WorkItemTrackingHttpClient>();
            object projectNo = "";

            try
            {
                // Get the specified work item
                WorkItem workitem = witClient.GetWorkItemAsync(workItemId).Result;
                var s = new StringBuilder();
                // Output the work item's field values
                projectNo = workitem.Fields.Where(a => a.Key == "logitudeteam.LogitudeProcess.Tester").Select(a => a.Value).FirstOrDefault();
            }
            catch (AggregateException aex)
            {
                VssServiceException vssex = aex.InnerException as VssServiceException;
                if (vssex != null)
                {
                    Console.WriteLine(vssex.Message);
                    string errorMessage = vssex.Message;
                    AzureLog.SaveLogsInStorage("VssServiceException connection Problem   " + Environment.NewLine + errorMessage, "E", DateTime.Now, vssex.Message, vssex.StackTrace, 0, null, null, null);
                    //throw;
                }
            }

            return projectNo.ToString();
        }
    }
}
