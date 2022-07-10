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
    public class MSPSPEDRepository : IRepository<MSPSPED>
    {
        private AmitalContext currentContext;
        public MSPSPEDRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public MSPSPEDRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public MSPSPED GetSingle(int SPDNO)
        {
            return (from a in context.MSPSPEDs
                    where a.SPDNO == SPDNO
                    select a).FirstOrDefault();
        }

        public IQueryable<MSPSPED> GetAll()
        {
            return from a in context.MSPSPEDs
                   select a;
        }

        public void Add(MSPSPED entity)
        {
            context.MSPSPEDs.Add(entity);
        }

        public void Remove(MSPSPED entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.MSPSPEDs.Attach(entity);
            }
            context.MSPSPEDs.Remove(entity);
        }

        public void Update(MSPSPED entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.MSPSPEDs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<MSPSPED> All()
        {
            return context.MSPSPEDs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<MSPSPED> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public MSPSPED GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as MSPSPEDKeys;
            return this.GetSingle(keys.SPDNO);
        }
    }
}
