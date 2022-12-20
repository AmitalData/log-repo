 
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
        public IQueryable<DigitalPortalScreen> GetDigitalPortalScreens(int tenant, string objectTableId, string screenCode)
        {
            return context.DigitalPortalScreens
                          .Where(a => a.Tenant == tenant
                                      && a.ObjectTableId.Equals(objectTableId)
                                      && a.ScreenCode.Equals(screenCode));
        }


        public List<DigitalPortalScreen> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

   }

}
   