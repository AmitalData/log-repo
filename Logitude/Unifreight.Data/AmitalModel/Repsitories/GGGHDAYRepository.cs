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
    public class GGGHDAYRepository
    {
        private AmitalContext currentContext;
        public GGGHDAYRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public GGGHDAYRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public GGGHDAY GetSingle(DateTime HOLIDAY)
        {
            return (from a in context.GGGHDAYS
                    where a.HOLIDAY == HOLIDAY
                    select a).FirstOrDefault();
        }

        public IQueryable<GGGHDAY> GetAll()
        {
            return from a in context.GGGHDAYS
                   select a;
        }

        public void Add(GGGHDAY entity)
        {
            context.GGGHDAYS.Add(entity);
        }

        public void Remove(GGGHDAY entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GGGHDAYS.Attach(entity);
            }
            context.GGGHDAYS.Remove(entity);
        }

        public void Update(GGGHDAY entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GGGHDAYS.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<GGGHDAY> All()
        {
            return context.GGGHDAYS.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<GGGHDAY> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

       
    }
}
