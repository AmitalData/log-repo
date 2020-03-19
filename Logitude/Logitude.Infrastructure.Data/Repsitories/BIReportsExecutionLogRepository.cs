 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Infrastructure.Data.Repsitories
{
    public partial class BIReportsExecutionLogRepository : IRepository<BIReportsExecutionLog>
    {

        public List<BIReportsExecutionLog> GetMulti(EntityKeyFields entityKeys)
        {

            throw new NotImplementedException();
        }

        public BIReportsExecutionLog GetSingleBIReportExecutionLog(string id, int tenant)
        {
            return (from record in context.BIReportsExecutionLogs where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public BIReportsExecutionLog GetSingleByBIReportId(string id, int tenant)
        {
            return (from record in context.BIReportsExecutionLogs where record.BIReportId == id && record.Tenant == tenant orderby record.CreateDate descending select record).FirstOrDefault();
        }

    }

}
