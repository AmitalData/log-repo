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

        public CCUCARSC GetSingle(int FILENO, int LINENO, int COUNTER)
        {
            return (from a in context.CCUCARSCs
                    where a.FILENO == FILENO && a.LINENO == LINENO && a.COUNTER == COUNTER
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
            SyncRecordCache.ClearCacheLasySync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public void Remove(CCUCARSC entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUCARSCs.Attach(entity);
            }
            //context.AddToCCUCARSCs 
            //context.CCUCARSCs.DeleteAllOnSubmit //ther isn't
            //http://forums.devart.com/viewtopic.php?t=13223
            //context.ExecuteStoreCommand 
            context.CCUCARSCs.Remove(entity);
            SyncRecordCache.ClearCacheLasySync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public void Update(CCUCARSC entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUCARSCs.Attach(entity); context.SetAsModified(entity);
            }
           
            SyncRecordCache.ClearCacheLasySync(entity.FILENO.ToString(), entity.TENANT.Value);
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
                    where a.FILENO == keys.FILENO && a.LINENO == keys.LINENO
                    select a).ToList();
        }

        public CCUCARSC GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUCARSCKeys;
            return this.GetSingle(keys.FILENO, keys.LINENO, keys.COUNTER);
        }

        public int FastDeleteMulti(CCUFILEMKeys parentEntityKeys)
        {
            var keys = parentEntityKeys as CCUFILEMKeys;
            return context.DeleteWhere<CCUCARSC>(rec => rec.FILENO == keys.FILENO);
        }
    }
}

