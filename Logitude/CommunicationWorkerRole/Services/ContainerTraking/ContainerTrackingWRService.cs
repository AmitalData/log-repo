using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.Services.ContainerTraking
{
    public class ContainerTrackingWRService
    {
        private readonly DbQueueService queueService;
        private readonly QueueResponse queueResponse;
        private ICommonDataContext Commoncontext;
        private CommunicationLog CommunicationLog;
        private CommunicationLogRepository CommunicationLogRep;
        private int Tenant;
        private Shipment Shipment;
        private IShipmentsContext ShipmentContext;
        public ContainerTrackingWRService(DbQueueService queueService, QueueResponse queueResponse)
        {
            this.queueService = queueService;
            this.queueResponse = queueResponse;

            InitiallizeFields();
        }
        
        public void ExecuteQueue()
        {
            FailCommunicationLog("Not implemented");
            throw new Exception("Not implemented");
        }
        private void InitiallizeFields()
        {
            Tenant = int.Parse(queueResponse.MessageValues["Tenant"]);
            Commoncontext = CommonDataContext.GetContext(Tenant);
            CommunicationLogRep = new CommunicationLogRepository(Commoncontext);
            CommunicationLog = GetCommunicationLog();
            //ShipmentContext = ShipmentsContext.GetContext(ContainerStatusSimulatorArgs.Tenant);
            //Shipment = GetShipment();

        }
        private CommunicationLog GetCommunicationLog()
        {
            var communicationLogId = queueResponse.MessageValues["CommunicationLogId"];
            CommunicationLog communicationLog = CommunicationLogRep.GetSingleCommunicationLog(communicationLogId, Tenant);
            return communicationLog;
        }
        private void FailCommunicationLog(string message)
        {
            CommunicationLog.CommunicationStatusTypeCode = "F";
            CommunicationLog.ExceptionMessage = message;
            CommunicationLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(CommunicationLog.Tenant);
            CommunicationLog.DoneDateUTC = DateTime.UtcNow;
            CommunicationLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(CommunicationLog.Tenant);
            CommunicationLog.LastStatusDateUTC = DateTime.UtcNow;
            CommunicationLogRep.Update(CommunicationLog);
            CommunicationLogRep.SubmitChanges();
        }

        private void CompleteCommunicationLog()
        {
            CommunicationLog.CommunicationStatusTypeCode = "D";
            CommunicationLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(CommunicationLog.Tenant);
            CommunicationLog.DoneDateUTC = DateTime.UtcNow;
            CommunicationLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(CommunicationLog.Tenant);
            CommunicationLog.LastStatusDateUTC = DateTime.UtcNow;
            CommunicationLogRep.Update(CommunicationLog);
            CommunicationLogRep.SubmitChanges();
        }
    }
}
