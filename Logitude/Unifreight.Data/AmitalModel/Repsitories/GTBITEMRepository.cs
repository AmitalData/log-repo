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
    public class GTBITEMRepository : IRepository<GTBITEM>
    {
        private AmitalContext currentContext;
        public GTBITEMRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public GTBITEMRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public GTBITEM GetSingle(string PARTNERID, string ITEMID)
        {
            return (from a in context.GTBITEMs
                    where a.PARTNERID == PARTNERID && a.ITEMID == ITEMID
                    select a).FirstOrDefault();
        }

        public IQueryable<GTBITEM> GetAll()
        {
            return from a in context.GTBITEMs
                   select a;
        }

        public void Add(GTBITEM entity)
        {
            context.GTBITEMs.Add(entity);
        }

        public void Remove(GTBITEM entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GTBITEMs.Attach(entity);
            }
            //context.AddToGTBITEMs 
            context.GTBITEMs.Remove(entity);
        }

        public void Update(GTBITEM entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GTBITEMs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<GTBITEM> All()
        {
            return context.GTBITEMs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public GTBITEM GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GTBITEMKeys;
            return this.GetSingle(keys.PARTNERID, keys.ITEMID);

        }

        public List<GTBITEM> GetMulti(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GTBITEMKeys;
            return (from a in context.GTBITEMs
                    where a.PARTNERID == keys.PARTNERID && a.ITEMID == keys.ITEMID
                    select a).ToList();

        }

        public List<GTBITEM> GetListBy(string vendorId, string customerId, string search, int top)
        {
            return (from a in context.GTBITEMs
                    where a.SEARCHENG.Contains(search)  
                    select a).Take(top).ToList ();

        }
    }
}
	 