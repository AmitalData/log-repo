using Logitude.BL.Helpers;
using Logitude.Server.Tools.Counters;
using Logitude.TimeManagement.BL.EntityPMs;
using Logitude.TimeManagement.BL.EntityQueryServices;
using Logitude.TimeManagement.BL.EntityUpdateServices;
using Logitude.TimeManagement.Data;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.Data.Repositories;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
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
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using Microsoft.VisualStudio.Services.WebApi.Patch;

namespace WebFreight.Web.Helpers
{
    public class TFSAggregatorParseWebhook
    {
        public string AnalyzeQueueId;
        public int Tenant = 0;
        public bool IsAnalyzeQueueFaild = false;
        public object Description = "";
        public string projectNo = "";

        public TFSAggregatorParseWebhook()
        {

        }
        public TFSAggregatorParseWebhook(string WiId, string AnalyzeQueueId, int tenant)
        {
            this.AnalyzeQueueId = AnalyzeQueueId;
            this.Tenant = tenant;
              if(!String.IsNullOrEmpty(WiId))
            {
                int Id ;
                int.TryParse(WiId, out Id);
                this.DOJOB(Id); 
            }
        
        }


        private void DOJOB(int wi)
        {
            string accountUri = "https://logitudeteam.visualstudio.com";
            var personalAccessToken = "qsxsy6j454xpslikiuzc5oynhh5djttgxj4gmnlzpuaeypbuyc3q";
            int workItemId = wi;
            VssConnection connection = new VssConnection(new Uri(String.Format(accountUri)), new VssBasicCredential("logitudo@live.com", personalAccessToken));
            WorkItemTrackingHttpClient witClient = connection.GetClient<WorkItemTrackingHttpClient>();

            try
            {
                WorkItem workitem = witClient.GetWorkItemAsync(workItemId, null, null, WorkItemExpand.Relations).Result;
                JsonPatchDocument patchDocument = new JsonPatchDocument();
                if (workitem != null)
                {
                    if (workitem.Fields.GetValueOrDefault("System.WorkItemType").ToString() == "Task" || workitem.Fields.GetValueOrDefault("System.WorkItemType").ToString() == "Bug")
                    {
                        string parent = workitem.Relations[0].Url.Split('/').Last();
                        if (!string.IsNullOrEmpty(parent))
                        {
                            WorkItem parentItem = GetWorkItemById(int.Parse(parent));
                            if (parentItem != null)
                            {
                                var effort = Convert.ToSingle(parentItem.Fields.GetValueOrDefault("Custom.TasksEffort"));
                                var completedwork = Convert.ToSingle(parentItem.Fields.GetValueOrDefault("Microsoft.VSTS.Scheduling.CompletedWork"));

                                List<WorkItemRelation> itemss = parentItem.Relations.Where(a => a.Rel == "System.LinkTypes.Hierarchy-Forward").ToList();
                                double EffotSum = 0;
                                double completedworkSum = 0;
                                if (itemss.Count != 0)
                                {
                                    foreach (WorkItemRelation item in itemss)
                                    {
                                        string childId = item.Url.Split('/').Last();
                                        WorkItem childItem = GetWorkItemById(int.Parse(childId));
                                        if (childItem != null)
                                        {
                                            EffotSum += Convert.ToSingle(childItem.Fields.GetValueOrDefault("Microsoft.VSTS.Scheduling.Effort"));
                                            completedworkSum += Convert.ToSingle(childItem.Fields.GetValueOrDefault("Microsoft.VSTS.Scheduling.CompletedWork"));
                                        }
                                    }

                                    if (effort != EffotSum)
                                    {
                                        patchDocument.Add(new JsonPatchOperation()
                                        {

                                            Operation = Operation.Replace,
                                            Path = "/fields/Custom.TasksEffort",
                                            Value = EffotSum
                                        });
                                        witClient.UpdateWorkItemAsync(patchDocument, int.Parse((parentItem.Id + "")));
                                    }


                                    if (completedwork != completedworkSum)
                                    {
                                        patchDocument.Add(new JsonPatchOperation()
                                        {

                                            Operation = Operation.Replace,
                                            Path = "/fields/Microsoft.VSTS.Scheduling.CompletedWork",
                                            Value = completedworkSum
                                        });
                                        witClient.UpdateWorkItemAsync(patchDocument, int.Parse((parentItem.Id + "")));
                                    }
                                }
                            }
                        }

                    }
                }

            }
            catch (AggregateException aex)
            {
                VssServiceException vssex = aex.InnerException as VssServiceException;
                if (vssex != null)
                {
                    Console.WriteLine(vssex.Message);
                }
            }

        }
        private WorkItem GetWorkItemById(int id)
        {
            string accountUri = "https://logitudeteam.visualstudio.com";
            var personalAccessToken = "qsxsy6j454xpslikiuzc5oynhh5djttgxj4gmnlzpuaeypbuyc3q";
            VssConnection connection = new VssConnection(new Uri(String.Format(accountUri)), new VssBasicCredential("maram@logitudeworld.com", personalAccessToken));
            WorkItemTrackingHttpClient witClient = connection.GetClient<WorkItemTrackingHttpClient>();
            object projectNo = "";

            try
            {
                WorkItem workitem = witClient.GetWorkItemAsync(id, null, null, WorkItemExpand.Relations).Result;

                return workitem;
            }
            catch (Exception e)
            {
                VssServiceException vssex = e.InnerException as VssServiceException;
                if (vssex != null)
                {
                    Console.WriteLine(vssex.Message);
                }
                return null;

            }
        }


        

    }
}