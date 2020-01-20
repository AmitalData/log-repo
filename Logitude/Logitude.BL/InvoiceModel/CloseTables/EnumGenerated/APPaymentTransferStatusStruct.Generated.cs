

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Logitude.BL.InvoiceModel.CloseTables
{
    public struct APPaymentTransferStatusValues
    {   
       public const string Blocked = "BL";  
       public const string ErrorInTransfer = "ET";  
       public const string InProgress = "IP";  
       public const string NotReady = "NR";  
       public const string Ready = "RD";  
       public const string Transferred = "TR";  
    }
}

