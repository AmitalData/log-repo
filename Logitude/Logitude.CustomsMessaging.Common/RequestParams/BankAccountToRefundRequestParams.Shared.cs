using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class BankAccountToRefundRequestParams : RequestParamsBase
    {
        public string FileType { get; set; }
        public string FileNumber { get; set; }
        public int Numeral { get; set; }
        public string IdentifierType { get; set; }
        public string IdentifierCode { get; set; }
        public string CountryCode { get; set; }
        public string InternalBank { get; set; }
        public string BankCode { get; set; }
        public string AccountBranch { get; set; }
        public string AccountNumber { get; set; }
        public string AccountCurrency { get; set; }
    }
}
