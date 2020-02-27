

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Logitude.Accounting.BL.CloseTables
{
    public struct TaxReportLineTransmitStatusValues
    {   
       public const string Fortransmit = "1";  
       public const string Notfortransmitforthisreport = "2";  
       public const string Notfortransmitatall = "3";  
       public const string WithoutTransmit = "0";  
    }
}

