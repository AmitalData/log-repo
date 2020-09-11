using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.BL.DataContract;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Data.Helpers;

namespace Logitude.Accounting.BL.CoreBL.InterestReport
{
    public class InterestReportsCreationForCustomerDataPreparation : IInterestReportsCreationForCustomerDataPreparation
    {
        private DateTime interestCalculationDate;
        private int tenant;
        private string email;
        public InterestReportsCreationForCustomerDataPreparation(InterestReportsCreationForCustomersBatchArgs args)
        {
            tenant = args.Tenant;
            interestCalculationDate = args.InterestCalculationDate;
            email = args.Email;
        }
        public InterestReportPM CreateInterestReportForCustomerGlAccount(InterestReportCustomerPM interestReportCustomerPM)
        {
            DateTime createDate = TenantServerConfigration.GetCurrentDateTime(interestReportCustomerPM.Tenant);
            InterestReportPM interestReportPM = new InterestReportPM()
            {
                IsCreatedFromBatch = true,
                Tenant = tenant,
                CreateDateTime = createDate,
                CustomerId = interestReportCustomerPM.CustomerId,
                GLAccountId = interestReportCustomerPM.GLAccountId,
                InterestCalculationDate = interestCalculationDate,
                InterestReportStatusCode = "5",
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                BatchReportUserEmail = email,
             
            };
            IAccountingContext context = AccountingContext.GetContext(tenant);
            InterestReportUpdateService interestReportUpdateService = new InterestReportUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
            interestReportUpdateService.Update(interestReportPM, true);
            return interestReportPM;
        }

        public InterestReportPM GetDraftInterestReportForCustomer(InterestReportCustomerPM interestReportCustomerPM)
        {
            InterestReportQueryService interestReportQueryService = new InterestReportQueryService(tenant);
            InterestReportPM interestReportPM = interestReportQueryService.GetDraftInterestReportForCustomer(
                interestReportCustomerPM.CustomerId, interestReportCustomerPM.GLAccountId, tenant);
            return interestReportPM;
        }

        public InterestReportPM GetPreviousInvoicedOrCloseWithoutInvoicedtInterestReportForCustomer(InterestReportCustomerPM interestReportCustomerPM, DateTime interestCalculationDate)
        {
            InterestReportQueryService interestReportQueryService = new InterestReportQueryService(tenant);
            InterestReportPM interestReportPM = interestReportQueryService.GetPreviousInvoicedOrCloseWithoutInvoicedtInterestReportForCustomer(
                                                interestReportCustomerPM.CustomerId, tenant, interestCalculationDate);
            return interestReportPM;
        }

        public List<InterestReportCustomerPM> GetEligibleCustomersForInterestReports(int tenant)
        {
            GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(tenant);
            List<InterestReportCustomerPM> eligibleCustomers = gLAccountQueryService.GetEligibleCustomersForInterestReports(tenant);
            return eligibleCustomers;
        }

        public string GetErrorMessage(Exception exception)
        {
            string errorMessage = "";
            errorMessage += exception.Message;

            if (exception.InnerException != null)
            {
                errorMessage += Environment.NewLine + exception.InnerException.Message;

                if (exception.InnerException.InnerException != null)
                {
                    errorMessage += Environment.NewLine + exception.InnerException.InnerException.Message;

                    if (exception.InnerException.InnerException.InnerException != null)
                    {
                        errorMessage += Environment.NewLine + exception.InnerException.InnerException.InnerException.Message;
                    }
                }
            }
            return errorMessage;
        }
    }
}
