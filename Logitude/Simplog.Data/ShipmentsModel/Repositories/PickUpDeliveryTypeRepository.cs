using System.Collections.Generic;
using System.Linq;

using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class PickUpDeliveryTypeRepository: IRepository<PickUpDeliveryType>
    {
        IShipmentsContext shipmentContext;

        public PickUpDeliveryTypeRepository()
        {
            shipmentContext = new ShipmentsContext();
        }

        public PickUpDeliveryTypeRepository(IShipmentsContext context)
        {
            shipmentContext = context;
        }

        public PickUpDeliveryTypeRepository(int tenant)
        {
            shipmentContext = ShipmentsContext.GetContext(tenant);
        }

        public PickUpDeliveryType GetSinglePickUpDeliveryType(string code)
        {
            return (from a in context.PickUpDeliveryTypes
                        where a.Code==code
                        select a).FirstOrDefault();
        }
        
        public IQueryable<PickUpDeliveryType> GetPickUpDeliveryTypes()
        {
            return context.PickUpDeliveryTypes;
        }

        public void Add(PickUpDeliveryType entity)
        {
            context.PickUpDeliveryTypes.Add(entity);
        }

        public void Remove(PickUpDeliveryType entity)
        {
            context.PickUpDeliveryTypes.Attach(entity);
            context.PickUpDeliveryTypes.Remove(entity);
        }

        public void Update(PickUpDeliveryType entity)
        {
            context.PickUpDeliveryTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PickUpDeliveryType> All()
        {
            return context.PickUpDeliveryTypes.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<PickUpDeliveryType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public PickUpDeliveryType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}