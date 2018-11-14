using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class PickUpDeliveryPackageHarmonizeRepository : IRepository<PickUpDeliveryPackageHarmonize>
    {
        IShipmentsContext shipmentsContext;

        public PickUpDeliveryPackageHarmonizeRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public PickUpDeliveryPackageHarmonizeRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }

        public IQueryable<PickUpDeliveryPackageHarmonize> GetPickUpDeliveryPackageHarmonizes(int tenant)
        {
            return (from record in context.PickUpDeliveryPackageHarmonizes where record.Tenant == tenant select record);
        }

        public PickUpDeliveryPackageHarmonize GetSinglePickUpDeliveryPackageHarmonize(string id, int tenant)
        {
            return (from record in context.PickUpDeliveryPackageHarmonizes where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public IQueryable<PickUpDeliveryPackageHarmonize> GetPickUpDeliveryPackageHarmonizesByShipmentPackageId(string shipmentPackageId, int tenant)
        {
            var insidePackages = from a in context.PickUpDeliveryPackageHarmonizes where a.Tenant == tenant && a.PackageId == shipmentPackageId select a;
            return insidePackages;
        }

        public void Add(PickUpDeliveryPackageHarmonize entity)
        {
            context.PickUpDeliveryPackageHarmonizes.Add(entity);
        }

        public void Remove(PickUpDeliveryPackageHarmonize entity)
        {
            context.PickUpDeliveryPackageHarmonizes.Attach(entity);
            context.PickUpDeliveryPackageHarmonizes.Remove(entity);
        }

        public void Update(PickUpDeliveryPackageHarmonize entity)
        {
            context.PickUpDeliveryPackageHarmonizes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PickUpDeliveryPackageHarmonize> All()
        {
            return context.PickUpDeliveryPackageHarmonizes.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<PickUpDeliveryPackageHarmonize> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public PickUpDeliveryPackageHarmonize GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
