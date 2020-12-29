using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.ReportScheduler
{
    class GLAccountValidator : IReportSchedulerValidator
    {
        private List<QueryFilterItem> reportFilterItems;
        private int tenant;
        private AdditionalValidate additionalValidate;
        public GLAccountValidator(List<QueryFilterItem> reportFilterItems, int tenant, AdditionalValidate additionalValidate)
        {
            this.reportFilterItems = reportFilterItems;
            this.tenant = tenant;
            this.additionalValidate = additionalValidate;
        }
        public ValidateResult validate()
        {
            ValidateResult result = new ValidateResult();
            string gLAccountId = GetFilterFieldValueByName(reportFilterItems, "GLAccountId");
            if (!string.IsNullOrEmpty(gLAccountId))
            {
                bool inActiveGlAccount = ValidateGLAccount(gLAccountId, tenant);
                result.IsValid = (!inActiveGlAccount && additionalValidate != null && additionalValidate.recepients != null);
                if (!result.IsValid)
                    result.ErrorMessage = "The E-mail was not sent, one or more selected filters partners are inactive";
            }
            else {
                result.IsValid = true;
                result.ErrorMessage = "";
            }

            return result;
        }

        private bool ValidateGLAccount(string gLAccountId, int tenant)
        {
            GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(tenant);
            GLAccount gLAccount = gLAccountQueryService.GetSingleByAccountId(gLAccountId, tenant);
            if (gLAccount != null && gLAccount.Inactive != null)
                return (bool)gLAccount.Inactive;
            return false;
        }

        private string GetFilterFieldValueByName(List<QueryFilterItem> reportFilterItems, string fieldName)
        {
            string fieldValue = null;
            reportFilterItems.ForEach(item => {
                if (item.FieldName == fieldName)
                {
                    if (item.FieldValue != null)
                        fieldValue = item.FieldValue.ToString();
                }
            });
            return fieldValue;
        }
    }
}
