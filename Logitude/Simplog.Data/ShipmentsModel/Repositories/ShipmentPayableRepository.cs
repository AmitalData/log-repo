using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ShipmentPayableRepository: IRepository<ShipmentPayable>
    {
        IShipmentsContext shipmentsContext;

        public ShipmentPayableRepository()
        {
            shipmentsContext = new ShipmentsContext();
        }

        public ShipmentPayableRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }

        public ShipmentPayableRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public ShipmentPayable GetSingleShipmentPayable(string id)
        {
            return (from a in context.ShipmentPayables.Include("Currency")
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public List<ShipmentPayable> GetShipemntPayablesByShipmentId(string shipmentId, int tenant)
        {
            return (from a in context.ShipmentPayables.Include("ChargesType").Include("Currency").Include("Measurement")
                    where a.ShipmentId == shipmentId && a.Tenant == tenant
                    select a).ToList();
        }

        public List<ShipmentPayable> GetChildPayablesByParentPayable(string payableId, int tenant)
        {
            return (from a in context.ShipmentPayables where a.ShipmentPayableParentId == payableId && a.Tenant == tenant select a).ToList();
        }

        public List<ShipmentPayable> GetConsoleChildPayables(string shipmentId, int tenant)
        {
            return (from a in context.ShipmentPayables
                    where a.ShipmentId == shipmentId 
                    && a.Tenant == tenant
                    && a.ShipmentPayableParentId != null
                    select a).ToList();
        }

        public List<ShipmentPayable> GetShipmentPayablesByEntityIds(List<string> ids, int tenant)
        {
            List<ShipmentPayable> myResult = new List<ShipmentPayable>();

            if (ids.Count > 0)
            {
                myResult = (from a in context.ShipmentPayables
                            where a.Tenant == tenant && ids.Contains(a.ShipmentId)
                            select a).ToList();                
            }

            return myResult;
        }
        public List<ShipmentPayable> GetShipmentPayablesFromIdList(List<string> ids, int tenant)
        {
            List<ShipmentPayable> myResult = new List<ShipmentPayable>();

            if (ids.Count > 0)
            {
                myResult = (from a in context.ShipmentPayables
                            where a.Tenant == tenant && ids.Contains(a.Id)
                            select a).ToList();
            }

            return myResult;
        }

        public IQueryable<ShipmentPayable> GetShipmentPayablesByShipmentIds(List<string> ids, int tenant)
        {
            IQueryable<ShipmentPayable> myResult = null;

            if (ids.Count > 0)
            {
                myResult = from a in context.ShipmentPayables.Include("ChargesType").Include("VendorCard")
                           where a.Tenant == tenant && ids.Contains(a.ShipmentId)
                           select a;
            }

            return myResult;
        }
       
        public IQueryable<ShipmentPayable> GetShipmentPayables(int tenant)
        {
            return (from record in context.ShipmentPayables
                    where record.Tenant == tenant
                    select record);
        }

        public void Add(ShipmentPayable entity)
        {
            context.ShipmentPayables.Add(entity);
        }

        public void Remove(ShipmentPayable entity)
        {
            context.ShipmentPayables.Attach(entity);
            context.ShipmentPayables.Remove(entity);
        }

        public void Update(ShipmentPayable entity)
        {
            try
            {
                context.ShipmentPayables.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<ShipmentPayable> All()
        {
            return context.ShipmentPayables.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ShipmentPayable> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ShipmentPayable GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
