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
    public class CCUCARSCRepository : IRepository<CCUCARSC>
    {
        private AmitalContext currentContext;
        public CCUCARSCRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CCUCARSCRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CCUCARSC GetSingle(int FILENO, int LINENO, int COUNTER , int? tenant)
        {
            return (from a in context.CCUCARSCs
                    where a.FILENO == FILENO && a.LINENO == LINENO && a.COUNTER == COUNTER && a.TENANT == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CCUCARSC> GetAll()
        {
            return from a in context.CCUCARSCs
                   select a;
        }

        public void Add(CCUCARSC entity)
        {
            context.CCUCARSCs.Add(entity);
            SyncRecordCache.ClearCacheLastSync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public void Remove(CCUCARSC entity)
        {
             {
                context.CCUCARSCs.Attach(entity);
            }
       
            context.CCUCARSCs.Remove(entity);
            SyncRecordCache.ClearCacheLastSync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public void Update(CCUCARSC entity)
        {
             {
                context.CCUCARSCs.Attach(entity); context.SetAsModified(entity);
            }
           
            SyncRecordCache.ClearCacheLastSync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public List<CCUCARSC> All()
        {
            return context.CCUCARSCs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CCUCARSC> GetMulti(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUCUSTITEMKeys;
            return (from a in context.CCUCARSCs
                    where a.FILENO == keys.FILENO && a.LINENO == keys.LINENO && a.TENANT == keys.TENANT
                    select a).ToList();
        }

        public CCUCARSC GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUCARSCKeys;
            return this.GetSingle(keys.FILENO, keys.LINENO, keys.COUNTER, keys.TENANT);
        }

        public int FastDeleteMulti(CCUFILEMKeys parentEntityKeys)
        {
            var keys = parentEntityKeys as CCUFILEMKeys;
            return context.DeleteWhere<CCUCARSC>(rec => rec.FILENO == keys.FILENO && rec.TENANT == keys.TENANT);
        }
    }
}

