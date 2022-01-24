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
    public class MTBCARRRepository : IRepository<MTBCARR>
    {
        private AmitalContext currentContext;
        public MTBCARRRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public MTBCARRRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public MTBCARR GetSingle(string AIRLINEID)
        {
            return (from a in context.MTBCARRs
                    where a.AIRLINEID == AIRLINEID
                    select a).FirstOrDefault();
        }

        public IQueryable<MTBCARR> GetAll()
        {
            return from a in context.MTBCARRs
                   select a;
        }

        public void Add(MTBCARR entity)
        {
            context.MTBCARRs.Add(entity);
        }

        public void Remove(MTBCARR entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.MTBCARRs.Attach(entity);
            }
            context.MTBCARRs.Remove(entity);
        }

        public void Update(MTBCARR entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.MTBCARRs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<MTBCARR> All()
        {
            return context.MTBCARRs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<MTBCARR> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public MTBCARR GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as MTBCARRKeys;
            return this.GetSingle(keys.AIRLINEID);
        }
    }
}
