using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ShipmentProductItemRepository : IRepository<ShipmentProductItem>
    {
        IShipmentsContext shipmentContext;

        public ShipmentProductItemRepository(IShipmentsContext context)
        {
            shipmentContext = context;
        }

        public ShipmentProductItemRepository()
        {
            shipmentContext = new ShipmentsContext();
        }

        public ShipmentProductItemRepository(int tenant)
        {
            shipmentContext = ShipmentsContext.GetContext(tenant);
        }

        public IQueryable<ShipmentProductItem> GetShipmentProductItems(int tenant)
        {
            return (from record in context.ShipmentProductItems
                    where record.Tenant == tenant
                    select record);
        }

        public ShipmentProductItem GetSingleShipmentProductItem(string id, int tenant)
        {
            return (from record in context.ShipmentProductItems where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public IQueryable<ShipmentProductItem> GetShipmentProductItemsForShipmentTenant(string shipmentId, int tenant)
        {
            IQueryable<ShipmentProductItem> result = from a in context.ShipmentProductItems where a.Tenant == tenant && a.ShipmentId == shipmentId select a;
            return result;
        }

        public void Add(ShipmentProductItem entity)
        {
            context.ShipmentProductItems.Add(entity);
        }

        public void Remove(ShipmentProductItem entity)
        {
            try
            {
                context.ShipmentProductItems.Attach(entity);
            }
            catch { }
            context.ShipmentProductItems.Remove(entity);
        }

        public void Update(ShipmentProductItem entity)
        {
            try
            {
                context.ShipmentProductItems.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<ShipmentProductItem> All()
        {
            return context.ShipmentProductItems.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ShipmentProductItem> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ShipmentProductItem GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
