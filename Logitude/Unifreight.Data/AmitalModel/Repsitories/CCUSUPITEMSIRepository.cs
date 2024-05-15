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
    public class CCUSUPITEMSIRepository : IRepository<CCUSUPITEMSI>
    {
        private AmitalContext currentContext;
        public CCUSUPITEMSIRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CCUSUPITEMSIRepository(AmitalContext context)
        {
            currentContext = context;
        }



        public CCUSUPITEMSI GetSingle(int FILENO, int LINENO, int ACCLINENO)
        {
            return (from a in context.CCUSUPITEMSIs
                    where a.FILENO == FILENO && a.LINENO == LINENO && a.ACCLINENO == ACCLINENO
                    select a).FirstOrDefault();
        }

        public IQueryable<CCUSUPITEMSI> GetAll()
        {
            return from a in context.CCUSUPITEMSIs
                   select a;
        }



        public void Add(CCUSUPITEMSI entity)
        {
            context.CCUSUPITEMSIs.Add(entity);
            SyncRecordCache.ClearCacheLasySync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public void Remove(CCUSUPITEMSI entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUSUPITEMSIs.Attach(entity);
            }
            //context.AddToCCUSUPITEMSIs 
            context.CCUSUPITEMSIs.Remove(entity);
            SyncRecordCache.ClearCacheLasySync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public void Update(CCUSUPITEMSI entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUSUPITEMSIs.Attach(entity); context.SetAsModified(entity);
            }

            SyncRecordCache.ClearCacheLasySync(entity.FILENO.ToString(), entity.TENANT.Value);
        }

        public List<CCUSUPITEMSI> All()
        {
            return context.CCUSUPITEMSIs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }



        public List<CCUSUPITEMSI> GetMulti(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUACCSUPKeys;
            return (from a in context.CCUSUPITEMSIs
                    where a.FILENO == keys.FILENO && a.ACCLINENO == keys.LINENO
                    select a).ToList();
        }

        public CCUSUPITEMSI GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUSUPITEMSIKeys;
            return this.GetSingle(keys.FILENO, keys.LINENO, keys.ACCLINENO);
        }

        public int FastDeleteMulti(EntityKeyFields parentEntityKeys)
        {
            var keys = parentEntityKeys as CCUFILEMKeys;
            return context.DeleteWhere<CCUSUPITEMSI>(rec => rec.FILENO == keys.FILENO);
        }
    }
}

