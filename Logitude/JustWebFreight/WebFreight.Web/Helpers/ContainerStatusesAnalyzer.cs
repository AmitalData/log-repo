using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel;
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
        private ShipmentPM shipmentPM;
        private IShipmentsContext shipmentContext;
        private ICommonDataContext commonContext;
        private ShipmentRepository shipmentRepository;
        private ShipmentQuery shipmentQuery;
        private CommunicationLogRepository communicationLogRepository;
        private CommunicationLog communicationLog;
        private string communicationLogId;
        public ContainerStatusesAnalyzer(string communicationLogId, int tenant)
        {
            this.communicationLogId = communicationLogId;
            this.tenant = tenant;
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            shipmentContext = ShipmentsContext.GetContext(this.tenant);
            commonContext = CommonDataContext.GetContext(this.tenant);
            shipmentRepository = new ShipmentRepository(shipmentContext);
            shipmentQuery = new ShipmentQuery(shipmentRepository);
            communicationLogRepository = new CommunicationLogRepository(commonContext);
            communicationLog = communicationLogRepository.GetSingleCommunicationLog(communicationLogId, this.tenant);
            this.shipmentPM = shipmentQuery.GetSinglePM(communicationLog.EntityId, this.tenant);
        }

        public void Run()
        {
            this.AnalyzeMessageQueue();
        }

        private void AnalyzeMessageQueue()
        {
            if (communicationLog != null && communicationLog.EntityId == null)
            {
                throw new Exception("Analyzing shipment faild, shipment not found");
            }
            else
            {
                if (this.shipmentPM == null)
                {
                    throw new Exception("Analyzing shipment faild, shipment not found");
                }
                else
                {
                    this.SendContainerStatusRequestToOceanInsightSevice();
                    this.DoneCommunicationLog();
                }
            }
        }

        private void SendContainerStatusRequestToOceanInsightSevice()
        {
            OceanInsightsWcfService oceanInsightsWcfService = new OceanInsightsWcfService();
            oceanInsightsWcfService.Insert(tenant, null, shipmentPM.ShipmentNumber, null);
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