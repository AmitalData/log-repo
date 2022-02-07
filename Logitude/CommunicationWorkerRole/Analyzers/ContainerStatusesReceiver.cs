using CommunicationWorkerRole.LoginServiceReference;
using CommunicationWorkerRole.ExternalTasksQueueWcfService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using WebFreight.Web.Helpers;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.Server.Tools;
using System.IO;
using System.Xml.Serialization;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure;
using System.Threading.Tasks;
using Simplog.Global.Data.GlobalModel;

namespace CommunicationWorkerRole.Analyzers
{
    public class ContainerStatusesReceiver
    {
        private string token;
        private int queuePriority = 1;
        private string communicationId;
        private List<QueueTask> externalTasksQueueTasksEnvelope;
        private AnalyzeQueueRepository analyzeQueueReposiory;
        public void Run()
        {
            var loginToExternalServiceTask = LoginToExternalService();
            loginToExternalServiceTask.Wait();
            if (loginToExternalServiceTask.Result != null && loginToExternalServiceTask.Result.HasError)
            {
                throw new ApplicationException(loginToExternalServiceTask.Result.ErrorMessage);
            }
            token = loginToExternalServiceTask.Result.Result;
            this.ReadContainerStatusRequestToOceanInsightSevice();
        }

        private async Task<Logitude.Server.Tools.Response> LoginToExternalService()
        {
            if (string.IsNullOrEmpty(token))
            {
                BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                binding.MaxBufferSize = 2147483647;
                binding.MaxReceivedMessageSize = 2147483647;
                binding.ReaderQuotas.MaxStringContentLength = 2147483647;
                binding.ReaderQuotas.MaxArrayLength = 2147483647;
                binding.Security.Transport.ClientCredentialType =HttpClientCredentialType.None;
                binding.Security.Transport.ProxyCredentialType =  HttpProxyCredentialType.None;

                var endpoint = new EndpointAddress(LogitudeSettings.AmitalCloudEnvironmentURL + "WcfApi/LoginWcfService.svc");
                LoginWcfServiceClient loginService = new LoginWcfServiceClient(binding, endpoint);

                if (loginService.Endpoint.Address.Uri.Scheme == "https")
                {
                    binding.Security.Mode = BasicHttpSecurityMode.Transport;
                    binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.Certificate;
                }
                else
                {
                    binding.Security.Mode = BasicHttpSecurityMode.None;
                    binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
                }

                var aPICredentialsParameters = new APICredentialsParameters()
                {
                    PrimaryKey = LogitudeSettings.AmitalCloudLogitudeTenantPrimaryKey,
                    Tenant = LogitudeSettings.OITenantNumber
                };
                return await loginService.LoginByCredentialAsync(null, aPICredentialsParameters);
            }
            return null;
        }

