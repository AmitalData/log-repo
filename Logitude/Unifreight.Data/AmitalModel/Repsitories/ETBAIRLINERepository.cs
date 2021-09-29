using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.EntityKeys;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.Data.AmitalModel.Repsitories
{
    public class ETBAIRLINERepository : IRepository<ETBAIRLINE>
    {
        private AmitalContext currentContext;
        public ETBAIRLINERepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public ETBAIRLINERepository(AmitalContext context)
        {
            currentContext = context;
        }

        public ETBAIRLINE GetSingle(string AIRLINEID)
        {
            return (from a in context.ETBAIRLINEs
                    where a.AIRLINEID == AIRLINEID
                    select a).FirstOrDefault();
        }

        public IQueryable<ETBAIRLINE> GetAll()
        {
            return from a in context.ETBAIRLINEs
                   select a;
        }

        public void Add(ETBAIRLINE entity)
        {
            context.ETBAIRLINEs.Add(entity);
        }

        public void Remove(ETBAIRLINE entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.ETBAIRLINEs.Attach(entity);
            }
            context.ETBAIRLINEs.Remove(entity);
        }

        public void Update(ETBAIRLINE entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.ETBAIRLINEs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<ETBAIRLINE> All()
        {
            return context.ETBAIRLINEs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ETBAIRLINE> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ETBAIRLINE GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as ETBAIRLINEKeys;
            return this.GetSingle(keys.AIRLINEID);
        }
    }
}
