using Logitude.Accounting.BL.InterestEntityQueryServices.InterestQueryServises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.InterestEntityQueryServices.InterestFactory
{
    public class InterestQueryFactory
    {
        const string InterestEntityTypeCode_ARInvoice = "1";
        const string InterestEntityTypeCode_ARPayment = "2";
        const string InterestEntityTypeCode_Journal = "3";
        const string InterestEntityTypeCode_InterestReport = "4";
        public static IInterestEntityQueryService CreateInstance(string code)
        {
            IInterestEntityQueryService entifty=null; 
            switch (code)
            {
                case InterestEntityTypeCode_ARInvoice:
                    {
                        entifty = new InterestARInvoiceQueryService();
                        break;
                    }
                 case InterestEntityTypeCode_ARPayment:
                    {
                        entifty = new InterestARPaymentQueryService();
                        break;
                    }
                case InterestEntityTypeCode_Journal:
                    {
                        entifty = new InterestJournalQueryService();
                        break;
                    }
                case InterestEntityTypeCode_InterestReport:
                    {
                        entifty = new InterestInterestReportQueryService();
                        break;
                    }
            }
            return entifty;
        }
    }
}
