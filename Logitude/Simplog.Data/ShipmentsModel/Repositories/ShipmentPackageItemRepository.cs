using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ShipmentPackageItemRepository : IRepository<ShipmentPackageItem>
    {
        IShipmentsContext shipmentsContext;

        public ShipmentPackageItemRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }

        public ShipmentPackageItemRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public ShipmentPackageItem GetSingleShipmentPackageItem(string packageId, int lineNumber, int tenant)
        {
            return (from a in context.ShipmentPackageItems where a.PackageId == packageId && a.LineNumber == lineNumber && a.Tenant == tenant select a).FirstOrDefault();
        }

        public IQueryable<ShipmentPackageItem> GetShipmentPackageItemsbyPackageId(string packageId, int tenant)
        {
            return (from a in context.ShipmentPackageItems where a.PackageId == packageId && a.Tenant == tenant select a);
        }

        public IQueryable<ShipmentPackageItem> GetShipmentPackageItems()
        {
            return (from a in context.ShipmentPackageItems select a);
        }

        public void Add(ShipmentPackageItem entity)
        {
            context.ShipmentPackageItems.Add(entity);
        }

        public void Remove(ShipmentPackageItem entity)
        {
            context.ShipmentPackageItems.Attach(entity);
            context.ShipmentPackageItems.Remove(entity);
        }

        public void Update(ShipmentPackageItem entity)
        {
            context.ShipmentPackageItems.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ShipmentPackageItem> All()
        {
            return context.ShipmentPackageItems.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ShipmentPackageItem> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ShipmentPackageItem GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
