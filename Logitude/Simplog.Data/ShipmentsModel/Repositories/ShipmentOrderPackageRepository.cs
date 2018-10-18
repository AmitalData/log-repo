using System.Collections.Generic;
using System.Linq;

using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ShipmentOrderPackageRepository: IRepository<ShipmentOrderPackage>
    {
        IShipmentsContext shipmentContext;

        public ShipmentOrderPackageRepository()
        {
            shipmentContext = new ShipmentsContext();
        }

        public ShipmentOrderPackageRepository(IShipmentsContext context)
        {
            shipmentContext = context;
        }

        public ShipmentOrderPackageRepository(int tenant)
        {
            shipmentContext = ShipmentsContext.GetContext(tenant);
        }

        public ShipmentOrderPackage GetSingleShipmentOrderPackage(string id)
        {
            return (from a in context.ShipmentOrderPackages
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public void Add(ShipmentOrderPackage entity)
        {
            context.ShipmentOrderPackages.Add(entity);
        }

        public void Remove(ShipmentOrderPackage entity)
        {
            context.ShipmentOrderPackages.Attach(entity);
            context.ShipmentOrderPackages.Remove(entity);
        }

        public void Update(ShipmentOrderPackage entity)
        {
            context.ShipmentOrderPackages.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ShipmentOrderPackage> All()
        {
            return context.ShipmentOrderPackages.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ShipmentOrderPackage> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ShipmentOrderPackage GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}