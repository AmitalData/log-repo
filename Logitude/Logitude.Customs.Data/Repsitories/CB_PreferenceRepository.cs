 
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
using Logitude.Customs.Data.EntityMapping;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class CB_PreferenceRepository:IRepository<CB_Preference>
   {
        
		public List<CB_Preference> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public CB_Preference GetCB_PreferenceByUserIdAndTenant(string userId, int tenant = 0)
        {
            return (from a in context.CB_Preferences
                    where a.UserId == userId && (a.Tenant == tenant || tenant == 0)
                    select a).FirstOrDefault();
        }
    }

}
   