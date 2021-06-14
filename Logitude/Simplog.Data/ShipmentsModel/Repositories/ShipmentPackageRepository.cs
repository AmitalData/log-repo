using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using System;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ShipmentPackageRepository : IRepository<ShipmentPackage>
    {
        IShipmentsContext shipmentContext;

        public ShipmentPackageRepository(IShipmentsContext context)
        {
            shipmentContext = context;
        }

        public ShipmentPackageRepository()
        {
            shipmentContext = new ShipmentsContext();
        }

        public ShipmentPackageRepository(int tenant)
        {
            shipmentContext = ShipmentsContext.GetContext(tenant);
        }

        public IQueryable<ShipmentPackage> GetShipmentPackages(int tenant)
        {
            return (from record in context.ShipmentPackages
                    where record.Tenant == tenant
                    select record);
        }

        public ShipmentPackage GetSingleShipmentPackage(string id, int tenant)
        {
            return (from record in context.ShipmentPackages where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public IQueryable<ShipmentPackage> GetShipmentPackagesForShipmentTenant(string shipmentId, int tenant)
        {
            IQueryable<ShipmentPackage> shipmentPackages = from a in context.ShipmentPackages.Include("PackageType") where a.Tenant == tenant && a.ShipmentId == shipmentId select a;
            return shipmentPackages;
        }

        public IQueryable<ShipmentPackage> GetShipmentPackagesByCommodityId(string shipmentId, string commodityId, int tenant)
        {
            return (from a in context.ShipmentPackages where a.ShipmentId == shipmentId && a.Tenant == tenant && a.CommodityId == commodityId select a);
        }

        public IQueryable<ShipmentPackage> GetPackagesFromShipmentsIds(List<string> shipmentsIds, int tenant)
        {
            IQueryable<ShipmentPackage> shipments = (from a in context.ShipmentPackages.Include("PackageType").Include("LCLPackageType")
                                                     where a.Tenant == tenant && shipmentsIds.Contains(a.ShipmentId)
                                                     select a);
            return shipments;
        }

        public ShipmentPackage GetSingleShipmentPackageByContainerId(string shipmentId, string containerId,int tenant)
        {
            return (from record in context.ShipmentPackages where record.ShipmentId != shipmentId && 
                    record.Tenant == tenant &&  record.ContainerEntityId == containerId
                    select record).FirstOrDefault();
        }

        public void Add(ShipmentPackage entity)
        {
            context.ShipmentPackages.Add(entity);
        }

        public void Remove(ShipmentPackage entity)
        {
            try
            {
                context.ShipmentPackages.Attach(entity);
            }
            catch { }
            context.ShipmentPackages.Remove(entity);
        }

        public void Update(ShipmentPackage entity)
        {
            try
            {
                context.ShipmentPackages.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<ShipmentPackage> All()
        {
            return context.ShipmentPackages.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ShipmentPackage> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ShipmentPackage GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public string GetContainersNumbersByShipmentIdAndTenant(string id, int tenant)
        {
            string containerNumbers = string.Join(",", (from a in context.ShipmentPackages
                                                        where a.Tenant == tenant && a.ShipmentId == id && a.ContainerNumber != null
                                                        select a.ContainerNumber));
            return containerNumbers;
        }
    }
}