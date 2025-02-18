using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class INTTRASIStatusRepository : IRepository<INTTRASIStatus>
    {
        IShipmentsContext shipmentsContext;
        public INTTRASIStatusRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }
        public INTTRASIStatusRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public INTTRASIStatus GetSingleINTTRASIStatus(string code)
        {
            return (from a in context.INTTRASIStatus where a.Code == code select a).FirstOrDefault();
        }

        public IQueryable<INTTRASIStatus> GetINTTRASIStatus()
        {
            return (from a in context.INTTRASIStatus select a);
        }

        public void Add(INTTRASIStatus entity)
        {
            context.INTTRASIStatus.Add(entity);
        }

        public void Remove(INTTRASIStatus entity)
        {
            context.INTTRASIStatus.Attach(entity);
            context.INTTRASIStatus.Remove(entity);
        }

        public void Update(INTTRASIStatus entity)
        {
            context.INTTRASIStatus.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<INTTRASIStatus> All()
        {
            return context.INTTRASIStatus.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<INTTRASIStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public INTTRASIStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
