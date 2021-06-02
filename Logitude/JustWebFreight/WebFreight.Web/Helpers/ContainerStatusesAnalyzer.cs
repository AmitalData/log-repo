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
using WebFreight.Web.WcfApi;

namespace WebFreight.Web.Helpers
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
            this.GetLogitudeOceanInsightsTenant();

        }

        private void GetLogitudeOceanInsightsTenant()
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                SettingRepository settingRepository = new SettingRepository();
                Setting setting = settingRepository.GetSingleSetting("1");
                if (setting != null)
                {
                    logitudeOceanInsightsTenant = setting.OITenantNumber;
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
        string refrenceNumber;
        string scacCode;
        string oceanInsightInsertType; 
        private void ReadAdditionalFieldsFromCommunicationLog()
        {
            this.refrenceNumber = communicationLog.EntityReference;
            this.scacCode = communicationLog.AdditionalFields?.Split(',')[0];
            this.oceanInsightInsertType = communicationLog.AdditionalFields?.Split(',')[1];
        }

        private void SendContainerStatusRequestToOceanInsightSevice()
        {
            OceanInsightsWcfService oceanInsightsWcfService = new OceanInsightsWcfService(); 
            var oceanInsightResponse = oceanInsightsWcfService.Insert(logitudeOceanInsightsTenant, scacCode, refrenceNumber, oceanInsightInsertType);
            oceanInsightId = oceanInsightResponse.Result;     
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
                Type = this.oceanInsightInsertType
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