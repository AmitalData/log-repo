 
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
   public partial class ExternalReconciliationLineRepository:IRepository<ExternalReconciliationLine>
   {
        public List<ExternalReconciliationLine> GetMulti(EntityKeyFields entityKeys)
        {
            ExternalReconciliationKeys reconciliationKeys = entityKeys as ExternalReconciliationKeys;

            return (from a in context.ExternalReconciliationLines
                    where a.ReconciliationId == reconciliationKeys.Id
                    select a).ToList();
        }
    }

}
   