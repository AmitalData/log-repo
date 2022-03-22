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
    public class GGGQRepository : IRepository<GGGQ>
    {
        private AmitalContext currentContext;
        public GGGQRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public GGGQRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public GGGQ GetSingle(string QUEID)
        {
            return (from a in context.GGGQs
                    where a.QUEID == QUEID
                    select a).FirstOrDefault();
        }

        public IQueryable<GGGQ> GetAll()
        {
            return from a in context.GGGQs
                   select a;
        }

        public void Add(GGGQ entity)
        {
            context.GGGQs.Add(entity);
        }

        public void Remove(GGGQ entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GGGQs.Attach(entity);
            }
            //context.AddToGGGQs 
            context.GGGQs.Remove(entity);
        }

        public void Update(GGGQ entity)
        {
            context.GGGQs.Attach(entity); context.SetAsModified(entity);
        }

        public List<GGGQ> All()
        {
            return context.GGGQs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public GGGQ GetByPrimary(string primary, string entity, string originQue, string formId, string status)
        {
            var q = (from a in context.GGGQs
                     where a.PRIMARYNUM == primary & a.ENTNAME == entity & a.ORIGINQUE == originQue & a.FORMID == formId & a.STATUS == status
                     select a);
            return q.FirstOrDefault();
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<GGGQ> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public GGGQ GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GGGQKeys;
            return this.GetSingle(keys.QUEID);
        }
    }
}
	 