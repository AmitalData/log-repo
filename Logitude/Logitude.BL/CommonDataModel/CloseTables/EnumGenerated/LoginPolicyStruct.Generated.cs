

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Logitude.BL.CommonDataModel.CloseTables
{
    public struct LoginPolicyValues
    {   
       public const string CompanyIPsonly = "COMPIP";  
       public const string Disabled = "DISABLED";  
       public const string Enabled = "ENABLED";  
       public const string EnabledforExternalIPsonly = "ENFEXIPO";  
       public const string NoRestriction = "NOREST";  
       public const string TwoFactorAuthentication = "TFAUTH";  
    }
}

