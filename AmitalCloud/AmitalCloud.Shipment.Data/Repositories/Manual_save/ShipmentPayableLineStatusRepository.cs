using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ShipmentPayableLineStatusRepository : IRepository<ShipmentPayableLineStatus>
    {
        IShipmentsContext shipmentsContext;
        public ShipmentPayableLineStatusRepository()
        {
            shipmentsContext = new ShipmentsContext();
        }

        public ShipmentPayableLineStatusRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }
        public ShipmentPayableLineStatusRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }
        public IQueryable<ShipmentPayableLineStatus> GetPayableStatusTypes()
        {
            return context.ShipmentPayableLineStatus;
        }

        public  ShipmentPayableLineStatus GetSinglePayableStatusTypeByCode(string code)
        {
            return (from a in context.ShipmentPayableLineStatus
                    where a.Code == code
                    select a).FirstOrDefault();
        }
        
        public void Add(ShipmentPayableLineStatus entity)
        {
            context.ShipmentPayableLineStatus.Add(entity);
        }

        public void Remove(ShipmentPayableLineStatus entity)
        {
            context.ShipmentPayableLineStatus.Attach(entity);
            context.ShipmentPayableLineStatus.Remove(entity);
        }

        public void Update(ShipmentPayableLineStatus entity)
        {
            context.ShipmentPayableLineStatus.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ShipmentPayableLineStatus> All()
        {
            return context.ShipmentPayableLineStatus.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ShipmentPayableLineStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ShipmentPayableLineStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}