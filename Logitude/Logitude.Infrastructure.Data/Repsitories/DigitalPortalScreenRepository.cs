 
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
   public partial class DigitalPortalScreenRepository:IRepository<DigitalPortalScreen>
   {
        public IQueryable<DigitalPortalScreen> GetDigitalPortalScreens(int tenant, string objectTableId, string screenCode, string profileCode = "")
        {
            return context.DigitalPortalScreens
                          .Where(a => (a.Tenant == tenant || a.Tenant == 0)
                                      && a.ObjectTableId.Equals(objectTableId)
                                      && (string.IsNullOrEmpty(screenCode) || a.ScreenCode.Equals(screenCode))
                                      && (a.DigitalProfile.Code.Equals(profileCode)));
        }
        
        public IQueryable<DigitalPortalScreen> GetDigitalPortalScreenNames(int tenant, string profileCode)
        {
            return context.DigitalPortalScreens.Where(a => a.Tenant == tenant && a.DigitalProfile.Code.Equals(profileCode));
        }
        
        public IQueryable<DigitalPortalScreen> GetDigitalPortalScreenNamesTenant0()
        {
            return context.DigitalPortalScreens.Where(a => a.Tenant == 0);
        }


        public List<DigitalPortalScreen> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

   }

}
   