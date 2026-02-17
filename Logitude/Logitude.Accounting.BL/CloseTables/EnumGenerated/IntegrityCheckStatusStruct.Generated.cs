

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Logitude.Accounting.BL.CloseTables
{
    public struct IntegrityCheckStatusValues
    {   
       public const string Created = "1";  
       public const string InProgress = "2";  
       public const string CheckCompleted = "3";  
       public const string FixCompleted = "4";  
       public const string Failed = "5";  
    }
}

