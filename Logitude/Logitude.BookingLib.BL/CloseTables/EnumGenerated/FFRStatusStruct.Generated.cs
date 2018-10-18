

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Logitude.BookingLib.BLs
{
    public struct FFRStatusValues
    {   
       public const string BookingConfirmed = "BCN";  
       public const string BookingRequest = "BRQ";  
       public const string BookingRequestRejected = "BRR";  
       public const string CancellationAccepted = "CAA";  
       public const string CancellationRequestRejected = "CRR";  
       public const string CancellationRequestSent = "CRS";  
       public const string CancelledManually = "CNM";  
       public const string Confirmed = "CNF";  
       public const string ConfirmedManually = "CFM";  
       public const string FMA = "FMA";  
       public const string FNA = "FNA";  
       public const string NotSent = "NST";  
       public const string Partiallyconfirmed = "PAR";  
       public const string Sent = "SNT";  
       public const string WaitingforAirlineCancellation = "RBC";  
       public const string WaitingforAirlineConfirmation = "RBA";  
    }
}

