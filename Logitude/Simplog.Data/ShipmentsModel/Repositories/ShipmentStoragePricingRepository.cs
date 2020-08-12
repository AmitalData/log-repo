using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ShipmentStoragePricingRepository : IRepository<ShipmentStoragePricing>
    {
        IShipmentsContext shipmentContext;

        public ShipmentStoragePricingRepository()
        {
            shipmentContext = new ShipmentsContext();
        }

        public ShipmentStoragePricingRepository(IShipmentsContext context)
        {
            shipmentContext = context;
        }

        public ShipmentStoragePricingRepository(int tenant)
        {
            shipmentContext = ShipmentsContext.GetContext(tenant);
        }

        public ShipmentStoragePricing GetSingleShipmentStoragePricing(string id, int tenant)
        {
            return (from a in context.ShipmentStoragePricings
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public void Add(ShipmentStoragePricing entity)
        {
            context.ShipmentStoragePricings.Add(entity);
        }

        public void Remove(ShipmentStoragePricing entity)
        {
            context.ShipmentStoragePricings.Attach(entity);
            context.ShipmentStoragePricings.Remove(entity);
        }

        public void Update(ShipmentStoragePricing entity)
        {
            context.ShipmentStoragePricings.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ShipmentStoragePricing> All()
        {
            return context.ShipmentStoragePricings.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
        
        public List<ShipmentStoragePricing> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ShipmentStoragePricing GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}