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
    public class GTBSERLVRepository : IRepository<GTBSERLV>
    {
        private AmitalContext currentContext;
        public GTBSERLVRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public GTBSERLVRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public GTBSERLV GetSingle(string SERVLEVELID)
        {
            return (from a in context.GTBSERLVs
                    where a.SERVLEVELID == SERVLEVELID
                    select a).FirstOrDefault();
        }

        public IQueryable<GTBSERLV> GetAll()
        {
            return from a in context.GTBSERLVs
                   select a;
        }

        public void Add(GTBSERLV entity)
        {
            context.GTBSERLVs.Add(entity);
        }

        public void Remove(GTBSERLV entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GTBSERLVs.Attach(entity);
            }
            context.GTBSERLVs.Remove(entity);
        }

        public void Update(GTBSERLV entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GTBSERLVs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<GTBSERLV> All()
        {
            return context.GTBSERLVs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<GTBSERLV> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public GTBSERLV GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GTBSERLVKeys;
            return this.GetSingle(keys.SERVLEVELID);
        }
    }
}
