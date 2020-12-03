 
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
   public partial class CalculatedChartsOfAccountRepository:IRepository<CalculatedChartsOfAccount>
   {
        
		public List<CalculatedChartsOfAccount> GetMulti(EntityKeyFields entityKeys)
        {

            UserDefinedReportKeys myEntityKeys = entityKeys as UserDefinedReportKeys;
            return (from a in context.CalculatedChartsOfAccounts where a.UserDefinedReportId == myEntityKeys.Id select a).ToList();
        }

   }

}
   