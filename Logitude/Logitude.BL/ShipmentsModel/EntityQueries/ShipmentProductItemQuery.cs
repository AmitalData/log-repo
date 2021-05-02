using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentProductItemQuery
    {
        ShipmentProductItemRepository repository;

        public ShipmentProductItemQuery(int tenant)
        {
            repository = new ShipmentProductItemRepository(tenant);
        }

        public ShipmentProductItemQuery(ShipmentProductItemRepository repository)
        {
            this.repository = repository;
        }

        public ShipmentProductItemPM GetSinglePM(string shipmentId, int tenant)
        {
            ShipmentProductItemPM myResult
                = (from a in repository.context.ShipmentProductItems
                   where a.ShipmentId == shipmentId && a.Tenant == tenant
                   select new ShipmentProductItemPM()
                   {
                       Id = a.Id,
                       Tenant = a.Tenant,
                       ShipmentId = a.ShipmentId,
                       ProductItemId = a.ProductItemId,
                       Description = a.Description,
                       HTSCode = a.HTSCode,
                   }).FirstOrDefault();

            return myResult;
        }

        public List<ShipmentProductItemPM> GetShipmentProductItems(string shipmentId, int tenant)
        {
            List<ShipmentProductItemPM> shipmentAssembleies
                = (from a in repository.context.ShipmentProductItems
                   where a.ShipmentId == shipmentId && a.Tenant == tenant
                   select new ShipmentProductItemPM()
                   {
                       Id = a.Id,
                       Tenant = a.Tenant,
                       ShipmentId = a.ShipmentId,
                       ProductItemId = a.ProductItemId,
                       Description = a.Description,
                       HTSCode = a.HTSCode,
                   }).ToList();

            return shipmentAssembleies;
        }
    }
}
