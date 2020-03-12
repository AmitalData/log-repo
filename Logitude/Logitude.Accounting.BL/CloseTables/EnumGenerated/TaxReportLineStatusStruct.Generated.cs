

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Logitude.Accounting.BL.CloseTables
{
    public struct TaxReportLineStatusValues
    {   
       public const string MissingVatNo = "1";  
       public const string Invoicenumberisnotvalid = "3";  
       public const string Invoiceamountisnotvalid = "4";  
       public const string DuplicateThereisanothertransactionwiththesameVATNoandReference = "5";  
       public const string Readyfortransmit = "6";  
       public const string WrongVATNumber = "2";  
       public const string Invoicenotpreviouslyreported = "7";  
    }
}

