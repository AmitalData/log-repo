using Logitude.Accounting.Data.EntityPOCOs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFreight.Web.AccountingModel.Reports.BankDeposit
{
    public class BankDepositDataProvider
    {
        public string CreatedByUserName { get; set; }
        public int DepositNumber { get; set; }
        public DateTime DepositDate { get; set; }
        public decimal LocalDepositAmount { get; set; }
        public decimal ForeignAmount { get; set; }
        public List<BankDepositLine> BankDepositLines { get; set; }

        // BankAccount
        public string BankAccountNumber { get; set; }
        public string BankAccountBranchNo { get; set; }
        public string BankAccountBranchAddress { get; set; }
        public string BankAccountLocalName { get; set; }
        public string CurrencyCode { get; set; }

    }

    public class BankDepositLine
    {
        public int Line { get; set; }
        public string ChequeNumber { get; set; }
        public decimal ForiegnAmount { get; set; }
        public decimal LocalAmount { get; set; }
        public DateTime DueDate { get; set; }
        public string Currency { get; set; }
        public string AccountNumber { get; set; }
        public string Bank { get; set; }
        public string Branch { get; set; }
        public string ARPaymentNumber { get; set; }
    }
}
