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
    public class GTBFUSTATURepository : IRepository<GTBFUSTATU>
    {
        private AmitalContext currentContext;
        public GTBFUSTATURepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public GTBFUSTATURepository(AmitalContext context)
        {
            currentContext = context;
        }

        public GTBFUSTATU GetSingle(string ENTNAME)
        {
            return (from a in context.GTBFUSTATUs
                    where a.ENTNAME == ENTNAME
                    select a).FirstOrDefault();
        }

        public IQueryable<GTBFUSTATU> GetAll()
        {
            return from a in context.GTBFUSTATUs
                   select a;
        }

        public void Add(GTBFUSTATU entity)
        {
            context.GTBFUSTATUs.Add(entity);
        }

        public void Remove(GTBFUSTATU entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GTBFUSTATUs.Attach(entity);
            }
            context.GTBFUSTATUs.Remove(entity);
        }

        public void Update(GTBFUSTATU entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GTBFUSTATUs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<GTBFUSTATU> All()
        {
            return context.GTBFUSTATUs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<GTBFUSTATU> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public GTBFUSTATU GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GTBFUSTATUKeys;
            return this.GetSingle(keys.ENTNAME);
        }
    }
}
