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
    public class GTBREQCERTRepository : IRepository<GTBREQCERT>
    {
        private AmitalContext currentContext;
        public GTBREQCERTRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public GTBREQCERTRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public GTBREQCERT GetSingle(string REQCERT, string ENTITY)
        {
            return (from a in context.GTBREQCERTs
                    where a.REQCERT == REQCERT && a.ENTITY == ENTITY
                    select a).FirstOrDefault();
        }

        public IQueryable<GTBREQCERT> GetAll()
        {
            return from a in context.GTBREQCERTs
                   select a;
        }

        public void Add(GTBREQCERT entity)
        {
            context.GTBREQCERTs.Add(entity);
        }

        public void Remove(GTBREQCERT entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GTBREQCERTs.Attach(entity);
            }
            //context.AddToGTBREQCERTs 
            //context.GTBREQCERTs.DeleteAllOnSubmit //ther isn't
            //http://forums.devart.com/viewtopic.php?t=13223
            //context.ExecuteStoreCommand 
            context.GTBREQCERTs.Remove(entity);
        }

        public void Update(GTBREQCERT entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GTBREQCERTs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<GTBREQCERT> All()
        {
            return context.GTBREQCERTs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public GTBREQCERT GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GTBREQCERTKeys;
            return this.GetSingle(keys.REQCERT, keys.ENTITY);
        }

        public List<GTBREQCERT> GetMulti(EntityKeyFields entityKeys)
        {
            return null;
        }
    }
}
	 

