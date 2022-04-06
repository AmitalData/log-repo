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
                InterestReportPM interestReportPM = null;
                try
                {
                    interestReportPM = interestReportsCreationForCustomerDataPreparation.GetPreviousInvoicedOrCloseWithoutInvoicedtInterestReportForCustomer(eligibleCustomers[i], interestCalculationDate);
                    if (interestReportPM!=null)
                    {
                        string LogMessage = "there is already a report with a calculation date less than or equal to the previously Invoiced/Closed report [Report "+ interestReportPM.ReportNumber + " on "+ interestReportPM.InterestCalculationDate.ToString("dd/MM/yyyy") + "].";
                        interestReportPM = null;
                        throw new ApplicationException(LogMessage);
                    }
                    interestReportPM = interestReportsCreationForCustomerDataPreparation.GetDraftInterestReportForCustomer(eligibleCustomers[i]);
                    if (interestReportPM != null && interestReportPM.InterestReportStatusCode == "1")
                    {
                        SetInterestReportStatusInProgress(interestReportPM);
                    }
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        if (interestReportPM == null)
                        {
                            interestReportPM = interestReportsCreationForCustomerDataPreparation.CreateInterestReportForCustomerGlAccount(eligibleCustomers[i]);
                        }
                        else
                        {
                            interestReportPM.RecalculateData = true;
                        }
                        interestReportPM.InterestCalculationDate = interestCalculationDate;
                        scope.Complete();

                    }
                    CalculateDataForInterestReport(interestReportPM);
                }
                catch (Exception ex)
                {
                    string errorMessage = interestReportsCreationForCustomerDataPreparation.GetErrorMessage(ex);
                    log += GetErrorInReportForCustomer(eligibleCustomers[i], interestReportPM, errorMessage);

                }
            }
            return log;
        }

        private string GetErrorInReportForCustomer(InterestReportCustomerPM  eligibleCustomer , InterestReportPM interestReportPM, string errorMessage)
        {
            string log = ""; 
            log = log + Environment.NewLine + "Error in report for customer: "
                        + eligibleCustomer.EnglishName;
            if (interestReportPM != null)
            {
                log = log + " and report number: " + interestReportPM.ReportNumber;
            }
            log = log + Environment.NewLine + " Error: " + errorMessage;

            return log;
        }
        private void CalculateDataForInterestReport(InterestReportPM interestReportPM)
        {
            InterestReportArgs interestReportArgs = new InterestReportArgs()
            {
                InterestReport = interestReportPM,
                InterestReportId = interestReportPM.Id,
                ReportNumber = interestReportPM.ReportNumber,
                Tenant = tenant,
                RecalculateData = interestReportPM.RecalculateData,
            };
            InterestReportDataCalculations interestReportDataCalculation = new InterestReportDataCalculations(interestReportArgs);
            interestReportDataCalculation.StartCalculations();
        }

        private void SetInterestReportStatusInProgress(InterestReportPM interestReportPM)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                interestReportPM.InterestReportStatusCode = "5";
                SubmitChangesToInterestReport(interestReportPM);
                scope.Complete();
            }
        }

        private void SubmitChangesToInterestReport(InterestReportPM interestReportPM)
        {
            interestReportPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            InterestReportUpdateService interestReportUpdateService = new InterestReportUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
            interestReportPM.IsUpdatedFromBatch = true;
            interestReportUpdateService.Update(interestReportPM, true);
        }

    }
}
