using System.Collections.Generic;
using System.Linq;

using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ShipmentCustomerTypeRepository: IRepository<ShipmentCustomerType>
    {
        IShipmentsContext shipmentContext;

        public ShipmentCustomerTypeRepository()
        {
            shipmentContext = new ShipmentsContext();
        }

        public ShipmentCustomerTypeRepository(IShipmentsContext context)
        {
            shipmentContext = context;
        }

        public ShipmentCustomerTypeRepository(int tenant)
        {
            shipmentContext = ShipmentsContext.GetContext(tenant);
        }

        public ShipmentCustomerType GetSingleShipmentCustomerType(string code)
        {
            return (from a in context.ShipmentCustomerTypes
                    where a.Code == code
                    select a).FirstOrDefault();
        }
        public IQueryable<ShipmentCustomerType> GetAll()
        {
            return context.ShipmentCustomerTypes;
        }

        public IQueryable<ShipmentCustomerType> GetShipmentCustomerTypes()
        {
            return context.ShipmentCustomerTypes;
        }

        public void Add(ShipmentCustomerType entity)
        {
            context.ShipmentCustomerTypes.Add(entity);
        }

        public void Remove(ShipmentCustomerType entity)
        {
            context.ShipmentCustomerTypes.Attach(entity);
            context.ShipmentCustomerTypes.Remove(entity);
        }

        public void Update(ShipmentCustomerType entity)
        {
            context.ShipmentCustomerTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ShipmentCustomerType> All()
        {
            return context.ShipmentCustomerTypes.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ShipmentCustomerType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ShipmentCustomerType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}