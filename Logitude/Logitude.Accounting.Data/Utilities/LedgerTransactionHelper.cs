using Logitude.Accounting.Data.EntityLists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Data.Utilities
{
    public class LedgerTransactionHelper
    {
        public   string getEntityIcon(string _sourceTypeCode)
        {
            var iconTxt = "";
            switch (_sourceTypeCode)
            {
                // 1-Journal
                case "1":
                    {
                        iconTxt = "JR";
                        break;
                    }

                // 2-ARInvoice
                case "2":
                    {
                        iconTxt = "IN";
                        break;
                    }

                // 3-ARPayment
                case "3":
                    {
                        iconTxt = "PY";
                        break;
                    }

                // 4-APInvoice
                case "4":
                    {
                        iconTxt = "IN";
                        break;
                    }

                // 5-APPayment
                case "5":
                    {
                        iconTxt = "PY";

                        break;
                    }

                // 6-Cheque Deposit
                case "6":
                    {
                        iconTxt = "DP";

                        break;
                    }

                // 7-Cash Deposit
                case "7":
                    {
                        iconTxt = "DP";

                        break;
                    }

                // 8-Revaluation
                case "8":
                    {
                        iconTxt = "RV";

                        break;
                    }

                // 9-PaymentCheque
                case "9":
                    {
                        iconTxt = "CH";

                        break;
                    }

                // 10-Adjustment
                case "10":
                    {
                        iconTxt = "AJ";

                        break;
                    }
            }
            return iconTxt;
        }

        public void MapAmountWithNegativeValue(LedgerTransactionList rec)
        {
            if (rec.IsLocalAmountCreditPos)
                rec.LocalAmountCredit = rec.LocalAmountCredit * -1;
            if (rec.IsCumulativeLocalAmountPos)
                rec.CumulativeLocalAmount = rec.CumulativeLocalAmount * -1;
            if (rec.IsForeignAmountCreditPos)
                rec.ForeignAmountCredit = rec.ForeignAmountCredit * -1;
            if (rec.IsCumulativeForeignAmountPos)
                rec.CumulativeForeignAmount = rec.CumulativeForeignAmount * -1;
            if (rec.IsOriginalAmountPos)
                rec.OriginalAmount = rec.OriginalAmount * -1;
            if (rec.IsForeignAmountPos)
                rec.ForeignAmount = rec.ForeignAmount * -1;

        }
        public  decimal CalculateOriginalAmount(LedgerTransactionList LedgerTransaction)
        {
            if (!string.IsNullOrEmpty(LedgerTransaction.ReconcileMethodCode))
            {
                if (LedgerTransaction.ReconcileMethodCode == "0")
                {   // 0-local currency
                    if (LedgerTransaction.LocalAmountCredit == 0)
                    {
                        return LedgerTransaction.LocalAmountDebit;
                    }
                    else
                    {
                        return LedgerTransaction.LocalAmountCredit; // -1 *
                    }
                }
                else if (LedgerTransaction.ReconcileMethodCode == "1")
                {   // 1-foreign currency
                    if (LedgerTransaction.ForeignAmountCredit == 0)
                    {
                        return LedgerTransaction.ForeignAmountDebit;
                    }
                    else
                    {
                        return LedgerTransaction.ForeignAmountCredit;  // -1 *
                    }
                }
            }

            return 0;

        }
    }
}
