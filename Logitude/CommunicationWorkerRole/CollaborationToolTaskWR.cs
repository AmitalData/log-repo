using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationWorkerRole
{
    public class CollaborationToolTaskWR : WorkerEntryPoint
    {
        DbQueueService queueservice;
        string queueName = "CreateTaskCollaborationTool";

        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        queueservice = new DbQueueService(queueName, 1);
                        var response = queueservice.Receive(new TimeSpan(0, 0, 0, 10));
                        LastActivity = DateTime.UtcNow;

                        if(response.MessageId != null)
                        {
                            CollaborationToolTask collaborationToolTask = GenerateCollaborationToolTaskModel(response);

                            //{ "EntityNumber":"SHIP0001","AssigneeId":"1-796","OwnerId":"1-796","Tenant":"1","TaskType":"TestType","EndDate":"2021-04-09"}


                            using (var client = new HttpClient())
                            {
                    //            var result = client.PostAsync("", collaborationToolTask);
                            }

                            // call  create task
                        }
                    }
                    catch (Exception ex)
                    {
                        ConnectClient();
                        ExceptionHandler.HandleException(ex, DateTime.Now, 1, null, "AgentsSharedLogistics worker role start", null, null);
                        Thread.Sleep(10000);
                    }
                }
                else
                {
                    Thread.Sleep(60000);
                }
            }
        }

        public override bool OnStart()
        {
            ConnectClient();

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "CreateTaskCollaborationTool";

            return base.OnStart();
        }

        public void ConnectClient()
        {
            try
            {
                queueservice = new DbQueueService(queueName, 1);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 1, null, "Connect client method", null, null);
            }
        }

        #region Private methods
        private CollaborationToolTask GenerateCollaborationToolTaskModel(QueueResponse response)
        {
            return new CollaborationToolTask()
            {
                Tenant = int.Parse(response.MessageValues["Tenant"].ToString()),
                Title = "New task from Logitude",
                Description = string.Empty,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.Parse(response.MessageValues["EndDate"]),
               // AssigneeEmail = GetUserEmailFromId(),
                PriorityCode = "M",
              //  OwnerEmail = GetUserEmailFromId(),
                StatusCode = "NEW",
                EntityNumber = response.MessageValues["EntityNumber"].ToString(),
                UserEntryFields = string.Empty,
                Reminder = false,
                TaskTypeName = response.MessageValues["TaskType"].ToString(),
                CreatedDate = DateTime.UtcNow,
              //  CreatedByUserEmail = GetUserEmailFromId()
            };
        }

        //private string GetUserEmailFromId()
        //{
            
        //}
        #endregion

    }
}

public class CollaborationToolTask
{
    public int Tenant { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string AssigneeEmail { get; set; }
    public string PriorityCode { get; set; }
    public string OwnerEmail { get; set; }
    public string StatusCode { get; set; }
    public string EntityNumber { get; set; }
    public string UserEntryFields { get; set; }
    public bool Reminder { get; set; }
    public string TaskTypeName { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedByUserEmail { get; set; }
}