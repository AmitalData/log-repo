using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class LedgerTransactionsDataProvider : BaseDataProvider
    {
        // Filter values
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string AccountNumber { get; set; }
        public string AccountEnglishName { get; set; }
        public string AccountLocalName { get; set; }
        public bool IsAccountMulticurrency { get; set; }
        public string AccountCurrencySign { get; set; }
        public string AccountCurrencyCode { get; set; }

        // Others
        public string PrintedByUser { get; set; }
        public string TenantCurrencyCode { get; set; }
        public string TenantCurrencySign { get; set; }

        // Data
        public decimal LocalOpenBalance { get; set; }
        public decimal LocalClosedBalance { get; set; }

        // List
        public List<ReportLedgerTransaction> Transactions { get; set; }
    }

    public class ReportLedgerTransaction
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string JournalId { get; set; }
        public int JournalLineNumber { get; set; }
        public DateTime CreateDate { get; set; }
        public string ControlAccountId { get; set; }
        public string AccountId { get; set; }
        public DateTime AccountingDate { get; set; }
        public DateTime DocumentDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal LocalAmountDebit { get; set; }
        public decimal LocalAmountCredit { get; set; }
        public string CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySign { get; set; }
        public decimal ForeignAmountDebit { get; set; }
        public decimal ForeignAmountCredit { get; set; }
        public decimal ExchangeRate { get; set; }
        public string Reference1 { get; set; }
        public string Reference2 { get; set; }
        public string Reference3 { get; set; }
        public decimal OpenAmount { get; set; }
        public string OppositeAccountId { get; set; }
        public string SearchFields { get; set; }
        public string OpenAmountCurrencyId { get; set; }
        public string OpenAmountCurrencyCode { get; set; }
        public string OpenAmountCurrencySign { get; set; }
        public string Notes { get; set; }
        public decimal AmountToReconcile { get; set; }
        public bool Mark { get; set; }
        public bool IsReconciled { get; set; }
        public bool IsExternalReconcile { get; set; }
        public bool InReconcileProgress { get; set; }
        public string ReconcileRemarks { get; set; }

        // foreign fields
        public string GLAccountRecoMethodCode { get; set; }
        public string TenantCurrencySign { get; set; }
        public string Source { get; set; }
        public string JournalNumber { get; set; }



        // calculated fields
        public decimal OriginalAmount
        {
            get
            {
                decimal result = 0;
                if (!string.IsNullOrEmpty(GLAccountRecoMethodCode))
                {

                    if (GLAccountRecoMethodCode == "0")  // 0-local currency
                    {

                        if (LocalAmountCredit == 0)
                        {
                            result = LocalAmountDebit;
                        }
                        else
                        {
                            result = -1 * LocalAmountCredit;
                        }

                    }
                    else if (GLAccountRecoMethodCode == "1") // 1-foreign currency
                    {

                        if (ForeignAmountCredit == 0)
                        {
                            result = ForeignAmountDebit;
                        }
                        else
                        {
                            result = -1 * ForeignAmountCredit;
                        }

                    }

                }
                return result;
            }
        }
        public string OriginalAmountCurrencySign
        {
            get
            {
                string result = TenantCurrencySign;
                if (!string.IsNullOrEmpty(GLAccountRecoMethodCode))
                {

                    if (GLAccountRecoMethodCode == "0")  // 0-local currency
                    {
                        result = TenantCurrencySign;
                    }
                    else if (GLAccountRecoMethodCode == "1") // 1-foreign currency
                    {
                        result = CurrencySign;
                    }

                }
                return result;
            }
        }
    }
}