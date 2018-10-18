using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ShipmentCustomsTransmissionRepository : IRepository<ShipmentCustomsTransmission>
    {
        IShipmentsContext myContext;
        public IShipmentsContext Context
        {
            get { return myContext; }
        }

        public ShipmentCustomsTransmissionRepository(int tenant)
        {
            myContext = ShipmentsContext.GetContext(tenant);
        }

        public ShipmentCustomsTransmissionRepository(IShipmentsContext context)
        {
            myContext = context;
        }

        public ShipmentCustomsTransmission GetSingleShipmentCustomsTransmission(string id, int tenant)
        {
            return (from a in Context.ShipmentCustomsTransmissions where a.Id == id select a).FirstOrDefault();
        }

        public IQueryable<ShipmentCustomsTransmission> GetShipmentCustomsTransmissions(int tenant)
        {
            return (from a in Context.ShipmentCustomsTransmissions where a.Tenant == tenant select a);
        }

        public IQueryable<ShipmentCustomsTransmission> GetShipmentCustomsTransmissionsbyShipmentId(string shipmentId, int tenant)
        {
            return (from a in Context.ShipmentCustomsTransmissions where a.ShipmentId == shipmentId select a);
        }

        public void Add(ShipmentCustomsTransmission entity)
        {
            Context.ShipmentCustomsTransmissions.Add(entity);
        }

        public void Remove(ShipmentCustomsTransmission entity)
        {
            Context.ShipmentCustomsTransmissions.Attach(entity);
            Context.ShipmentCustomsTransmissions.Remove(entity);
        }

        public void Update(ShipmentCustomsTransmission entity)
        {
            Context.ShipmentCustomsTransmissions.Attach(entity);
            Context.SetAsModified(entity);
        }

        public List<ShipmentCustomsTransmission> All()
        {
            return Context.ShipmentCustomsTransmissions.ToList();
        }

        public List<ShipmentCustomsTransmission> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ShipmentCustomsTransmission GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public void SubmitChanges()
        {
            Context.SaveChanges();
        }

        public ShipmentCustomsTransmission GetSingleCustomsTransmissionbyShipmentIdAndMessageType(string shipmentId, string messageType, int tenant)
        {
            return (from a in Context.ShipmentCustomsTransmissions where a.ShipmentId == shipmentId 
                    && a.MessageCode == messageType 
                    && a.Tenant == tenant
                    select a).FirstOrDefault();
        }
    }
}