

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Logitude.BL.InvoiceModel.CloseTables
{
    public struct SATTransferStatusValues
    {   
       public const string CancellationRequestSent = "CS";  
       public const string NoTransferNeeded = "ND";  
       public const string NotTransferred = "NT";  
       public const string Transferred = "TD";  
       public const string TransferredwithErrors = "TE";  
       public const string Transferring = "TG";  
    }
}

