 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Infrastructure.Data.Repsitories
{
   public partial class DigitalFieldSecurityRepository:IRepository<DigitalFieldSecurity>
   {
        
		public List<DigitalFieldSecurity> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public IQueryable<DigitalFieldSecurity> GetDigitalFieldSecurity(int tenant, string objectTableId, string profileCode)
        {
            return context.DigitalFieldSecurities
                          .Where(a => a.Tenant == tenant
                                      && a.ObjectTableId.Equals(objectTableId)
                                      && a.DigitalProfile.Code.Equals(profileCode));
        }
        
        public IQueryable<DigitalFieldSecurity> GetDigitalFieldSecurityTenant0()
        {
            return context.DigitalFieldSecurities.Where(a => a.Tenant == 0);
        }
   }
}
   