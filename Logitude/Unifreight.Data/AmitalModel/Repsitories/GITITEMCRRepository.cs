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
    public class GITITEMCRRepository : IRepository<GITITEMCR>
    {
        private AmitalContext currentContext;
        public GITITEMCRRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public GITITEMCRRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public GITITEMCR GetSingle(string COUNTER, string REQCERT)
        {
            return (from a in context.GITITEMCRs
                    where a.COUNTER.ToString() == COUNTER && a.REQCERT == REQCERT
                    select a).FirstOrDefault();
        }

        public IQueryable<GITITEMCR> GetAll()
        {
            return from a in context.GITITEMCRs
                   select a;
        }

        public void Add(GITITEMCR entity)
        {
            context.GITITEMCRs.Add(entity);
        }

        public void Remove(GITITEMCR entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GITITEMCRs.Attach(entity);
            }
            //context.AddToGITITEMCRs 
            context.GITITEMCRs.Remove(entity);
        }

        public void Update(GITITEMCR entity)
        {
            context.GITITEMCRs.Attach(entity); context.SetAsModified(entity);
        }

        public List<GITITEMCR> All()
        {
            return context.GITITEMCRs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<GITITEMCR> GetMulti(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GITITEMKeys;

            return (from a in context.GITITEMCRs
                    where a.COUNTER.ToString() == keys.COUNTER
                    select a).ToList();
        }

        public GITITEMCR GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GITITEMCRKeys;
            return this.GetSingle(keys.COUNTER,keys.REQCERT);
        }

        
    }

}
