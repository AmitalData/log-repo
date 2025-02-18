using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class MappedShipmentDirectionsRepository : IRepository<MappedShipmentDirections>
    {
        IShipmentsContext shipmentsContext;

        public MappedShipmentDirectionsRepository()
        {
            shipmentsContext = new ShipmentsContext();
        }

        public MappedShipmentDirectionsRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }

        public MappedShipmentDirectionsRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public MappedShipmentDirections GetSingleMappedShipmentDirection(int tenant,string shipmentDirectionId)
        {
            return (from a in context.MappedShipmentDirections.Include("Direction")
                    where a.Tenant == tenant && a.ShipmentDirectionId == shipmentDirectionId
                    select a).FirstOrDefault();
        }


        public void Add(MappedShipmentDirections entity)
        {
            context.MappedShipmentDirections.Add(entity); 
        }

        public void Remove(MappedShipmentDirections entity)
        {
            context.MappedShipmentDirections.Attach(entity);
            context.MappedShipmentDirections.Remove(entity);

        }

        public void Update(MappedShipmentDirections entity)
        {
            try
            {
                context.MappedShipmentDirections.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);

        }

        public List<MappedShipmentDirections> All()
        {
            return context.MappedShipmentDirections.ToList();

        }

        public void SubmitChanges()
        {
            context.SaveChanges(); 
        }

        public List<MappedShipmentDirections> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public MappedShipmentDirections GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }


        public IQueryable<MappedShipmentDirections> GetMappedShipmentDirections(int tenant)
        {
            return context.MappedShipmentDirections.Where(aa => aa.Tenant == tenant);
        }
    }
}
