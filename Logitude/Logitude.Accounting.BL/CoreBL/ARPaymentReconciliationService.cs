using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InvoiceModel.CloseTables;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.Accounting.BL.CoreBL
{
    public class ARPaymentReconciliationService
    {
        IInvoiceContext _invoiceContext = null;
        private string autoCreditedInvoiceStatusCode = "AR";
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

                paymentPM.PaymentInvoices = new List<ARPaymentInvoicePM>(); // [!] payment invoices removed in order to avoid validation (CheckLinesAmountToReconcileTotal) in ARPayment service, in this block we only need to update open amount and status , WI 58101
                if (paymentPM.GLAccountId == null) paymentPM.GLAccountId = entityPM.AccountId;
                CalculatePaymentOpenAmount(paymentPM);

                CalculatePaymentStatus(paymentPM);

                ARPaymentService paymentService = new ARPaymentService(_invoiceContext, entityPM.Tenant);
                paymentPM.UpdateAmountAndStatuses = true;
                paymentService.Update(paymentPM);

            }

        }

        public void UpdatePaymentOpenAmountForReconciliation(ReconciliationPM reconciliationPM, 
            LedgerTransactionPM paymentTransaction,ARPaymentPM paymentPM)
        {
            if (paymentTransaction != null)
            {
                CalculatePaymentOpenAmount(paymentPM);
            }
        }

        public void UpdatePaymentStatusForReconciliation(ReconciliationPM reconciliationPM,
            LedgerTransactionPM paymentTransaction, ARPaymentPM paymentPM)
        {
            if (paymentTransaction != null)
            {
                CalculatePaymentStatus(paymentPM);
            }
        }

        public void SubmitChangesToPayment(ARPaymentPM paymentPM)
        {
            ARPaymentService paymentService = new ARPaymentService(_invoiceContext, paymentPM.Tenant);
            paymentService.Update(paymentPM);
        }
         
        public void UpdateConnectedInvoices(ReconciliationPM entityPM)
        {
            ARInvoiceService invoiceService = new ARInvoiceService(_invoiceContext, entityPM.Tenant);
            LedgerTransactionPM paymentTransaction = GetPaymentTransactionFromReconciliation(entityPM);
            List<LedgerTransactionPM> recoTransactions  = GetReconciliationTransactions(entityPM);

            if (paymentTransaction != null)
            {
                // get invoices
                List<ARInvoicePM> paymentInvoices = GetPaymentInvoices(entityPM, paymentTransaction);

                foreach (ARInvoicePM invoice in paymentInvoices)
                {
                    LedgerTransactionPM transaction = recoTransactions.Where(d => d.SourceId == invoice.Id).FirstOrDefault();
                    //decimal recoAmount = entityPM.ReconciliationLines.Where(d => d.TransactionId == transaction.Id).FirstOrDefault().ReconciliationAmount;

                    CaclulateInvoiceAmount(invoice, transaction, entityPM);
                    CaclulateInvoiceStatus(invoice, entityPM.AccountReconcileMethodCode);

                    invoiceService.Update(invoice);
                }

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
            if(paymentPM != null && paymentPM.StatusCode != "VD") { 
            LedgerTransactionQueryService transQuery = new LedgerTransactionQueryService(paymentPM.Tenant);

                    List<LedgerTransactionPM> reconciledInvoicesTransactions
                        = transQuery.GetReconciledInvoicesTransactionsForPayment(paymentPM.Id, paymentPM.GLAccountId, paymentPM.Tenant);

                    decimal paymentReconciledInvoicesTotal = reconciledInvoicesTransactions.Sum(d => d.PaymentReconciledAmount).Value;

                    // calculate open amount for payment
                    double openAmount = paymentPM.AmountInPaymentCurrency.Value - (double)paymentReconciledInvoicesTotal;
                    paymentPM.OpenAmount = MethodHelper.Round(openAmount, 2);
                }
        }
        private void CalculatePaymentStatus(ARPaymentPM paymentPM)
        {
            if (paymentPM != null && paymentPM.StatusCode !="VD")
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


        private List<ARInvoicePM> GetPaymentInvoices(ReconciliationPM entityPM, LedgerTransactionPM paymentTransaction)
        {
            ARInvoiceQuery invoicesQuery = new ARInvoiceQuery(entityPM.Tenant);
            LedgerTransactionQueryService transQuery = new LedgerTransactionQueryService(entityPM.Tenant);

            // get invoices ids from reconciliation
            List<string> invoicesTransactionIds = entityPM.ReconciliationLines
                                                    .Where(d => d.TransactionId != paymentTransaction.Id)
                                                    .Select(d => d.TransactionId).ToList();
            List<LedgerTransactionPM> invoicesTransactions = transQuery.GetLedgerTransactionPMsByIdList(invoicesTransactionIds, entityPM.Tenant);
            List<string> invoicesIds = invoicesTransactions.Select(d => d.SourceId).ToList();

            // get invoices
            List<ARInvoicePM> invoicesPM = invoicesQuery.GetARInvoicePMsByIdList(invoicesIds, entityPM.Tenant);

            invoicesPM = ExcludeAutoCreditedInvoices(invoicesPM);

            return invoicesPM;
        }
        private List<ARInvoicePM> ExcludeAutoCreditedInvoices(List<ARInvoicePM> invoicesPM)
        {
            return invoicesPM.Where(invoice => invoice.StatusCode != autoCreditedInvoiceStatusCode).ToList();
        }
        private void CaclulateInvoiceStatus(ARInvoicePM invoice, string reconcileMethodCode)
        {
            
            var invoiceAmount = invoice.AmountInInvoiceCurrency;

            if (invoice.AmountDue <= 0)
            {
                invoice.IsClosed = true;
                invoice.StatusCode = ARInvoiceStatusValues.Paid;
            }
            else if (invoice.AmountDue < invoiceAmount)
            {
                invoice.IsClosed = false;
                invoice.StatusCode = ARInvoiceStatusValues.PartiallyPaid;
            }
            else
            {
                invoice.IsClosed = false;
                invoice.StatusCode = ARInvoiceStatusValues.Unpaid;

            }
        }
        private void CaclulateInvoiceAmount(ARInvoicePM invoice, LedgerTransactionPM transaction, ReconciliationPM recoPM)
        {
            string tenantCurrencyId = GetTenantCurrencyId(transaction.Tenant);

            if (recoPM.AccountReconcileMethodCode == ReconcileMethodValues.LocalCurrency)
            {
                invoice.AmountDueInLocalCurrency = (double)transaction.OpenAmount;
                invoice.AmountDue = MethodHelper.Round((invoice.AmountDueInLocalCurrency / invoice.InvoiceCurrencyExchangeRate), 2);
            }
            else
            {
                double glaCurrencyRate = GetGLAccountCurrencyRate(recoPM.AccountCurrencyId, tenantCurrencyId, transaction.Tenant);

                invoice.AmountDueInLocalCurrency = GetLocal((double)transaction.OpenAmount, glaCurrencyRate);
                invoice.AmountDue = GetForeign(invoice.AmountDueInLocalCurrency, invoice.InvoiceCurrencyExchangeRate);
            }

            invoice.AmountDueInProfitCurrency = GetForeign(invoice.AmountDueInLocalCurrency, invoice.ProfitCurrencyExchangeRate);

        }

        private double GetGLAccountCurrencyRate(string accountCurrencyId, string tenantCurrencyId, int tenant)
        {
            double rate = 1;
            RatesTableQuery rateQuery = new RatesTableQuery(tenant);

            if (accountCurrencyId == tenantCurrencyId)
                rate = 1;
            else
            {
                LastRate glaToLocalRate = rateQuery.GetLastRecord(tenant, accountCurrencyId, tenantCurrencyId);
                if (glaToLocalRate == null) throw new ApplicationException("Account currency exchange rate does not exist");

                rate = (double)glaToLocalRate.Rate;
            }

            return rate;

        }

        private string GetTenantCurrencyId(int tenant)
        {
            TenantRepository tRepo = new TenantRepository(tenant);
            Tenant t = tRepo.GetSingleByTenant(tenant);
            string tenantCurrency = (t == null ? null : t.CurrencyId);
            return tenantCurrency;
        }
        private decimal GetOriginalAmountOfTransaction(LedgerTransactionPM transaction, string reconcileMethodCode)
        {
            if (!string.IsNullOrEmpty(reconcileMethodCode))
            {

                if (reconcileMethodCode == "0")
                { // 0-local currency

                    if (transaction.LocalAmountCredit == 0)
                    {
                        return transaction.LocalAmountDebit;
                    }
                    else
                    {
                        return -1 * transaction.LocalAmountCredit;
                    }

                }
                else if (reconcileMethodCode == "1")
                { // 1-foreign currency

                    if (transaction.ForeignAmountCredit == 0)
                    {
                        return transaction.ForeignAmountDebit;
                    }
                    else
                    {
                        return -1 * transaction.ForeignAmountCredit;
                    }

                }

            }

            return 0;
        }
        private double GetLocal(double? foreignAmount, double? currencyRate)
        {
            double result = 0;

            result = (double)foreignAmount * (double)currencyRate;

            return (double)MethodHelper.Round(result,2);
        }
        private double GetForeign(double? localAmount, double? currencyRate)
        {
            double result = 0;

            result = (double)localAmount / (double)currencyRate;

            return (double)MethodHelper.Round(result, 2);
        }
    }

    

}
