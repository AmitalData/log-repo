using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class CargoSealsRequestParams : RequestParamsBase
    {
        public DateTime UpdateDate { get; set; }
        public string ContainerNumber { get; set; }
        public string CargoRowNumber { get; set; }
        public string ImporterNumber { get; set; }
        public string CargoIdentifierTypeCode { get; set; }
        public string CargoIdentifierKey1 { get; set; }
        public string CargoIdentifierKey2 { get; set; }
        public string CargoIdentifierKey3 { get; set; }
        public string DeclarationNumber { get; set; }
        public string DeclarationID { get; set; }
        public string CustomFileNo { get; set; }

        public List<CargoSealDetails> CargoSealList { get; set; }
    }

    public class CargoSealDetails
    {
        public string SealNumber { get; set; }
        public string SealTypeCode { get; set; }
        public string SealCompletenessStateCode { get; set; }
        public string UpdateReasonCode { get; set; }
        public string UpdateTypeCode { get; set; }
        public string Remarks { get; set; }
    }
}
