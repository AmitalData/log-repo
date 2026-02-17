using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ShipmentLevelRepository: IRepository<ShipmentLevel>
    {
        IShipmentsContext shipmentContext;

        public ShipmentLevelRepository(IShipmentsContext context)
        {
            shipmentContext = context;
        }

        public ShipmentLevelRepository()
        {
            shipmentContext = new ShipmentsContext();
        }

        public ShipmentLevelRepository(int tenant)
        {
            shipmentContext = ShipmentsContext.GetContext(tenant);
        }

        public IQueryable<ShipmentLevel> GetShipmentLevels(int tenant)
        {
            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant tenantEntity = tenantRepository.GetSingleTenant(tenant); ////
            if (tenantEntity.IsHybrid)
            {
                return context.ShipmentLevels;
            }
            else
            {
                return context.ShipmentLevels.Where(s => s.Code != "A");
            }            
        }

        public IQueryable<ShipmentLevel> GetAll()
        {
            return context.ShipmentLevels;
        }

        public IQueryable<ShipmentLevel> GetShipmentLevels()
        {
            return context.ShipmentLevels;
        }

        public ShipmentLevel GetSingleShipmentLevel(string code)
        {
            return (from a in context.ShipmentLevels where a.Code == code select a).FirstOrDefault();
        }

        public void Add(ShipmentLevel entity)
        {
            context.ShipmentLevels.Add(entity);
        }

        public void Remove(ShipmentLevel entity)
        {
            context.ShipmentLevels.Attach(entity);
            context.ShipmentLevels.Remove(entity);
        }

        public void Update(ShipmentLevel entity)
        {
            context.ShipmentLevels.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ShipmentLevel> All()
        {
            return context.ShipmentLevels.ToList();
        }

        public IShipmentsContext context
        {
            get {return shipmentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ShipmentLevel> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ShipmentLevel GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}