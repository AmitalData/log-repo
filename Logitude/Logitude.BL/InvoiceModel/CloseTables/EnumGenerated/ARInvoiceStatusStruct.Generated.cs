

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Logitude.BL.InvoiceModel.CloseTables
{
    public struct ARInvoiceStatusValues
    {   
       public const string AutoCredit = "AC";  
       public const string Unpaid = "AD";  
       public const string AutoCredited = "AR";  
       public const string Connected = "CN";  
       public const string Draft = "DR";  
       public const string Cancelled = "LL";  
       public const string NotConnected = "NT";  
       public const string Paid = "PD";  
       public const string PartiallyPaid = "PP";  
       public const string Void = "VD";  
    }
}

