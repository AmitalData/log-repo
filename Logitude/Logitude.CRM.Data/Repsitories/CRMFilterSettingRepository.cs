 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.CRM.Data.Repsitories
{
   public partial class CRMFilterSettingRepository:IRepository<CRMFilterSetting>
   {        
		public List<CRMFilterSetting> GetMulti(EntityKeyFields entityKeys)
        {            
			throw new NotImplementedException();
        }

        public CRMFilterSetting GetFilterByDetails(int tenant, string myUserId, string myControlName, string myFilterName)
        {
            return (from a in context.CRMFilterSettings
                    where a.Tenant == tenant
                    && a.UserId == myUserId
                    && a.ControlNameSpace == myControlName
                    && a.FilterName == myFilterName
                    select a).FirstOrDefault();
        }
   }

}
   