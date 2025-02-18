using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ShipmentReceivableLineStatusRepository : IRepository<ShipmentReceivableLineStatus>
    {
        IShipmentsContext shipmentsContext;

        public ShipmentReceivableLineStatusRepository()
        {
            shipmentsContext = new ShipmentsContext();
        }

        public ShipmentReceivableLineStatusRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public ShipmentReceivableLineStatusRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }

        public IQueryable<ShipmentReceivableLineStatus> GetShipmentReceivableLineStatus()
        {
            return context.ShipmentReceivableLineStatus;
        }

        public  ShipmentReceivableLineStatus GetSingleShipmentReceivableLineStatusByCode(string code)
        {
            return (from a in context.ShipmentReceivableLineStatus
                    where a.Code == code
                    select a).FirstOrDefault();
        }

        public void Add(ShipmentReceivableLineStatus entity)
        {
            context.ShipmentReceivableLineStatus.Add(entity);
        }

        public void Remove(ShipmentReceivableLineStatus entity)
        {
            context.ShipmentReceivableLineStatus.Attach(entity);
            context.ShipmentReceivableLineStatus.Remove(entity);
        }

        public void Update(ShipmentReceivableLineStatus entity)
        {
            context.ShipmentReceivableLineStatus.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ShipmentReceivableLineStatus> All()
        {
            return context.ShipmentReceivableLineStatus.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ShipmentReceivableLineStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ShipmentReceivableLineStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}