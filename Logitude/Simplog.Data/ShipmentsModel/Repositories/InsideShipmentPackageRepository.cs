using System.Collections.Generic;
using System.Linq;

using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class InsideShipmentPackageRepository: IRepository<InsideShipmentPackage>
    {
        IShipmentsContext shipmentsContext;

        public InsideShipmentPackageRepository()
        {
            shipmentsContext = new ShipmentsContext();
        }

        public InsideShipmentPackageRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public InsideShipmentPackageRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }

        public IQueryable<InsideShipmentPackage> GetInsideShipmentPackages(int tenant)
        {
            return (from record in context.InsideShipmentPackages where record.Tenant == tenant select record);
        }

        public InsideShipmentPackage GetSingleInsideShipmentPackage(string id, int tenant)
        {
            return (from record in context.InsideShipmentPackages where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }
        
        public IQueryable<InsideShipmentPackage> GetInsidePackagesByShipmentPackageId(string shipmentPackageId, int tenant)
        {
            var insidePackages = from a in context.InsideShipmentPackages where a.Tenant == tenant && a.ShipmentPackageId == shipmentPackageId select a;
            return insidePackages;
        }

        public void Add(InsideShipmentPackage entity)
        {
           context.InsideShipmentPackages.Add(entity);
        }

        public void Remove(InsideShipmentPackage entity)
        {
            context.InsideShipmentPackages.Attach(entity);
            context.InsideShipmentPackages.Remove(entity);
        }

        public void Update(InsideShipmentPackage entity)
        {
            context.InsideShipmentPackages.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<InsideShipmentPackage> All()
        {
            return context.InsideShipmentPackages.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<InsideShipmentPackage> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public InsideShipmentPackage GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}