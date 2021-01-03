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
        public static IInterestEntityQueryService CreateInstance(string code)
        {
            IInterestEntityQueryService entifty=null; 
            switch (code)
            {
                case "1":
                    {
                        entifty = new InterestARInvoiceQueryService();
                        break;
                    }
                 case "2":
                    {
                        entifty = new InterestARPaymentQueryService();
                        break;
                    }
                case "3":
                    {
                        entifty = new InterestJournalQueryService();
                        break;
                    }
                case "4":
                    {
                        entifty = new InterestInterestReportQueryService();
                        break;
                    }
            }
            return entifty;
        }
    }
}
