using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ShipmentCustomsMessageTypeRepository : IRepository<ShipmentCustomsMessageType>
    {
        IShipmentsContext shipmentsContext;

        public ShipmentCustomsMessageTypeRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }

        public ShipmentCustomsMessageTypeRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public ShipmentCustomsMessageType GetSingleShipmentCustomsMessageType(string code)
        {
            return (from a in context.ShipmentCustomsMessageTypes where a.Code == code select a).FirstOrDefault();
        }
        public IQueryable<ShipmentCustomsMessageType> GetAll()
        {
            return (from a in context.ShipmentCustomsMessageTypes select a);
        }
        public IQueryable<ShipmentCustomsMessageType> GetShipmentCustomsMessageTypes()
        {
            return (from a in context.ShipmentCustomsMessageTypes select a);
        }

        public void Add(ShipmentCustomsMessageType entity)
        {
            context.ShipmentCustomsMessageTypes.Add(entity);
        }

        public void Remove(ShipmentCustomsMessageType entity)
        {
            context.ShipmentCustomsMessageTypes.Attach(entity);
            context.ShipmentCustomsMessageTypes.Remove(entity);
        }

        public void Update(ShipmentCustomsMessageType entity)
        {
            context.ShipmentCustomsMessageTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ShipmentCustomsMessageType> All()
        {
            return context.ShipmentCustomsMessageTypes.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ShipmentCustomsMessageType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ShipmentCustomsMessageType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
