using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CommunicationWorkerRole
{
    public class CollaborationToolTaskWR : WorkerEntryPoint
    {
        DbQueueService queueservice;
        string queueName = "CreateTaskCollaborationTool";
        string URL = "https://localhost:44362/api/Task/PostExternal";

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

                        if(response.MessageId != null)
                        {
                            CollaborationToolTask collaborationToolTask = GenerateCollaborationToolTaskModel(response);

                            using (var client = new HttpClient())
                            {
                                var serializedObject = JsonConvert.SerializeObject(collaborationToolTask);
                                var result = client.PostAsync(URL, new StringContent(serializedObject, Encoding.UTF8, "application/json"));
                                result.Wait();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ConnectClient();
                        ExceptionHandler.HandleException(ex, DateTime.Now, 1, null, "CreateTaskCollaborationTool worker role start", null, null);
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
            int tenant = int.Parse(response.MessageValues["Tenant"].ToString());
            return new CollaborationToolTask()
            {
                Tenant = tenant,
                Title = "New task from Logitude",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.Parse(response.MessageValues["EndDate"]),
                AssigneeEmail = GetUserEmailFromId(tenant, response.MessageValues["AssigneeId"].ToString()),
                PriorityCode = "M",
                OwnerEmail = GetUserEmailFromId(tenant, response.MessageValues["OwnerId"].ToString()),
                StatusCode = "NEW",
                EntityNumber = response.MessageValues["EntityNumber"].ToString(),
                TaskTypeName = response.MessageValues["TaskType"].ToString(),
                CreatedDate = DateTime.UtcNow,
                CreatedByUserEmail = GetUserEmailFromId(tenant, response.MessageValues["OwnerId"].ToString())
            };
        }

        private string GetUserEmailFromId(int tenant, string userId)
        {
            ContactRepository contactRep = new ContactRepository(tenant);
            Contact contact = contactRep.GetSingleContact(userId, tenant);

            return contact.Email;
        }
        #endregion

    }
}

public class CollaborationToolTask
{
    public int Tenant { get; set; }
    public string Title { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string AssigneeEmail { get; set; }
    public string PriorityCode { get; set; }
    public string OwnerEmail { get; set; }
    public string StatusCode { get; set; }
    public string EntityNumber { get; set; }
    public string TaskTypeName { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedByUserEmail { get; set; }
}