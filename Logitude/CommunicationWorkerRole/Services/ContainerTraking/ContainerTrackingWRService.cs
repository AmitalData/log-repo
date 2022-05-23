using Logitude.Server.Tools.QueueService;
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
        public ContainerTrackingWRService(DbQueueService queueService, QueueResponse queueResponse)
        {
            this.queueService = queueService;
            this.queueResponse = queueResponse;
            //SetQueueResponseFields();
            //InitiallizeServices();
            //InitiallizeFields();
        }
        public void ExecuteQueue()
        {
            //if (queueService == null || queueResponse == null || tenant == null || string.IsNullOrEmpty(shipmentOrderId)) return;
            //GetApiLog();
            //GetToken();
            //try
            //{
            //    ShipmentOrderAM shipmentOrderAM = GetShipmentOrderAM();
            //    SendShipmentAM(shipmentOrderAM);
            //    queueService.Complete();
            //}
            //catch (Exception ex)
            //{
            //    HandleQueueException(ex);
            //}

        }
    }
}
