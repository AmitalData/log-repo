using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.CargoTracking.Data;
using Logitude.Server.Tools.KafkaConfigurations;
using Logitude.Server.Tools.Messages;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using Logitude.CargoTracking.Data.EntityPOCOs;
using System.Data.Entity;

namespace CommunicationWorkerRole
{
    public class CargoReferencesSyncWorkerRole : WorkerEntryPoint
    {
        private ICargoTrackingContext cargoContext;
        public override void Run()
        {
            while (IsRunning)
            {
                Do();
            }
        }

        private void Do()
        {
            if (General.IsUpdating())
            {
                Thread.Sleep(60000);
                return;
            }
            try
            {
                SyncConnectedSepments();
            }
            catch (Exception ex)
            {
                Thread.Sleep(10000);
            }

        }

        private void SyncConnectedSepments()
        {
            var ForwardingShipmentIds = GetTop100fromQueue();
            if (ForwardingShipmentIds.Count <= 0)
                return;
            var forwardingHasCustom = GetAllForwardingHasCustom(ForwardingShipmentIds);
        }

        private object GetAllForwardingHasCustom(List<string> forwardingShipmentIds)
        {
            var t = cargoContext.CargoTrackingShipments.Include("").Where(e => e.CustomsShipmentHeaderId != null & forwardingShipmentIds.Contains(e.EntityId));
        }

        private List<string> GetTop100fromQueue()
        {
             return cargoContext.CargoReferencesSyncQueues.Take(100).Select(e=>e.ShipmentId).ToList();
        }

        public override bool OnStart()
        {


            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "CargoReferencesSync";
            currentContext = CargoTrackingContext.GetContext(0);
            return base.OnStart();
        }

    }
}
