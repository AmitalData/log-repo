

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Logitude.Accounting.BL.CloseTables
{
    public struct InterestReportStatuseValues
    {   
       public const string Draft = "1";  
       public const string Cancelled = "3";  
       public const string ClosedwithoutInvoice = "4";  
       public const string Invoiced = "2";  
    }
}

