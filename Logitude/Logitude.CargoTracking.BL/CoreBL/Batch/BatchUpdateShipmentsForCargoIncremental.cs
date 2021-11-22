using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using Logitude.ShipmentOrderModule.Data.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.CargoTracking.BL.CoreBL.Batch
{
    public class BatchUpdateShipmentsForCargoIncremental : BatchTaskExecutionsService
    {
        const int ShipmentsPerTime = 1000;
        public BatchUpdateShipmentsForCargoIncremental(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {
        }
        public override void RunCode()
        {
            var parameterArgs = DeserilaizeParameters();

            UpdateOrders(parameterArgs);
            UpdateShipments(parameterArgs);

        }

        private void UpdateOrders(UpdateShipmetsBatchArgs parameterArgs)
        {
            ShipmentOrderRepository shipmentOrderRepository = new ShipmentOrderRepository(parameterArgs.Tenant);

            var count = 0;
            var shipmentOrderIds = shipmentOrderRepository.GetShipmentOrdersIdsByTenant(parameterArgs.Tenant, 0, ShipmentsPerTime);
            while (shipmentOrderIds.Count > 0)
            {
                shipmentOrderRepository.UpdateLastUpdateDate(GetIdsAsString(shipmentOrderIds));
                Thread.Sleep(1 * 60 * 1000);// 1 minute 
                count += ShipmentsPerTime;
                shipmentOrderIds = shipmentOrderRepository.GetShipmentOrdersIdsByTenant(parameterArgs.Tenant, count, ShipmentsPerTime);

            }
        }

        private void UpdateShipments(UpdateShipmetsBatchArgs parameterArgs)
        {
            ShipmentRepository shipmentRepository = new ShipmentRepository(parameterArgs.Tenant);

            var count = 0;
            var shipments = shipmentRepository.GetShipmentIdsByTenant(parameterArgs.Tenant, 0, ShipmentsPerTime);
            while (shipments.Count > 0)
            {
                shipmentRepository.UpdateLastUpdateDate(GetIdsAsString(shipments));
                Thread.Sleep(1 * 60 * 1000);// 1 minute 
                count += ShipmentsPerTime;
                shipments = shipmentRepository.GetShipmentIdsByTenant(parameterArgs.Tenant, count, ShipmentsPerTime);

            }



        }

        private UpdateShipmetsBatchArgs DeserilaizeParameters()
        {
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(UpdateShipmetsBatchArgs));
            return serializer.Deserialize(stringReader) as UpdateShipmetsBatchArgs;
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
