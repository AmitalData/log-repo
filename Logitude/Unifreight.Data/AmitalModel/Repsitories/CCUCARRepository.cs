
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
    public class CCUCARRepository : IRepository<CCUCAR>
    {
        private AmitalContext currentContext;
        public CCUCARRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CCUCARRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CCUCAR GetSingle(int FILENO, int LINENO)
        {
            return (from a in context.CCUCARs
                    where a.FILENO == FILENO && a.LINENO == LINENO 
                    select a).FirstOrDefault();
        }

        public IQueryable<CCUCAR> GetAll()
        {
            return from a in context.CCUCARs
                   select a;
        }

        public void Add(CCUCAR entity)
        {
            context.CCUCARs.Add(entity);
            SyncRecordCache.ClearCacheLasySync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public void Remove(CCUCAR entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUCARs.Attach(entity);
            }
            //context.AddToCCUCARs 
            //context.CCUCARs.DeleteAllOnSubmit //ther isn't
            //http://forums.devart.com/viewtopic.php?t=13223
            //context.ExecuteStoreCommand 
            context.CCUCARs.Remove(entity);
            SyncRecordCache.ClearCacheLasySync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public void Update(CCUCAR entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUCARs.Attach(entity); context.SetAsModified(entity);
            }

            SyncRecordCache.ClearCacheLasySync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public List<CCUCAR> All()
        {
            return context.CCUCARs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CCUCAR> GetMulti(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUCUSTITEMKeys;
            return (from a in context.CCUCARs
                    where a.FILENO == keys.FILENO && a.LINENO == keys.LINENO
                    select a).ToList();
        }

        public CCUCAR GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUCARKeys;
            return this.GetSingle(keys.FILENO, keys.LINENO);
        }

        public int FastDeleteMulti(CCUFILEMKeys parentEntityKeys)
        {
            var keys = parentEntityKeys as CCUFILEMKeys;
            return context.DeleteWhere<CCUCAR>(rec => rec.FILENO == keys.FILENO);
        }
    }
}

