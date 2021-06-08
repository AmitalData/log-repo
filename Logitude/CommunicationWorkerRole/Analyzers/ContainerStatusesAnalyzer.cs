using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Transactions;
using CommunicationWorkerRole.LoginServiceReference;
using CommunicationWorkerRole.OceanInsightsServiceReference;
using System.ServiceModel;
using WebFreight.Web.Helpers;

namespace CommunicationWorkerRole.Analyzers
{
    public class ContainerStatusesAnalyzer
    {
        private int tenant;
        private ICommonDataContext commonContext;
        private CommunicationLogRepository communicationLogRepository;
        private CommunicationLog communicationLog;
        private string communicationLogId;
        private string oceanInsightId;
        private LogitudeOceanInsightsRequestRepository logitudeOceanInsightsRequestRepository;
        private int logitudeOceanInsightsTenant;
        private string amitalCloudEnvironmentURL;
        private string amitalCloudLogitudeTenantPrimaryKey;
        public ContainerStatusesAnalyzer(string communicationLogId, int tenant)
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
            this.GetLogitudeOceanInsightsTenantConfigurations();

        }

        private void GetLogitudeOceanInsightsTenantConfigurations()
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                SettingRepository settingRepository = new SettingRepository();
                Setting setting = settingRepository.GetSingleSetting("1");
                if (setting != null)
                {
                    logitudeOceanInsightsTenant = setting.OITenantNumber;
                    amitalCloudEnvironmentURL = setting.AmitalCloudEnvironmentURL;
                    amitalCloudLogitudeTenantPrimaryKey = setting.AmitalCloudLogitudeTenantPrimaryKey;
                }
                scope.Complete();
            }
        }

        public void Run()
        {
            this.AnalyzeMessageQueue();
        }

        private void AnalyzeMessageQueue()
        {
            if (communicationLog != null && communicationLog.EntityId == null)
            {
                throw new Exception("Analyzing containetr status request faild, containetr not found");
            }
            else
            {
                this.ReadAdditionalFieldsFromCommunicationLog();
                this.SendContainerStatusRequestToOceanInsightSevice();
                this.InsertLogitudeOceanInsightsRequest();
                this.DoneCommunicationLog();
            }
        }
        private string refrenceNumber;
        private string scacCode;
        private string oceanInsightInsertType;
        private string shipmentId;
        private void ReadAdditionalFieldsFromCommunicationLog()
        {
            this.refrenceNumber = communicationLog.EntityReference;
            this.scacCode = communicationLog.AdditionalFields?.Split(',')[0];
            this.oceanInsightInsertType = communicationLog.AdditionalFields?.Split(',')[1];
            this.shipmentId = communicationLog.AdditionalFields?.Split(',')[2];
        }

        private void SendContainerStatusRequestToOceanInsightSevice()
        {
            var token = LoginToCloud();
            if (!string.IsNullOrEmpty(token))
            {
                BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                binding.MaxBufferSize = 2147483647;
                binding.MaxReceivedMessageSize = 2147483647;
                binding.ReaderQuotas.MaxStringContentLength = 2147483647;
                binding.ReaderQuotas.MaxArrayLength = 2147483647;
                var endpoint = new EndpointAddress(amitalCloudEnvironmentURL);
                OceanInsightsWcfServiceClient oceanInsightsWcfService = new OceanInsightsWcfServiceClient(binding, endpoint);
                using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)oceanInsightsWcfService.InnerChannel))
                {
                    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", token);
                    var oceanInsightResponse = oceanInsightsWcfService.Insert(logitudeOceanInsightsTenant, scacCode, refrenceNumber, oceanInsightInsertType);
                    if (!oceanInsightResponse.HasError)
                    {
                        oceanInsightId = oceanInsightResponse.Result;
                    }
                    else
                    {
                        throw new Exception("Analyzing containetr status request faild, " + oceanInsightResponse.ErrorMessage);
                    }
                }
            }
            else
            {
                throw new Exception("Analyzing containetr status request faild, invalid token");
            }
        }

        private string LoginToCloud()
        {
            string token = "";
            LoginWcfServiceClient loginService = new LoginWcfServiceClient();
            var aPICredentialsParameters = new APICredentialsParameters()
            {
                PrimaryKey = this.amitalCloudLogitudeTenantPrimaryKey,
                Tenant = this.logitudeOceanInsightsTenant
            };
            Logitude.Server.Tools.Response loginResponse = loginService.LoginByCredential(null, aPICredentialsParameters);
            if (!loginResponse.HasError)
            {
                token = loginResponse.Result;
            }
            return token;
        }

        private void InsertLogitudeOceanInsightsRequest()
        {
            LogitudeOceanInsightsRequest logitudeOceanInsightsRequest = new LogitudeOceanInsightsRequest()
            {
                Id = IdCounter.GetNumber("LogitudeOceanInsightsRequest", tenant),
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                OceanInsigntId = oceanInsightId,
                BLNumber = this.refrenceNumber,
                ContainerNumber = this.refrenceNumber,
                SCACCode = scacCode, 
                Tenant = this.tenant,
                Type = this.oceanInsightInsertType,
                ShipmentId = shipmentId
            };

            logitudeOceanInsightsRequestRepository.Add(logitudeOceanInsightsRequest);
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
    }
}