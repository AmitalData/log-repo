using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class MasterBOLFeedBackResponseData : ResponseDataBase
    {
        public List<InternalCargoResult> InternalCargosList { get; set; }
    }

    public class InternalCargoResult
    {
        public string CargoIdentiferTypeId { get; set; }
        public string CargoIdentifierKey1 { get; set; }
        public string CargoIdentifierKey2 { get; set; }
        public string CargoIdentifierKey3 { get; set; }
        public string PacakgesQuantity { get; set; }
        public string TotalWheight { get; set; }
        public string Submitter { get; set; }
    }
}
