using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.ReportScheduler
{
    class ReportSchedulerValidator : IReportSchedulerValidator
    {
        private IReportSchedulerValidator GLAccountValidator;
        public ReportSchedulerValidator(List<QueryFilterItem> reportFilterItems, int tenant, AdditionalValidate additionalValidate)
        {
            GLAccountValidator = new GLAccountValidator(reportFilterItems, tenant, additionalValidate);
        }
        public ValidateResult validate()
        {
            ValidateResult result = GLAccountValidator.validate();
            return result;
        }
    }
}
