using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ShipmentCommodityRepository : IRepository<ShipmentCommodity>
    {
        IShipmentsContext myContext;
        public IShipmentsContext Context
        {
            get { return myContext; }
        }

        public ShipmentCommodityRepository(int tenant)
        {
            myContext = ShipmentsContext.GetContext(tenant);
        }

        public ShipmentCommodityRepository(IShipmentsContext context)
        {
            myContext = context;
        }

        public ShipmentCommodity GetSingleCommodity(string id, int tenant)
        {
            return (from a in Context.ShipmentCommodities where a.Id == id && a.Tenant == tenant select a).FirstOrDefault();
        }

        public IQueryable<ShipmentCommodity> GetCommodities()
        {
            return (from a in Context.ShipmentCommodities select a);
        }

        public IQueryable<ShipmentCommodity> GetCommoditiesbyShipmentId(string shipmentId, int tenant)
        {
            return (from a in Context.ShipmentCommodities where a.ShipmentId == shipmentId && a.Tenant == tenant select a);
        }

        public void Add(ShipmentCommodity entity)
        {
            Context.ShipmentCommodities.Add(entity);
        }

        public void Remove(ShipmentCommodity entity)
        {
            Context.ShipmentCommodities.Attach(entity);
            Context.ShipmentCommodities.Remove(entity);
        }

        public void Update(ShipmentCommodity entity)
        {
            Context.ShipmentCommodities.Attach(entity);
            Context.SetAsModified(entity);
        }

        public List<ShipmentCommodity> All()
        {
            return Context.ShipmentCommodities.ToList();
        }

        public List<ShipmentCommodity> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ShipmentCommodity GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public void SubmitChanges()
        {
            Context.SaveChanges();
        }

    }
}
