using System.Collections.Generic;
using System.Linq;

using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ShipmentPickUpDeliveryPackageRepository: IRepository<ShipmentPickUpDeliveryPackage>
    {
        IShipmentsContext shipmentsContext;

        public ShipmentPickUpDeliveryPackageRepository()
        {
            shipmentsContext = new ShipmentsContext();
        }

        public ShipmentPickUpDeliveryPackageRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public ShipmentPickUpDeliveryPackageRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }
        
        public ShipmentPickUpDeliveryPackage GetSingleShipmentPickUpDeliveryPackage(string id)
        {
            return (from a in context.ShipmentPickUpDeliveryPackages
                    where a.Id == id
                    select a).FirstOrDefault();
        }
        public IQueryable<ShipmentPickUpDeliveryPackage> GetSinglePickUpDeliveryPackageByIdsList(List<string> ids, int tenant)
        {
            return (from a in context.ShipmentPickUpDeliveryPackages
                    where ids.Contains(a.ShipmentPickUpDeliveryId) && a.Tenant == tenant
                    select a);
        }

        public void Add(ShipmentPickUpDeliveryPackage entity)
        {
            context.ShipmentPickUpDeliveryPackages.Add(entity);
        }

        public void Remove(ShipmentPickUpDeliveryPackage entity)
        {
            context.ShipmentPickUpDeliveryPackages.Attach(entity);
            context.ShipmentPickUpDeliveryPackages.Remove(entity);
        }

        public void Update(ShipmentPickUpDeliveryPackage entity)
        {
            context.ShipmentPickUpDeliveryPackages.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ShipmentPickUpDeliveryPackage> All()
        {
            return context.ShipmentPickUpDeliveryPackages.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ShipmentPickUpDeliveryPackage> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ShipmentPickUpDeliveryPackage GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<ShipmentPickUpDeliveryPackage> GetPackagesByDeliveryId(string deliveryId, int tenant)
        {
            return context.ShipmentPickUpDeliveryPackages.Where(d => d.Tenant == tenant && d.ShipmentPickUpDeliveryId == deliveryId);
        }
    }
}