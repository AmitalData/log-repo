using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using CommunicationWorkerRole.LoginServiceReference;
using CommunicationWorkerRole.OceanInsightsServiceReference;
using System.ServiceModel;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure;
using System.Threading.Tasks;
using System.Threading;

namespace CommunicationWorkerRole.Analyzers
{
    public class ContainerRequestSender
    {
        private int tenant;
        private ICommonDataContext commonContext;
        private CommunicationLogRepository communicationLogRepository;
        private CommunicationLog communicationLog;
        private string communicationLogId;
        private string oceanInsightId;
        private LogitudeOceanInsightsRequestRepository logitudeOceanInsightsRequestRepository;
        private string refrenceNumber;
        private string scacCode;
        private string oceanInsightInsertType;
        private string shipmentId;
        private string containerNumber;

        public ContainerRequestSender(string communicationLogId, int tenant)
        {
            this.communicationLogId = communicationLogId;
            this.tenant = tenant;
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            commonContext = CommonDataContext.GetContext(this.tenant);
            logitudeOceanInsightsRequestRepository = new LogitudeOceanInsightsRequestRepository(this.tenant);
            communicationLogRepository = new CommunicationLogRepository(commonContext);
            communicationLog = communicationLogRepository.GetSingleCommunicationLog(communicationLogId, this.tenant);
        }

        public async void Send()
        {
            ValidateRequest();
            SetRequestArguments();
            var loginToExternalServiceTask = LoginToExternalService();
            loginToExternalServiceTask.Wait();
            if (loginToExternalServiceTask.Result != null && loginToExternalServiceTask.Result.HasError)
            {
                throw new Exception(loginToExternalServiceTask.Result.ErrorMessage);
            }
            var token = loginToExternalServiceTask.Result.Result;
            SendContainerStatusRequestToOceanInsightSevice(token);
        }

        private void ValidateRequest()
        {
            if (communicationLog != null && communicationLog.EntityId == null)
            {
                throw new Exception("Analyzing containetr status request faild, containetr not found");
            }
        }
        private void SetRequestArguments()
        {
            if (communicationLog.AdditionalFields?.Split(',').Length > 3)
            {
                this.refrenceNumber = communicationLog.EntityReference;
                this.scacCode = communicationLog.AdditionalFields?.Split(',')?[0];
                this.oceanInsightInsertType = communicationLog.AdditionalFields?.Split(',')?[1];
                this.shipmentId = communicationLog.AdditionalFields?.Split(',')?[2];
                this.containerNumber = communicationLog.AdditionalFields?.Split(',')?[3];
            }
        }
        private async Task<Logitude.Server.Tools.Response> LoginToExternalService()
        {
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            binding.MaxBufferSize = 2147483647;
            binding.MaxReceivedMessageSize = 2147483647;
            binding.ReaderQuotas.MaxStringContentLength = 2147483647;
            binding.ReaderQuotas.MaxArrayLength = 2147483647;
            var endpoint = new EndpointAddress(LogitudeSettings.AmitalCloudEnvironmentURL + "WcfApi/LoginWcfService.svc");
            LoginWcfServiceClient loginService = new LoginWcfServiceClient(binding, endpoint);
            var aPICredentialsParameters = new APICredentialsParameters()
            {
                PrimaryKey = LogitudeSettings.AmitalCloudLogitudeTenantPrimaryKey,
                Tenant = LogitudeSettings.OITenantNumber
            };
            return await loginService.LoginByCredentialAsync(null, aPICredentialsParameters);
        }

