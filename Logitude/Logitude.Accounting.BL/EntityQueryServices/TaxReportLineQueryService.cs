using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class TaxReportLineQueryService : EntityQueryService<TaxReportLine, TaxReportLineKeys, TaxReportLinePM, object, TaxReportLineKeys>
    {
        public List<TaxReportLinePM> GetAllExternalLines(int tenant, string taxReportId)
        {
            var listPoco = this.repository.GetAllExternalLines(tenant, taxReportId).ToList();
            var pmList = listPoco.Select(poco => this.GetEntityPM(poco)).ToList();
            return pmList;
        }
        public List<TaxReportLinePM> GetAllLines(int tenant, string taxReportId)
        {
            var listPoco = this.repository.GetAllLines(tenant, taxReportId).ToList();
            var pmList = listPoco.Select(poco => this.GetEntityPM(poco)).ToList();
            return pmList;
        }

    }
}
