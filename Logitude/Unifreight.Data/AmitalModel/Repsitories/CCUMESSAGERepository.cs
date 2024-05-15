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
    public class CCUMESSAGERepository : IRepository<CCUMESSAGE>
    {
        private AmitalContext currentContext;
        public CCUMESSAGERepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CCUMESSAGERepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CCUMESSAGE GetSingle(int FILENO, int LINENO)
        {
            return (from a in context.CCUMESSAGEs
                    where a.FILENO == FILENO && a.LINENO == LINENO
                    select a).FirstOrDefault();
        }

        public IQueryable<CCUMESSAGE> GetAll()
        {
            return from a in context.CCUMESSAGEs
                   select a;
        }

        public void Add(CCUMESSAGE entity)
        {
            context.CCUMESSAGEs.Add(entity);
            SyncRecordCache.ClearCacheLasySync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public void Remove(CCUMESSAGE entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUMESSAGEs.Attach(entity);
            }
            //context.AddToCCUMESSAGEs 
            //context.CCUMESSAGEs.DeleteAllOnSubmit //ther isn't
            //http://forums.devart.com/viewtopic.php?t=13223
            //context.ExecuteStoreCommand 
            context.CCUMESSAGEs.Remove(entity);
            SyncRecordCache.ClearCacheLasySync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public void Update(CCUMESSAGE entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUMESSAGEs.Attach(entity); context.SetAsModified(entity);
            }
            SyncRecordCache.ClearCacheLasySync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public List<CCUMESSAGE> All()
        {
            return context.CCUMESSAGEs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CCUMESSAGE> GetMulti(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUFILEMKeys;
            return (from a in context.CCUMESSAGEs
                    where a.FILENO == keys.FILENO
                    select a).ToList();
        }

        public CCUMESSAGE GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUMESSAGEKeys;
            return this.GetSingle(keys.FILENO, keys.LINENO);
        }

        public int FastDeleteMulti(CCUFILEMKeys parentEntityKeys)
        {
            var keys = parentEntityKeys as CCUFILEMKeys;
            return context.DeleteWhere<CCUMESSAGE>(rec => rec.FILENO == keys.FILENO);
        }
    }
}

