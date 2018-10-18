using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.RequestParams
{
      public class DeclarationStatusRequestParams: RequestParamsBase
    {

          public string DeclarationNumber { get; set; }
          public string CargoTypeCode { get; set; }
          public string ManifestNumber { get; set; }
          public string SecondCargoID { get; set; }
          public string ThirdCargoID { get; set; }

    }
}
