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
    public class CCUMSHGRRepository : IRepository<CCUMSHGR>
    {
        private AmitalContext currentContext;
        public CCUMSHGRRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CCUMSHGRRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CCUMSHGR GetSingle(int FILENO, int LINENO)
        {
            return (from a in context.CCUMSHGRs
                    where a.FILENO == FILENO && a.LINENO == LINENO
                    select a).FirstOrDefault();
        }

        public IQueryable<CCUMSHGR> GetAll()
        {
            return from a in context.CCUMSHGRs
                   select a;
        }

        public void Add(CCUMSHGR entity)
        {
            context.CCUMSHGRs.Add(entity);
            SyncRecordCache.ClearCacheLastSyncByPrimaryNum(entity.FILENO.ToString(), entity.TENANT);
        }

        public void Remove(CCUMSHGR entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUMSHGRs.Attach(entity);
            }
            //context.AddToCCUMSHGRs 
            context.CCUMSHGRs.Remove(entity);
            SyncRecordCache.ClearCacheLastSyncByPrimaryNum(entity.FILENO.ToString(), entity.TENANT);
        }

        public void Update(CCUMSHGR entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUMSHGRs.Attach(entity); context.SetAsModified(entity);
            }
            SyncRecordCache.ClearCacheLastSyncByPrimaryNum(entity.FILENO.ToString(), entity.TENANT);
        }

        public List<CCUMSHGR> All()
        {
            return context.CCUMSHGRs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CCUMSHGR> GetMulti(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUFILEMKeys;

            return (from a in context.CCUMSHGRs
                    where a.FILENO == keys.FILENO
                    select a).ToList();
        }

        public CCUMSHGR GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUMSHGRKeys;
            return this.GetSingle(keys.FILENO, keys.LINENO);
        }

        public int FastDeleteMulti(EntityKeyFields parentEntityKeys)
        {
            var keys = parentEntityKeys as CCUFILEMKeys;
            return context.DeleteWhere<CCUMSHGR>(rec => rec.FILENO == keys.FILENO);
        }
    }
}
	 