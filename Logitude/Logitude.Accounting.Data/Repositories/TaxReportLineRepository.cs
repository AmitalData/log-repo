 
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
   public partial class TaxReportLineRepository:IRepository<TaxReportLine>
   {

        public List<TaxReportLine> GetMulti(EntityKeyFields entityKeys)
        {
            TaxReportKeys reconciliationKeys = entityKeys as TaxReportKeys;

            return (from a in context.TaxReportLines
                    where a.TaxReportId == reconciliationKeys.Id
                    select a).ToList();
        }
        public IQueryable<TaxReportLine> GetByReportId(string reportId, int tenant)
        {
            return (from a in context.TaxReportLines
                    where a.TaxReportId == reportId && a.Tenant == tenant
                    select a);
        }

    }

}
   