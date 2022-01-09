using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using Logitude.ShipmentOrderModule.Data.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.Repositories;
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
        const int ShipmentBulkSize = 1000;
        const int OffsetTimeToSeperateIncrementalExcutionOfShipments = 1 * 60 * 1000;
        const int DefaultMonthsBefore = 3;
        public BatchUpdateShipmentsForCargoIncremental(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {
        }
        public override void RunCode()
        {
            var parameterArgs = DeserilaizeParameters();
            var minStartDate = GetMinStartDate(parameterArgs.Tenant);
            UpdateOrdersBulkByBulk(parameterArgs, minStartDate);
            UpdateShipmentsBulkByBulk(parameterArgs, minStartDate);

        }

        private void UpdateOrdersBulkByBulk(UpdateShipmetsBatchArgs parameterArgs, DateTime minStartDate)
        {
            ShipmentOrderRepository shipmentOrderRepository = new ShipmentOrderRepository(parameterArgs.Tenant);

            var doneShipmentOrders = 0;
            var shipmentOrderIds = shipmentOrderRepository.GetShipmentOrdersIdsByTenant(parameterArgs.Tenant, doneShipmentOrders, ShipmentBulkSize, minStartDate);
            while (shipmentOrderIds.Count > 0)
            {
                shipmentOrderRepository.UpdateLastUpdateDate(GetIdsAsString(shipmentOrderIds));

                doneShipmentOrders += shipmentOrderIds.Count;
                shipmentOrderIds = shipmentOrderRepository.GetShipmentOrdersIdsByTenant(parameterArgs.Tenant, doneShipmentOrders, ShipmentBulkSize, minStartDate);
                if (shipmentOrderIds.Count > 0)
                    Thread.Sleep(OffsetTimeToSeperateIncrementalExcutionOfShipments);
            }
        }

        private void UpdateShipmentsBulkByBulk(UpdateShipmetsBatchArgs parameterArgs, DateTime minStartDate)
        {
            ShipmentRepository shipmentRepository = new ShipmentRepository(parameterArgs.Tenant);

            var doneShipments = 0;
            var shipments = shipmentRepository.GetShipmentIdsByTenant(parameterArgs.Tenant, doneShipments, ShipmentBulkSize, minStartDate);
            while (shipments.Count > 0)
            {
                shipmentRepository.UpdateLastUpdateDate(GetIdsAsString(shipments));
                doneShipments += shipments.Count;
                shipments = shipmentRepository.GetShipmentIdsByTenant(parameterArgs.Tenant, doneShipments, ShipmentBulkSize, minStartDate);
                if (shipments.Count > 0)
                    Thread.Sleep(OffsetTimeToSeperateIncrementalExcutionOfShipments);

            }



        }
        private DateTime GetMinStartDate(int tenant)
        {
            IGlobalContext MyContext = GlobalContext.GetContext();
            var tenantManagement = MyContext.TenantManagements.Find(tenant);
            if (!tenantManagement.PermissionBuildMonths.HasValue)
            {
                
                return DateTime.Now.AddDays(DefaultMonthsBefore * 30 * -1);
            }
            var days = tenantManagement.PermissionBuildMonths.Value * 30 * -1;
            return DateTime.Now.AddDays(days);
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
