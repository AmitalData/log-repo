using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class SendCollateralsRequestParams : RequestParamsBase
    {
          public List<string> Collaterals { get; set; }
    }

}
