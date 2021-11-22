using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System.Collections.Generic;
using System.Linq;


namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ShipmentUnassignedFieldRepository : IRepository<ShipmentUnassignedField>
    {
        IShipmentsContext shipmentContext;
        public IShipmentsContext context
        {
            get { return shipmentContext; }
        }
        public ShipmentUnassignedFieldRepository(IShipmentsContext context)
        {
            shipmentContext = context;
        }

        public ShipmentUnassignedFieldRepository()
        {
            shipmentContext = new ShipmentsContext();
        }

        public ShipmentUnassignedFieldRepository(int tenant)
        {
            shipmentContext = ShipmentsContext.GetContext(tenant);
        }

        public IQueryable<ShipmentUnassignedField> GetShipmentUnassignedFields(int tenant)
        {
            return (from record in context.ShipmentUnassignedFields
                    where record.Tenant == tenant
                    select record);
        }

        public ShipmentUnassignedField GetSingleShipmentUnassignedField(string id, int tenant)
        {
            return (from record in context.ShipmentUnassignedFields where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public List<ShipmentUnassignedField> All()
        {
            return context.ShipmentUnassignedFields.ToList();
        }

        public void Add(ShipmentUnassignedField entity)
        {
            context.ShipmentUnassignedFields.Add(entity);
        }

        public void Update(ShipmentUnassignedField entity)
        {
            try
            {
                context.ShipmentUnassignedFields.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public void Remove(ShipmentUnassignedField entity)
        {
            try
            {
                context.ShipmentUnassignedFields.Attach(entity);
            }
            catch { }
            context.ShipmentUnassignedFields.Remove(entity);
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ShipmentUnassignedField> GetMulti(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ShipmentUnassignedField GetSingle(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

    }
}
