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
    public class GTBITMCNRepository : IRepository<GTBITMCN>
    {
        private AmitalContext currentContext;
        public GTBITMCNRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public GTBITMCNRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public GTBITMCN GetSingle(string PARTNERID, string ITEMID, string PARTNER2ID, string ITEM2ID)
        {
            return (from a in context.GTBITMCNs
                    where a.PARTNERID == PARTNERID && a.ITEMID == ITEMID && a.PARTNER2ID == PARTNER2ID && a.ITEM2ID == ITEM2ID
                    select a).FirstOrDefault();
        }

        public IQueryable<GTBITMCN> GetAll()
        {
            return from a in context.GTBITMCNs
                   select a;
        }

        public void Add(GTBITMCN entity)
        {
            context.GTBITMCNs.Add(entity);
        }

        public void Remove(GTBITMCN entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GTBITMCNs.Attach(entity);
            }
            //context.AddToGTBITMCNs 
            context.GTBITMCNs.Remove(entity);
        }

        public void Update(GTBITMCN entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GTBITMCNs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<GTBITMCN> All()
        {
            return context.GTBITMCNs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public GTBITMCN GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GTBITMCNKeys;
            return this.GetSingle(keys.PARTNERID, keys.ITEMID, keys.PARTNER2ID, keys.ITEM2ID);

        }

        public List<GTBITMCN> GetMulti(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GTBITMCNKeys;
            return (from a in context.GTBITMCNs
                    where a.PARTNERID == keys.PARTNERID && a.ITEMID == keys.ITEMID && a.PARTNER2ID == keys.PARTNER2ID && a.ITEM2ID == keys.ITEM2ID
                    select a).ToList();

        }

    }
}
