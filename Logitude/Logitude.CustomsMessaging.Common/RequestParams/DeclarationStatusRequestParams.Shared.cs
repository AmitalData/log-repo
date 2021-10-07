using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class DeclarationStatusRequestParams : RequestParamsBase
    {

        public string DeclarationNumber { get; set; }
        public string CargoTypeCode { get; set; }
        public string ManifestNumber { get; set; }
        public string SecondCargoID { get; set; }
        public string ThirdCargoID { get; set; }
        public string CustomFileNo { get; set; }
        public bool DeclarationRadio { get; set; }
        public bool CargoRadio { get; set; }
        public bool OldReshimonRadio { get; set; }
        public string OldReshimonNumber { get; set; }
        public string RequestOrigin { get; set; } // moran 20.1.16 - Task 19428
        public string TesterSendOption { get; set; }
        public string DeclarationList { get; set; }
        public string CourierMaster { get; set; }
    }
}
