using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class INTTRAStatusRepository: IRepository<INTTRAStatus>
    {
        IShipmentsContext shipmentsContext;
        public INTTRAStatusRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }
        public INTTRAStatusRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public INTTRAStatus GetSingleINTTRAStatus(string code)
        {
            return (from a in context.INTTRAStatuses where a.Code == code select a).FirstOrDefault();
        }

        public IQueryable<INTTRAStatus> GetINTTRAStatuses()
        {
            return (from a in context.INTTRAStatuses select a);
        }

        public void Add(INTTRAStatus entity)
        {
            context.INTTRAStatuses.Add(entity);
        }

        public void Remove(INTTRAStatus entity)
        {
            context.INTTRAStatuses.Attach(entity);
            context.INTTRAStatuses.Remove(entity);
        }

        public void Update(INTTRAStatus entity)
        {
            context.INTTRAStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<INTTRAStatus> All()
        {
            return context.INTTRAStatuses.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<INTTRAStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public INTTRAStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
