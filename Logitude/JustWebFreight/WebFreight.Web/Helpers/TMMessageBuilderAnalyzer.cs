using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.OAuth;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Logitude.Server.Tools.QueueService;

namespace WebFreight.Web.Helpers
{
    public class TMMessageBuilderAnalyzer
    {

        public TMMessageBuilderAnalyzer(string  wiNumber, string completedWork, IQueueService queue, int tenant)
        {
            if (wiNumber != null)
            {
                this.UpdateWorkItemById(Int32.Parse(wiNumber), Double.Parse(completedWork), queue);
            }
        }

        public void UpdateWorkItemById(int wi, double completedWork, IQueueService queue)
        {
            // Create a connection to the account
            string accountUri = "https://logitudeteam.visualstudio.com";
            var personalAccessToken = "qsxsy6j454xpslikiuzc5oynhh5djttgxj4gmnlzpuaeypbuyc3q";
            int workItemId = wi;
            // new VssOAuthAccessTokenCredential(personalAccessToken)
            VssConnection connection = new VssConnection(new Uri(String.Format(accountUri)), new VssBasicCredential("logitudo@live.com", personalAccessToken));
            // Get an instance of the work item tracking client
            WorkItemTrackingHttpClient witClient = connection.GetClient<WorkItemTrackingHttpClient>();
            //object completedWork = null;

            try
            {
                // Get the specified work item
                WorkItem workitem = witClient.GetWorkItemAsync(workItemId, null, null, WorkItemExpand.Relations).Result;
                //completedWork = workitem.Fields.Where(a => a.Key == "Microsoft.VSTS.Scheduling.CompletedWork").Select(a => a).FirstOrDefault();
                JsonPatchDocument patchDocument = new JsonPatchDocument();
                patchDocument.Add(new JsonPatchOperation()
                {
                    Operation = Operation.Replace,
                    Path = "/fields/Microsoft.VSTS.Scheduling.CompletedWork",
                    Value = completedWork,
                });
                witClient.UpdateWorkItemAsync(patchDocument, wi);
                queue.Complete();
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
    }
}