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
    public class ITBPORTRepository : IRepository<ITBPORT>
    {
        private AmitalContext currentContext;
        public ITBPORTRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public ITBPORTRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public ITBPORT GetSingle(string PORTID)
        {
            return (from a in context.ITBPORTs
                    where a.PORTID == PORTID
                    select a).FirstOrDefault();
        }

        public IQueryable<ITBPORT> GetAll()
        {
            return from a in context.ITBPORTs
                   select a;
        }

        public void Add(ITBPORT entity)
        {
            context.ITBPORTs.Add(entity);
        }

        public void Remove(ITBPORT entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.ITBPORTs.Attach(entity);
            }
            context.ITBPORTs.Remove(entity);
        }

        public void Update(ITBPORT entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.ITBPORTs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<ITBPORT> All()
        {
            return context.ITBPORTs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ITBPORT> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ITBPORT GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as ITBPORTKeys;
            return this.GetSingle(keys.PORTID);
        }
    }
}
