using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ShipmentCarrierStatusRepository: IRepository<ShipmentCarrierStatus>
    {
        public IShipmentsContext Context { get; set; }

        public ShipmentCarrierStatusRepository()
        {
            this.Context = new ShipmentsContext();
        }

        public ShipmentCarrierStatusRepository(int tenant)
        {
            this.Context = ShipmentsContext.GetContext(tenant);
        }

        public ShipmentCarrierStatusRepository(IShipmentsContext context)
        {
            this.Context = context;
        }

        public bool DoesRecordExist(string hash)
        {
            bool exists = (from a in Context.ShipmentCarrierStatuses
                           where a.RecordHash == hash
                           select a).Any();
            return exists;
        }

        public IQueryable<ShipmentCarrierStatus> GetShipmentCarrierStatuses(int tenant)
        {
            return (from d in Context.ShipmentCarrierStatuses where d.Tenant == tenant select d);
        }

        public ShipmentCarrierStatus GetSingleShipmentCarrierStatus(string id, int tenant)
        {
            return (from d in Context.ShipmentCarrierStatuses where d.Id == id && d.Tenant == tenant select d).FirstOrDefault();
        }

        public IQueryable<ShipmentCarrierStatus> GetShipmentCarrierStatusesByShipmentId(string shipmentId, int tenant)
        {
            return (from d in Context.ShipmentCarrierStatuses where d.Tenant == tenant && d.ShipmentId == shipmentId select d);
        }
       
        public void Add(ShipmentCarrierStatus entity)
        {
            Context.ShipmentCarrierStatuses.Add(entity);
        }

        public void Remove(ShipmentCarrierStatus entity)
        {
            try
            {
                Context.ShipmentCarrierStatuses.Attach(entity);
            }
            catch { }
            Context.ShipmentCarrierStatuses.Remove(entity);
        }

        public void Update(ShipmentCarrierStatus entity)
        {
            try
            {
                Context.ShipmentCarrierStatuses.Attach(entity);
            }
            catch { }
            Context.SetAsModified(entity);
        }

        public List<ShipmentCarrierStatus> All()
        {
            return Context.ShipmentCarrierStatuses.ToList();
        }

        public void SubmitChanges()
        {
            Context.SaveChanges();
        }

        public List<ShipmentCarrierStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ShipmentCarrierStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}