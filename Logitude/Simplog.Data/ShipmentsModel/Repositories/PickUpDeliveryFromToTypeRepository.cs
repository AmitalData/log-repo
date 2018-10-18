using System.Collections.Generic;
using System.Linq;

using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class PickUpDeliveryFromToTypeRepository: IRepository<PickUpDeliveryFromToType>
    {
        IShipmentsContext shipmentContext;

        public PickUpDeliveryFromToTypeRepository()
        {
            shipmentContext = new ShipmentsContext();
        }

        public PickUpDeliveryFromToTypeRepository(IShipmentsContext context)
        {
            shipmentContext = context;
        }

        public PickUpDeliveryFromToTypeRepository(int tenant)
        {
            shipmentContext = ShipmentsContext.GetContext(tenant);
        }

        public PickUpDeliveryFromToType GetSinglePickUpDeliveryFromToType(string code)
        {
            return (from a in context.PickUpDeliveryFromToTypes
                        where a.Code==code
                        select a).FirstOrDefault();
        }
        
        public IQueryable<PickUpDeliveryFromToType> GetPickUpDeliveryFromToTypes()
        {
            return context.PickUpDeliveryFromToTypes;
        }

        public void Add(PickUpDeliveryFromToType entity)
        {
            context.PickUpDeliveryFromToTypes.Add(entity);
        }

        public void Remove(PickUpDeliveryFromToType entity)
        {
            context.PickUpDeliveryFromToTypes.Attach(entity);
            context.PickUpDeliveryFromToTypes.Remove(entity);
        }

        public void Update(PickUpDeliveryFromToType entity)
        {
            context.PickUpDeliveryFromToTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PickUpDeliveryFromToType> All()
        {
            return context.PickUpDeliveryFromToTypes.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }



        public List<PickUpDeliveryFromToType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public PickUpDeliveryFromToType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}