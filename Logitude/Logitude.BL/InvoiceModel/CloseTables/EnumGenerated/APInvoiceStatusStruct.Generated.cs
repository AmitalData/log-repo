

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Logitude.BL.InvoiceModel.CloseTables
{
    public struct APInvoiceStatusValues
    {   
       public const string ApprovalCanceled = "AC";  
       public const string Approved = "AD";  
       public const string Paid = "PD";  
       public const string PartiallyPaid = "PP";  
       public const string Void = "VD";  
       public const string WaitingForApproval = "WA";  
    }
}

