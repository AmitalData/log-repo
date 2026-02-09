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
    public class CCUTRANSPVALRepository : IRepository<CCUTRANSPVAL>
    {
        private AmitalContext currentContext;
        public CCUTRANSPVALRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CCUTRANSPVALRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CCUTRANSPVAL GetSingle(int FILENO, int LINENO , int? tenant)
        {
            return (from a in context.CCUTRANSPVALs
                    where a.FILENO == FILENO && a.LINENO == LINENO && a.TENANT == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CCUTRANSPVAL> GetAll()
        {
            return from a in context.CCUTRANSPVALs
                   select a;
        }

        public void Add(CCUTRANSPVAL entity)
        {
            context.CCUTRANSPVALs.Add(entity);
            SyncRecordCache.ClearCacheLastSync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public void Remove(CCUTRANSPVAL entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUTRANSPVALs.Attach(entity);
            }
            //context.AddToCCUTRANSPVALs 
            //context.CCUTRANSPVALs.DeleteAllOnSubmit //ther isn't
            //http://forums.devart.com/viewtopic.php?t=13223
            //context.ExecuteStoreCommand 
            context.CCUTRANSPVALs.Remove(entity);
            SyncRecordCache.ClearCacheLastSync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public void Update(CCUTRANSPVAL entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUTRANSPVALs.Attach(entity); context.SetAsModified(entity);
            }
            SyncRecordCache.ClearCacheLastSync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public List<CCUTRANSPVAL> All()
        {
            return context.CCUTRANSPVALs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CCUTRANSPVAL> GetMulti(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUFILEMKeys;
            return (from a in context.CCUTRANSPVALs
                    where a.FILENO == keys.FILENO && a.TENANT == keys.TENANT
                    select a).ToList();
        }

        public CCUTRANSPVAL GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUTRANSPVALKeys;
            return this.GetSingle(keys.FILENO, keys.LINENO, keys.Tenant);
        }

        public int FastDeleteMulti(EntityKeyFields parentEntityKeys)
        {
            var keys = parentEntityKeys as CCUFILEMKeys;
            return context.DeleteWhere<CCUTRANSPVAL>(rec => rec.FILENO == keys.FILENO && rec.TENANT == keys.TENANT);
        }
    }
}

