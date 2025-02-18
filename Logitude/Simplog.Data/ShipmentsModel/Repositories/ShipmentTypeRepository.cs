using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ShipmentTypeRepository: IRepository<ShipmentType>
    {
        IShipmentsContext shipmentsContext;

        public ShipmentTypeRepository()
        {
            shipmentsContext = new ShipmentsContext();
        }

        public ShipmentTypeRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public ShipmentTypeRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }

        public IQueryable<ShipmentType> GetShipmentTypes()
        {
            return context.ShipmentTypes;
        }

        public ShipmentType GetSingleShipmentType(string id)
        {
            return (from record in context.ShipmentTypes where record.Id == id select record).FirstOrDefault();
        }
        public ShipmentType GetSingleShipmentTypeByName(string name)
        {
            return (from record in context.ShipmentTypes where record.Name == name select record).FirstOrDefault();
        }

        public void Add(ShipmentType entity)
        {
            context.ShipmentTypes.Add(entity);
        }

        public void Remove(ShipmentType entity)
        {
            context.ShipmentTypes.Remove(entity);
        }

        public void Update(ShipmentType entity)
        {
            context.ShipmentTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ShipmentType> All()
        {
            return context.ShipmentTypes.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ShipmentType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ShipmentType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}