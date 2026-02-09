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
    public class CCUSUPITEMRepository :IRepository<CCUSUPITEM>
    {
        private AmitalContext currentContext;
        public CCUSUPITEMRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CCUSUPITEMRepository(AmitalContext context)
        {
            currentContext = context;
        }



        public CCUSUPITEM GetSingle(int FILENO, int LINENO, int ACCLINENO , int? tenant)
        {
            return (from a in context.CCUSUPITEMs
                    where a.FILENO == FILENO && a.LINENO == LINENO && a.ACCLINENO == ACCLINENO && a.TENANT == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CCUSUPITEM> GetAll()
        {
            return from a in context.CCUSUPITEMs  
                   select a;
        }

        
		 
        public void Add(CCUSUPITEM entity)
        {
            context.CCUSUPITEMs.Add(entity);
        }

        public void Remove(CCUSUPITEM entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUSUPITEMs.Attach(entity);
            }
            //context.AddToCCUSUPITEMs 
            context.CCUSUPITEMs.Remove(entity);
        }

        public void Update(CCUSUPITEM entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUSUPITEMs.Attach(entity); context.SetAsModified(entity);
            }
            
        }

        public List<CCUSUPITEM> All()
        {
            return context.CCUSUPITEMs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }



        public List<CCUSUPITEM> GetMulti(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUACCSUPKeys;
            return (from a in context.CCUSUPITEMs
                    where a.FILENO == keys.FILENO && a.ACCLINENO == keys.LINENO && a.TENANT == keys.Tenant
                    select a).ToList();
        }

        public CCUSUPITEM GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUSUPITEMKeys;
            return this.GetSingle(keys.FILENO, keys.LINENO, keys.ACCLINENO , keys.TENANT);
        }

        public int FastDeleteMulti(EntityKeyFields parentEntityKeys)
        {
            var keys = parentEntityKeys as CCUFILEMKeys;
            return context.DeleteWhere<CCUSUPITEM>(rec => rec.FILENO == keys.FILENO && rec.TENANT== keys.TENANT);
        }
    }
   }
	 
