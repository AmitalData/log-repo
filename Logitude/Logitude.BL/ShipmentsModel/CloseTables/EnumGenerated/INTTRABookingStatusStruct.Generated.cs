

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Logitude.BL.ShipmentsModel.CloseTables
{
    public struct INTTRABookingStatusValues
    {   
       public const string NotSent = "NS";  
       public const string Sent = "ST";  
       public const string Confirmed = "CD";  
       public const string Declined = "DC";  
       public const string Pending = "PG";  
       public const string Cancelled = "CA";  
       public const string Replaced = "RD";  
       public const string Error = "ER";  
       public const string WaitingForConfirmation = "WC";  
       public const string RejectedbyUser = "RU";  
    }
}

