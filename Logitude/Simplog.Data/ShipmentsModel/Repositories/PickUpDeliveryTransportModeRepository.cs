using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class PickUpDeliveryTransportModeRepository : IRepository<PickUpDeliveryTransportMode>
    {
        IShipmentsContext shipmentsContext;
        public PickUpDeliveryTransportModeRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }
        public PickUpDeliveryTransportModeRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public PickUpDeliveryTransportMode GetSinglePickUpDeliveryTransportMode(string code)
        {
            return (from a in context.PickUpDeliveryTransportModes where a.Code == code select a).FirstOrDefault();
        }

        public IQueryable<PickUpDeliveryTransportMode> GetPickUpDeliveryTransportModes()
        {
            return (from a in context.PickUpDeliveryTransportModes select a);
        }

        public IQueryable<PickUpDeliveryTransportMode> GetAll()
        {
            return (from a in context.PickUpDeliveryTransportModes select a);
        }

        public void Add(PickUpDeliveryTransportMode entity)
        {
            context.PickUpDeliveryTransportModes.Add(entity);
        }

        public void Remove(PickUpDeliveryTransportMode entity)
        {
            context.PickUpDeliveryTransportModes.Attach(entity);
            context.PickUpDeliveryTransportModes.Remove(entity);
        }

        public void Update(PickUpDeliveryTransportMode entity)
        {
            context.PickUpDeliveryTransportModes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PickUpDeliveryTransportMode> All()
        {
            return context.PickUpDeliveryTransportModes.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<PickUpDeliveryTransportMode> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public PickUpDeliveryTransportMode GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
