using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace CommunicationWorkerRole
{
    public class OpportunityStageChangingWorkerRole : WorkerEntryPoint
    {
        private DbQueueService queueService;
        private int tenant;
        private string stageName;
        private string clientId;
        private string logitudeTrackingId;
        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "OpportunityStageChangingWR";
            DoneItemsInRange = new Dictionary<DateTime, int>();
            return base.OnStart();
        }

        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        ReadQueueMessage();
                    }
                    catch (Exception exception)
                    {
                        ExceptionHandler.HandleException(exception, DateTime.Now, 0, null, "OpportunityStageChanging queue worker role start", null, null);
                        Thread.Sleep(new TimeSpan(0, 0, 1));
                    }
                }
                else Thread.Sleep(new TimeSpan(0, 0, 1));
            }
        }

        private void ReadQueueMessage()
        {
            queueService = new DbQueueService("OpportunityStageChangingQueue", 0);
            var queueResponse = queueService.Receive();

            if (queueResponse != null && queueResponse.MessageId != null)
            {
                try
                {
                    logitudeTrackingId = "UA-25875416-1";
                    MapQueueResponse(queueResponse);
                    SendToGoogleAnalytics();
                    queueService.Complete();
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, DateTime.Now, tenant, "", "OpportunityStageChanging WorkerRole Run Method", "", null);
                    queueService.CompleteAsFailed();
                    Thread.Sleep(10000);
                }
            }
            else
            {
                Thread.Sleep(new TimeSpan(0, 0, 1));
            }
        }

        private void MapQueueResponse(QueueResponse queueResponse)
        {
            tenant = int.Parse(queueResponse.MessageValues["Tenant"].ToString());
            stageName = queueResponse.MessageValues["StageName"].ToString();
            clientId = queueResponse.MessageValues["ClientId"].ToString();
        }
        private void SendToGoogleAnalytics()
        {
            string postDataString = this.BuildPostDataString();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.google-analytics.com/collect");
            request.Method = "POST";
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataString);
            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(postDataString);
            }

            try
            {
                var webResponse = (HttpWebResponse)request.GetResponse();
                if (webResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new HttpException((int)webResponse.StatusCode,
                                            "Google Analytics tracking did not return OK 200");
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, tenant, "", "Google Analytics tracking failed", "", null);
            }
        }

        private string BuildPostDataString()
        {
            Dictionary<string, string> postData = new Dictionary<string, string>
                           {
                               { "v", "1" },
                               { "tid", logitudeTrackingId },
                               { "cid", clientId },
                               { "t", "event" },
                               { "ec", "crm" },
                               { "ea", "update" },
                               { "cd3", stageName },
                               { "el", "Stage" },
                           };

            return postData.Aggregate("", (data, next) => string.Format("{0}&{1}={2}", data, next.Key,
                                                             HttpUtility.UrlEncode(next.Value))).TrimEnd('&');
        }
    }
}
