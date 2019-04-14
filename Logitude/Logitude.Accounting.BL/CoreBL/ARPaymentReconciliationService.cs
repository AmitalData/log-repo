using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.InvoiceModel.CloseTables;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InvoiceModel;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.Accounting.BL.CoreBL
{
    public class ARPaymentReconciliationService
    {
        IInvoiceContext _invoiceContext = null;
        public ARPaymentReconciliationService(int tenant)
        {
            _invoiceContext = InvoiceContext.GetContext(tenant);

        }

        public void UpdatePaymentOpenAmountAndStatusForReconciliaiton(ReconciliationPM entityPM)
        {
            LedgerTransactionPM paymentTransaction = GetPaymentTransactionFromReconciliation(entityPM);

            if(paymentTransaction != null)
            {
                ARPaymentPM paymentPM = GetARPaymentPMById(paymentTransaction.SourceId, paymentTransaction.Tenant);

                CalculatePaymentOpenAmount(paymentPM);

                CalculatePaymentStatus(paymentPM);

                ARPaymentService paymentService = new ARPaymentService(_invoiceContext, entityPM.Tenant);
                paymentService.Update(paymentPM);

            }

        }


        // private methods
        private LedgerTransactionPM GetPaymentTransactionFromReconciliation(ReconciliationPM entityPM)
        {
            LedgerTransactionPM paymentTransaction = null;

            List<LedgerTransactionPM> ledgerTransactions = GetReconciliationTransactions(entityPM);

            paymentTransaction = ledgerTransactions
                                        .Where(d => d.SourceTypeCode == AccountingEntityValues.ARPayment)
                                        .FirstOrDefault();

            return paymentTransaction;
        }
        private List<LedgerTransactionPM> GetReconciliationTransactions(ReconciliationPM entityPM)
        {
            LedgerTransactionQueryService transQuery = new LedgerTransactionQueryService(entityPM.Tenant);

            List<string> ledgerTransactionIds = entityPM.ReconciliationLines.Where(d => d.TransactionId != null).Select(d => d.TransactionId).ToList();

            List<LedgerTransactionPM> ledgerTransactions = transQuery.GetLedgerTransactionPMsByIdList(ledgerTransactionIds, entityPM.Tenant);

            return ledgerTransactions;
        }
        private ARPaymentPM GetARPaymentPMById(string paymentId,int tenant)
        {
            ARPaymentQuery paymentQuery = new ARPaymentQuery(tenant);
            ARPaymentPM paymentPM = paymentQuery.GetSinglePM(paymentId, tenant);

            return paymentPM;
        }
        private void CalculatePaymentOpenAmount(ARPaymentPM paymentPM)
        {
            LedgerTransactionQueryService transQuery = new LedgerTransactionQueryService(paymentPM.Tenant);

            List<LedgerTransactionPM> reconciledInvoicesTransactions
                = transQuery.GetReconciledInvoicesTransactionsForPayment(paymentPM.Id, paymentPM.GLAccountId, paymentPM.Tenant);

            decimal paymentReconciledInvoicesTotal = reconciledInvoicesTransactions.Sum(d => d.PaymentReconciledAmount).Value;

            // calculate open amount for payment
            double openAmount = paymentPM.AmountInPaymentCurrency.Value - (double)paymentReconciledInvoicesTotal;
            paymentPM.OpenAmount = MethodHelper.Round(openAmount, 2);
        }
        private void CalculatePaymentStatus(ARPaymentPM paymentPM)
        {
            if (paymentPM != null)
            {
                if (paymentPM.OpenAmount == 0)
                {
                    paymentPM.StatusCode = ARPaymentStatusValues.Closed;
                    paymentPM.IsClosed = true;
                }
                else
                {
                    paymentPM.StatusCode = ARPaymentStatusValues.Approved;
                    paymentPM.IsClosed = false;
                }
            }
        }



    }

}
