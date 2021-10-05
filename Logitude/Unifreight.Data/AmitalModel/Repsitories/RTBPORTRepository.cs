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
    public class RTBPORTRepository : IRepository<RTBPORT>
    {
        private AmitalContext currentContext;
        public RTBPORTRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public RTBPORTRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public RTBPORT GetSingle(string PORTID)
        {
            return (from a in context.RTBPORTs
                    where a.PORTID == PORTID
                    select a).FirstOrDefault();
        }

        public IQueryable<RTBPORT> GetAll()
        {
            return from a in context.RTBPORTs
                   select a;
        }

        public void Add(RTBPORT entity)
        {
            context.RTBPORTs.Add(entity);
        }

        public void Remove(RTBPORT entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.RTBPORTs.Attach(entity);
            }
            context.RTBPORTs.Remove(entity);
        }

        public void Update(RTBPORT entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.RTBPORTs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<RTBPORT> All()
        {
            return context.RTBPORTs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<RTBPORT> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public RTBPORT GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as RTBPORTKeys;
            return this.GetSingle(keys.PORTID);
        }
    }
}
