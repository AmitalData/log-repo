using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ShipmentAnalyticRepository : IRepository<ShipmentAnalytic>
    {
        readonly IShipmentsContext shipmentsContext;

        public ShipmentAnalyticRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }

        public ShipmentAnalyticRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public IQueryable<ShipmentAnalytic> GetAll()
        {
            return (from a in context.ShipmentAnalytics select a);
        }
        public IQueryable<ShipmentAnalytic> GetShipmentAnalytics()
        {
            return (from a in context.ShipmentAnalytics select a);
        }

        public void Add(ShipmentAnalytic entity)
        {
            context.ShipmentAnalytics.Add(entity);
        }

        public void Remove(ShipmentAnalytic entity)
        {
            context.ShipmentAnalytics.Attach(entity);
            context.ShipmentAnalytics.Remove(entity);
        }

        public void Update(ShipmentAnalytic entity)
        {
            context.ShipmentAnalytics.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ShipmentAnalytic> All()
        {
            return context.ShipmentAnalytics.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ShipmentAnalytic> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ShipmentAnalytic GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        internal void AddFromShipment(Shipment entity)
        {
            var shipmentAnalytic = ShipmentToShipmentAnalyticMapper.Map(entity);
            Add(shipmentAnalytic);

        }

        internal void UpdateFromShipment(Shipment entity)
        {
            var shipmentAnalytic = ShipmentToShipmentAnalyticMapper.Map(entity);
            if (context.ShipmentAnalytics.Any(e => e.Id == shipmentAnalytic.Id)) Update(shipmentAnalytic);
            else Add(shipmentAnalytic);
        }
    }
}
