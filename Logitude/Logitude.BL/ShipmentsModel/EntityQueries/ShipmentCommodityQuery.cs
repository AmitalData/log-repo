using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentCommodityQuery
    {
        ShipmentCommodityRepository repository;

        public ShipmentCommodityQuery(int tenant)
        {
            repository = new ShipmentCommodityRepository(tenant);
        }

        public ShipmentCommodityQuery(ShipmentCommodityRepository repository)
        {
            this.repository = repository;
        }

        public ShipmentCommodityPM GetSinglePM(string id, int tenant)
        {
            ShipmentCommodityPM myResult = (from a in repository.Context.ShipmentCommodities
                                            where a.Tenant == tenant && a.Id == id
                                            select new ShipmentCommodityPM()
                                                  {
                                                      Id = a.Id,
                                                      ShipmentId = a.ShipmentId,
                                                      ChargeableWeight = a.ChargeableWeight,
                                                      ChargeAmount = a.ChargeAmount,
                                                      ChargeRate = a.ChargeRate,
                                                      CommodityNumber = a.CommodityNumber,
                                                      DescriptionOfGoods = a.DescriptionOfGoods,
                                                      RateClassCode = a.RateClassCode,
                                                      Tenant = a.Tenant,
                                                      GrossWeight = a.GrossWeight,
                                                      Volume = a.Volume,
                                                      VolumetricWeight = a.VolumetricWeight,
                                                      NumberOfPackages = a.NumberOfPackages,
                                                      IsFirstLine = a.IsFirstLine,
                                                  }).FirstOrDefault();

            if (myResult != null)
            {
                ShipmentPackageQuery packagesQuery = new ShipmentPackageQuery(tenant);
                myResult.CommodityPackages = packagesQuery.GetPackagesByCommodityId(myResult.Id, tenant);
            }

            return myResult;
        }

        public List<ShipmentCommodityPM> GetCommoditiesByShipmentId(string shipmentId, int tenant)
        {
            List<ShipmentCommodityPM> myResult = (from a in repository.Context.ShipmentCommodities
                                                  where a.Tenant == tenant && a.ShipmentId == shipmentId
                                                  select new ShipmentCommodityPM()
                                                  {
                                                      Id = a.Id,
                                                      ShipmentId = a.ShipmentId,
                                                      ChargeableWeight = a.ChargeableWeight,
                                                      ChargeAmount = a.ChargeAmount,
                                                      ChargeRate = a.ChargeRate,
                                                      CommodityNumber = a.CommodityNumber,
                                                      DescriptionOfGoods = a.DescriptionOfGoods,
                                                      RateClassCode = a.RateClassCode,
                                                      Tenant = a.Tenant,
                                                      GrossWeight = a.GrossWeight,
                                                      Volume = a.Volume,
                                                      VolumetricWeight = a.VolumetricWeight,
                                                      NumberOfPackages = a.NumberOfPackages,
                                                      IsFirstLine = a.IsFirstLine,
                                                  }).ToList();

            if (myResult.Count > 0)
            {
                ShipmentPackageQuery packagesQuery = new ShipmentPackageQuery(tenant);

                foreach (ShipmentCommodityPM item in myResult)
                {
                    item.CommodityPackages = packagesQuery.GetPackagesByCommodityId(item.Id, tenant).ToList();
                }
            }

            return myResult;
        }

    }
}