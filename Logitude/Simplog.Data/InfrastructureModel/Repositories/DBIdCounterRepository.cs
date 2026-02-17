using System.Collections.Generic;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class DBIdCounterRepository : IRepository<DBIdCounter>
    {
        IWebFreightContext webFreightContext;
        public DBIdCounterRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public DBIdCounterRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }
        public DBIdCounterRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        #region IRepository<DBIdCounter> Members

        public void Add(DBIdCounter entity)
        {
            //this.context.Counters.Add(entity);
        }

        public void Remove(DBIdCounter entity)
        {
            //this.context.Counters.Remove(entity);
        }

        public void Update(DBIdCounter entity)
        {
            // this.context.Counters.Attach(entity);
            //this.context.SetAsModified(entity);
        }

        public List<DBIdCounter> All()
        {
            List<DBIdCounter> xx = new List<DBIdCounter>();
            return xx;
            //return this.context.Counters.ToList<Counter>();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }

        #endregion

        #region IRepository<DBIdCounter> Members




        #endregion


        public List<DBIdCounter> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public DBIdCounter GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}