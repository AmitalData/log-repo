using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ShipmentReceivableStatusRepository: IRepository<ShipmentReceivableStatus>
    {
        IShipmentsContext shipmentsContext;

        public ShipmentReceivableStatusRepository()
        {
            shipmentsContext = new ShipmentsContext();
        }

        public ShipmentReceivableStatusRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public ShipmentReceivableStatusRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }
        public IQueryable<ShipmentReceivableStatus> GetAll()
        {
            return context.ShipmentReceivableStatus;
        }
        public IQueryable<ShipmentReceivableStatus> GetStatusTypes()
        {
            return context.ShipmentReceivableStatus;
        }

        public ShipmentReceivableStatus GetSingleShipmentReceivableStatus(string code)
        {
            return (from a in context.ShipmentReceivableStatus where a.Code == code select a).FirstOrDefault();
        }

        public void Add(ShipmentReceivableStatus entity)
        {
            context.ShipmentReceivableStatus.Add(entity);
        }

        public void Remove(ShipmentReceivableStatus entity)
        {
            context.ShipmentReceivableStatus.Attach(entity);
            context.ShipmentReceivableStatus.Remove(entity);
        }

        public void Update(ShipmentReceivableStatus entity)
        {
            context.ShipmentReceivableStatus.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ShipmentReceivableStatus> All()
        {
            return context.ShipmentReceivableStatus.ToList();
        }

        public IShipmentsContext context
        {
            get {return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ShipmentReceivableStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ShipmentReceivableStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ShipmentReceivableStatus> GetShipmentReceivableStatus()
        {
            return context.ShipmentReceivableStatus;
        }
    }
}