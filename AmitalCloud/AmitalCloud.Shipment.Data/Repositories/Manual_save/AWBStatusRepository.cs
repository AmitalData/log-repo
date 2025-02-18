using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class AWBStatusRepository: IRepository<AWBStatus>
    {
       IShipmentsContext shipmentsContext;

        public AWBStatusRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }

        public AWBStatusRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public AWBStatus GetSingleAWBStatus(string code)
        {
            return (from a in context.AWBStatus where a.Code == code select a).FirstOrDefault();
        }

        public IQueryable<AWBStatus> GetAWBStatus()
        {
            return (from a in context.AWBStatus select a);
        }

        public void Add(AWBStatus entity)
        {
            context.AWBStatus.Add(entity);
        }

        public void Remove(AWBStatus entity)
        {
            context.AWBStatus.Attach(entity);
            context.AWBStatus.Remove(entity);
        }

        public void Update(AWBStatus entity)
        {
            context.AWBStatus.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AWBStatus> All()
        {
            return context.AWBStatus.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<AWBStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public AWBStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
