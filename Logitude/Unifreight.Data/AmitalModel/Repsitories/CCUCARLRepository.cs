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
    public class CCUCARLRepository : IRepository<CCUCARL>
    {
        private AmitalContext currentContext;
        public CCUCARLRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CCUCARLRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CCUCARL GetSingle(int FILENO, int LINENO, int COUNTER, int? tenant)
        {
            return (from a in context.CCUCARLs
                    where a.FILENO == FILENO && a.LINENO == LINENO && a.COUNTER == COUNTER && a.TENANT == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CCUCARL> GetAll()
        {
            return from a in context.CCUCARLs
                   select a;
        }

        public void Add(CCUCARL entity)
        {
            context.CCUCARLs.Add(entity);
            SyncRecordCache.ClearCacheLastSync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public void Remove(CCUCARL entity)
        {
             {
                context.CCUCARLs.Attach(entity);
            }
             context.CCUCARLs.Remove(entity);
            SyncRecordCache.ClearCacheLastSync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public void Update(CCUCARL entity)
        {
             {
                context.CCUCARLs.Attach(entity); context.SetAsModified(entity);
            }
           
            SyncRecordCache.ClearCacheLastSync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public List<CCUCARL> All()
        {
            return context.CCUCARLs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CCUCARL> GetMulti(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUCUSTITEMKeys;
            return (from a in context.CCUCARLs  
                    where a.FILENO == keys.FILENO && a.LINENO == keys.LINENO && a.TENANT == keys.TENANT
                    select a).ToList();
        }

        public CCUCARL GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUCARLKeys;
            return this.GetSingle(keys.FILENO, keys.LINENO, keys.COUNTER, keys.TENANT);
        }

        public int FastDeleteMulti(CCUFILEMKeys parentEntityKeys)
        {
            var keys = parentEntityKeys as CCUFILEMKeys;
            return context.DeleteWhere<CCUCARL>(rec => rec.FILENO == keys.FILENO && rec.TENANT == keys.TENANT);
        }
    }
}

