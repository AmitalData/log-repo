using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class FWBStatusRepository : IRepository<FWBStatus>
    {
        IShipmentsContext shipmentsContext;

        public FWBStatusRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }

        public FWBStatusRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public FWBStatus GetSingleFWBStatus(string code)
        {
            return (from a in context.FWBStatus where a.Code == code select a).FirstOrDefault();
        }

        public IQueryable<FWBStatus> GetFWBStatus()
        {
            return (from a in context.FWBStatus select a);
        }

        public void Add(FWBStatus entity)
        {
            context.FWBStatus.Add(entity);
        }

        public void Remove(FWBStatus entity)
        {
            context.FWBStatus.Attach(entity);
            context.FWBStatus.Remove(entity);
        }

        public void Update(FWBStatus entity)
        {
            context.FWBStatus.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<FWBStatus> All()
        {
            return context.FWBStatus.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<FWBStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public FWBStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
