using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.DataContracts
{
    public class LogitudeCRMReportFilter
    {
        public List<string> OpportunityTypes;
        public List<string> PaymentChannels;
        public string CustomerStatus;
        public string ResellerId;
        public bool ShowNet;
        public decimal ExchangeRate;
        public int Year;
    }
}
