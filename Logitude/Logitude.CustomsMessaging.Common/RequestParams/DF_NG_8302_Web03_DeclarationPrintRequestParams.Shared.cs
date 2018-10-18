using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{// moran 8.1.15 - Task 10004
    public class DF_NG_8302_Web03_DeclarationPrintRequestParams : RequestParamsBase
    {
        public bool IsSearchByDeclarationRadio { get; set; }
        public List<string> DeclarationNumber;

        public bool IsSearchByCargoRadio { get; set; }
        public string CargoTypeCode { get; set; }
        public string ManifestNumber { get; set; }
        public string SecondCargoID { get; set; }
        public string ThirdCargoID { get; set; }
        public string CustomFileNo { get; set; }

    }
}
