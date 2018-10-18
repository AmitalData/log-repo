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
    public class GITITEMRepository : IRepository<GITITEM>
    {
        private AmitalContext currentContext;
        public GITITEMRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public GITITEMRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public GITITEM GetSingle(string COUNTER)
        {
            return (from a in context.GITITEMs
                    where a.COUNTER.ToString() == COUNTER
                    select a).FirstOrDefault();
        }

        public IQueryable<GITITEM> GetAll()
        {
            return from a in context.GITITEMs
                   select a;
        }

        public void Add(GITITEM entity)
        {
            context.GITITEMs.Add(entity);
        }

        public void Remove(GITITEM entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GITITEMs.Attach(entity);
            }
            context.GITITEMs.Remove(entity);
        }

        public void Update(GITITEM entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GITITEMs.Attach(entity); context.SetAsModified(entity);
            }

        }

        public List<GITITEM> All()
        {
            return context.GITITEMs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<GITITEM> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public GITITEM GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GITITEMKeys;
            return this.GetSingle(keys.COUNTER);
        }
    }
}
