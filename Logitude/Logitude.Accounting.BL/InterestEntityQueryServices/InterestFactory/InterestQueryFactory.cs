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
        public static IInterestEntityQueryService CreateInstance(string Code)
        {
            IInterestEntityQueryService Entifty=null; 
            switch (Code)
            {
                case "1":
                    {
                        Entifty = new InterestARInvoiceQueryService();
                        break;
                    }
                 case "2":
                    {
                        Entifty = new InterestARPaymentQueryService();
                        break;
                    }
                case "3":
                    {
                        Entifty = new InterestJournalQueryService();
                        break;
                    }
                case "4":
                    {
                        Entifty = new InterestInterestReportQueryService();
                        break;
                    }
            }
            return Entifty;
        }
    }
}
