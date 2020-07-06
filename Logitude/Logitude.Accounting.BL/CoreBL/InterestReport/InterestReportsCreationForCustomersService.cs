using Logitude.Accounting.BL.DataContract;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.SystemLogs;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.Accounting.BL.CoreBL.InterestReport
{
    public class InterestReportsCreationForCustomersService
    {
        private DateTime interestCalculationDate;
        private int tenant;
        private List<InterestReportCustomerPM> eligibleCustomers;
        public InterestReportsCreationForCustomersService(InterestReportsCreationForCustomersArgs args)
        {
            interestCalculationDate = args.InterestCalculationDate;
            tenant = args.Tenant;
            eligibleCustomers = args.EligibleCustomers;
        }

        public string CreateReportsForCustomersByCalculationStartDateAndCalculateData()
        {
            string log = "";
            
            
            for (int i = 0; i < eligibleCustomers.Count; i++) 
            {
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    InterestReportPM interestReportPM = null;
                    try
                    {
                        interestReportPM = CreateInterestReportForCustomerGlAccount(eligibleCustomers[i]);
                        CalculateDataForInterestReport(interestReportPM);
                        scope.Complete();
                    }
                    catch(Exception ex)
                    {
                        string errorMessage = GetErrorMessage(ex);
                        log = log + Environment.NewLine + "Error in report for customer: " 
                            + eligibleCustomers[i].EnglishName+Environment.NewLine+"Error: "+ errorMessage;
                    }
                }
            }
            return log;
        }

        private string GetErrorMessage(Exception exception)
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
        private InterestReportPM CreateInterestReportForCustomerGlAccount(InterestReportCustomerPM interestReportCustomerPM)
        {
            DateTime createDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            InterestReportPM interestReportPM = new InterestReportPM()
            {
                IsCreatedFromBatch = true,
                Tenant = tenant,
                CreateDateTime = createDate,
                CustomerId = interestReportCustomerPM.CustomerId,
                GLAccountId = interestReportCustomerPM.GLAccountId,
                InterestCalculationDate = interestCalculationDate,
                InterestReportStatusCode = "5",
                ChangeSetOp=Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
            };
            IAccountingContext context = AccountingContext.GetContext(tenant);
            InterestReportUpdateService interestReportUpdateService = new InterestReportUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
            interestReportUpdateService.Update(interestReportPM, true);
            return interestReportPM;
        }

        private void CalculateDataForInterestReport(InterestReportPM interestReportPM) 
        {
            InterestReportArgs interestReportArgs = new InterestReportArgs()
            {
                InterestReport = interestReportPM,
                InterestReportId = interestReportPM.Id,
                ReportNumber = interestReportPM.ReportNumber,
                Tenant = tenant
            };
            InterestReportDataCalculations interestReportDataCalculation = new InterestReportDataCalculations(interestReportArgs);
            interestReportDataCalculation.StartCalculations();
        }
        private List<InterestReportCustomerPM> GetEligibleCustomersForInterestReports(int tenant)
        {
            GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(tenant);
            List<InterestReportCustomerPM> eligibleCustomers= gLAccountQueryService.GetEligibleCustomersForInterestReports(tenant);
            return eligibleCustomers;
        }
    }
}
