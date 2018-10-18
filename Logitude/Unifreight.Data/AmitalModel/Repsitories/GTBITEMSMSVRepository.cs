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
    public class GTBITEMSMSVRepository : IRepository<GTBITEMSMSV>
    {
        private AmitalContext currentContext;
        public GTBITEMSMSVRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public GTBITEMSMSVRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public GTBITEMSMSV GetSingle(string PRATID)
        {
            return (from a in context.GTBITEMSMSVs
                    where a.PRATID == PRATID
                    select a).FirstOrDefault();
        }

        public IQueryable<GTBITEMSMSV> GetAll()
        {
            return from a in context.GTBITEMSMSVs
                   select a;
        }

        public void Add(GTBITEMSMSV entity)
        {
            context.GTBITEMSMSVs.Add(entity);
        }

        public void Remove(GTBITEMSMSV entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GTBITEMSMSVs.Attach(entity);
            }
            context.GTBITEMSMSVs.Remove(entity);
        }

        public void Update(GTBITEMSMSV entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GTBITEMSMSVs.Attach(entity); context.SetAsModified(entity);
            }
            
        }

        public List<GTBITEMSMSV> All()
        {
            return context.GTBITEMSMSVs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<GTBITEMSMSV> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public GTBITEMSMSV GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GTBITEMSMSVKeys;
            return this.GetSingle(keys.PRATID);
        }
    }
}

