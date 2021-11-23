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
    public class ETBPAYTRRepository : IRepository<ETBPAYTR>
    {
        private AmitalContext currentContext;
        public ETBPAYTRRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public ETBPAYTRRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public ETBPAYTR GetSingle(string PTERMID)
        {
            return (from a in context.ETBPAYTRs
                    where a.PTERMID == PTERMID
                    select a).FirstOrDefault();
        }

        public IQueryable<ETBPAYTR> GetAll()
        {
            return from a in context.ETBPAYTRs
                   select a;
        }

        public void Add(ETBPAYTR entity)
        {
            context.ETBPAYTRs.Add(entity);
        }

        public void Remove(ETBPAYTR entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.ETBPAYTRs.Attach(entity);
            }
            context.ETBPAYTRs.Remove(entity);
        }

        public void Update(ETBPAYTR entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.ETBPAYTRs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<ETBPAYTR> All()
        {
            return context.ETBPAYTRs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ETBPAYTR> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ETBPAYTR GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as ETBPAYTRKeys;
            return this.GetSingle(keys.PTERMID);
        }
    }
}
