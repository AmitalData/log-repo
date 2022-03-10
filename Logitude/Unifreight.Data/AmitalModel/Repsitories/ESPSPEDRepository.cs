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
    public class ESPSPEDRepository : IRepository<ESPSPED>
    {
        private AmitalContext currentContext;
        public ESPSPEDRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public ESPSPEDRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public ESPSPED GetSingle(int SPD_NO)
        {
            return (from a in context.ESPSPEDs
                    where a.SPDNO == SPD_NO
                    select a).FirstOrDefault();
        }

        public IQueryable<ESPSPED> GetAll()
        {
            return from a in context.ESPSPEDs
                   select a;
        }

        public void Add(ESPSPED entity)
        {
            context.ESPSPEDs.Add(entity);
        }

        public void Remove(ESPSPED entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.ESPSPEDs.Attach(entity);
            }
            context.ESPSPEDs.Remove(entity);
        }

        public void Update(ESPSPED entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.ESPSPEDs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<ESPSPED> All()
        {
            return context.ESPSPEDs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ESPSPED> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ESPSPED GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as ESPSPEDKeys;
            return this.GetSingle(keys.SPD_NO);
        }
    }
}
