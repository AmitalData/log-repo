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
    public class GAQTEAMUSRRepository : IRepository<GAQTEAMUSR>
    {
        private AmitalContext currentContext;
        public GAQTEAMUSRRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public GAQTEAMUSRRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public GAQTEAMUSR GetSingle(string TEAMID)
        {
            return (from a in context.GAQTEAMUSRs
                    where a.TEAMID == TEAMID
                    select a).FirstOrDefault();
        }

        public IQueryable<GAQTEAMUSR> GetAll()
        {
            return from a in context.GAQTEAMUSRs
                   select a;
        }

        public void Add(GAQTEAMUSR entity)
        {
            context.GAQTEAMUSRs.Add(entity);
        }

        public void Remove(GAQTEAMUSR entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GAQTEAMUSRs.Attach(entity);
            }
            //context.AddToGAQTEAMUSRs 
            context.GAQTEAMUSRs.Remove(entity);
        }

        public void Update(GAQTEAMUSR entity)
        {
            context.GAQTEAMUSRs.Attach(entity); context.SetAsModified(entity);
        }

        public List<GAQTEAMUSR> All()
        {
            return context.GAQTEAMUSRs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<GAQTEAMUSR> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public GAQTEAMUSR GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GAQTEAMUSRKeys;
            return this.GetSingle(keys.TEAMID);
        }
    }
}
