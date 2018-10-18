using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;

using WebFreight.Web.Security;

namespace WebFreight.Web.ShipmentsModel.DomainServices
{
    public partial class ShipmentsDomainService
    {
        private ShipmentPayableLineStatusRepository shipmentPayableLineStatusRepository;

        public IQueryable<ShipmentPayableLineStatus> GetShipmentPayableLineStatus(int tenant)
        {
            shipmentPayableLineStatusRepository = new ShipmentPayableLineStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return shipmentPayableLineStatusRepository.GetPayableStatusTypes();
        }

        public IQueryable<ShipmentPayableLineStatus> GetShipmentPayableLineStatusByTenant(int tenant)
        {
            shipmentPayableLineStatusRepository = new ShipmentPayableLineStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return shipmentPayableLineStatusRepository.GetPayableStatusTypes();
        }

        public IQueryable<ShipmentPayableLineStatus> GetFirstShipmentPayableLineStatus(string input, int tenant)
        {
            shipmentPayableLineStatusRepository = new ShipmentPayableLineStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            input = input.ToUpper();
            return shipmentPayableLineStatusRepository.GetPayableStatusTypes().Where(p => p.Code.ToUpper().StartsWith(input) || p.Name.ToUpper().StartsWith(input));
        }

        public void InsertShipmentPayableLineStatus(ShipmentPayableLineStatus entity)
        {
            shipmentPayableLineStatusRepository.Add(entity);
        }

        public void UpdateShipmentPayableLineStatus(ShipmentPayableLineStatus currentEntity)
        {
            shipmentPayableLineStatusRepository.Update(currentEntity);
        }

        public void DeleteShipmentPayableLineStatus(ShipmentPayableLineStatus entity)
        {
            shipmentPayableLineStatusRepository.Remove(entity);
        }
    }
}