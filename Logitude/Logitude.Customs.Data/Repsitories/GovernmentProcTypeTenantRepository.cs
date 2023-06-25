 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class GovernmentProcTypeTenantRepository:IRepository<GovernmentProcTypeTenant>
   {
        
		public List<GovernmentProcTypeTenant> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        public IQueryable<GovernmentProcTypeTenant> GetAll(int tenant, string code)
        {
            return from a in context.GovernmentProcTypeTenants
                   where a.Tenant == tenant && a.Code == code
                   select a;
        }
        public GovernmentProcTypeTenant GetSingleByCode(string code, int tenant)
        {
            if (!string.IsNullOrEmpty(code))
            {
                GovernmentProcTypeTenant entity;

                entity = (from a in context.GovernmentProcTypeTenants
                          where a.Code == code && a.Tenant == tenant
                          select a).FirstOrDefault();

                return entity;
            }
            return null;
        }

    }

}
   