using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ShipmentPickUpDeliveryRepository: IRepository<ShipmentPickUpDelivery>
    {
        IShipmentsContext shipmentsContext;

        public ShipmentPickUpDeliveryRepository()
        {
            shipmentsContext = new ShipmentsContext();
        }

        public ShipmentPickUpDeliveryRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public ShipmentPickUpDeliveryRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }

        public IQueryable<ShipmentPickUpDelivery> GetShipmentDeliveries(int tenant)
        {
            return (from record in context.ShipmentPickUpDeliveries where record.Tenant == tenant select record);
        }

        public ShipmentPickUpDelivery GetSingleShipmentPickUpDelivery(int tenant, string id)
        {
            return (from record in context.ShipmentPickUpDeliveries where record.Id == id select record).Include("FromPort").Include("ToPort").FirstOrDefault();
        }

        public List<ShipmentPickUpDelivery> GetShipmentPickUpDeliveryForShipment(string shipmentId, int tenant)
        {
            return (from a in context.ShipmentPickUpDeliveries
                    where a.ShipmentId == shipmentId && a.Tenant == tenant
                    select a).ToList();
        }

        public void Add(ShipmentPickUpDelivery entity)
        {
            context.ShipmentPickUpDeliveries.Add(entity);
        }

        public void Remove(ShipmentPickUpDelivery entity)
        {
            try
            {
                context.ShipmentPickUpDeliveries.Attach(entity);
            }
            catch { }
            context.ShipmentPickUpDeliveries.Remove(entity);
        }

        public void Update(ShipmentPickUpDelivery entity)
        {
            try
            {
                context.ShipmentPickUpDeliveries.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<ShipmentPickUpDelivery> All()
        {
            return context.ShipmentPickUpDeliveries.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ShipmentPickUpDelivery> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ShipmentPickUpDelivery GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
