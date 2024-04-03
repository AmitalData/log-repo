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
        private   Dictionary<string, string> EntityIconsDictionary = new Dictionary<string, string>();
        const string LocalCurrency = "0";
        const string ForeignCurrency = "1";
        public LedgerTransactionHelper()
        {
            FillEntityIconsDictionary();
        }

        public void FillEntityIconsDictionary()
        {
            if (EntityIconsDictionary.Count==0)
            {
                EntityIconsDictionary.Add("1", "JR");// 1-Journal
                EntityIconsDictionary.Add("2", "IN");// 2-ARInvoice
                EntityIconsDictionary.Add("3", "PY");// 3-ARPayment
                EntityIconsDictionary.Add("4", "IN");// 4-APInvoice
                EntityIconsDictionary.Add("5", "PY");// 5-APPayment
                EntityIconsDictionary.Add("6", "DP");// 6-Cheque Deposit
                EntityIconsDictionary.Add("7", "DP");// 7-Cash Deposit
                EntityIconsDictionary.Add("8", "RV");// 8-Revaluation
                EntityIconsDictionary.Add("9", "CH");// 9-PaymentCheque
                EntityIconsDictionary.Add("10", "AJ");// 10-Adjustment
                EntityIconsDictionary.Add("11", "YT");// Year Transfer
                EntityIconsDictionary.Add("12", "BA");// Bank Adjustment
                EntityIconsDictionary.Add("13", "TR");// Tax Report
            }
        }
        public string getEntityIcon(string _sourceTypeCode)
        {
            string iconTxt = null;
            if (!string.IsNullOrEmpty(_sourceTypeCode))
            {
              iconTxt= EntityIconsDictionary[_sourceTypeCode];
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
            {
                rec.ForeignAmountCreditWithSign = (rec.ForeignAmountCredit * -1)+" "+ rec.CurrencySign;
                rec.ForeignAmountCredit = rec.ForeignAmountCredit * -1;
            }
            if (rec.IsCumulativeForeignAmountPos)
            {
                rec.CumulativeForeignAmountSign = (rec.CumulativeForeignAmount * -1)+" "+ rec.CurrencySign;
                rec.CumulativeForeignAmount = rec.CumulativeForeignAmount * -1;
            }
            if (rec.IsOriginalAmountPos)
                rec.OriginalAmount = rec.OriginalAmount * -1;
            if (rec.IsForeignAmountPos)
                rec.ForeignAmount = rec.ForeignAmount * -1;
        }
        public  decimal CalculateOriginalAmount(LedgerTransactionList LedgerTransaction)
        {
            if (!string.IsNullOrEmpty(LedgerTransaction.ReconcileMethodCode))
            {
                if (LedgerTransaction.ReconcileMethodCode == LocalCurrency)
                {   if (LedgerTransaction.LocalAmountCredit == 0)
                    {
                        return LedgerTransaction.LocalAmountDebit;
                    }
                    else
                    {
                        return LedgerTransaction.LocalAmountCredit;  
                    }
                }
                else if (LedgerTransaction.ReconcileMethodCode == ForeignCurrency)
                {   if (LedgerTransaction.ForeignAmountCredit == 0)
                    {
                        return LedgerTransaction.ForeignAmountDebit;
                    }
                    else
                    {
                        return LedgerTransaction.ForeignAmountCredit;   
                    }
                }
            }

            return 0;

        }
        public decimal CalculateAmountInNIS(LedgerTransactionList LedgerTransaction)
        {
            if (!string.IsNullOrEmpty(LedgerTransaction.ReconcileMethodCode))
            {
                if (LedgerTransaction.ReconcileMethodCode == "1")
                {
                    if (LedgerTransaction.LocalAmountCredit == 0)
                    {
                        return LedgerTransaction.LocalAmountDebit;
                    }
                    else
                    {
                        return LedgerTransaction.LocalAmountCredit;
                    }
                }
            }
            return 0;

        }

    }
}
