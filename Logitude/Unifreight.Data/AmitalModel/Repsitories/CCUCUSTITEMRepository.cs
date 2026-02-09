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
    public class CCUCUSTITEMRepository : IRepository<CCUCUSTITEM>
    {
        private AmitalContext currentContext;
        public CCUCUSTITEMRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CCUCUSTITEMRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CCUCUSTITEM GetSingle(int FILENO, int LINENO, int TENANT)
        {
            return (from a in context.CCUCUSTITEMs
                    where a.FILENO == FILENO && a.LINENO == LINENO && a.TENANT == TENANT    
                    select a).FirstOrDefault();
        }

        public IQueryable<CCUCUSTITEM> GetAll()
        {
            return from a in context.CCUCUSTITEMs
                   select a;
        }

        public void Add(CCUCUSTITEM entity)
        {
            context.CCUCUSTITEMs.Add(entity);
            SyncRecordCache.ClearCacheLastSync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public void Remove(CCUCUSTITEM entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUCUSTITEMs.Attach(entity);
            }
            //context.AddToCCUCUSTITEMs 
            context.CCUCUSTITEMs.Remove(entity);
            SyncRecordCache.ClearCacheLastSync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public void Update(CCUCUSTITEM entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUCUSTITEMs.Attach(entity); context.SetAsModified(entity);
            }
            SyncRecordCache.ClearCacheLastSync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public List<CCUCUSTITEM> All()
        {
            return context.CCUCUSTITEMs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public CCUCUSTITEM GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUCUSTITEMKeys;
            return this.GetSingle(keys.FILENO, keys.LINENO, keys.TENANT);

        }

        public List<CCUCUSTITEM> GetMulti(EntityKeyFields entityKeys)
        {
            var my105List = new List<CCUCUSTITEM>();
            var keys = entityKeys as CCUSUPITEMKeys;

            var join105and103 = (from b in context.CCUSUPITEMs.Where(rec => rec.FILENO == keys.FILENO && rec.ACCLINENO == keys.ACCLINENO && rec.LINENO == keys.LINENO)
                                 from a in context.CCUCUSTITEMs.Where(rec => rec.FILENO == keys.FILENO && rec.LINENO == b.ITEMLINENO.Value && rec.TENANT == b.TENANT)
                                 select new { my103 = b, my105 = a }).FirstOrDefault();
            if (join105and103 != null)
            {
                if (join105and103.my105 != null)
                {
                    my105List.Add(join105and103.my105);
                }
            }

            return my105List;

        }
        public int FastDeleteMulti(CCUFILEMKeys parentEntityKeys)
        {
            var keys = parentEntityKeys as CCUFILEMKeys;
            return context.DeleteWhere<CCUCUSTITEM>(rec => rec.FILENO == keys.FILENO && rec.TENANT == keys.TENANT);
        }

        public List<CCUCUSTITEM> GetFile105(int? FILENO)
        {
            var my105List = new List<CCUCUSTITEM>();

            return (from a in context.CCUCUSTITEMs
                    where a.FILENO == FILENO
                    select a).ToList();

        }
    }
}
	 