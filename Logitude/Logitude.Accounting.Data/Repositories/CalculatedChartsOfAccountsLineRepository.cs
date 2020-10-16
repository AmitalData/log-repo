 
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
   public partial class CalculatedChartsOfAccountsLineRepository:IRepository<CalculatedChartsOfAccountsLine>
   {
        
		public List<CalculatedChartsOfAccountsLine> GetMulti(EntityKeyFields entityKeys)
        {

            CalculatedChartsOfAccountKeys myEntityKeys = entityKeys as CalculatedChartsOfAccountKeys;
            return (from a in context.CalculatedChartsOfAccountsLines where a.CalculatedChartsOfAccountsId == myEntityKeys.Id select a).ToList();
        }

   }

}
   