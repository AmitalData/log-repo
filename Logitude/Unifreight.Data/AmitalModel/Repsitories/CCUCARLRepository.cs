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

        public CCUCARL GetSingle(int FILENO, int LINENO, int COUNTER)
        {
            return (from a in context.CCUCARLs
                    where a.FILENO == FILENO && a.LINENO == LINENO && a.COUNTER == COUNTER
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
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUCARLs.Attach(entity);
            }
            //context.AddToCCUCARLs 
            //context.CCUCARLs.DeleteAllOnSubmit //ther isn't
            //http://forums.devart.com/viewtopic.php?t=13223
            //context.ExecuteStoreCommand 
            context.CCUCARLs.Remove(entity);
            SyncRecordCache.ClearCacheLastSync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public void Update(CCUCARL entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
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
                    where a.FILENO == keys.FILENO && a.LINENO == keys.LINENO
                    select a).ToList();
        }

        public CCUCARL GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUCARLKeys;
            return this.GetSingle(keys.FILENO, keys.LINENO, keys.COUNTER);
        }

        public int FastDeleteMulti(CCUFILEMKeys parentEntityKeys)
        {
            var keys = parentEntityKeys as CCUFILEMKeys;
            return context.DeleteWhere<CCUCARL>(rec => rec.FILENO == keys.FILENO);
        }
    }
}

