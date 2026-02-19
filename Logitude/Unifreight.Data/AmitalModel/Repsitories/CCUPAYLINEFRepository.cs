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
    public class CCUPAYLINEFRepository : IRepository<CCUPAYLINEF>
    {
        private AmitalContext currentContext;
        public CCUPAYLINEFRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CCUPAYLINEFRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CCUPAYLINEF GetSingle(int FILENO, int LINENO)
        {
            return (from a in context.CCUPAYLINEFs
                    where a.FILENO == FILENO && a.LINENO == LINENO
                    select a).FirstOrDefault();
        }

        public IQueryable<CCUPAYLINEF> GetAll()
        {
            return from a in context.CCUPAYLINEFs
                   select a;
        }

        public void Add(CCUPAYLINEF entity)
        {
            context.CCUPAYLINEFs.Add(entity);
            SyncRecordCache.ClearCacheLastSync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public void Remove(CCUPAYLINEF entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUPAYLINEFs.Attach(entity);
            }
            //context.AddToCCUPAYLINEFs 
            context.CCUPAYLINEFs.Remove(entity);
            SyncRecordCache.ClearCacheLastSync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public void Update(CCUPAYLINEF entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUPAYLINEFs.Attach(entity); context.SetAsModified(entity);
            }
            SyncRecordCache.ClearCacheLastSync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public List<CCUPAYLINEF> All()
        {
            return context.CCUPAYLINEFs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CCUPAYLINEF> GetMulti(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUPAYHANDKeys;

            return (from a in context.CCUPAYLINEFs
                    where a.FILENO == keys.FILENO
                    select a).ToList();
        }

        public CCUPAYLINEF GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUPAYLINEFKeys;
            return this.GetSingle(keys.FILENO, keys.LINENO);
        }

        public int FastDeleteMulti(EntityKeyFields parentEntityKeys) // moran 5.1.16 - AMI-55274
        {
            var keys = parentEntityKeys as CCUFILEMKeys;
            return context.DeleteWhere<CCUPAYLINEF>(rec => rec.FILENO == keys.FILENO);
        }
    }
}
	 