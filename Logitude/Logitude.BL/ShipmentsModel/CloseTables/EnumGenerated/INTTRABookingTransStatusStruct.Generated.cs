

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Logitude.BL.ShipmentsModel.CloseTables
{
    public struct INTTRABookingTransStatusValues
    {   
       public const string NotSent = "NST";  
       public const string BookingRequestSent = "BRS";  
       public const string BookingConfirmed = "BCD";  
       public const string BookingRequestRejected = "BRR";  
       public const string UpdateRequestSent = "URS";  
       public const string UpdateRequestRejected = "URR";  
       public const string UpdateRequestConfirmed = "URC";  
       public const string CancellationRequestSent = "CRS";  
       public const string CancellationConfirmed = "CCD";  
       public const string CancellationRequestRejected = "CRR";  
    }
}

