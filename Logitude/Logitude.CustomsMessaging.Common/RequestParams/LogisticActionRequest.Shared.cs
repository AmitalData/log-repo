
                                                                    //Yuval Chalup 29.10.2015 TASK-16002

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class LogisticActionRequestRequestParams : RequestParamsBase
    {
        public int ExporterIdentifierType { get; set; }
        public int ExporterNumber { get; set; }
        public string PassportCountry { get; set; }
        public string PassportNumber { get; set; }
        public int RequestType { get; set; }
        public string RequestReason { get; set; }
        public string DeliverySiteID { get; set; }
        public int CargoIdentifierType { get; set; }
        public string CargoIdentifierKey1 { get; set; }
        public string CargoIdentifierKey2 { get; set; }
        public string CargoIdentifierKey3 { get; set; }
        public string PackagingTypeCode { get; set; }
        public string Quantity { get; set; }        
        public string LogisticActionRequestId { get; set; }        
        public string CustomsFile { get; set; }        
    }
}