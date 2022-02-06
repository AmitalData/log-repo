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

        public ShipmentProductItemPM GetSinglePM(string id, int tenant)
        {
            ShipmentProductItemPM myResult
                = (from a in repository.context.ShipmentProductItems.Include("OriginCountry")
                   where a.Id == id && a.Tenant == tenant
                   select new ShipmentProductItemPM()
                   {
                       Id = a.Id,
                       Tenant = a.Tenant,
                       ShipmentId = a.ShipmentId,
                       ProductItemId = a.ProductItemId,
                       Description = a.Description,
                       HTSCode = a.HTSCode,
                       SKU = a.SKU,
                       ApprovedByCustomer = a.ApprovedByCustomer,
                       Brand = a.Brand,
                       Name = a.Name,
                       ASIN = a.ASIN,
                       UPC = a.UPC,
                       OriginCountryId = a.OriginCountryId,
                       OriginCountryName = a.OriginCountry == null ? null : a.OriginCountry.EnglishName,
                       VATPercentage = a.VATPercentage,
                       DutiesPercentage = a.DutiesPercentage,
                       OtherDuties = a.OtherDuties,
                       Remarks = a.Remarks,
                   }).FirstOrDefault();

            return myResult;
        }

        public List<ShipmentProductItemPM> GetShipmentProductItems(string shipmentId, int tenant)
        {
            List<ShipmentProductItemPM> shipmentProductItems
                = (from a in repository.context.ShipmentProductItems.Include("OriginCountry")
                   where a.ShipmentId == shipmentId && a.Tenant == tenant
                   select new ShipmentProductItemPM()
                   {
                       Id = a.Id,
                       Tenant = a.Tenant,
                       ShipmentId = a.ShipmentId,
                       ProductItemId = a.ProductItemId,
                       Description = a.Description,
                       HTSCode = a.HTSCode,
                       SKU = a.SKU,
                       ApprovedByCustomer = a.ApprovedByCustomer,
                       Brand = a.Brand,
                       Name = a.Name,
                       ASIN = a.ASIN,
                       UPC = a.UPC,
                       OriginCountryId = a.OriginCountryId,
                       OriginCountryName = a.OriginCountry == null ? null : a.OriginCountry.EnglishName,
                       VATPercentage = a.VATPercentage,
                       DutiesPercentage = a.DutiesPercentage,
                       OtherDuties = a.OtherDuties,
                       Remarks = a.Remarks,
                   }).ToList();

            return shipmentProductItems;
        }
    }
}
