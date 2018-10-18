

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Logitude.BookingLib.BLs
{
    public struct BookingSpaceAllocationValues
    {   
       public const string SellingSpaceAllocationAgainstAllotment = "CA";  
       public const string CancellationNoted = "CN";  
       public const string HoldingConfirmed = "HK";  
       public const string HoldingWaitList = "HL";  
       public const string HaveRequestedSpaceAllocation = "HN";  
       public const string Confirming = "KK";  
       public const string WaitList = "LL";  
       public const string RequestingSpaceAllocationifNotAvailableWillAcceptAlternative = "NA";  
       public const string RequestingSpaceAllocationWillNotAcceptAlternative = "NN";  
       public const string UnableFlightDoesNotOperate = "UN";  
       public const string Unable = "UU";  
       public const string CancelAnyPreviousSpaceAllocation = "XX";  
    }
}

