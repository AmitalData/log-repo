

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Logitude.Accounting.BL.CloseTables
{
    public struct ARPaymentChequeStatusValues
    {   
       public const string InCashbook = "1";  
       public const string InBank = "2";  
       public const string InBankAccount = "3";  
       public const string ReturnedFromBank = "4";  
       public const string ReturnedToCustomer = "5";  
       public const string Redeemed = "6";  
    }
}

