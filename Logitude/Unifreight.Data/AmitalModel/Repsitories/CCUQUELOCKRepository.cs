using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.EntityKeys;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.Data.AmitalModel.Repsitories
{
    public class CCUQUELOCKRepository : IRepository<CCUQUELOCK>
    {
        private AmitalContext currentContext;
        public CCUQUELOCKRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CCUQUELOCKRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CCUQUELOCK GetSingle(string ENTNAME, string FILENO, int? tenant)
        {
            return (from a in context.CCUQUELOCKs
                    where a.ENTNAME == ENTNAME && a.FILE_NO == FILENO && a.TENANT == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CCUQUELOCK> GetAll()
        {
            return from a in context.CCUQUELOCKs
                   select a;
        }

        public void Add(CCUQUELOCK entity)
        {
            context.CCUQUELOCKs.Add(entity);
            SyncRecordCache.ClearCacheLastSync(entity.FILE_NO, entity.TENANT.Value);
        }

        public void Remove(CCUQUELOCK entity)
        {

            AttachIfNot(entity);context.SetAsModified(entity); //context.CCUQUELOCKs.Attach(entity);
            //context.AddToCCUQUELOCKs 
            context.CCUQUELOCKs.Remove(entity);
            SyncRecordCache.ClearCacheLastSync(entity.FILE_NO, entity.TENANT.Value);
        }

        public void Update(CCUQUELOCK entity)
        {
            AttachIfNot(entity);context.SetAsModified(entity);

            context.SetAsModified(entity);
            SyncRecordCache.ClearCacheLastSync(entity.FILE_NO, entity.TENANT.Value);
        }
        void AttachIfNot(CCUQUELOCK entity)
        {
            // if ((entity as EntityObject).EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUQUELOCKs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<CCUQUELOCK> All()
        {
            return context.CCUQUELOCKs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CCUQUELOCK> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CCUQUELOCK GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUQUELOCKKeys;
            return this.GetSingle(keys.ENTNAME, keys.FILENO, keys.Tenant);
        }


        public CCUQUELOCK GetSingleGeneralLockNOWAIT(string ENTNAME, string FILENO)
        {
            return (context as DbContextBase)
                .GetListNOWAITWhere<CCUQUELOCK>(rec => rec.ENTNAME == ENTNAME &&
                rec.FILE_NO 
                == FILENO 
                ).FirstOrDefault(); ;
        }
    }


 
}
	 