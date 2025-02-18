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
    public class CCUCRREQRepository : IRepository<CCUCRREQ>
    {
        private AmitalContext currentContext;
        public CCUCRREQRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CCUCRREQRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CCUCRREQ GetSingle(string ENTNAME, int FILENO, int ACCLINENO, int ITEMLINE, int LINENO)
        {
            return (from a in context.CCUCRREQs
                    where a.ENTNAME == ENTNAME && a.FILENO == FILENO && a.ACCLINENO == ACCLINENO && a.ITEMLINE == ITEMLINE && a.LINENO == LINENO
                    select a).FirstOrDefault();
        }

        public IQueryable<CCUCRREQ> GetAll()
        {
            return from a in context.CCUCRREQs
                   select a;
        }

        public void Add(CCUCRREQ entity)
        {
            context.CCUCRREQs.Add(entity);
            SyncRecordCache.ClearCacheLastSync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public void Remove(CCUCRREQ entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUCRREQs.Attach(entity);
            }
            //context.AddToCCUCRREQs 
            context.CCUCRREQs.Remove(entity);
            SyncRecordCache.ClearCacheLastSync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public void Update(CCUCRREQ entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUCRREQs.Attach(entity); context.SetAsModified(entity);
            }
            SyncRecordCache.ClearCacheLastSync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public List<CCUCRREQ> All()
        {
            return context.CCUCRREQs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CCUCRREQ> GetMulti(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUSUPITEMKeys;
            return (from a in context.CCUCRREQs
                    where a.ENTNAME == "CCUFILEM" && a.FILENO == keys.FILENO && 
                            a.ACCLINENO == keys.ACCLINENO && a.ITEMLINE == keys.LINENO 
                    select a).ToList();
        }

        public CCUCRREQ GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUCRREQKeys;
            return this.GetSingle(keys.ENTNAME, keys.FILENO, keys.ACCLINENO, keys.ITEMLINE, keys.LINENO);
        }

       
        public int FastDeleteMulti(CCUFILEMKeys parentEntityKeys)
        {
            var keys = parentEntityKeys as CCUFILEMKeys;
            return context.DeleteWhere<CCUCRREQ>(rec => rec.FILENO == keys.FILENO);
        }

        public List<CCUCRREQ> GetFiles105Documents(int FILENO, int ACCLINENO, int ITEMLINE)
        {
            return (from a in context.CCUCRREQs
                    where a.ENTNAME == "CCUFILEM" && a.FILENO == FILENO &&
                          a.ACCLINENO == ACCLINENO && a.ITEMLINE == ITEMLINE 
                    select a).ToList();
        }
    }
}
	 