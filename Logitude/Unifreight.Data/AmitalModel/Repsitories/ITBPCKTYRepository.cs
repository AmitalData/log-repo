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
    public class ITBPCKTYRepository : IRepository<ITBPCKTY>
    {
        private AmitalContext currentContext;
        public ITBPCKTYRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public ITBPCKTYRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public ITBPCKTY GetSingle(string PACKTYPEID)
        {
            return (from a in context.ITBPCKTIES
                    where a.PACKTYPEID == PACKTYPEID
                    select a).FirstOrDefault();
        }

        public IQueryable<ITBPCKTY> GetAll()
        {
            return from a in context.ITBPCKTIES
                   select a;
        }

        public void Add(ITBPCKTY entity)
        {
            context.ITBPCKTIES.Add(entity);
        }

        public void Remove(ITBPCKTY entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.ITBPCKTIES.Attach(entity);
            }
            context.ITBPCKTIES.Remove(entity);
        }

        public void Update(ITBPCKTY entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.ITBPCKTIES.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<ITBPCKTY> All()
        {
            return context.ITBPCKTIES.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ITBPCKTY> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ITBPCKTY GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as ITBPCKTYKeys;
            return this.GetSingle(keys.PACKTYPEID);
        }
    }
}
