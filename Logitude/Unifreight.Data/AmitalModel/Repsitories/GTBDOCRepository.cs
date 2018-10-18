using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.EntityKeys;
using Unifreight.Data.AmitalModel.EntityPOCOs;
//using EntityFramework.Extensions;

namespace Unifreight.Data.AmitalModel.Repsitories
{
    public class GTBDOCRepository : IRepository<GTBDOC>
    {
        private AmitalContext currentContext;
        public GTBDOCRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public GTBDOCRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public GTBDOC GetSingle(string DOCID)
        {
            return (from a in context.GTBDOCs
                    where a.DOCID == DOCID
                    select a).FirstOrDefault();
        }

        public IQueryable<GTBDOC> GetAll()
        {
            return from a in context.GTBDOCs
                   select a;
        }

        public void Add(GTBDOC entity)
        {
            context.GTBDOCs.Add(entity);
        }

        public void Remove(GTBDOC entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GTBDOCs.Attach(entity);
            }
            //context.AddToGTBDOCs 
            //context.GTBDOCs.DeleteAllOnSubmit //ther isn't
            //http://forums.devart.com/viewtopic.php?t=13223
            //context.ExecuteStoreCommand 
            context.GTBDOCs.Remove(entity);
        }

        public void Update(GTBDOC entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GTBDOCs.Attach(entity); context.SetAsModified(entity);
            }


        }

        public List<GTBDOC> All()
        {
            return context.GTBDOCs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<GTBDOC> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public GTBDOC GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GTBDOCKeys;
            return this.GetSingle(keys.DOCID);
        }
    }
}

