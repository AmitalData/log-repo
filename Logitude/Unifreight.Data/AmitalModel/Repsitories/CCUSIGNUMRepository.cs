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
    public class CCUSIGNUMRepository : IRepository<CCUSIGNUM>
    {
        private AmitalContext currentContext;
        public CCUSIGNUMRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CCUSIGNUMRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CCUSIGNUM GetSingle(int FILENO, int LINENOMSHGR, int LINENOSIGN)
        {
            return (from a in context.CCUSIGNUMs
                    where a.FILENO == FILENO && a.LINENOMSHGR == LINENOMSHGR && a.LINENOSIGN == LINENOSIGN
                    select a).FirstOrDefault();
        }

        public IQueryable<CCUSIGNUM> GetAll()
        {
            return from a in context.CCUSIGNUMs
                   select a;
        }

        public void Add(CCUSIGNUM entity)
        {
            context.CCUSIGNUMs.Add(entity);
            SyncRecordCache.ClearCacheLastSync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public void Remove(CCUSIGNUM entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUSIGNUMs.Attach(entity);
            }
            //context.AddToCCUSIGNUMs 
            context.CCUSIGNUMs.Remove(entity);
            SyncRecordCache.ClearCacheLastSync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public void Update(CCUSIGNUM entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUSIGNUMs.Attach(entity); context.SetAsModified(entity);
            }
            SyncRecordCache.ClearCacheLastSync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public List<CCUSIGNUM> All()
        {
            return context.CCUSIGNUMs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CCUSIGNUM> GetMulti(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUMSHGRKeys;

            return (from a in context.CCUSIGNUMs
                    where a.FILENO == keys.FILENO && a.LINENOMSHGR == keys.LINENO
                    select a).ToList();
        }

        public CCUSIGNUM GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUSIGNUMKeys;
            return this.GetSingle(keys.FILENO, keys.LINENOMSHGR, keys.LINENOSIGN);
        }

        public int FastDeleteMulti(EntityKeyFields parentEntityKeys)
        {
            //var keys = parentEntityKeys as CCUMSHGRKeys;
            //return context.DeleteWhere<CCUSIGNUM>(rec => rec.FILENO == keys.FILENO && rec.LINENOMSHGR == keys.LINENO);
            var keys = parentEntityKeys as CCUFILEMKeys;
            return context.DeleteWhere<CCUSIGNUM>(rec => rec.FILENO == keys.FILENO);
        }
    }
}
	 