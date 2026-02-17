

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Logitude.Accounting.BL.CloseTables
{
    public struct JournalStatusTypeValues
    {   
       public const string Draft = "0";  
       public const string WaitingforApproval = "1";  
       public const string Approved = "2";  
       public const string Voided = "3";  
       public const string Failed = "4";  
    }
}

