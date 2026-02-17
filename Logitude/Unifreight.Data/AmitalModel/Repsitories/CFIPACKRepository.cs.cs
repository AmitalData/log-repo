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
    public class CFIPACKRepository : IRepository<CFIPACK>
    {
        private AmitalContext currentContext;
        public CFIPACKRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CFIPACKRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CFIPACK GetSingle(long FILENO, int LINENO)
        {
            return (from a in context.CFIPACKs
                    where a.FILENO == FILENO && a.LINENO == LINENO
                    select a).FirstOrDefault();
        }

        public IQueryable<CFIPACK> GetAll()
        {
            return from a in context.CFIPACKs
                   select a;
        }

        public void Add(CFIPACK entity)
        {
            context.CFIPACKs.Add(entity);
        }

        public void Remove(CFIPACK entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CFIPACKs.Attach(entity);
            }
            context.CFIPACKs.Remove(entity);
        }

        public void Update(CFIPACK entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CFIPACKs.Attach(entity); context.SetAsModified(entity);
            }

        }

        public List<CFIPACK> All()
        {
            return context.CFIPACKs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CFIPACK> GetMulti(int FILENO)
        {
            return (from a in context.CFIPACKs
                    where a.FILENO == FILENO
                    select a).ToList();
        }

        public List<CFIPACK> GetMulti(EntityKeyFields entityKeys)
        {
            return null;
        }

        public CFIPACK GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CFIPACKKeys;
            return this.GetSingle(keys.FILENO, keys.LINENO);
        }

    }
}
