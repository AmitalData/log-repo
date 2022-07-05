using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ShipmentDocsFieldRepository : IRepository<ShipmentDocsField>
    {
        IShipmentsContext shipmentContext;

        public ShipmentDocsFieldRepository(IShipmentsContext context)
        {
            shipmentContext = context;
        }

        public ShipmentDocsFieldRepository()
        {
            shipmentContext = new ShipmentsContext();
        }

        public ShipmentDocsFieldRepository(int tenant)
        {
            shipmentContext = ShipmentsContext.GetContext(tenant);
        }

        public ShipmentDocsField GetSingleShipmentDocsField(string id, int tenant)
        {
            if (!string.IsNullOrEmpty(id))
            {

                ShipmentDocsField entity = (from a in context.ShipmentDocsFields
                                                 where a.Id == id && a.Tenant == tenant
                                                 select a).FirstOrDefault();

                return entity;
            }
            return null;
        }

        public void Add(ShipmentDocsField entity)
        {
            context.ShipmentDocsFields.Add(entity);
        }

        public void Remove(ShipmentDocsField entity)
        {
            context.ShipmentDocsFields.Attach(entity);
            context.ShipmentDocsFields.Remove(entity);
        }

        public void Update(ShipmentDocsField entity)
        {
            context.ShipmentDocsFields.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ShipmentDocsField> All()
        {
            return context.ShipmentDocsFields.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ShipmentDocsField> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ShipmentDocsField GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
