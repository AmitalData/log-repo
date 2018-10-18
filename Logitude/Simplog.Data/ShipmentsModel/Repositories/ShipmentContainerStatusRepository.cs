using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ShipmentContainerStatusRepository: IRepository<ShipmentContainerStatus>
    {
        public IShipmentsContext Context { get; set; }
        public ShipmentContainerStatusRepository()
        {
            this.Context = new ShipmentsContext();
        }
        public ShipmentContainerStatusRepository(int tenant)
        {
            this.Context = ShipmentsContext.GetContext(tenant);
        }
        public ShipmentContainerStatusRepository(IShipmentsContext context)
        {
            this.Context = context;
        }

        public bool DoesRecordExist(string hash)
        {
            bool exists = (from a in Context.ShipmentContainerStatuses where a.RecordHash == hash select a).Any();
            return exists;
        }

        public IQueryable<ShipmentContainerStatus> GetShipmentContainerStatuses(int tenant)
        {
            return (from d in Context.ShipmentContainerStatuses where d.Tenant == tenant select d);
        }

        public ShipmentContainerStatus GetSingleShipmentContainerStatus(string id, int tenant)
        {
            return (from d in Context.ShipmentContainerStatuses where d.Id == id && d.Tenant == tenant select d).FirstOrDefault();
        }

        public IQueryable<ShipmentContainerStatus> GetShipmentContainerStatusByShipmentId(string shipmentId, int tenant)
        {
            return (from d in Context.ShipmentContainerStatuses where d.Tenant == tenant && d.ShipmentId == shipmentId select d);
        }

        public void Add(ShipmentContainerStatus entity)
        {
            Context.ShipmentContainerStatuses.Add(entity);
        }

        public void Remove(ShipmentContainerStatus entity)
        {
            try
            {
                Context.ShipmentContainerStatuses.Attach(entity);
            }
            catch { }
            Context.ShipmentContainerStatuses.Remove(entity);
        }

        public void Update(ShipmentContainerStatus entity)
        {
            try
            {
                Context.ShipmentContainerStatuses.Attach(entity);
            }
            catch { }
            Context.SetAsModified(entity);
        }

        public List<ShipmentContainerStatus> All()
        {
            return Context.ShipmentContainerStatuses.ToList();
        }

        public void SubmitChanges()
        {
            Context.SaveChanges();
        }

        public List<ShipmentContainerStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ShipmentContainerStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
