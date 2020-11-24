using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System.Collections.Generic;
using System.Linq;

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

        public List<int> CheckErrorsInLines(string taxReportId, int tenant, List<string> errorCodes)
        {
           
            return (from a in context.TaxReportLines
                  
                    where a.TaxReportId == taxReportId && a.Tenant == tenant && errorCodes.Contains(a.StatusCode)
                    select   a.Line).ToList();

        }

        public IQueryable<TaxReportLine> GetAllExternalLines(int tenant, string taxReportId)
        {
            var q = (from a in context.TaxReportLines
                     where a.Tenant == tenant
                     && a.TaxReportId == taxReportId
                     && a.IsExternalLine == true
                     select a);
            return q;
        }

    }

}
   