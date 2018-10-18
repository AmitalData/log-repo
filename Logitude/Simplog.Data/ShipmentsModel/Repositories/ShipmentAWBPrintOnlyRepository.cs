using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;
namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ShipmentAWBPrintOnlyRepository: IRepository<ShipmentAWBPrintOnly>
    {
        IShipmentsContext shipmentContext;

        public ShipmentAWBPrintOnlyRepository(IShipmentsContext context)
        {
            shipmentContext = context;
        }

        public ShipmentAWBPrintOnlyRepository()
        {
            shipmentContext = new ShipmentsContext();
        }

        public ShipmentAWBPrintOnlyRepository(int tenant)
        {
            shipmentContext = ShipmentsContext.GetContext(tenant);
        }

        public ShipmentAWBPrintOnly GetSingleShipmentAWBPrintOnly(string id)
        {
            return (from a in context.ShipmentAWBPrintOnlies.Include("Currency")
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public IQueryable<ShipmentAWBPrintOnly> GetShipmentAWBPrintOnlyByTenant(int tenant)
        {
            return (from a in context.ShipmentAWBPrintOnlies.Include("Currency")
                    where a.Tenant == tenant
                    select a);
        }

        public List<ShipmentAWBPrintOnly> GetShipmentAWBPrintOnliesByShipment(string shipmentId, int tenant)
        {
            return (from a in context.ShipmentAWBPrintOnlies.Include("Currency")
                    where a.ShipmentId == shipmentId && a.Tenant == tenant
                    select a).ToList();
        }

        public void Add(ShipmentAWBPrintOnly entity)
        {
            context.ShipmentAWBPrintOnlies.Add(entity);
        }

        public void Remove(ShipmentAWBPrintOnly entity)
        {
            context.ShipmentAWBPrintOnlies.Attach(entity);
            context.ShipmentAWBPrintOnlies.Remove(entity);
        }

        public void Update(ShipmentAWBPrintOnly entity)
        {
            context.ShipmentAWBPrintOnlies.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ShipmentAWBPrintOnly> All()
        {
            return context.ShipmentAWBPrintOnlies.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ShipmentAWBPrintOnly> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ShipmentAWBPrintOnly GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}