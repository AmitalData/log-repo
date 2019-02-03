using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ShipmentPackageHarmonizeRepository : IRepository<ShipmentPackageHarmonize>
    {
        IShipmentsContext shipmentsContext;

        public ShipmentPackageHarmonizeRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public ShipmentPackageHarmonizeRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }

        public IQueryable<ShipmentPackageHarmonize> GetShipmentPackageHarmonizes(int tenant)
        {
            return (from record in context.ShipmentPackageHarmonizes where record.Tenant == tenant select record);
        }

        public ShipmentPackageHarmonize GetSingleShipmentPackageHarmonize(string id, int tenant)
        {
            return (from record in context.ShipmentPackageHarmonizes where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public IQueryable<ShipmentPackageHarmonize> GetShipmentPackageHarmonizesByShipmentPackageId(string shipmentPackageId, int tenant)
        {
            var insidePackages = from a in context.ShipmentPackageHarmonizes where a.Tenant == tenant && a.PackageId == shipmentPackageId select a;
            return insidePackages;
        }

        public void Add(ShipmentPackageHarmonize entity)
        {
            context.ShipmentPackageHarmonizes.Add(entity);
        }

        public void Remove(ShipmentPackageHarmonize entity)
        {
            context.ShipmentPackageHarmonizes.Attach(entity);
            context.ShipmentPackageHarmonizes.Remove(entity);
        }

        public void Update(ShipmentPackageHarmonize entity)
        {
            context.ShipmentPackageHarmonizes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ShipmentPackageHarmonize> All()
        {
            return context.ShipmentPackageHarmonizes.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ShipmentPackageHarmonize> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ShipmentPackageHarmonize GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