        private void ReadContainerStatusRequestToOceanInsightSevice()
        {
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            binding.MaxBufferSize = 2147483647;
            binding.MaxReceivedMessageSize = 2147483647;
            binding.ReaderQuotas.MaxStringContentLength = 2147483647;
            binding.ReaderQuotas.MaxArrayLength = 2147483647;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;

            var endpoint = new EndpointAddress(LogitudeSettings.AmitalCloudEnvironmentURL + "WcfApi/ExternalTasksQueueWcfService.svc");
            ExternalTasksQueueWcfServiceClient externalTasksQueueWcfService = new ExternalTasksQueueWcfServiceClient(binding, endpoint);

            if (externalTasksQueueWcfService.Endpoint.Address.Uri.Scheme == "https")
            {
                binding.Security.Mode = BasicHttpSecurityMode.Transport;
                binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.Certificate;
            }
            else
            {
                binding.Security.Mode = BasicHttpSecurityMode.None;
                binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            }
            this.GetTaskFromQueue(externalTasksQueueWcfService);
        }
        private async void GetTaskFromQueue(ExternalTasksQueueWcfServiceClient externalTasksQueueWcfService)
        {
            Task<string> oceanInsightResponseTask;
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)externalTasksQueueWcfService.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", token);
                oceanInsightResponseTask = externalTasksQueueWcfService.GetTaskFromQueueAsync(LogitudeSettings.OITenantNumber, queuePriority);
            }
            Envelope envelopeResponse;
            var oceanInsightResponseXML = await oceanInsightResponseTask;
            if (!string.IsNullOrEmpty(oceanInsightResponseXML))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Envelope));
                using (TextReader reader = new StringReader(oceanInsightResponseXML))
                {
                    envelopeResponse = (Envelope)serializer.Deserialize(reader);
                }
                if (envelopeResponse.HasError)
                {
                    externalTasksQueueWcfService.Close();
                    throw new ApplicationException(envelopeResponse.ErrorMessage);
                }
                var oceanInsightsPushUpdate = envelopeResponse?.Tasks?.Where(a => a.Action == "OceanInsights.PushUpdate").FirstOrDefault();
                if (oceanInsightsPushUpdate != null)
                {
                    this.ReadExternalTasksQueueWcfServiceResponse(envelopeResponse);
                    this.InsertNewAnalyzeQueue();
                    this.MarkTaskAsDone(externalTasksQueueWcfService);
                }
            }
        }

        private async void MarkTaskAsDone(ExternalTasksQueueWcfServiceClient externalTasksQueueWcfService)
        {
            try
            {
                System.Threading.Tasks.Task<CommunicationWorkerRole.ExternalTasksQueueWcfService.Response> response;
                using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)externalTasksQueueWcfService.InnerChannel))
                {
                    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", token);
                    response = externalTasksQueueWcfService.MarkTaskAsDoneAsync(communicationId, LogitudeSettings.OITenantNumber, queuePriority);
                }
                var communicationLogResponse = await response;
                externalTasksQueueWcfService.Close();
                if (communicationLogResponse.HasError)
                {
                    externalTasksQueueWcfService.Close();
                    throw new ApplicationException(communicationLogResponse.ErrorMessage);
                }
            }
            catch (Exception exception)
            {
                externalTasksQueueWcfService.Close();
                throw new ApplicationException(exception.Message);
            }
            finally
            {
                externalTasksQueueWcfService.Close();
            }
        }

        private void ReadExternalTasksQueueWcfServiceResponse(Envelope envelopeResponse)
        {
            var externalTasksQueueEnvelope = envelopeResponse;// LogitudeXmlSerializer.DeserializeObject<Envelope>(oceanInsightResponseXML);
            this.communicationId = externalTasksQueueEnvelope.CommunicationLogId;
            this.externalTasksQueueTasksEnvelope = externalTasksQueueEnvelope.Tasks;
        }

        private void InsertNewAnalyzeQueue()
        {
            IGlobalContext globalContext = GlobalContext.GetContext();
            analyzeQueueReposiory = new AnalyzeQueueRepository(globalContext);
            byte[] analyzeQueueMessageBody = this.GetAnalyzeQueueByteArray();
            if (this.IsAnalyzeQueueExsit(analyzeQueueMessageBody))
            {
                return;
            }
            AnalyzeQueue analyzeQueue = new AnalyzeQueue()
            {
                CreateDate = TenantServerConfigration.GetCurrentDateTime(0),
                From = "ContainerStatusesReceiver",
                Id = IdCounter.GetNumber("AnalyzeQueue", 0),
                MessageBody = analyzeQueueMessageBody,
                Status = "W",
                Retries = 0,
                ConnectedToEntity = false,
                ConnectedToTenant = false,
                FileSize = analyzeQueueMessageBody.Length,
                Tenant = 0,
            };
            analyzeQueue.SearchFields = analyzeQueue.From + ',' + analyzeQueue.Status;
            analyzeQueueReposiory.Add(analyzeQueue);
            analyzeQueueReposiory.SubmitChanges();
        }

        private byte[] GetAnalyzeQueueByteArray()
        {
            Type myType = externalTasksQueueTasksEnvelope.GetType();
            MemoryStream myMemoryStream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(myType);
            ser.Serialize(myMemoryStream, externalTasksQueueTasksEnvelope);
            myMemoryStream.Seek(0, SeekOrigin.Begin);
            byte[] bytearray = myMemoryStream.ToArray();
            return bytearray;
        }

        private bool IsAnalyzeQueueExsit(byte[] analyzeQueueMessageBody)
        {
            var isAnalyzeQueueExsit = analyzeQueueReposiory.IsAnalyzeQueueExsit(analyzeQueueMessageBody);
            return isAnalyzeQueueExsit;
        }
    }
}
