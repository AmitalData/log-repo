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
    public class GITITEMAPRepository : IRepository<GITITEMAP>
    {
        private AmitalContext currentContext;
        public GITITEMAPRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public GITITEMAPRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public GITITEMAP GetSingle(decimal COUNTER,string APPROVTYPEID)
        {
            return (from a in context.GITITEMAPs
                    where a.COUNTER == COUNTER && a.APPROVTYPEID == APPROVTYPEID
                    select a).FirstOrDefault();
        }

        public IQueryable<GITITEMAP> GetAll()
        {
            return from a in context.GITITEMAPs
                   select a;
        }

        public void Add(GITITEMAP entity)
        {
            context.GITITEMAPs.Add(entity);
        }

        public void Remove(GITITEMAP entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GITITEMAPs.Attach(entity);
            }
            context.GITITEMAPs.Remove(entity);
        }

        public void Update(GITITEMAP entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GITITEMAPs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<GITITEMAP> All()
        {
            return context.GITITEMAPs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<GITITEMAP> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public GITITEMAP GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GITITEMAPKeys;
            return this.GetSingle(keys.COUNTER,keys.APPROVTYPEID);
        }
    }
}
