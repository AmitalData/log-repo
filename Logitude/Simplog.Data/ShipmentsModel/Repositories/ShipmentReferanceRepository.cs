using System.Collections.Generic;
using System.Linq;

using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;
using System;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ShipmentReferanceRepository: IRepository<ShipmentReferance>
    {
        IShipmentsContext shipmentsContext;
        public ShipmentReferanceRepository()
        {
            shipmentsContext = new ShipmentsContext();
        }
        public ShipmentReferanceRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }
        public ShipmentReferanceRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }


        public IQueryable<ShipmentReferance> GetShipmentReferances(int tenant)
        {
            return (from record in context.ShipmentReferances where record.Tenant == tenant select record);
        }

        public ShipmentReferance GetSingleShipmentReferance(string id, int tenant)
        {
            return (from record in context.ShipmentReferances where record.ShipmentId == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public List<ShipmentReferance> GetShipmentReferancesByIds(List<string> ids, int tenant)
        {
            List<ShipmentReferance> myResult = new List<ShipmentReferance>();

            if (ids.Count > 0)
            {
                myResult = (from a in context.ShipmentReferances
                            where a.Tenant == tenant && ids.Contains(a.ShipmentId)
                            select a).ToList();
            }

            return myResult;
        }
        public List<ShipmentReferance> GetShipmentReferancesByEntityIds(List<string> ids, int tenant)
        {
            List<ShipmentReferance> myResult = new List<ShipmentReferance>();

            if (ids.Count > 0)
            {
                myResult = (from a in context.ShipmentReferances
                            where a.Tenant == tenant && ids.Contains(a.ShipmentId)
                            select a).ToList();
            }

            return myResult;
        }

        public List<ShipmentReferance> GetShipmentReferancesByShipmentId(string id, int tenant)
        {
            List<ShipmentReferance> shipmentReferances = (from a in context.ShipmentReferances.Include("ChargesType").Include("Currency").Include("Measurement")
                                                            where a.Tenant == tenant && a.ShipmentId == id 
                                                            select a).ToList();
            return shipmentReferances;
        }

        public void Add(ShipmentReferance entity)
        {
            context.ShipmentReferances.Add(entity);
        }

        public void Remove(ShipmentReferance entity)
        {
            try
            {

                context.ShipmentReferances.Attach(entity);
            }
            catch { }
            context.ShipmentReferances.Remove(entity);
        }

        public void Update(ShipmentReferance entity)
        {
            try
            {
                context.ShipmentReferances.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<ShipmentReferance> All()
        {
            return context.ShipmentReferances.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ShipmentReferance> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ShipmentReferance GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
