using CommunicationWorkerRole.LoginServiceReference;
using CommunicationWorkerRole.ExternalTasksQueueWcfService;
using Logitude.SystemLogs;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading;
using WebFreight.Web.Helpers;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.Server.Tools;
using System.IO;
using System.Xml.Serialization;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure;
using System.Threading.Tasks;

namespace CommunicationWorkerRole
{
    public class ContainerStatusesReceiverWR : WorkerEntryPoint
    {
        private string token;
        private int queuePriority = 1;

        public override void Run()
        {
            
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        var loginResponse = LoginToCloud();

                        loginResponse.Wait();
                        if (loginResponse.Result != null && !loginResponse.Result.HasError)
                        {
                            token = loginResponse.Result.Result;
                            this.ReadContainerStatusRequestToOceanInsightSevice();
                        }

                    }
                    catch (Exception e)
                    {
                        ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "ContainerStatusesReceiverWR : Run() Method", null);
                        Thread.Sleep(30000);
                    }
                }
                else
                {
                    Thread.Sleep(60000);
                }
            }
        }


        private async void ReadContainerStatusRequestToOceanInsightSevice()
        {
            if (!string.IsNullOrEmpty(token))
            {
                BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                binding.MaxBufferSize = 2147483647;
                binding.MaxReceivedMessageSize = 2147483647;
                binding.ReaderQuotas.MaxStringContentLength = 2147483647;
                binding.ReaderQuotas.MaxArrayLength = 2147483647;
                var endpoint = new EndpointAddress(LogitudeSettings.AmitalCloudEnvironmentURL + "WcfApi/ExternalTasksQueueWcfService.svc");
                ExternalTasksQueueWcfServiceClient externalTasksQueueWcfService = new ExternalTasksQueueWcfServiceClient(binding, endpoint);
                Task<string> oceanInsightResponseTask;
                using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)externalTasksQueueWcfService.InnerChannel))
                {
                    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", token);
                    oceanInsightResponseTask =   externalTasksQueueWcfService.GetTaskFromQueueAsync(LogitudeSettings.OITenantNumber, queuePriority);
                }
                var oceanInsightResponseXML = await oceanInsightResponseTask;
                if (!string.IsNullOrEmpty(oceanInsightResponseXML))
                {
                    this.ReadExternalTasksQueueWcfServiceResponse(oceanInsightResponseXML);
                    this.InsertNewAnalyzeQueue();
                    externalTasksQueueWcfService.MarkTaskAsDone(communicationId, LogitudeSettings.OITenantNumber, queuePriority);
                }
            }
            else
            {
                //ExceptionHandler.HandleException(new Exception("Analyzing containetr status request faild, invalid token"), DateTime.Now, 0, "", "WorkerRole", "ContainerStatusesReceiverWR : ReadContainerStatusRequestToOceanInsightSevice() Method", null);
            }
        }

        private string communicationId;
        private List<QueueTask> externalTasksQueueTasksEnvelope;
        private void ReadExternalTasksQueueWcfServiceResponse(string oceanInsightResponseXML)
        {
            var externalTasksQueueEnvelope = LogitudeXmlSerializer.DeserializeObject<Envelope>(oceanInsightResponseXML);
            this.communicationId = externalTasksQueueEnvelope.CommunicationLogId;
            this.externalTasksQueueTasksEnvelope = externalTasksQueueEnvelope.Tasks;
        }

        private void InsertNewAnalyzeQueue()
        {
            Type myType = externalTasksQueueTasksEnvelope.GetType();
            MemoryStream myMemoryStream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(myType);
            ser.Serialize(myMemoryStream, externalTasksQueueTasksEnvelope);
            myMemoryStream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(myMemoryStream);
            string content = reader.ReadToEnd();
            byte[] bytearray = myMemoryStream.ToArray();
            AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();
            AnalyzeQueue analyzeQueue = new AnalyzeQueue()
            {
                CreateDate = TenantServerConfigration.GetCurrentDateTime(0),
                From = "ContainerStatusesReceiver",
                Id = IdCounter.GetNumber("AnalyzeQueue", 0),
                MessageBody = bytearray,
                Status = "W",
                Retries = 0,
                ConnectedToEntity = false,
                ConnectedToTenant = false,
                FileSize = bytearray.Length,
                Tenant = 0,
            };
            analyzeQueue.SearchFields = analyzeQueue.From + ',' + analyzeQueue.Status;
            analyzeQueueReposiory.Add(analyzeQueue);
            analyzeQueueReposiory.SubmitChanges();
        }

        private async Task<Logitude.Server.Tools.Response> LoginToCloud()
        {
            if (string.IsNullOrEmpty(token))
            {
                LoginWcfServiceClient loginService = new LoginWcfServiceClient();
                var aPICredentialsParameters = new APICredentialsParameters()
                {
                    PrimaryKey = LogitudeSettings.AmitalCloudLogitudeTenantPrimaryKey,
                    Tenant = LogitudeSettings.OITenantNumber
                };
                return  await loginService.LoginByCredentialAsync(null, aPICredentialsParameters);
            }
            return null;
        }

        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = 12;

            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "ContainerStatusesReceiverWR";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                e.Cancel = true;
            }
        }
    }
}
