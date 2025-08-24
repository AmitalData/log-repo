using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InvoiceModel.CloseTables;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Data.Helpers;

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



        public void UpdatePaymentOpenAmountAndStatusForReconciliaitonLT(ReconciliationPM entityPM)
        {
            LedgerTransactionJournalLineLT paymentTransaction = GetPaymentTransactionFromReconciliationLT(entityPM);
            if (paymentTransaction != null)
            {

                ARPaymentPM paymentPM = GetARPaymentPMById(paymentTransaction.SourceId, paymentTransaction.Tenant);

                paymentPM.PaymentInvoices = new List<ARPaymentInvoicePM>(); // [!] payment invoices removed in order to avoid validation (CheckLinesAmountToReconcileTotal) in ARPayment service, in this block we only need to update open amount and status , WI 58101
                if (paymentPM.GLAccountId == null) paymentPM.GLAccountId = entityPM.AccountId;
                double? old_openAmount = paymentPM.OpenAmount;
                CalculatePaymentOpenAmountLT(paymentPM);

                string old_statusCode = paymentPM.StatusCode;
                bool old_isClosed = paymentPM.IsClosed;
                CalculatePaymentStatus(paymentPM);
                if (paymentPM.IsClosed != old_isClosed || paymentPM.StatusCode != old_statusCode 
                    || IsDifferent(paymentPM.OpenAmount, old_openAmount))
                {
                    ARPaymentService paymentService = new ARPaymentService(_invoiceContext, entityPM.Tenant);
                    paymentPM.UpdateAmountAndStatuses = true;
                    paymentService.Update(paymentPM);
                }

            }

        }



        public void SubmitChangesToPayment(ARPaymentPM paymentPM)
        {
            ARPaymentService paymentService = new ARPaymentService(_invoiceContext, paymentPM.Tenant);
            paymentService.Update(paymentPM);
        }


        private bool IsDifferent(double? left, double? right)
        {
            bool rv = false;
            rv = (left.HasValue != right.HasValue)
                     || (left.HasValue && right.HasValue && (Math.Abs(left.Value - right.Value) >= 0.005));
            return rv;
        }



        public void UpdateConnectedInvoicesLT(ReconciliationPM entityPM)
        {
            ARInvoiceService invoiceService = new ARInvoiceService(_invoiceContext, entityPM.Tenant);
            LedgerTransactionJournalLineLT paymentTransaction = GetPaymentTransactionFromReconciliationLT(entityPM);
            List<LedgerTransactionJournalLineLT> recoTransactions = GetReconciliationTransactionsLT(entityPM);

            if (paymentTransaction != null)
            {
                // get invoices
                List<ARInvoicePM> paymentInvoices = GetPaymentInvoicesLT(entityPM, paymentTransaction);

                foreach (ARInvoicePM invoice in paymentInvoices.Where(inv => inv.StatusCode != ARInvoiceStatusValues.Void))
                {
                    LedgerTransactionJournalLineLT transaction = recoTransactions.Where(d => d.SourceId == invoice.Id).FirstOrDefault();
                    double? old_amountDueInLocalCurrency = invoice.AmountDueInLocalCurrency;
                    double? old_amountDue = invoice.AmountDue;
                    double? old_amountDueInProfitCurrency = invoice.AmountDueInProfitCurrency;
                    bool old_isClosed = invoice.IsClosed;
                    string old_statusCode = invoice.StatusCode;
                    CaclulateInvoiceAmountLT(invoice, transaction, entityPM);
                    CaclulateInvoiceStatusLT(invoice, entityPM.AccountReconcileMethodCode, transaction);
                    if (invoice.IsClosed != old_isClosed || invoice.StatusCode != old_statusCode
                        || IsDifferent(invoice.AmountDueInLocalCurrency, old_amountDueInLocalCurrency)
                        || IsDifferent(invoice.AmountDue, old_amountDue)
                        || IsDifferent(invoice.AmountDueInProfitCurrency, old_amountDueInProfitCurrency))
                    {
                        if (FeatureToggleHelper.HasFeatureToggle("ILO", invoice.Tenant) && invoice.IsMultiCurrency)
                        {
                            LedgerTransactionQueryService transQuery = new LedgerTransactionQueryService(entityPM.Tenant);
                            var arinvoiceTransactions = transQuery.GetByJournalIdAndForeignAmountDebitNotEqualZero(invoice.JournalId, invoice.Tenant).ToList();
                            
                            if (arinvoiceTransactions.All(x => x.OpenAmount == 0))
                            {
                                invoice.StatusCode = ARInvoiceStatusValues.Paid;
                                invoice.IsClosed = true;
                                if (invoice.PaidDate == null)
                                {
                                    invoice.PaidDate = TenantServerConfigration.GetCurrentDateTime(invoice.Tenant).Date;
                                }
                            }
                            else if (arinvoiceTransactions.Any(x => x.ForeignAmountDebit != 0 && x.OpenAmount != x.ForeignAmountDebit))
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
                    }
                    invoiceService.Update(invoice);
                }

            }
        }


        private LedgerTransactionJournalLineLT GetPaymentTransactionFromReconciliationLT(ReconciliationPM entityPM)
        {
            LedgerTransactionJournalLineLT paymentTransaction = null;

            List<LedgerTransactionJournalLineLT> ledgerTransactions = GetReconciliationTransactionsLT(entityPM);

            paymentTransaction = ledgerTransactions
                                        .Where(d => d.SourceTypeCode == CloseTables.AccountingEntityValues.ARPayment)
                                        .FirstOrDefault();

            return paymentTransaction;
        }


        private List<LedgerTransactionJournalLineLT> GetReconciliationTransactionsLT(ReconciliationPM entityPM)
        {
            LedgerTransactionQueryService transQuery = new LedgerTransactionQueryService(entityPM.Tenant);

            List<string> ledgerTransactionIds = entityPM.ReconciliationLines.Where(d => d.TransactionId != null).Select(d => d.TransactionId).ToList();

            List<LedgerTransactionJournalLineLT> ledgerTransactions = transQuery.GetLedgerTransactionJournalLineLTsByIdList(ledgerTransactionIds, entityPM.Tenant);

            return ledgerTransactions;
        }

        private ARPaymentPM GetARPaymentPMById(string paymentId,int tenant)
        {
            ARPaymentQuery paymentQuery = new ARPaymentQuery(tenant);
            ARPaymentPM paymentPM = paymentQuery.GetSinglePM(paymentId, tenant);

            return paymentPM;
        }

        private void CalculatePaymentOpenAmountLT(ARPaymentPM paymentPM)
        {
            if (paymentPM != null && paymentPM.StatusCode != "VD")
            {
                LedgerTransactionQueryService transQuery = new LedgerTransactionQueryService(paymentPM.Tenant);

                List<LedgerTransactionJournalLineLT> reconciledInvoicesTransactions
                    = transQuery.GetReconciledInvoicesTransactionsForPaymentLT(paymentPM.Id, paymentPM.GLAccountId, paymentPM.Tenant);

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


        private List<ARInvoicePM> GetPaymentInvoicesLT(ReconciliationPM entityPM, LedgerTransactionJournalLineLT paymentTransaction)
        {
            ARInvoiceQuery invoicesQuery = new ARInvoiceQuery(entityPM.Tenant);
            LedgerTransactionQueryService transQuery = new LedgerTransactionQueryService(entityPM.Tenant);

            // get invoices ids from reconciliation
            List<string> invoicesTransactionIds = entityPM.ReconciliationLines
                                                    .Where(d => d.TransactionId != paymentTransaction.Id)
                                                    .Select(d => d.TransactionId).ToList();
            List<LedgerTransactionJournalLineLT> invoicesTransactions = transQuery.GetLedgerTransactionJournalLineLTsByIdList(invoicesTransactionIds, entityPM.Tenant);
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


        private void CaclulateInvoiceStatusLT(ARInvoicePM invoice, string reconcileMethodCode, LedgerTransactionJournalLineLT transaction)
        {
            if (FeatureToggleHelper.HasFeatureToggle("ILO", transaction.Tenant))
            {
                var transactionAmount = reconcileMethodCode == ReconcileMethodValues.LocalCurrency ? transaction.LocalAmountDebit : transaction.ForeignAmountDebit;
                if (transaction.OpenAmount <= 0)
                {
                    invoice.IsClosed = true;
                    invoice.StatusCode = ARInvoiceStatusValues.Paid;
                    if (invoice.PaidDate == null)
                    {
                        invoice.PaidDate = TenantServerConfigration.GetCurrentDateTime(invoice.Tenant).Date;
                    }
                }
                else if (transaction.OpenAmount < transactionAmount)
                 {
                    invoice.IsClosed = false;
                    invoice.StatusCode = ARInvoiceStatusValues.PartiallyPaid;
                    invoice.PaidDate = null;
                }
                else
                {
                    invoice.IsClosed = false;
                    if (invoice.StatusCode != ARInvoiceStatusValues.Draft)
                        invoice.StatusCode = ARInvoiceStatusValues.Unpaid;
                    invoice.PaidDate = null;
                }
            }
            else
            {
                var invoiceAmount = invoice.AmountInInvoiceCurrency;
                if (invoice.AmountDue <= 0)
                {
                    invoice.IsClosed = true;
                    invoice.StatusCode = ARInvoiceStatusValues.Paid;
                    if (invoice.PaidDate == null)
                    {
                        invoice.PaidDate = TenantServerConfigration.GetCurrentDateTime(invoice.Tenant).Date;
                    }
                }
                else if (invoice.AmountDue < invoiceAmount)
                {
                    invoice.IsClosed = false;
                    invoice.StatusCode = ARInvoiceStatusValues.PartiallyPaid;
                    invoice.PaidDate = null;
                }
                else
                {
                    invoice.IsClosed = false;
                    if (invoice.StatusCode != ARInvoiceStatusValues.Draft)
                        invoice.StatusCode = ARInvoiceStatusValues.Unpaid;
                    invoice.PaidDate = null;

                }
            }
 
 

        }




        private void CaclulateInvoiceAmountLT(ARInvoicePM invoice, LedgerTransactionJournalLineLT transaction, ReconciliationPM recoPM)
        {
            string tenantCurrencyId = GetTenantCurrencyId(transaction.Tenant);

            if (recoPM.AccountReconcileMethodCode == ReconcileMethodValues.LocalCurrency)
            {
                invoice.AmountDueInLocalCurrency = (double)transaction.OpenAmount;
                invoice.AmountDue = MethodHelper.Round((invoice.AmountDueInLocalCurrency / invoice.InvoiceCurrencyExchangeRate), 2);
            }
            else
            {
                string currencyId = recoPM.AccountCurrencyId;
                if (string.IsNullOrEmpty(recoPM.AccountCurrencyId))
                {
                    currencyId = recoPM.ReconciliationLines[0]?.CurrencyId;
                }
                double glaCurrencyRate = GetGLAccountCurrencyRate(currencyId, tenantCurrencyId, transaction.Tenant);

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
