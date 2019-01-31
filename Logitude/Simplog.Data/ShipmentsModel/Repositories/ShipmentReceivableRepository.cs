using System.Collections.Generic;
using System.Linq;

using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;
using System;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ShipmentReceivableRepository: IRepository<ShipmentReceivable>
    {
        IShipmentsContext shipmentsContext;

        public ShipmentReceivableRepository()
        {
            shipmentsContext = new ShipmentsContext();
        }

        public ShipmentReceivableRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public ShipmentReceivableRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }

        public IQueryable<ShipmentReceivable> GetShipmentReceivables(int tenant)
        {
            return (from record in context.ShipmentReceivables where record.Tenant == tenant select record);
        }

        public ShipmentReceivable GetSingleShipmentReceivable(string id, int tenant)
        {
            return (from record in context.ShipmentReceivables where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public List<ShipmentReceivable> GetShipmentReceivablesByEntityIds(List<string> ids, int tenant)
        {
            List<ShipmentReceivable> myResult = new List<ShipmentReceivable>();

            if (ids.Count > 0)
            {
                myResult = (from a in context.ShipmentReceivables
                            where a.Tenant == tenant && ids.Contains(a.ShipmentId)
                            select a).ToList();
            }

            return myResult;
        }

        public List<ShipmentReceivable> GetShipmentReceivablesByShipmentId(string id, int tenant)
        {
            List<ShipmentReceivable> shipmentReceivables = (from a in context.ShipmentReceivables.Include("ChargesType").Include("Currency").Include("Measurement")
                                                            where a.Tenant == tenant && a.ShipmentId == id 
                                                            select a).ToList();
            return shipmentReceivables;
        }
        public List<ShipmentReceivable> GetShipmentReceivablesByParentId(string myParentId, int tenant)
        {
            List<ShipmentReceivable> shipmentReceivables = (from a in context.ShipmentReceivables
                                                            where a.Tenant == tenant && a.ShipmentReceivableParentId == myParentId
                                                            select a).ToList();
            return shipmentReceivables;
        }

        public void Add(ShipmentReceivable entity)
        {
            context.ShipmentReceivables.Add(entity);
        }

        public void Remove(ShipmentReceivable entity)
        {
            try
            {

                context.ShipmentReceivables.Attach(entity);
            }
            catch { }
            context.ShipmentReceivables.Remove(entity);
        }

        public void Update(ShipmentReceivable entity)
        {
            try
            {
                context.ShipmentReceivables.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<ShipmentReceivable> All()
        {
            return context.ShipmentReceivables.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ShipmentReceivable> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ShipmentReceivable GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public List<ShipmentReceivable> GetChildReceivableByParentPayable(string id, int tenant)
        {
            return (from a in context.ShipmentReceivables where a.ShipmentReceivableParentId == id && a.Tenant == tenant select a).ToList();
        }

        public List<ShipmentReceivable> GetConsoleChildReceivables(string shipmentId, int tenant)
        {
            return (from a in context.ShipmentReceivables
                    where a.ShipmentId == shipmentId
                    && a.Tenant == tenant
                    && a.ShipmentReceivableParentId != null
                    select a).ToList();
        }
    }
}
