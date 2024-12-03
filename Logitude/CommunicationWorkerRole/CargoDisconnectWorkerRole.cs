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
using Logitude.CargoTracking.BL.CargoTrackingServices;
using System.Data;
using System.Data.SqlClient;
using Simplog.Data.ShipmentsModel;
using System.Linq.Expressions;

namespace CommunicationWorkerRole
{
    /// <summary>
    /// this worker role to disconnect shipments, return all disconnected shipment as it was before connect
    /// In incremental process > any shipment that dose not contains the CustomfileId or any order dose not contains the shipmentId will add to queue 
    /// this worker role get all records from Queue and check if the shipments were connected in cargo or no
    /// and update just (shipments that were connected) on the logitude database
    /// the incremental process will get all updated shipments and re-create the shipments as it was before connect
    /// </summary>
    public class CargoDisconnectWorkerRole : WorkerEntryPoint
    {

        const int MaxShepmentsNumberPerTime = 100;
        private ICargoTrackingContext cargoContext;
        private IShipmentsContext shipmentsContext;
        const int MaximumNumberOfConcurrentConnections = 12;
        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = MaximumNumberOfConcurrentConnections;
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "CargoDisconnec";
            cargoContext = CargoTrackingContext.GetContext((int)Tenant);
            shipmentsContext = ShipmentsContext.GetContext((int)Tenant);
            return base.OnStart();
        }
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
                DisconnectShepments();
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Thread.Sleep(10000);
            }

        }

        private void DisconnectShepments()
        {
            var disconnectShipmentQueue = GetDisconnectQueue();
            if (disconnectShipmentQueue.Count <= 0)
                return;
            var connectedShipmentsIds = GetConnectedShipmentsIds(disconnectShipmentQueue);
            if (connectedShipmentsIds.Count > 0)
                UpdateShipmetsByIds(connectedShipmentsIds);
            RemoveQueueRecords(disconnectShipmentQueue);
        }
        private List<CargoDisconnectQueue> GetDisconnectQueue()
        {
            return cargoContext.CargoDisconnectQueues.Take(MaxShepmentsNumberPerTime).ToList();
        }

        private List<string> GetConnectedShipmentsIds(List<CargoDisconnectQueue> disconnectShipmentQueue)
        {
            var shipmentsConnectedIds = GetShipmentsConnectedIds(disconnectShipmentQueue);
            shipmentsConnectedIds = shipmentsConnectedIds.GroupBy(e => e).Select(e => e.Key).ToList();
            return shipmentsConnectedIds;

        }
        private void UpdateShipmetsByIds(List<string> connectedShipmentsIds)
        {
            var ids = GetIdsAsString(connectedShipmentsIds);
            var query = $"update Shipments set AutomaticLastUpdateDate = GETDATE() where id in ({ids}) ";
            shipmentsContext.GetActiveDbContext().Database.ExecuteSqlCommand(query);
        }
        private void RemoveQueueRecords(List<CargoDisconnectQueue> orderShipmentQueue)
        {
            var ids = GetIdsAsString(orderShipmentQueue.Select(e => e.Id + "").ToList());
            var query = $"delete from CargoDisconnectQueues where id in ({ids}) ";
            cargoContext.GetActiveDbContext().Database.ExecuteSqlCommand(query);
        }
        private List<string> GetShipmentsConnectedIds(List<CargoDisconnectQueue> disconnectShipmentQueue)
        {
            var fromIds = disconnectShipmentQueue.Select(e => e.ShipmentId).ToList();
            var connectedIds = cargoContext.CargoTrackingShipmentSearches
                .Where(e => e.ReferenceFromShipmentId != null && fromIds.Contains(e.ReferenceFromShipmentId))
                .Select(e => e.ShipmentId).ToList();
            return connectedIds;
        }
        private string GetIdsAsString(List<string> Ids)
        {
            var ids = "";
            foreach (var item in Ids)
            {
                ids += $"'{item}',";
            }
            if (Ids.Count > 0)
                ids = ids.Substring(0, ids.Length - 1);
            return ids;

        }
    }

   
}
