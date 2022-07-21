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
    public class LFIFILEMRepository : IRepository<LFIFILEM>
    {
        private AmitalContext currentContext;
        public LFIFILEMRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public LFIFILEMRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public LFIFILEM GetSingle(int DELIVERYNO)
        {
            return (from a in context.LFIFILEMs
                    where a.DELIVERYNO == DELIVERYNO
                    select a).FirstOrDefault();
        }

        public IQueryable<LFIFILEM> GetAll()
        {
            return from a in context.LFIFILEMs
                   select a;
        }

        public void Add(LFIFILEM entity)
        {
            context.LFIFILEMs.Add(entity);
        }

        public void Remove(LFIFILEM entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.LFIFILEMs.Attach(entity);
            }
            context.LFIFILEMs.Remove(entity);
        }

        public void Update(LFIFILEM entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.LFIFILEMs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<LFIFILEM> All()
        {
            return context.LFIFILEMs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<LFIFILEM> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public LFIFILEM GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as LFIFILEMKeys;
            return this.GetSingle(keys.DELIVERYNO);
        }
    }
}
