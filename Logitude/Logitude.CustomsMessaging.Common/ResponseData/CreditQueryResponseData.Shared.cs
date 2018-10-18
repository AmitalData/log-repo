                                            //Yuval Chalup 23.06.2015 TASK-13278
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class CreditQueryResponseData : ResponseDataBase
    {
        public List<BalanceDetailsResult> BalanceDetailsList { get; set; }
        public List<BankAccountsResult> BankAccountsList { get; set; }
        public string ResponseStatusXML { get; set; }

        public class BalanceDetailsResult
        {
            public string FreeBalance { get; set; }
            public string TemporaryCeiling { get; set; }
            public string UsedBalance { get; set; }
        }

        public class BankAccountsResult
        {
            public string BankAccount { get; set; }
            public string UsedBalanceForBankAccount { get; set; }
        }
    }
}