        private async void SendContainerStatusRequestToOceanInsightSevice(string externalServiceToken)
        {
            try
            {
                BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                binding.MaxBufferSize = 2147483647;
                binding.MaxReceivedMessageSize = 2147483647;
                binding.ReaderQuotas.MaxStringContentLength = 2147483647;
                binding.ReaderQuotas.MaxArrayLength = 2147483647;
                var endpoint = new EndpointAddress(LogitudeSettings.AmitalCloudEnvironmentURL + "WcfApi/OceanInsightsWcfService.svc");
                OceanInsightsWcfServiceClient oceanInsightsWcfService = new OceanInsightsWcfServiceClient(binding, endpoint);
                Task<Logitude.Server.Tools.Response> oceanInsightResponseTask;
                using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)oceanInsightsWcfService.InnerChannel))
                {
                    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", externalServiceToken);
                    oceanInsightResponseTask = oceanInsightsWcfService.InsertAsync(LogitudeSettings.OITenantNumber, scacCode, refrenceNumber, oceanInsightInsertType);
                }
                var oceanInsightResponse = await oceanInsightResponseTask;
                if (oceanInsightResponse.HasError)
                {
                    this.MarkCommunicationLogAsFaild(oceanInsightResponse.ErrorMessage); // Need to retry or not (Issue) 
                    throw new Exception(oceanInsightResponse.ErrorMessage);
                }
                else
                {
                    if (string.IsNullOrEmpty(oceanInsightId))
                    {
                        oceanInsightId = oceanInsightResponse.Result;
                        this.HandelLogitudeOceanInsightsRequest();
                        this.DoneCommunicationLog();
                    }
                }
                oceanInsightsWcfService.Close();
            }
            catch (Exception exception)
            {
                this.MarkCommunicationLogAsFaild(exception.Message);
                throw exception;
            }
        }

        private void HandelLogitudeOceanInsightsRequest()
        {
            LogitudeOceanInsightsRequest logitudeOceanInsightsRequest = logitudeOceanInsightsRequestRepository.GetSingleLogitudeOceanInsightsByOceanInsigntId(oceanInsightId, this.tenant);
            if (logitudeOceanInsightsRequest == null)
            {
                logitudeOceanInsightsRequest = new LogitudeOceanInsightsRequest()
                {
                    Id = IdCounter.GetNumber("LogitudeOceanInsightsRequest", tenant),
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    OceanInsigntId = oceanInsightId,
                    BLNumber = this.refrenceNumber,
                    ContainerNumber = this.containerNumber,
                    SCACCode = scacCode,
                    Tenant = this.tenant,
                    Type = this.oceanInsightInsertType,
                    ShipmentId = shipmentId
                };
                logitudeOceanInsightsRequestRepository.Add(logitudeOceanInsightsRequest);
            }
            else
            {
                logitudeOceanInsightsRequest.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                logitudeOceanInsightsRequest.BLNumber = this.refrenceNumber;
                logitudeOceanInsightsRequest.ContainerNumber = this.containerNumber;
                logitudeOceanInsightsRequest.SCACCode = scacCode;
                logitudeOceanInsightsRequest.Type = this.oceanInsightInsertType;
                logitudeOceanInsightsRequest.ShipmentId = shipmentId;
                logitudeOceanInsightsRequestRepository.Update(logitudeOceanInsightsRequest);
            }
            logitudeOceanInsightsRequestRepository.SubmitChanges();
        }

        private void DoneCommunicationLog()
        {
            communicationLog.CommunicationStatusTypeCode = "D";
            communicationLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(this.tenant);
            communicationLog.DoneDateUTC = DateTime.UtcNow;
            communicationLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(this.tenant);
            communicationLog.LastStatusDateUTC = DateTime.UtcNow;
            communicationLogRepository.Update(communicationLog);
            communicationLogRepository.SubmitChanges();
        }

        private void MarkCommunicationLogAsFaild(string exception)
        {
            communicationLog.CommunicationStatusTypeCode = "F";
            communicationLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(this.tenant);
            communicationLog.DoneDateUTC = DateTime.UtcNow;
            communicationLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(this.tenant);
            communicationLog.LastStatusDateUTC = DateTime.UtcNow;
            communicationLog.ExceptionMessage = exception;
            communicationLogRepository.Update(communicationLog);
            communicationLogRepository.SubmitChanges();
        }
    }
}