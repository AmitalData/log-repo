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
    public class ETBPORTRepository : IRepository<ETBPORT>
    {
        private AmitalContext currentContext;
        public ETBPORTRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public ETBPORTRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public ETBPORT GetSingle(string PORTID)
        {
            return (from a in context.ETBPORTs
                    where a.PORTID == PORTID
                    select a).FirstOrDefault();
        }

        public IQueryable<ETBPORT> GetAll()
        {
            return from a in context.ETBPORTs
                   select a;
        }

        public void Add(ETBPORT entity)
        {
            context.ETBPORTs.Add(entity);
        }

        public void Remove(ETBPORT entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.ETBPORTs.Attach(entity);
            }
            context.ETBPORTs.Remove(entity);
        }

        public void Update(ETBPORT entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.ETBPORTs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<ETBPORT> All()
        {
            return context.ETBPORTs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ETBPORT> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ETBPORT GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as ETBPORTKeys;
            return this.GetSingle(keys.PORTID);
        }
    }
}
