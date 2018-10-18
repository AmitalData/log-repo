using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ShipmentAssemblyRepository : IRepository<ShipmentAssembly>
    {
        IShipmentsContext shipmentContext;

        public ShipmentAssemblyRepository(IShipmentsContext context)
        {
            shipmentContext = context;
        }

        public ShipmentAssemblyRepository()
        {
            shipmentContext = new ShipmentsContext();
        }

        public ShipmentAssemblyRepository(int tenant)
        {
            shipmentContext = ShipmentsContext.GetContext(tenant);
        }

        public IQueryable<ShipmentAssembly> GetShipmentAssemblies(int tenant)
        {
            return (from record in context.ShipmentAssemblies
                    where record.Tenant == tenant
                    select record);
        }

        public ShipmentAssembly GetSingleShipmentAssembly(string id, int tenant)
        {
            return (from record in context.ShipmentAssemblies where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public IQueryable<ShipmentAssembly> GetShipmentAssembliesForShipmentTenant(string shipmentId, int tenant)
        {
            IQueryable<ShipmentAssembly> result = from a in context.ShipmentAssemblies where a.Tenant == tenant && a.ShipmentId == shipmentId select a;
            return result;
        }
        
        public void Add(ShipmentAssembly entity)
        {
            context.ShipmentAssemblies.Add(entity);
        }

        public void Remove(ShipmentAssembly entity)
        {
            try
            {
                context.ShipmentAssemblies.Attach(entity);
            }
            catch { }
            context.ShipmentAssemblies.Remove(entity);
        }

        public void Update(ShipmentAssembly entity)
        {
            try
            {
                context.ShipmentAssemblies.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<ShipmentAssembly> All()
        {
            return context.ShipmentAssemblies.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ShipmentAssembly> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ShipmentAssembly GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
