 
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
   public partial class CashBookLineRepository:IRepository<CashBookLine>
   {
        
		public List<CashBookLine> GetMulti(EntityKeyFields entityKeys)
        {

            CashBookKeys bankDepositKeys = entityKeys as CashBookKeys;

            return (from a in context.CashBookLines
                    where a.CashBookId == bankDepositKeys.Id
                    select a).ToList();
        
        }

   }

}
   