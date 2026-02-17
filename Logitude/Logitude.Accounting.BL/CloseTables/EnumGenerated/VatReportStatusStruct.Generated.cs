

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Logitude.Accounting.BL.CloseTables
{
    public struct VatReportStatusValues
    {   
       public const string Draft = "D";  
       public const string Approved = "A";  
       public const string Cancelled = "C";  
       public const string Transmitted = "T";  
       public const string Error = "E";  
       public const string InProgress = "P";  
    }
}

