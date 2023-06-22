 
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
   public partial class CustomDocumentTypeTenantRepository:IRepository<CustomDocumentTypeTenant>
   {
        
		public List<CustomDocumentTypeTenant> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public IQueryable<CustomDocumentTypeTenant> GetAll(int tenant, string code)
        {
            return from a in context.CustomDocumentTypeTenants
                   where a.Tenant == tenant && a.Code == code
                   select a;
        }
        public CustomDocumentTypeTenant GetSingleByCode(string code, int tenant)
        {
            if (!string.IsNullOrEmpty(code))
            {
                CustomDocumentTypeTenant entity;

                entity = (from a in context.CustomDocumentTypeTenants
                          where a.Code == code && a.Tenant == tenant
                          select a).FirstOrDefault();
                
                return entity;
            }
            return null;
        }


    }

}
   