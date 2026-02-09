using Devart.Data.Oracle;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Unifreight.Data.AmitalModel.EntityKeys;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.Data.AmitalModel.Repsitories
{
    public class CCUFILEMRepository : IRepository<CCUFILEM>
    {
        private AmitalContext currentContext;
        public CCUFILEMRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CCUFILEMRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CCUFILEM GetSingle(int FILENO,int? tenant)
        {
            return (from a in context.CCUFILEMs
                    where a.FILENO == FILENO && a.TENANT == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CCUFILEM> GetAll()
        {
            return from a in context.CCUFILEMs
                   select a;
        }

        public void Add(CCUFILEM entity)
        {
            context.CCUFILEMs.Add(entity);
            SyncRecordCache.ClearCacheLastSync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public void Remove(CCUFILEM entity)
        {

            AttachIfNot(entity);context.SetAsModified(entity); //context.CCUFILEMs.Attach(entity);
            //context.AddToCCUFILEMs 
            context.CCUFILEMs.Remove(entity);
            SyncRecordCache.ClearCacheLastSync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public void Update(CCUFILEM entity)
        {
            AttachIfNot(entity);context.SetAsModified(entity);
            SyncRecordCache.ClearCacheLastSync(entity.FILENO.ToString(), entity.TENANT.Value);
        }
        void AttachIfNot(CCUFILEM entity)
        {
            // if ((entity as EntityObject).EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUFILEMs.Attach(entity);
            }
        }

        public List<CCUFILEM> All()
        {
            return context.CCUFILEMs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CCUFILEM> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CCUFILEM GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUFILEMKeys;
            return this.GetSingle(keys.FILENO, keys.TENANT);
        }

        public int? GetFILENOByCUSTOMFILENO(long lCUSTOMFILENO, int? tenant)
        {
            var rec = (from a in context.CCUFILEMs
                       where a.CUSTOMFILENO == lCUSTOMFILENO && a.TENANT == tenant
                       select a).FirstOrDefault();
            
            if (rec == null)
            {
                return null;
            }
            return rec.FILENO;
        }

        public int LockByCUSTOMFILENO_forUpdateNOWAIT(long lCUSTOMFILENO, int tenant)
        {

            var succ =  context.FirstOrDefaultFUNOWAITWhere<CCUFILEM>( rec=> rec.CUSTOMFILENO == lCUSTOMFILENO && rec.TENANT==tenant);
          
            
            return succ;
        }

         public CCUFILEM GetCCUFILEMByRESHIMONNO(string reshimonNumber, int tenant)
        {
            var rec = (from a in context.CCUFILEMs
                       where a.RESHIMONNO == reshimonNumber && a.TENANT == tenant
                       select a).FirstOrDefault();
            if (rec == null)
            {
                return null;
            }
            return rec;
        }
 
        public int FastDeleteMulti(EntityKeyFields parentEntityKeys)  
        {
            var keys = parentEntityKeys as CCUFILEMKeys;
            return context.DeleteWhere<CCUFILEM>(rec => rec.FILENO == keys.FILENO && rec.TENANT== keys.TENANT);
        }

        public void GetWeeklyStatistic(int tenant,
            out int LastMonthOpenByUserCCU,
            out int TotalOpenCCULastWeek
            )

        {
            TotalOpenCCULastWeek = LastMonthOpenByUserCCU = -1;
            var lastWeek = 
                DateTime.Now.Add(TimeSpan.FromDays(-7));

            var qLastMonth = (from a in context.CCUFILEMs
                              where a.TENANT==tenant && a.OPENDATE > lastWeek
                              select a);
            var qTotalOpenCCULastMonth =
                (from a in qLastMonth
                 where (a.FROMIIG == null || a.FROMIIG.Trim() == string.Empty)
                 where (a.RESHIMONNON == null || a.RESHIMONNON.Trim() == string.Empty)
                 select a);
            var qLastMonthOpenByUserCCU =
                (from a in qLastMonth
                 group a by a.OPENBYUSER into g
                 select g.Key
                   );

            var qRes = (from a in qLastMonth
                        select new
                        {
                            Dummy = qLastMonth.FirstOrDefault(),
                            TotalOpenCCULastMonth = qTotalOpenCCULastMonth.Count(),
                            LastMonthOpenByUserCCU = qLastMonthOpenByUserCCU.Count()
                        }
                        );

            var res = qRes.FirstOrDefault();
            if (res!=null)
            {
                TotalOpenCCULastWeek = res.TotalOpenCCULastMonth;
                LastMonthOpenByUserCCU = res.LastMonthOpenByUserCCU;
            }

            return;
 
        }
    }
}
	 