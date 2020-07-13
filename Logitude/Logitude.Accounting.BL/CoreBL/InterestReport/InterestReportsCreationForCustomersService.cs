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
using System.Web;

namespace Logitude.Accounting.BL.CoreBL.InterestReport
{
    public class InterestReportsCreationForCustomersService
    {
        
        private int tenant;
        private IInterestReportsCreationForCustomerDataPreparation interestReportsCreationForCustomerDataPreparation;
        private DateTime interestCalculationDate;
        private string loggedEmail;
        public InterestReportsCreationForCustomersService(InterestReportsCreationForCustomersArgs args)
        {
            tenant = args.Tenant;
            interestCalculationDate = args.InterestCalculationDate;
            loggedEmail = args.Email;
            interestReportsCreationForCustomerDataPreparation = args.InterestReportsCreationForCustomerDataPreparation;
        }

        public string CreateReportsIfNotExistAndCalculateReportsDataForCustomers()
        {
            string log = "";
            List<InterestReportCustomerPM> eligibleCustomers = interestReportsCreationForCustomerDataPreparation.GetEligibleCustomersForInterestReports(tenant);

            for (int i = 0; i < eligibleCustomers.Count; i++)
            {
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    InterestReportPM interestReportPM = null;
                    try
                    {
                        interestReportPM = interestReportsCreationForCustomerDataPreparation.GetDraftInterestReportForCustomer(eligibleCustomers[i]);
                        interestReportPM = interestReportPM ?? interestReportsCreationForCustomerDataPreparation.CreateInterestReportForCustomerGlAccount(eligibleCustomers[i]);
                        interestReportPM.InterestCalculationDate = interestCalculationDate;
                        interestReportPM.IsCreatedFromBatch = true;
                        interestReportPM.BatchReportUserEmail = loggedEmail;
                        CalculateDataForInterestReport(interestReportPM);
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        string errorMessage = interestReportsCreationForCustomerDataPreparation.GetErrorMessage(ex);
                        log = log + Environment.NewLine + "Error in report for customer: "
                            + eligibleCustomers[i].EnglishName + Environment.NewLine + "Error: " + errorMessage;
                    }
                }
            }
            return log;
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
      
    }
}
