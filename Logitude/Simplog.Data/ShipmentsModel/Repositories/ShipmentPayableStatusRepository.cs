using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ShipmentPayableStatusRepository: IRepository<ShipmentPayableStatus>
    {
        IShipmentsContext shipmentsContext;

        public ShipmentPayableStatusRepository()
        {
            shipmentsContext = new ShipmentsContext();
        }

        public ShipmentPayableStatusRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public ShipmentPayableStatusRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }

        public IQueryable<ShipmentPayableStatus> GetStatusTypes()
        {
            return context.ShipmentPayableStatus;
        }
        public IQueryable<ShipmentPayableStatus> GetAll()
        {
            return context.ShipmentPayableStatus;
        }

        public ShipmentPayableStatus GetSingleShipmentPayableStatus(string code)
        {
            return (from a in context.ShipmentPayableStatus
                    where a.Code == code
                    select a).FirstOrDefault();
        }

        public void Add(ShipmentPayableStatus entity)
        {
            context.ShipmentPayableStatus.Add(entity);
        }

        public void Remove(ShipmentPayableStatus entity)
        {
            context.ShipmentPayableStatus.Attach(entity);
            context.ShipmentPayableStatus.Remove(entity);
        }

        public void Update(ShipmentPayableStatus entity)
        {
            context.ShipmentPayableStatus.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ShipmentPayableStatus> All()
        {
            return context.ShipmentPayableStatus.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ShipmentPayableStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ShipmentPayableStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ShipmentPayableStatus> GetShipmentPayableStatus()
        {
            return context.ShipmentPayableStatus;
        }
    }
}