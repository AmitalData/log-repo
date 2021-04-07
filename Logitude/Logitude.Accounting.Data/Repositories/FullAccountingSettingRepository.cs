 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Accounting.Data.Repositories
{
   public partial class FullAccountingSettingRepository:IRepository<FullAccountingSetting>
   {
        

		public List<FullAccountingSetting> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public FullAccountingSetting GetSingleFullAccountingSetting(int tenant)
        {
            FullAccountingSetting myFullAccountingSetting = (from a in context.FullAccountingSettings
                                     where a.Tenant == tenant
                                     select a).FirstOrDefault();
            return myFullAccountingSetting;
        }

        public List<int> GetNumberOfAgingMonthTenants()
        {
            var q = (
                from a in context.FullAccountingSettings
                where a.NumberOfAgingMonths.GetValueOrDefault() > 0
                select a.Tenant
                );
            return q.ToList();
        }
    }

}
   