using Logitude.Base.Models.UserTenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.ReportTests.Models.Builders
{
    public class ReportFliterBuilder
    {
        private ReportFliter _reportFliter;
        public ReportFliterBuilder()
        {
            this.Reset();
        }

        private void Reset()
        {
            _reportFliter = new ReportFliter();
        }

        public ReportFliter Build()
        {
            ReportFliter result = _reportFliter;
            this.Reset();
            return result;
        }

        public ReportFliterBuilder WithModel(ReportFliter reportFliter)
        {
            _reportFliter = reportFliter;
            return this;
        }

        public ReportFliterBuilder WithDefualtValues()
        {
            _reportFliter = new ReportFliter
            {
                Tenant = UserTenant.Tenant,
                UserId = UserTenant.UserId,
                DefaultTemplateVsersion = 1,
                ReportsRunUsingWR = true,
                NumberOfPage = 1,
                ProcessType = "GenerateReport",
                QueryFilterItemLists = new List<ReportFliterItem>()
            };
            return this;
        }

        public ReportFliterBuilder ReportCode(string reportCode)
        {
            _reportFliter.ReportCode = reportCode;
            return this;
        }

        public ReportFliterBuilder ReportName(string reportName)
        {
            _reportFliter.ReportName = reportName;
            return this;
        }

        public ReportFliterBuilder ReportId(string reportId)
        {
            _reportFliter.ReportId = reportId;
            return this;
        }

        public ReportFliterBuilder DefaultTemplateId(string defaultTemplateId)
        {
            _reportFliter.DefaultTemplateId = defaultTemplateId;
            return this;
        }
    }
}
