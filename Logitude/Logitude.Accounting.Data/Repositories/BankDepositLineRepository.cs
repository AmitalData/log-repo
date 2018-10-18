 
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
   public partial class BankDepositLineRepository:IRepository<BankDepositLine>
   {
        
		public List<BankDepositLine> GetMulti(EntityKeyFields entityKeys)
        {
            BankDepositKeys bankDepositKeys = entityKeys as BankDepositKeys;

            return (from a in context.BankDepositLines
                    where a.DepositId == bankDepositKeys.Id
                    select a).ToList();
        }

   }

}
   