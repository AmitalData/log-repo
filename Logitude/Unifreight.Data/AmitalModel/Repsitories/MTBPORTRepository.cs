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
    public class MTBPORTRepository : IRepository<MTBPORT>
    {
        private AmitalContext currentContext;
        public MTBPORTRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public MTBPORTRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public MTBPORT GetSingle(string PORTID)
        {
            return (from a in context.MTBPORTs
                    where a.PORTID == PORTID
                    select a).FirstOrDefault();
        }

        public IQueryable<MTBPORT> GetAll()
        {
            return from a in context.MTBPORTs
                   select a;
        }

        public void Add(MTBPORT entity)
        {
            context.MTBPORTs.Add(entity);
        }

        public void Remove(MTBPORT entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.MTBPORTs.Attach(entity);
            }
            context.MTBPORTs.Remove(entity);
        }

        public void Update(MTBPORT entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.MTBPORTs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<MTBPORT> All()
        {
            return context.MTBPORTs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<MTBPORT> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public MTBPORT GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as MTBPORTKeys;
            return this.GetSingle(keys.PORTID);
        }
    }
}
