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
    public class ETBSERLVRepository : IRepository<ETBSERLV>
    {
        private AmitalContext currentContext;
        public ETBSERLVRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public ETBSERLVRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public ETBSERLV GetSingle(string SERVLEVELID)
        {
            return (from a in context.ETBSERLVs
                    where a.SERVLEVELID == SERVLEVELID
                    select a).FirstOrDefault();
        }

        public IQueryable<ETBSERLV> GetAll()
        {
            return from a in context.ETBSERLVs
                   select a;
        }

        public void Add(ETBSERLV entity)
        {
            context.ETBSERLVs.Add(entity);
        }

        public void Remove(ETBSERLV entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.ETBSERLVs.Attach(entity);
            }
            context.ETBSERLVs.Remove(entity);
        }

        public void Update(ETBSERLV entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.ETBSERLVs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<ETBSERLV> All()
        {
            return context.ETBSERLVs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ETBSERLV> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ETBSERLV GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as ETBSERLVKeys;
            return this.GetSingle(keys.SERVLEVELID);
        }
    }
}
