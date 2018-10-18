using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class FHLStatusRepository: IRepository<FHLStatus>
    {
       IShipmentsContext shipmentsContext;

        public FHLStatusRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }

        public FHLStatusRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public FHLStatus GetSingleFHLStatus(string code)
        {
            return (from a in context.FHLStatus where a.Code == code select a).FirstOrDefault();
        }

        public IQueryable<FHLStatus> GetFHLStatus()
        {
            return (from a in context.FHLStatus select a);
        }

        public void Add(FHLStatus entity)
        {
            context.FHLStatus.Add(entity);
        }

        public void Remove(FHLStatus entity)
        {
            context.FHLStatus.Attach(entity);
            context.FHLStatus.Remove(entity);
        }

        public void Update(FHLStatus entity)
        {
            context.FHLStatus.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<FHLStatus> All()
        {
            return context.FHLStatus.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<FHLStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public FHLStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
