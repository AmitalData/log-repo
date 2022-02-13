using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentUnassignedFieldQuery
    {
        ShipmentUnassignedFieldRepository repository;

        public ShipmentUnassignedFieldQuery(int tenant)
        {
            repository = new ShipmentUnassignedFieldRepository(tenant);
        }

        public ShipmentUnassignedFieldQuery(ShipmentUnassignedFieldRepository repository)
        {
            this.repository = repository;
        }

        public ShipmentUnassignedFieldPM GetSinglePM(string id, int tenant)
        {
            ShipmentUnassignedFieldPM shipmentUnassignedFieldPM
                = (from a in repository.context.ShipmentUnassignedFields
                   where a.Id == id && a.Tenant == tenant
                   select new ShipmentUnassignedFieldPM()
                   {
                       Id = a.Id,
                       Tenant = a.Tenant,
                       ShipmentId = a.ShipmentId,
                       FieldName = a.FieldName,
                       ReceivedCode = a.ReceivedCode,
                       ReceivedData = a.ReceivedData,
                       ReplacedDataId = a.ReplacedDataId,
                       ObjectTableId = a.ObjectTableId,
                       ComputingPartnrCode = a.ComputingPartnrCode,
                   }).FirstOrDefault();

            return shipmentUnassignedFieldPM;
        }

        public List<ShipmentUnassignedFieldPM> GetShipmentUnassignedFields(string shipmentId, int tenant)
        {
            List<ShipmentUnassignedFieldPM> shipmentUnassignedFields
                = (from a in repository.context.ShipmentUnassignedFields
                   where a.ShipmentId == shipmentId && a.Tenant == tenant
                   select new ShipmentUnassignedFieldPM()
                   {
                       Id = a.Id,
                       Tenant = a.Tenant,
                       ShipmentId = a.ShipmentId,
                       FieldName = a.FieldName,
                       ReceivedCode = a.ReceivedCode,
                       ReceivedData = a.ReceivedData,
                       ReplacedDataId = a.ReplacedDataId,
                       ObjectTableId = a.ObjectTableId,
                       ComputingPartnrCode = a.ComputingPartnrCode,
                   }).ToList();

            return shipmentUnassignedFields;
        }
    }
}
