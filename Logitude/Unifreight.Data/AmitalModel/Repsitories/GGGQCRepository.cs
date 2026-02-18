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
    public class GGGQCRepository : IRepository<GGGQC>
    {
        private AmitalContext currentContext;
        public GGGQCRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public GGGQCRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public GGGQC GetSingle(string QUEID)
        {
            return (from a in context.GGGQCs
                    where a.QUEID == QUEID
                    select a).FirstOrDefault();
        }

        public IQueryable<GGGQC> GetAll()
        {
            return from a in context.GGGQCs
                   select a;
        }

        public void Add(GGGQC entity)
        {
            context.GGGQCs.Add(entity);
        }

        public void Remove(GGGQC entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GGGQCs.Attach(entity);
            }
            //context.AddToGGGQCs 
            context.GGGQCs.Remove(entity);
        }

        public void Update(GGGQC entity)
        {
            context.GGGQCs.Attach(entity); context.SetAsModified(entity);
        }

        public List<GGGQC> All()
        {
            return context.GGGQCs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<GGGQC> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public GGGQC GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GGGQCKeys;
            return this.GetSingle(keys.QUEID);
        }
    }
}
