using Logitude.Accounting.BL.InterestEntityQueryServices.InterestQueryServises;
using Logitude.Accounting.Data.Enums;
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
                case InterestEntityTypes.ARInvoice:
                    {
                        entifty = new InterestARInvoiceQueryService();
                        break;
                    }
                 case InterestEntityTypes.ARPayment:
                    {
                        entifty = new InterestARPaymentQueryService();
                        break;
                    }
                case InterestEntityTypes.Journal:
                    {
                        entifty = new InterestJournalQueryService();
                        break;
                    }
                case InterestEntityTypes.OpenBalance:
                    {
                        entifty = new InterestInterestReportQueryService();
                        break;
                    }
                case InterestEntityTypes.Adjustments:
                    {
                        entifty = new InterestAdjustmentQueryService();
                        break;
                    }
            }
            return entifty;
        }
    }
}
