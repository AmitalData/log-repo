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
        private ShipmentReceivableLineStatusRepository shipmentReceivableLineStatusRepository;

        public IQueryable<ShipmentReceivableLineStatus> GetShipmentReceivableLineStatus(int tenant)
        {
            shipmentReceivableLineStatusRepository = new ShipmentReceivableLineStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return shipmentReceivableLineStatusRepository.GetShipmentReceivableLineStatus();
        }

        public IQueryable<ShipmentReceivableLineStatus> GetShipmentReceivableLineStatusByTenant(int tenant)
        {
            shipmentReceivableLineStatusRepository = new ShipmentReceivableLineStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return shipmentReceivableLineStatusRepository.GetShipmentReceivableLineStatus();
        }

        public IQueryable<ShipmentReceivableLineStatus> GetFirstShipmentReceivableLineStatus(string input, int tenant)
        {
            shipmentReceivableLineStatusRepository = new ShipmentReceivableLineStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            input = input.ToUpper();
            return shipmentReceivableLineStatusRepository.GetShipmentReceivableLineStatus().Where(p => p.Code.ToUpper().StartsWith(input) || p.Name.ToUpper().StartsWith(input));
        }

        public void InsertShipmentReceivableLineStatus(ShipmentReceivableLineStatus entity)
        {
            shipmentReceivableLineStatusRepository.Add(entity);
        }

        public void UpdateShipmentReceivableLineStatus(ShipmentReceivableLineStatus currentEntity)
        {
            shipmentReceivableLineStatusRepository.Update(currentEntity);
        }

        public void DeleteShipmentReceivableLineStatus(ShipmentReceivableLineStatus entity)
        {
            shipmentReceivableLineStatusRepository.Remove(entity);
        }
    }
}