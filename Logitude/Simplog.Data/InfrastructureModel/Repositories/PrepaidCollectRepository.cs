using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class PrepaidCollectRepository:IRepository<PrepaidCollect>
    {
         IWebFreightContext webFreightContext;
        public PrepaidCollectRepository()
        {
            webFreightContext = new WebFreightContext();
        }

        public PrepaidCollectRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }
        public PrepaidCollectRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public IQueryable<PrepaidCollect> GetPrepaidCollects()
        {
            return context.PrepaidCollects;
        }

        public IQueryable<PrepaidCollect> GetAll()
        {
            return context.PrepaidCollects;
        }


        public PrepaidCollect GetSinglePrepaidCollect(string id)
        {
            return (from a in context.PrepaidCollects
                    where a.Id == id
                    select a).FirstOrDefault();
        }

      

        #region IRepository<PrepaidCollect> Members

        public void Add(PrepaidCollect entity)
        {
            context.PrepaidCollects.Add(entity);
        }

        public void Remove(PrepaidCollect entity)
        {
            context.PrepaidCollects.Attach(entity);
            context.PrepaidCollects.Remove(entity);
        }

        public void Update(PrepaidCollect entity)
        {
            context.PrepaidCollects.Attach(entity);
            context.SetAsModified(entity);


        }

        public List<PrepaidCollect> All()
        {
            return context.PrepaidCollects.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        #endregion


        public List<PrepaidCollect> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public PrepaidCollect GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}